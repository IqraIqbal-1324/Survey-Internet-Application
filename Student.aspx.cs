using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace IADLAB08_QuizApplication
{
    public partial class Student : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["QuizDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Student")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                lblWelcome.Text = "Welcome Surveyor: " + Session["Username"].ToString();
                LoadQuestions();
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void btnBackLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        private void LoadQuestions()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT QuestionID, QuestionText, OptionA, OptionB, OptionC, OptionD FROM Questions WHERE Subject='Survey'";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptQuestions.DataSource = dt;
                rptQuestions.DataBind();
            }
        }
        public DataTable GetOptions(object optionA, object optionB, object optionC, object optionD)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");

            if (optionA != null && optionA.ToString() != "")
                dt.Rows.Add(optionA.ToString(), "A");

            if (optionB != null && optionB.ToString() != "")
                dt.Rows.Add(optionB.ToString(), "B");

            if (optionC != null && optionC.ToString() != "")
                dt.Rows.Add(optionC.ToString(), "C");

            if (optionD != null && optionD.ToString() != "")
                dt.Rows.Add(optionD.ToString(), "D");

            return dt;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int studentID=Convert.ToInt32(Session["UserID"]);
            foreach (RepeaterItem item in rptQuestions.Items)
            {
                RadioButtonList rbl=item.FindControl("rblOptions") as RadioButtonList;

                if (rbl==null||rbl.SelectedValue=="")
                {
                    lblMessage.Text = "Please answer all the questions before submitting.";
                    return; // stops the submission
                }
            }
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                foreach (RepeaterItem item in rptQuestions.Items)
                {
                    HiddenField hfQuestionID = (HiddenField)item.FindControl("hfQuestionID");
                    RadioButtonList rbl = (RadioButtonList)item.FindControl("rblOptions");

                    if (hfQuestionID != null && rbl != null && rbl.SelectedValue != "")
                    {
                        string query = @"INSERT INTO Answers (StudentID, QuestionID, SelectedOption)
                                 VALUES (@StudentID, @QuestionID, @SelectedOption)";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentID;
                        cmd.Parameters.Add("@QuestionID", SqlDbType.Int).Value = Convert.ToInt32(hfQuestionID.Value);
                        cmd.Parameters.Add("@SelectedOption", SqlDbType.VarChar, 1).Value = rbl.SelectedValue;

                        cmd.ExecuteNonQuery();

                        rbl.Enabled = false;
                    }
                }
            }

            btnSubmit.Enabled = false;
            lblMessage.Text = "Your survey has been submitted successfully!";
        }
    }
}