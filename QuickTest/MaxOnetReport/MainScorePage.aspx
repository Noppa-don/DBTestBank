<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MainScorePage.aspx.vb" Inherits="QuickTest.MainScorePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div id="ProfileImageDiv">
            <table>
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" />
                    </td>
                </tr>    
            </table>
        </div>
       <div id="UserDetailDiv">
            <table>
                <tr>
                    <td>
                        <span></span>
                    </td>
                </tr> 
                 <tr>
                    <td>
                        <span></span> 
                    </td>
                </tr>    
                 <tr>
                    <td>
                         <span></span>
                    </td>
                </tr>       
            </table>
        </div>
       <div id ="RangeScoreDiv">
            <asp:Repeater ID="RptHomeWork" runat="server">
                <HeaderTemplate>
                    <table class="bordered">
                        <thead>
                            <tr>
                                <th style="width: 40%;">
                                    หน่วยการเรียนรู้
                                </th>
                                <th style="width: 10%;">
                                    A (100% - 80%)
                                </th>
                                 <th style="width: 10%;">
                                    B (79% - 70%)
                                </th>
                               <th style="width: 10%;">
                                    C (69% - 60%)
                                </th>
                                <th style="width: 10%;">
                                    D (59% - 50%)
                                </th>
                                <th style="width: 10%;">
                                    E (49% - 0%)
                                </th>
                                <th style="width: 10%;">
                                    ไม่ได้ทำ
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="<%# Container.DataItem("QcategoryId")%>">
                            <td onclick="GotoStudentScorePage(<%# Container.DataItem("QSetId")%>);" style="background: #FFFFCC;">
                                <%# Container.DataItem("QCatgoryName")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFCC;">
                                <%# Container.DataItem("ScoreAAmount")%>
                            </td>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFCC;">
                                <%# Container.DataItem("ScoreBAmount")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFCC;">
                                <%# Container.DataItem("ScoreCAmount")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFCC;">
                                <%# Container.DataItem("ScoreDAmount")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFCC;">
                                <%# Container.DataItem("ScoreEAmount")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFCC;">
                                <%# Container.DataItem("NotScoreAmount")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                          <tr id="<%# Container.DataItem("QcategoryId")%>">
                            <td onclick="GotoStudentScorePage(<%# Container.DataItem("QSetId")%>);" style="background: #FFFFFF;">
                                <%# Container.DataItem("QCatgoryName")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFFF;">
                                <%# Container.DataItem("ScoreAAmount")%>
                            </td>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFFF;">
                                <%# Container.DataItem("ScoreBAmount")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFFF;">
                                <%# Container.DataItem("ScoreCAmount")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFFF;">
                                <%# Container.DataItem("ScoreDAmount")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFFF;">
                                <%# Container.DataItem("ScoreEAmount")%>
                            </td>
                            <td onclick="GotoRangeDetailPage(<%# Container.DataItem("QSetId")%>);"  style="background: #FFFFFF;">
                                <%# Container.DataItem("NotScoreAmount")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
       </div>
    </form>
</body>
</html>
