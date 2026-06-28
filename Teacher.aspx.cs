using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IADLAB08_QuizApplication
{
    public partial class Teacher : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QuizDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Teacher")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                lblWelcome.Text = "Welcome Survey Builder: " + Session["Username"].ToString();
                LoadQuestions();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void ddlQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQuestionType.SelectedValue == "TF")
            {
                txtOptionA.Text = "True";
                txtOptionB.Text = "False";
                txtOptionC.Text = "";
                txtOptionD.Text = "";

                txtOptionC.Enabled = false;
                txtOptionD.Enabled = false;
            }
            else
            {
                txtOptionA.Text = "";
                txtOptionB.Text = "";
                txtOptionC.Text = "";
                txtOptionD.Text = "";

                txtOptionC.Enabled = true;
                txtOptionD.Enabled = true;
            }
        }

        protected void btnAddQuestion_Click(object sender, EventArgs e)
        {
            if (txtQuestion.Text.Trim() == "" ||
                txtOptionA.Text.Trim() == "" ||
                txtOptionB.Text.Trim() == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please enter question and required options.";
                return;
            }

            if (ddlQuestionType.SelectedValue == "MCQ")
            {
                if (txtOptionC.Text.Trim() == "" || txtOptionD.Text.Trim() == "")
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "For MCQ, please enter all four options.";
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Questions
                                (QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption, Subject, DifficultyLevel, QuestionType, TeacherID)
                                VALUES
                                (@QuestionText, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectOption, @Subject, @DifficultyLevel, @QuestionType, @TeacherID)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@QuestionText", SqlDbType.NVarChar).Value = txtQuestion.Text.Trim();
                cmd.Parameters.Add("@OptionA", SqlDbType.NVarChar, 200).Value = txtOptionA.Text.Trim();
                cmd.Parameters.Add("@OptionB", SqlDbType.NVarChar, 200).Value = txtOptionB.Text.Trim();
                cmd.Parameters.Add("@OptionC", SqlDbType.NVarChar, 200).Value = txtOptionC.Text.Trim();
                cmd.Parameters.Add("@OptionD", SqlDbType.NVarChar, 200).Value = txtOptionD.Text.Trim();

                // Survey does not need a real correct answer, but your DB column is NOT NULL.
                // So A is stored only to satisfy database requirement.
                cmd.Parameters.Add("@CorrectOption", SqlDbType.Char, 1).Value = "A";

                cmd.Parameters.Add("@Subject", SqlDbType.NVarChar, 50).Value = "Survey";
                cmd.Parameters.Add("@DifficultyLevel", SqlDbType.NVarChar, 20).Value = ddlDifficulty.SelectedValue;
                cmd.Parameters.Add("@QuestionType", SqlDbType.NVarChar, 20).Value = ddlQuestionType.SelectedValue;
                cmd.Parameters.Add("@TeacherID", SqlDbType.Int).Value = Convert.ToInt32(Session["UserID"]);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Survey question added successfully.";

            ClearFields();
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD, QuestionType, DifficultyLevel
                                 FROM Questions
                                 WHERE Subject = 'Survey'
                                 ORDER BY QuestionID DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvQuestions.DataSource = dt;
                gvQuestions.DataBind();
            }
        }

        protected void gvQuestions_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int questionID = Convert.ToInt32(gvQuestions.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Questions WHERE QuestionID = @QuestionID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionID;

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "Question deleted successfully.";
            LoadQuestions();
        }

        private void ClearFields()
        {
            txtQuestion.Text = "";
            txtOptionA.Text = "";
            txtOptionB.Text = "";
            txtOptionC.Text = "";
            txtOptionD.Text = "";

            txtOptionC.Enabled = true;
            txtOptionD.Enabled = true;

            ddlQuestionType.SelectedIndex = 0;
            ddlDifficulty.SelectedIndex = 0;
        }
    }
}