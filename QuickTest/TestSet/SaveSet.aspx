<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SaveSet.aspx.vb"
    Inherits="QuickTest.SaveSet" %>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function onSave(questionId, chkBox, qSetId, testSetId, userId) {
            var valReturnFromCodeBehide;
            $.ajax({ type: "POST",
                url: "/testset/SelectEachQuestion.aspx/OnSaveCodeBehide",
                data: "{ questionId : '" + questionId + "', needRemove : '" + !(chkBox.checked) + "', qSetId : '" + qSetId + "', testSetId : '" + testSetId + "', userId : '" + userId + "' }",  //" 
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    valReturnFromCodeBehide = msg.d;
                    //alert('success'+valReturnFromCodeBehide);
                },
                error: function myfunction(request, status) {
                    //alert('shin' + request.statusText + status);    
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="centeringdivouter">
        <div id="centeringdivmiddle">
            <div id="centeringdivinner">
                <div id="main" style="width: 550px;">
                    <div id="site_content" style="width: 530px;">
                        <div class="content" style="width: 520px;">
                            <center>
                                <h2>
                                    บันทึกข้อสอบชุดนี้ไว้
                                </h2>
                                <form id="saveset" runat="server">
                                <div class="form_settings">
                                    <table>
                                        <tr>
                                            <td style="text-align: right; background-color: White;">
                                                ตั้งชื่อว่า :&nbsp;
                                            </td>
                                            <td style="background-color: White;">
                                                <asp:TextBox ID="txtName" runat="server" Style="width: 250px"></asp:TextBox><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtName"
                                                    Display="Dynamic" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; background-color: White;">
                                                ให้เวลาในการทำ :&nbsp;
                                            </td>
                                            <td style="background-color: White;">
                                                <asp:TextBox ID="txtTime" runat="server" value="30" MaxLength="3" Style="width: 50px;
                                                    text-align: right;"></asp:TextBox>
                                                นาที<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                    ControlToValidate="txtTime" Display="Dynamic" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; background-color: White;">
                                                ตัวหนังสือ :&nbsp;
                                            </td>
                                            <td style="background-color: White;">
                                                <asp:DropDownList ID="ddlTextSize" runat="server">
                                                    <asp:ListItem Selected="True" Value="0">ขนาดตัวปกติ (สำหรับเด็กมัธยม)</asp:ListItem>
                                                    <asp:ListItem Value="1">ขนาดตัวใหญ่ (สำหรับประถมปลาย)</asp:ListItem>
                                                    <asp:ListItem Value="2">ขนาดตัวใหญ่มาก (สำหรับประถมต้น)</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                          <tr>
                                                  <td style="padding-left: 220px;" colspan="2">
                                                    <asp:CheckBox ID="IsPractice" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;ให้นักเรียนใช้ฝึกฝนได้ด้วย" />
                                                </td>
                                            </tr>
                                    </table>
                                    <p>
                                    </p>
                                    <p>
                                    </p>
                                    <br />
                                    <asp:Button Style="margin-left: 15px" ID="btnSave" runat="server" Width="100px" Text="บันทึก"
                                        class="submit" />
                                </div>
                                </form>
                            </center>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <center>
    </center>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <link rel="stylesheet" href="<%=ResolveUrl("~")%>css/prettyLoader.css" type="text/css"
        media="screen" charset="utf-8" />
    <script src="<%=ResolveUrl("~")%>js/jquery.prettyLoader.js" type="text/javascript"
        charset="utf-8"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $.prettyLoader();
        });
    </script>
</asp:Content>
