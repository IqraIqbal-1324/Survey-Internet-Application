using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IADLAB08_QuizApplication
{
    public partial class Admin : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QuizDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                lblWelcome.Text = "Welcome Admin: " + Session["Username"].ToString();

                LoadUsers();
                LoadResults();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        
        //Add User
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter username and password.";
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Username already exists.";
                    return;
                }

                string insertQuery = @"INSERT INTO Users (Username, Password, Role)
                                       VALUES (@Username, @Password, @Role)";

                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.Add("@Username", System.Data.SqlDbType.VarChar).Value = txtUsername.Text.Trim();

                cmd.Parameters.Add("@Password", System.Data.SqlDbType.VarChar).Value = txtPassword.Text.Trim();

                cmd.Parameters.Add("@Role", System.Data.SqlDbType.VarChar).Value = ddlRole.SelectedValue;

                cmd.ExecuteNonQuery();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = ddlRole.SelectedValue + " added successfully.";

            txtUsername.Text = "";
            txtPassword.Text = "";

            LoadUsers();
        }

        //Load Users
        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT UserID, Username, Role
                                 FROM Users
                                 WHERE Role IN ('Teacher', 'Student')
                                 ORDER BY Role, Username";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        //Delete User
        protected void gvUsers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int userID = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string deleteAnswers = "DELETE FROM Answers WHERE StudentID = @UserID";
                SqlCommand cmdAnswers = new SqlCommand(deleteAnswers, conn);
                cmdAnswers.Parameters.AddWithValue("@UserID", userID);
                cmdAnswers.ExecuteNonQuery();

                string deleteQuestions = "DELETE FROM Questions WHERE TeacherID = @UserID";
                SqlCommand cmdQuestions = new SqlCommand(deleteQuestions, conn);
                cmdQuestions.Parameters.AddWithValue("@UserID", userID);
                cmdQuestions.ExecuteNonQuery();

                string deleteUser = "DELETE FROM Users WHERE UserID = @UserID";
                SqlCommand cmdUser = new SqlCommand(deleteUser, conn);
                cmdUser.Parameters.AddWithValue("@UserID", userID);
                cmdUser.ExecuteNonQuery();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "User deleted successfully.";

            LoadUsers();
            LoadResults();
        }

        //Survey Results
        private void LoadResults()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        q.QuestionText,
                        CASE 
                            WHEN a.SelectedOption = 'A' THEN q.OptionA
                            WHEN a.SelectedOption = 'B' THEN q.OptionB
                            WHEN a.SelectedOption = 'C' THEN q.OptionC
                            WHEN a.SelectedOption = 'D' THEN q.OptionD
                        END AS OptionText,
                        COUNT(a.AnswerID) AS TotalResponses
                    FROM Answers a
                    JOIN Questions q ON a.QuestionID = q.QuestionID
                    WHERE q.Subject = 'Survey'
                    GROUP BY q.QuestionText, a.SelectedOption, q.OptionA, q.OptionB, q.OptionC, q.OptionD
                    ORDER BY q.QuestionText";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvSurveyResults.DataSource = dt;
                gvSurveyResults.DataBind();
            }
        }
    }
}