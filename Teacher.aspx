<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Teacher.aspx.cs" Inherits="IADLAB08_QuizApplication.Teacher" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Survey Builder</title>

    <style>
        body {
            margin: 0;
            font-family: "Segoe UI", Arial, sans-serif;
            background: #f4f7fb;
            color: #1f2d3d;
        }

        .container {
            width: 85%;
            max-width: 1000px;
            margin: 35px auto;
            background: white;
            padding: 30px 40px;
            border-radius: 14px;
            box-shadow: 0 8px 25px rgba(0,0,0,0.12);
        }

        h2 {
            text-align: center;
            color: #1f3a5f;
            margin-bottom: 8px;
        }

        h3 {
            color: #1f3a5f;
            border-bottom: 2px solid #2e86c1;
            padding-bottom: 10px;
            margin-top: 30px;
        }

        .welcome {
            display: block;
            text-align: center;
            color: green;
            font-weight: bold;
            margin-bottom: 20px;
        }

        .topbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: #eef6fc;
            padding: 15px 18px;
            border-radius: 10px;
            margin-bottom: 25px;
            border: 1px solid #d6eaf8;
        }

        .form-box {
            background: #fbfdff;
            border: 1px solid #d9e4ec;
            padding: 25px;
            border-radius: 12px;
            margin-bottom: 25px;
        }

        label {
            font-weight: 600;
            margin-bottom: 6px;
            display: block;
        }

        input[type="text"], textarea, select {
            width: 100%;
            padding: 11px;
            border: 1px solid #b8c7d3;
            border-radius: 8px;
            box-sizing: border-box;
            font-size: 15px;
            margin-bottom: 16px;
        }

        textarea {
            resize: vertical;
        }

        .row {
            display: flex;
            gap: 18px;
        }

        .col {
            flex: 1;
        }

        .btn {
            background: #2e86c1;
            color: white;
            border: none;
            padding: 11px 24px;
            border-radius: 8px;
            cursor: pointer;
            font-weight: bold;
        }

        .btn:hover {
            background: #21618c;
        }

        .logout {
            background: #c0392b;
        }

        .logout:hover {
            background: #922b21;
        }

        .message {
            display: block;
            margin-top: 15px;
            font-weight: bold;
        }

        .grid {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
            background: white;
        }

        .grid th {
            background: #2c3e50;
            color: white;
            padding: 11px;
            text-align: left;
        }

        .grid td {
            border: 1px solid #ddd;
            padding: 10px;
        }

        .grid tr:nth-child(even) {
            background: #f6f9fc;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

<div class="container">

    <h2>Survey Builder</h2>
    <asp:Label ID="lblWelcome" runat="server" CssClass="welcome"></asp:Label>

    <div class="topbar">
        <strong>Survey Question Control Panel</strong>
        <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn logout" OnClick="btnLogout_Click" />
    </div>

    <div class="form-box">
        <h3>Add Survey Question</h3>

        <label>Question Statement:</label>
        <asp:TextBox ID="txtQuestion" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>

        <label>Question Type:</label>
        <asp:DropDownList ID="ddlQuestionType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlQuestionType_SelectedIndexChanged">
            <asp:ListItem Text="Multiple Choice" Value="MCQ"></asp:ListItem>
            <asp:ListItem Text="True / False" Value="TF"></asp:ListItem>
        </asp:DropDownList>

        <label>Difficulty Level:</label>
        <asp:DropDownList ID="ddlDifficulty" runat="server">
            <asp:ListItem Text="Easy" Value="Easy"></asp:ListItem>
            <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
            <asp:ListItem Text="Hard" Value="Hard"></asp:ListItem>
        </asp:DropDownList>

        <div class="row">
            <div class="col">
                <label>Option A:</label>
                <asp:TextBox ID="txtOptionA" runat="server"></asp:TextBox>
            </div>

            <div class="col">
                <label>Option B:</label>
                <asp:TextBox ID="txtOptionB" runat="server"></asp:TextBox>
            </div>
        </div>

        <div class="row">
            <div class="col">
                <label>Option C:</label>
                <asp:TextBox ID="txtOptionC" runat="server"></asp:TextBox>
            </div>

            <div class="col">
                <label>Option D:</label>
                <asp:TextBox ID="txtOptionD" runat="server"></asp:TextBox>
            </div>
        </div>

        <asp:Button ID="btnAddQuestion" runat="server" Text="Add Survey Question" CssClass="btn" OnClick="btnAddQuestion_Click" />

        <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
    </div>

    <div class="form-box">
        <h3>Survey Questions</h3>

        <asp:GridView ID="gvQuestions" runat="server"
            AutoGenerateColumns="False"
            CssClass="grid"
            DataKeyNames="QuestionID"
            OnRowDeleting="gvQuestions_RowDeleting">

            <Columns>
                <asp:BoundField DataField="QuestionID" HeaderText="ID" />
                <asp:BoundField DataField="QuestionText" HeaderText="Question" />
                <asp:BoundField DataField="OptionA" HeaderText="Option A" />
                <asp:BoundField DataField="OptionB" HeaderText="Option B" />
                <asp:BoundField DataField="OptionC" HeaderText="Option C" />
                <asp:BoundField DataField="OptionD" HeaderText="Option D" />
                <asp:BoundField DataField="QuestionType" HeaderText="Type" />
                <asp:BoundField DataField="DifficultyLevel" HeaderText="Difficulty" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server"
                            Text="Delete"
                            CommandName="Delete"
                            CssClass="btn logout"
                            OnClientClick="return confirm('Delete this question?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</div>

</form>
</body>
</html>