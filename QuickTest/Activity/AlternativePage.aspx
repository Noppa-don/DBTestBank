<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master"
    CodeBehind="AlternativePage.aspx.vb" Inherits="QuickTest.Alternative2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <script src="../js/GFB.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.fancybox.js" type="text/javascript"></script>
    <script src="../js/json2.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.signalR-2.0.2.min.js" type="text/javascript"></script>
    <%If Not IE = "1" Then%>
    <%If Not Session("selectedSession") = "PracticeFromComputer" Then%>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script type="text/javascript">var baseUrl = "<%= ResolveUrl("~/") %>"; var Groupname = '<%=GroupName %>';</script>
    <script src="../js/DashboardSignalR.js" type="text/javascript"></script>
    <%End If%>
    <%End If%>
    <script type="text/javascript">
        document.createElement('article');
        document.createElement('aside');
        document.createElement('figure');
        document.createElement('footer');
        document.createElement('header');
        document.createElement('hgroup');
        document.createElement('nav');
        document.createElement('section');
    </script>
    <script type="text/javascript">
        var CurrentQuizId = '<%=VBQuizId %>';

        $(function () {
            //ปุ่ม ดูคะแนน
            if ($('#btnReport').length != 0) {
                new FastButton(document.getElementById('btnReport'), TriggerServerButton);
            }

            //ปุ่ม ทบทวนกิจกรรม
            if ($('#Button1').length != 0) {
                new FastButton(document.getElementById('Button1'), TriggerServerButton);
            }

            //ปุ่ม ทำควิซ/จัดชุดใหม่
            if ($('#Button2').length != 0) {
                new FastButton(document.getElementById('Button2'), TriggerServerButton);
            }
        });

        $(function () {
            $('#btnReport').click(function (e) {
                e.preventDefault();
            });
        });
        function CheckScore() { // check score
            $.fancybox({
                'autoScale': true,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'href': '<%=ResolveUrl("~")%>activity/ActivityReport.aspx?ReportMenu=1&ShowBtnBack=False&QuizId=' + CurrentQuizId,
                'type': 'iframe',
                'width': 900,
                'minHeight': 520
            });
        }
    </script>
    <style type="text/css">
        #site_content{
            border-radius:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<script src="../js/jquery-1.7.1.js" type="text/javascript"></script>--%>
    <form runat="server" id='form1'>
        <br />
        <br />      
        <div id="main">
            <div id="site_content">
                <div class="content" style="width: 930px;">
                    <center>
                        <%If NewQuizButtonName = "Practice" OrElse NewQuizButtonName = "PracticeFromComputer" Then%>
                        <h2>
                        ดูคะแนน-ทบทวน หรือทำฝึกฝนใหม่คะ ?
                        </h2>
                        <%else %>
                        <h2>
                        ดูคะแนน-ทบทวน หรือทำควิซใหม่คะ ?
                        </h2>
                        <%End If %>
                    
                    <div id="div-1">
                        <table>
                            <tr>
                                <td style="border-bottom: initial;">
                                    <div class="form_settings">
                                        <table>
                                            <tr>
                                                <%Try%>

                                                <%If Session("QuizUseTablet").ToString() = "True" Then%>
                                                <td style="text-align: center; width: 33%;">
                                                    <a onclick="CheckScore()" class="aCheckscore">
                                                        <img alt="ดูคะแนน" class="imgAlternative" src="../images/Activity/chart3.png" /></a>
                                                </td>
                                                <%End If%>

                                                <td style="text-align: center; width: 34%;">
                                                    <a href="<%=ResolveUrl("~")%>Activity/ReviewMostWrongAnswer.aspx">
                                                    <img alt="ทบทวนกิจกรรม" class="imgAlternative" src="../images/Activity/File-Open-icon.png" /></a>
                                                </td>
                                                <td style="text-align: center; width: 33%;">
                                                      <%If NewQuizButtonName = "Practice" Then%>
                                                         <a href="<%=ResolveUrl("~")%>Practice/DashboardPracticePage.aspx">
                                                    <img alt="ทำฝึกฝนใหม่" class="imgAlternative" src="../images/answer.png" /></a>
                                                     <%ElseIf NewQuizButtonName = "PracticeFromComputer" Then%>
                                                        <%If Session("PClassId") IsNot Nothing And Session("PSubjectName") IsNot Nothing Then%>
                                                            <a href="<%=ResolveUrl("~")%>PracticeMode_Pad/ChooseQuestionSet.aspx">
                                                            <img alt="ทำฝึกฝนใหม่" class="imgAlternative" src="../images/answer.png" /></a>
                                                        <%Else%>
                                                            <a href="<%=ResolveUrl("~")%>LoginPage.aspx">
                                                            <img alt="ทำฝึกฝนใหม่" class="imgAlternative" src="../images/answer.png" /></a>
                                                        <%End If%>
                                                     <%Else%>
                                                         <a href="<%=ResolveUrl("~")%>Quiz/DashboardQuizPage.aspx">
                                                     <img alt="ทำควิซใหม่" class="imgAlternative" src="../images/answer.png" /></a>
                                                     <%End If%>
                                                </td>
                                            </tr>

                                            <tr style="height: 40px;">

                                                <%If Session("QuizUseTablet").ToString() = "True" Then%>
                                                <td style="text-align: center; border-bottom: initial;">
                                                    <a onclick="CheckScore()" class="aCheckscore" style="text-decoration: none;">
                                                        <%-- <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;" ID="btnReport"
                                                            runat="server" Text="ดูคะแนน" class="submit"  />--%>
                                                        <input type="button" style="width: 200px; position: relative; margin: 0 0 0 0px;height:40px;line-height:40px;"
                                                            value="ดูคะแนน" class="submit" id="btnReport" />
                                                    </a>
                                                </td>
                                                <%End If%>
                                               
                                                <td style="text-align: center; border-bottom: initial;">
                                                    <%--<a href="ReviewMostWrongAnswer.aspx" style="text-decoration: none;">--%>
                                                    <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;height:40px;line-height:40px;" ID="BtnReview"
                                                        runat="server" Text="ทบทวนกิจกรรม" ClientIDMode="Static" class="submit" /><%--</a>--%>
                                                </td>
                                                <td style="text-align: center; border-bottom: initial;">
                                                    <%--<a href="../TestSet/step1.aspx" style="text-decoration: none;">--%>
                                                     <%If NewQuizButtonName = "Practice" Then%>
                                                          <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;height:40px;line-height:40px;" ID="BtnPractice"
                                                        runat="server" Text="ทำฝึกฝนใหม่" ClientIDMode="Static" class="submit" /><%--</a>--%>
                                                     <%ElseIf NewQuizButtonName = "PracticeFromComputer" Then%>
                                                      <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;height:40px;line-height:40px;" ID="BtnPracticeFromComputer"
                                                        runat="server" Text="ทำฝึกฝนใหม่" ClientIDMode="Static" class="submit" /><%--</a>--%>
                                                    <%Else%>
                                                      <asp:Button Style="margin: 0 0 0 0px; width: 200px; position: relative;height:40px;line-height:40px;" ID="BtnQuiz"
                                                        runat="server" Text="ทำควิซใหม่" ClientIDMode="Static" class="submit" /><%--</a>--%>
                                                    
                                                     <%End If%>
                                                <%Catch%>
                                                <%--<%Response.Redirect("~/LoginPage.aspx", False)%>--%>
                                                <%End Try%>
                                                  
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ExtraScript" runat="server">
    <script type="text/javascript">
        var ua = navigator.userAgent.toLowerCase();
        var isAndroid = ua.indexOf("android") > -1;

        $(function () {
            //hover animation
            InjectionHover('.imgAlternative', 5);
            InjectionHover('#btnReport', 5);

            //page transition
            if (!isAndroid) {
                $('a:not(".aCheckscore")').click(function () {
                    FadePageTransitionOut();
                });
                $(':submit').click(function () {
                    FadePageTransitionOut();
                });
            }
        });
    </script>
</asp:Content>
