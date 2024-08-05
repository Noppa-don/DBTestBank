<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/QuizCreator/QuizCreatorSiteMaster.Master" CodeBehind="MyTestSet.aspx.vb" Inherits="QuickTest.MyTestSet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script src="../js/jquery-1.7.1.js"></script>
       <script type="text/javascript">

           $(function () {

               //เมื่อกดที่รุปตา เพื่อ Offline ชุดนี้
               $('.OpenEye').live('click',(function () {
                   var id = $(this).attr('eyeid');
                   $.ajax({
                       type: "POST",
                       async: false,
                       url: "<%=ResolveUrl("~")%>QuizCreator/MyTestSet.aspx/UpdateIsOnline",
                       data: "{ QCT_Id: '" + id + "',UpdateValue:'" + 0 + "'}",
                       contentType: "application/json; charset=utf-8", dataType: "json",
                       success: function (msg) {
                           if (msg.d == 'Complete') {
                               $('#imgeye_' + id).removeClass('OpenEye');
                               $('#imgeye_' + id).addClass('CloseEye');
                               $('#imgeye_' + id).attr('src', 'Images/closeEye.jpg')
                           }
                           else {
                               alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                           }
                       },
                       error: function myfunction(request, status) {
                           //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                       }
                           });
               }));

               //เมื่อกดที่รูปตา เพื่อ Online ชุดนี้
               $('.CloseEye').live('click',(function () {
                   var id = $(this).attr('eyeid');
                   $.ajax({
                       type: "POST",
                       url: "<%=ResolveUrl("~")%>QuizCreator/MyTestSet.aspx/UpdateIsOnline",
                       async: false,
                       data: "{ QCT_Id: '" + id + "',UpdateValue:'" + 1 + "'}",
                       contentType: "application/json; charset=utf-8", dataType: "json",
                       success: function (msg) {
                           if (msg.d == 'Complete') {
                               $('#imgeye_' + id).removeClass('CloseEye');
                               $('#imgeye_' + id).addClass('OpenEye');
                               $('#imgeye_' + id).attr('src', 'Images/eye.jpg')
                           }
                           else {
                               alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                           }
                       },
                       error: function myfunction(request, status) {
                           //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                       }
                        });
               }));

               //เมื่อกดที่ ชื่อชุด เพื่อไปดูรายละเอียดของชุดนั้นๆ
               $('.GotoEachTestset').click(function () {
                   var id = $(this).attr('TestsetId');
                   window.location = 'DashboardTestset.aspx?TestsetId=' + id;
               });

               //เมื่อกดที่ รูปถังขยะ เพื่อลบชุดนี้
               $('.DeleteTestset').click(function () {
                   if (confirm('ต้องการลบข้อสอบชุดนี้ ?') == true) {
                       var id = $(this).attr('deleteid');
                       //alert(id);
                       $.ajax({
                           type: "POST",
                           url: "<%=ResolveUrl("~")%>QuizCreator/MyTestSet.aspx/DeleteQCTestset",
                       async: false,
                       data: "{ QCT_Id: '" + id + "'}",
                       contentType: "application/json; charset=utf-8", dataType: "json",
                       success: function (msg) {
                           if (msg.d == 'Complete') {
                               $('#tr_' + id).stop(true, true).hide(200);
                           }
                           else {
                               alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                           }
                       },
                       error: function myfunction(request, status) {
                           //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                       }
                   });
                   }
                
               });

           });

       </script>

    <style type="text/css">
        #DivSearch {
        width:980px;
        margin-left:auto;
        margin-right:auto;
        padding:15px;
        }
        .spnSearch {
        font-size:25px;
        }
        table {
        width: 980px;
        margin-left: auto;
        margin-right: auto;
        margin-top: 30px;
        /*padding: 10px;*/
        font-size:20px;
        border:1px solid;
        border-radius:6px;
        
        /*color:#fff;*/
        border-color:#1e8cbe;
        }
            table th {
            background:#2E3839;
            padding:10px;
            color:#fff;
            font-size:25px;
            }
            table td {
            padding:10px;
            border-bottom:1px solid;
            border-color:#1e8cbe;
            }
        .OpenEye,.CloseEye {
        width:50px;
        cursor:pointer;
        }
        .GotoEachTestset {
        cursor:pointer;
        }
        .DeleteTestset {
        width:50px;
        cursor:pointer;
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="MainDivMyTestset" class="ForMainDiv">
        <div id="DivSearch">
            <span class="spnSearch">ค้นหาชุดข้อสอบ</span>
            <asp:TextBox ID="txtSearch" Font-Size="25px" style="padding:5px;margin-left:10px;" runat="server"></asp:TextBox>
            <asp:ImageButton ID="btnSearch" ImageUrl="~/QuizCreator/Images/Search.png" runat="server" style="margin-left:10px;position:relative;top:10px;width:40px;" />
        </div>
        <div id="DivContent">
            <table>
                 <tr>
                    <th>เปิดใช้ ?</th>
                    <th style="width:500px;">ชุดข้อสอบ
                    </th>
                    <th>ความนิยม</th>
                    <th>เข้าทำ</th>
                    <th>ลบทิ้ง ?</th>
                </tr>

                <asp:Repeater ID="RptItem" runat="server">
                    <ItemTemplate>
                         <tr id='tr_<%# Container.DataItem("QCT_Id") %>'>
                    <td style="text-align:center;">
                            <img class="<%# Container.DataItem("ClassName") %>" id="imgeye_<%# Container.DataItem("QCT_Id") %>" eyeid="<%# Container.DataItem("QCT_Id") %>" src="Images/<%# Container.DataItem("ImageName") %>.jpg" />
                    </td>
                    <td>
                        <div TestsetId="<%# Container.DataItem("Testset_Id") %>" class="GotoEachTestset">
                        <div>
                            <span style="font-size:23px;"><%# Container.DataItem("TestSet_Name") %></span>
                        </div>
                        <div style="margin-top:10px;">
                            <span style="font-size:20px;"><%# Container.DataItem("txtDetail") %></span>
                        </div>
                            </div>
                    </td>
                    <td style="text-align:center;"><%# Container.DataItem("QCT_AVGRating") %></td>
                    <td style="text-align:center;"><%# Container.DataItem("QCT_TotalUse") %></td>
                    <td style="text-align:center;">
                        <img src="Images/Delete.png" deleteid='<%# Container.DataItem("QCT_Id") %>' class="DeleteTestset" />
                    </td>
                </tr>
                    </ItemTemplate>
                </asp:Repeater>

                   <%--  <tr>
                    <td style="text-align:center;">
                        <img class="CloseEye" src="Images/closeEye.jpg" />
                    </td>
                    <td>
                        <div>
                            <span style="font-size:23px;">Pre-ONET:20</span>
                        </div>
                        <div style="margin-top:10px;">
                            <span style="font-size:20px;"></span>
                        </div>
                    </td>
                    <td style="text-align:center;">asdasd</td>
                    <td style="text-align:center;">asdasd</td>
                    <td style="text-align:center;">asdasd</td>
                </tr>--%>
            </table>
        </div>
    </div>
</asp:Content>
