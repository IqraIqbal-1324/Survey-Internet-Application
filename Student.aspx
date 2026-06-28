<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Student.aspx.cs" Inherits="IADLAB08_QuizApplication.Student" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Survey</title>

    <style>
        body {
            font-family: Arial;
            background: #f4f6f9;
        }

        .container {
            width: 80%;
            margin: 30px auto;
            background: white;
            padding: 30px;
            border-radius: 10px;
        }

        h2 {
            text-align: center;
        }

        .question {
            margin-bottom: 20px;
            padding: 15px;
            border: 1px solid #ddd;
            border-radius: 8px;
        }

        .btn {
            background: #2e86c1;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

<div class="container">

    <h2>Survey Page</h2>

    <asp:Label ID="lblWelcome" runat="server"></asp:Label>
    <br /><br />

    <asp:Repeater ID="rptQuestions" runat="server">
        <ItemTemplate>
            <div class="question">
                <b><%# Eval("QuestionText") %></b><br /><br />
                <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />
                <asp:RadioButtonList ID="rblOptions" runat="server"
    DataSource='<%# GetOptions(Eval("OptionA"), Eval("OptionB"), Eval("OptionC"), Eval("OptionD")) %>'
    DataTextField="Text"
    DataValueField="Value">
</asp:RadioButtonList>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Button ID="btnSubmit" runat="server" Text="Submit Survey" CssClass="btn" OnClick="btnSubmit_Click" />

    <br /><br />
    <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
    <br />
        <div style="margin-bottom:30px;">
    <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn" OnClick="btnLogout_Click" />
    <asp:Button ID="btnBackLogin" runat="server" Text="Back to Login" CssClass="btn" OnClick="btnBackLogin_Click" />
</div>
</div>
    

</form>
</body>
</html>