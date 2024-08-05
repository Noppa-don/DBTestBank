<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DefaultMaxOnet.aspx.vb" Inherits="QuickTest.DefaultMaxOnet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
    <style type="text/css">
        .NotUsage, .BeginUse {
            height: auto;
            border-radius: 10px;
            position: relative;
            background-color: white;
        }

            .BeginUse span.headspan {
                font-size: 20px;
            }

            .BeginUse span.normalspan {
                font-size: 16px;
            }

        .divUnderLine {
            border: 1px solid #faa51a;
        }

        .divSubject {
            width: 65px;
            text-align: center;
            border: 1px solid white;
            height: 100px;
            padding: 10px;
            display: inline-block;
            margin: 5px;
            position: relative;
            display: none;
            cursor: pointer;
        }

            .divSubject > .divSubjectImage {
                border: 1px solid #46c4dd;
                border-radius: 5px;
                width: inherit;
            }

                .divSubject > .divSubjectImage > input {
                    width: inherit;
                    outline: none;
                    display: block;
                }

        .divSubjectSelected {
            border: 1px solid #c6e7f0;
            background-color: #c6e7f0;
            border-radius: 10px;
        }

        .mainChooseSubject {
            width: 850px;
            padding: 5px 20px 5px;
            margin-top: 10px;
            text-align: center;
        }

        .divSelectedImage {
            background: url('../Images/Maxonet/btnChecked.png');
            background-repeat: no-repeat;
            height: 20px;
            width: 20px;
            position: absolute;
            right: -10px;
            top: -5px;
        }

        .submit {
            font: 110% 'THSarabunNew';
            padding: 2px 0 3px 0;
            cursor: pointer;
            background: #1EC9F4;
            border-radius: .5em;
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #0D8AA9;
            text-shadow: 1px 1px #178497;
            width: 180px;
        }

        .menuBar {
            margin: 5px;
            width: 90%;
            position: static;
            background: #D3F2F7;
            color: #FFF;
            padding: 5px;
            text-align: left;
            border-radius: 0.5em;
            margin-left: auto;
            margin-right: auto;
        }

        .confirm {
            border: 4px solid red;
            border-radius: 5px;
        }
    </style>
    <style type="text/css">
        input[type="radio"], input[type="checkbox"] {
            display: none;
        }

            input[type="radio"] + label, input[type="checkbox"] + label {
                font-size: 20px;
                margin-left: 30px;
                padding: 5px 13px 7px 13px;
                position: relative;
                background: initial !important;
            }

                input[type="radio"] + label span, input[type="checkbox"] + label span {
                    display: inline-block;
                    width: 30px;
                    height: 30px;
                    margin: -1px 4px 0 0;
                    vertical-align: middle;
                    cursor: pointer;
                }

                input[type="radio"] + label span, input[type="checkbox"] + label span {
                    background-color: black;
                    margin-right: 10px;
                    /*background: url("../Images/Activity/Subject/Thai.png");*/
                    background-size: 100%;
                }

            input[type="radio"]:checked + label s, input[type="checkbox"]:checked + label s {
                height: 20px;
                width: 20px;
                position: absolute;
                right: -10px;
                top: -5px;
            }

            input[type="radio"]:checked + label span, input[type="checkbox"]:checked + label span {
                /*background-color: #70D4E6;*/
            }

            input[type="radio"]:checked + label s, input[type="checkbox"]:checked + label s {
                background: url('../Images/Maxonet/btnChecked.png');
                background-repeat: no-repeat;
            }

            input[type="radio"] + label span, input[type="radio"]:checked + label span, input[type="checkbox"] + label span, input[type="checkbox"]:checked + label span {
                border: 1px solid #46c4dd;
                border-radius: 5px;
            }

            input[type="radio"]:checked + label, input[type="checkbox"]:checked + label {
                background-color: #c6e7f0 !important;
                border-radius: 10px;
            }

            input[type="checkbox"]:disabled + label {
                opacity: 0.4;
            }

        table tr td {
            background: none !important;
            color: none !important;
            border-bottom: none !important;
        }

        .divWarningMaxOnet {
            padding-top: 0;
            color: red;
            text-align: left;
            width: 600px;
            margin-left: auto;
            margin-right: auto;
            font-size: 20px;
        }

        .subjectNotUsage {
            opacity: 0.3;
        }

        .mainblogUI {
            width: 100%;
            height: 100%;
            position: absolute;
            top: 0;
            left: 0;
            display: none;
        }

            .mainblogUI > .blogUI {
                background-color: black;
                opacity: 0.5;
                width: inherit;
                height: inherit;
                position: inherit;
            }

            .mainblogUI > span {
                color: white;
                z-index: 1;
                font-size: 35px;
                top: 50%;
                left: 50%;
                position: absolute;
                margin-top: -35px;
                margin-left: -219px;
            }

        .subjectSelected {
            border: 2px solid red !important;
        }

        span.txtUnitAmount {
            font-size: 28px;
        }

        div.addCredit {
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            position: absolute;
            display: none;
            z-index: 99;
        }

            div.addCredit > div.divOpacity {
                background: black;
                opacity: 0.8;
                width: inherit;
                height: inherit;
            }

            div.addCredit > div.divContentCredit {
                background-color: whitesmoke;
                width: 500px;
                height: 246px;
                border-radius: 5px;
                margin: auto;
                position: absolute;
                left: 0;
                right: 0;
                margin-left: auto;
                margin-right: auto;
                top: 0;
                bottom: 0;
                padding: 10px;
                text-align: center;
            }

                div.addCredit > div.divContentCredit > div {
                    border-radius: 5px;
                    border: 1px solid #f47a20;
                    /* height: inherit; */
                    padding: 20px;
                }

            div.addCredit span.header {
                font-weight: bold;
                font-size: 22px;
            }

            div.addCredit table tr td:first-child {
                width: 35%;
                text-align: right;
                padding-right: 20px;
            }

            div.addCredit table input {
                padding: 1px;
                width: 200px;
                font: 80% 'THSarabunNew';
                border: 1px solid #C6E7F0;
                background: #EFF8FB;
                color: #47433F;
                border-radius: 5px;
                font-size: 16px;
            }

            div.addCredit input[type=button] {
                font: 120% 'THSarabunNew';
                border: 0;
                padding: 2px 0 3px 0;
                cursor: pointer;
                border-radius: .5em;
                box-shadow: 0 1px 2px rgba(0,0,0,.2);
                color: #FFF;
                border: solid 1px #0D8AA9;
                text-shadow: 1px 1px #178497;
                background: #63cfdf;
            }

        div.addSubject {
            width: 150px;
            height: 50px;
            position: absolute;
            background-color: rgba(255, 0, 0, 0.61);
            line-height: 50px;
            border-radius: 5px;
            color: whitesmoke;
            cursor: pointer;
            z-index: 1;
            right: 0;
            margin-right: 160px;
            text-align: center;
        }

            div.addSubject > span, div.backToMaxonet > span {
                font-size: 22px;
                font-weight: bold;
            }

        div.backToMaxonet {
            width: 140px;
            height: 50px;
            position: absolute;
            background-color: rgba(255, 141, 0, 0.61);
            line-height: 50px;
            border-radius: 5px;
            color: whitesmoke;
            cursor: pointer;
            z-index: 1;
            right: 0;
            margin-right: 15px;
            text-align: center;
        }

        html {
            background-image: url(../Images/MaxOnet/bg.png);
            background-repeat: repeat-y;
            background-attachment: fixed;
            background-position: center top;
            background-color: #ff8201;
            background-size: initial;
        }

        .btnSub {
            width: 250px !important;
            margin-top: 10px;
        }
    </style>
    <style type="text/css">
        #site_content {
            /*padding: 15px 12px 15px 0 !important;*/
            position: relative;
        }

        .content {
            padding: 0 !important;
            margin: 0 !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="main">

        <div id="site_content">
            <%If Request.QueryString("addsubject") IsNot Nothing Then %>
            <div class="addSubject">
                <span>เติมเครดิต</span>
            </div>

            <div class="backToMaxonet">
                <span>ไป MaxOnet</span>
            </div>
            <%End if %>
            <div class="content" style="width: 930px;">
                <div id="deviceDisable" runat="server" clientidmode="Static" class="NotUsage" style="display: none; height: 450px; text-align: center;">
                    <img src="../Images/NotApproveButton.png" width="120" /><br />
                    <h1 style="color: red; font-weight: bold; font-size: 24px; padding: 0 0 10px 0;">ไม่สามารถใช้งานได้ค่ะ</h1>
                    <div class="divWarningMaxOnet">
                        <span>- ใช้ได้แค่เครื่องเดียวนะคะ</span><br />
                        <span>- ถ้าลงทะเบียนเครื่องอื่นไว้ จะใช้ได้จากเครื่องล่าสุดเครื่องเดียวค่ะ</span><br />
                        <span>- หากต้องการสลับมาใช้เครื่องนี้ ให้ลบแอพ แล้วติดตั้ง ลงทะเบียนใหม่ค่ะ</span><br />
                        <br />
                        <span style="font-weight: bold;">สอบถามติดต่อ email : maxonet@iknow.co.th</span>
                    </div>
                </div>
                <div id="deviceEnable" runat="server" clientidmode="Static" class="BeginUse" style="background-color: white; padding: 10px 20px; display: none;">
                    <span class="txtUnitAmount"></span>
                    <br />
                    <span class="headspan">เลือกวิชา</span>
                    <%If Request.QueryString("addsubject") IsNot Nothing Then %>
                    <div class="divMore" style="text-align: right; margin-top: -37px;">
                        <input type="image" id="btnMore" src="../Images/MaxOnet/more.png" style="width: 25px;" />
                    </div>
                    <%End if %>
                    <span class="normalspan" style="margin-left: 20px; display: none;">(เลือกได้ครั้งเดียว วิชาเดียวนะคะ)</span>
                    <div class="divUnderLine"></div>
                    <div class="mainChooseSubject">
                        <div class="divSubject" id="<%= ThaiId %>">
                            <div></div>
                            <div class="divSubjectImage">
                                <input type="image" src="../Images/MaxOnet/Subject/Thai.png" sid="<%= ThaiId %>" sname="ไทย" />
                            </div>
                            <span>ไทย</span>
                        </div>
                        <div class="divSubject" id="<%= MathId %>">
                            <div></div>
                            <div class="divSubjectImage">
                                <input type="image" src="../Images/MaxOnet/Subject/Math.png" sid="<%= MathId %>" sname="คณิตฯ" />
                            </div>
                            <span>คณิตฯ</span>
                        </div>
                        <div class="divSubject" id="<%= EngId %>">
                            <div></div>
                            <div class="divSubjectImage">
                                <input type="image" src="../Images/MaxOnet/Subject/Eng.png" sid="<%= EngId %>" sname="อังกฤษ" />
                            </div>
                            <span>อังกฤษ</span>
                        </div>
                        <div class="divSubject" id="<%= SocialId %>">
                            <div></div>
                            <div class="divSubjectImage">
                                <input type="image" src="../Images/MaxOnet/Subject/Social.png" sid="<%= SocialId %>" sname="สังคม" />
                            </div>
                            <span>สังคม</span>
                        </div>
                        <div class="divSubject" id="<%= ScienceId %>">
                            <div></div>
                            <div class="divSubjectImage">
                                <input type="image" src="../Images/MaxOnet/Subject/Science.png" sid="<%= ScienceId %>" sname="วิทย์ฯ" />
                            </div>
                            <span>วิทย์ฯ</span>
                        </div>
                        <div class="divSubject" id="<%= HomeId %>">
                            <div></div>
                            <div class="divSubjectImage">
                                <input type="image" src="../Images/MaxOnet/Subject/home.png" sid="<%= HomeId %>" sname="การงาน" />
                            </div>
                            <span>การงาน</span>
                        </div>
                        <div class="divSubject" id="<%= HealthId %>">
                            <div></div>
                            <div class="divSubjectImage">
                                <input type="image" src="../Images/MaxOnet/Subject/Suk.png" sid="<%= HealthId %>" sname="สุขศึกษา" />
                            </div>
                            <span>สุขศึกษา</span>
                        </div>
                        <div class="divSubject" id="<%= ArtId %>">
                            <div></div>
                            <div class="divSubjectImage">
                                <input type="image" src="../Images/MaxOnet/Subject/Art.png" sid="<%= ArtId %>" sname="ศิลปะ" />
                            </div>
                            <span>ศิลปะ</span>
                        </div>
                    </div>
                    <div class="divPanelClass" style="display: none;">
                        <span class="headspan">เลือกชั้น</span>
                        <div class="divUnderLine"></div>
                        <div class="mainChooseClass">
                            <table>
                                <tr>
                                    <td>
                                        <input type="checkbox" id="K4" value="5F4765DB-0917-470B-8E43-6D1C7B030818" lname="ป.1" name="radio" />
                                        <label for="K4"><s></s><span style="background: url('../Images/MaxOnet/Levels/K4.png');"></span>ป.1</label></td>
                                    <td>
                                        <input type="checkbox" id="K5" value="EFA0855F-7AA5-40C1-98D0-F332F1298CEE" lname="ป.2" name="radio" />
                                        <label for="K5"><s></s><span style="background: url('../Images/MaxOnet/Levels/K5.png');"></span>ป.2</label></td>
                                    <td>
                                        <input type="checkbox" id="K6" value="5CAF2A9B-B26B-4C16-9980-90BA760B5C43" lname="ป.3" name="radio" />
                                        <label for="K6"><s></s><span style="background: url('../Images/MaxOnet/Levels/K6.png');"></span>ป.3</label></td>
                                    <td>
                                        <input type="checkbox" id="K7" value="DD73B147-B098-4F1D-8144-C5FCF510AEA9" lname="ป.4" name="radio" />
                                        <label for="K7"><s></s><span style="background: url('../Images/MaxOnet/Levels/K7.png');"></span>ป.4</label></td>
                                    <td>
                                        <input type="checkbox" id="K8" value="BCBCC0C8-2A39-4AAE-9AA6-173DE86AF6AE" lname="ป.5" name="radio" />
                                        <label for="K8"><s></s><span style="background: url('../Images/MaxOnet/Levels/K8.png');"></span>ป.5</label></td>
                                    <td>
                                        <input type="checkbox" id="K9" value="93B163B6-4F87-476D-8571-4029A6F34C84" lname="ป.6" name="radio" />
                                        <label for="K9"><s></s><span style="background: url('../Images/MaxOnet/Levels/K9.png');"></span>ป.6</label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <input type="checkbox" id="K10" value="E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C" lname="ม.1" name="radio" />
                                        <label for="K10"><s></s><span style="background: url('../Images/MaxOnet/Levels/K10.png');"></span>ม.1</label></td>
                                    <td>
                                        <input type="checkbox" id="K11" value="DB95E7F8-7BF3-468D-AD9E-0AAF1B328D45" lname="ม.2" name="radio" />
                                        <label for="K11"><s></s><span style="background: url('../Images/MaxOnet/Levels/K11.png');"></span>ม.2</label></td>
                                    <td>
                                        <input type="checkbox" id="K12" value="14A28F3D-1AFF-429D-B7A1-927A28E010BD" lname="ม.3" name="radio" />
                                        <label for="K12"><s></s><span style="background: url('../Images/MaxOnet/Levels/K12.png');"></span>ม.3</label></td>
                                    <td>
                                        <input type="checkbox" id="K13" value="2E0FFC04-BCEE-45BE-9C0C-B40742523F43" lname="ม.4" name="radio" />
                                        <label for="K13"><s></s><span style="background: url('../Images/MaxOnet/Levels/K13.png');"></span>ม.4</label></td>
                                    <td>
                                        <input type="checkbox" id="K14" value="6736D029-6B78-4570-9DBB-991217DA8FEE" lname="ม.5" name="radio" />
                                        <label for="K14"><s></s><span style="background: url('../Images/MaxOnet/Levels/K14.png');"></span>ม.5</label></td>
                                    <td>
                                        <input type="checkbox" id="K15" value="6BF52DC7-314C-40ED-B7F3-BCC87F724880" lname="ม.6" name="radio" />
                                        <label for="K15"><s></s><span style="background: url('../Images/MaxOnet/Levels/K15.png');"></span>ม.6</label></td>
                                </tr>
                            </table>

                        </div>
                    </div>
                    <div class="notUsageEng" style="text-align: center; display: none;">
                        <span style="color: red; font-size: 16px;">วิชาภาษาอังกฤษ จะเข้าใช้ได้ประมาณ 15 ส.ค. 59 ค่ะ</span>
                    </div>
                    <div class="textConfirm" style="text-align: center; display: none; margin-top: 20px;">
                        <span style="color: red; font-size: 22px;">ยืนยันการเลือกชั้น และวิชานะคะ ถ้าเลือกผิดจะไม่สามารถเลือกใหม่ได้แล้วนะคะ</span>
                    </div>
                    <div style="text-align: center; margin-top: 20px;">
                        <input type="button" id="btnBeginMaxOnet" value="เริ่มใช้งาน" class="submit" style="display: none;" />
                        <input type="button" id="btnConfirmMaxOnet" value="ยืนยัน" class="submit" style="display: none;" />
                    </div>
                </div>
            </div>
        </div>
        <div class="mainblogUI">
            <span>กำลังลงทะเบียน รอสักครู่นะคะ</span>
            <div class="blogUI"></div>
        </div>
    </div>
    <div id="dialog"></div>
    <div id="dialog2" class="dialogSession"></div>
    <div id="divAddCreditMaxoent" class="addCredit" runat="server" clientidmode="Static">
        <div class="divOpacity"></div>
        <div class="divContentCredit">
            <div>
                <span class="header">เติมเครดิต เพื่อเลือกวิชาเพิ่ม</span>
                <table>
                    <tr>
                        <td>
                            <span>ชื่อผู้ใช้</span>
                        </td>
                        <td>
                            <input type="text" id="txtUserName" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>รหัสลับ</span>
                        </td>
                        <td>
                            <input type="text" id="txtPassword" />
                        </td>
                    </tr>
                </table>
                <div style="height: 42px;">
                    <input type="button" value="ยกเลิก" style="width: 100px; float: left;" onclick="onBtnBackCredit();" />
                    <input type="button" value="เพิ่มเครดิต" style="width: 150px; float: right;" onclick="onBtnAddCredit();" />
                </div>
            </div>
        </div>
    </div>
    <div id="divRegisteredDetail" class="addCredit" runat="server" clientidmode="Static">
        <div class="divOpacity"></div>
        <div class="divContentCredit" style="height: fit-content; display: flex; width: fit-content;">
            <table>
                <tr>
                    <td colspan="2">
                        <div  style="display: flex;">
                        <div id="divSub" style="background-color: #f9d0b2; width: 300px; height: 470px; position: relative; margin-right: 20px; text-align: center;border-radius:10px;padding:10px;">
                            <span style="font-weight: bold; font-size: 23px;">วิชาที่เลือกแล้ว</span>
                            <div id="SelectedSubList"></div>
                        </div>

                        <div id="divLev" style="background-color: #c6e7f0; width: 300px; position: relative; border-color: #059fca; text-align: center;border-radius:10px;padding:10px;"">
                                <span id="spSelectSubDetail" style="font-weight: bold; font-size: 23px;">ชั้นที่เลือกในวิชา : สุขศึกษา</span>
                                <div>
                                    <div id="newSelected" style="background-color: #fafeff; padding: 10px; border-radius: 6px; padding-bottom: 15px;"">
                                        <%If Request.QueryString("addsubject") IsNot Nothing Then %>
                                           <span style="font-weight: bold; font-size: 20px;">ระดับชั้นที่ลงทะเบียนเพิ่ม</span><br />
                                        <%Else %>
                                            <span style="font-weight: bold; font-size: 20px;">ระดับชั้นที่ลงทะเบียนใหม่</span><br />
                                        <%End if %>

                                        <div id="SelectedLevList"></div>
                                    </div>

                                    <%If Request.QueryString("addsubject") IsNot Nothing Then %>
                                        <div id="Registered" style="background-color: #fafeff; padding: 10px; border-radius: 6px; padding-bottom: 15px; margin-top: 10px;">
                                            <span style="font-weight: bold; font-size: 20px;">ระดับชั้นที่เคยลงทะเบียนแล้ว</span><br />
                                            <div id="RegisteredLevList"></div>
                                            <%-- <span style="font-size: 16px; color: gray;">ยังไม่มีระดับชั้นที่ลงทะเบียน</span>--%>
                                        </div>
                                    <%End if %>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="button" value="แก้ไข" style="width: 100px; float: left;" onclick="onBtnEdit();" /></td>
                    <td>
                        <input type="button" value="ยืนยัน" style="width: 100px; float: right;" onclick="SaveSubject();" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">

    <script type="text/javascript" src="../js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.18.min.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/GFB.js"></script>
    <script type="text/javascript" src="../js/CheckAndSetNowDevice.js"></script>

    <script type="text/javascript">

        var isAddSubject = "<%=Request.QueryString("addSubject")%>";
        var tokenId = "<%=TokenId%>";
        var deviceId = "<%=DeviceId%>";
        var subjects = "<%=SubjectsIdStr%>";
        var enableSelect = "<%=EnableSelect%>";
        var studentId = "<%=StudentId%>";
        var kClassName = "<%=StudentClass%>";
        var engId = "<%=EngId%>";
        var unitAmount = "<%=CreditAmount%>";

        var subjectClassSelected = [];


        var subjectId;
        var baseURL = "<%=ResolveUrl("~")%>";

        $(document).ready(function () {

        });
        var result = '<%=SubjectRegisteredJson%>';

        var subjectRegistered = result == "" ? null : JSON.parse(result);
        var AllsubjectClass = result == "" ? null : JSON.parse(result);
        var listSubjectClass;

        $(function () {
            $('#' + kClassName).attr("checked", "checked");
            $('.divSubject').css("display", "none");

            subjects = subjects.split(',');
            for (var i = 0; i < subjects.length  ; i++) {
                $('#' + subjects[i]).css("display", "inline-block");
            }

            setTxtUnitAmount();

            $('.divSubject').each(function () {
                new FastButton(this, TriggerSelectSubject);
            });


            $('input[type="checkbox"]').click(function () {
                RemoveConfirm();
                var classId = $(this).attr('value');
                var checked = $(this).attr('checked');
                
                var isExist = false;

                if (unitAmount == 0 && checked != undefined) {
                    callAlertDialog('เลือกจำนวนเครดิตครบแล้วค่ะ');
                    return false;
                }

                for (var i = 0; i < subjectClassSelected.length; i++) {
                    if (subjectId == subjectClassSelected[i].sId) {
                        isExist = true;
                        if (checked == undefined) {
                            var tempIndex = -1;
                            $.each(subjectClassSelected[i].value, function (j, item) {
                                if (item == classId) { tempIndex = j; }
                            });
                            subjectClassSelected[i].value.splice(tempIndex, 1);
                            console.log('splice');

                            if (subjectClassSelected[i].value.length == 0) {
                                $('#' + subjectId).removeClass("divSubjectSelected");
                                $('#' + subjectId).children().first().removeClass("divSelectedImage");
                            }
                            unitAmount += 1;
                        } else {
                            subjectClassSelected[i].value.push(classId);
                            console.log('pushclassId');
                            $('#' + subjectId).addClass("divSubjectSelected");
                            $('#' + subjectId).children().first().addClass("divSelectedImage");
                            unitAmount -= 1;
                        }
                        break;
                    }
                }

                if (!isExist) {

                    unitAmount -= 1;
                    subjectClassSelected.push({ sId: subjectId, value: [classId] });
                    console.log('pushsubjectIdclassId');
                    $('#' + subjectId).addClass("divSubjectSelected");
                    $('#' + subjectId).children().first().addClass("divSelectedImage");
                }
                /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

                isExist = false;

                if (AllsubjectClass == null) {
                    AllsubjectClass = [];
                }
                    for (var a = 0; a < AllsubjectClass.length; a++) {

                        if (subjectId == AllsubjectClass[a].SubjectId) {
                            isExist = true;
                            console.log(checked);
                            if (checked == undefined) {
                                var tempIndex = -1;

                                $.each(AllsubjectClass[a].ClassId, function (j, item) {
                                    if (item == classId) { tempIndex = j; }
                                });

                                AllsubjectClass[a].ClassId.splice(tempIndex, 1);
                                AllsubjectClass[a].Registered.splice(tempIndex, 1);
                                console.log('AllsubjectClassSplice');

                            } else {
                                AllsubjectClass[a].ClassId.push(classId);
                                AllsubjectClass[a].Registered.push(0);
                                console.log('AllsubjectClassPushclassId');
                            }
                            break;
                        }
                    }

                if (!isExist) {

                    AllsubjectClass.push({ SubjectId: subjectId, ClassId: [classId], Registered: [0] });
                    console.log('AllsubjectClassPushsubjectIdclassId');

                }
                /*----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

                setTxtUnitAmount();
                if (getUnitSelectedAmount() > 0) {
                    $('#btnBeginMaxOnet').show();
                } else {
                    $('#btnBeginMaxOnet').hide();
                }
            });

            $('#btnBeginMaxOnet').click(function () {

                if (getUnitSelectedAmount() == 0) {
                    callAlertDialog("ต้องเลือกวิชาชั้นอย่างน้อย 1 เครดิต ก่อนค่ะ");
                    return 0;
                }

                // แสดงส่วนให้กด ยืนยันอีกครั้ง
                $('input[type="radio"]:checked').next().addClass("confirm");

                for (var i = 0; i < AllsubjectClass.length; i++) {
                    var sname = $(".divSubjectImage input[sid='" + AllsubjectClass[i].SubjectId.toUpperCase() + "']").attr("sname");
                    var newdivSub = $("<input type='button' class='btnSub' value='" + sname + " (" + AllsubjectClass[i].ClassId.length + ")' onclick='CreatelistLevel(" + i + ");' />");
                    $('#SelectedSubList').append(newdivSub);
                    if (i == 0) {
                        CreatelistLevel(i)
                    }
                }

                $('#divRegisteredDetail').show();

            });

            $('input[type="radio"]').change(function () {
                //chooseSound.play();
                RemoveConfirm();

                // สำหรับวิชาอังกฤษที่ยังไม่มีข้อสอบ
                $('.notUsageEng').hide();
                $('#' + engId).removeClass('subjectNotUsage');
                var classId = $(this).attr('id');
                if (classId == 'K13' || classId == 'K14' || classId == 'K15') {
                    if (subjects.length == 8) {
                        $('#' + engId).removeClass("divSubjectSelected");
                        $('#' + engId).children().first().removeClass("divSelectedImage");
                    }
                    $('.notUsageEng').show();
                    $('#' + engId).addClass('subjectNotUsage');
                }
            });

            $('#btnConfirmMaxOnet').click(function () {
                openBlockUI();
                $('#deviceEnable').slideUp(1000);
                console.log('beforesave');
                saveMultiSubjectClass();
            });

            $('.addSubject').click(function () {
                $('#divAddCreditMaxoent').show();
            });

            $('#btnMore').click(function () {

                for (var i = 0; i < AllsubjectClass.length; i++) {
                    var sname = $(".divSubjectImage input[sid='" + AllsubjectClass[i].SubjectId.toUpperCase() + "']").attr("sname");
                    var newdivSub = $("<input type='button' class='btnSub' value='" + sname + " (" + AllsubjectClass[i].ClassId.length + ")' onclick='CreatelistLevel(" + i + ");' />");
                    $('#SelectedSubList').append(newdivSub);
                    if (i == 0) {
                        CreatelistLevel(i)
                    }
                }

                $('#divRegisteredDetail').show();
            });

            $('.backToMaxonet').click(function () {
                window.location = "choosetestsetmaxonet.aspx?deviceuniqueid=" + deviceId + "&token=" + tokenId + "&IsNewRegister=True";
            });
        });

        function CreatelistLevel(sindex) {

            console.log(AllsubjectClass);

            $('#SelectedLevList').empty();
            $('#RegisteredLevList').empty();

            var sname = $(".divSubjectImage input[sid='" + AllsubjectClass[sindex].SubjectId + "']").attr("sname");
            $('#spSelectSubDetail').html('ชั้นที่เลือกในวิชา : ' + sname);

            for (var i = 0; i < AllsubjectClass[sindex].ClassId.length; i++) {
                var lname = $(".mainChooseClass input[value='" + AllsubjectClass[sindex].ClassId[i] + "']").attr("lname");
                var newdivLev = $("<input type='button' value='" + lname + "' style='width: 70px; margin-top: 10px;margin-right: 10px;' />");

                var isre = AllsubjectClass[sindex].Registered[i]
                console.log('isre' + isre);
                if (isre == 0) {
                    $('#SelectedLevList').append(newdivLev);
                } else {
                    $('#RegisteredLevList').append(newdivLev);
                }
            }

            var newEmpdiv;

            if ($('#SelectedLevList').html() == '') {
                newEmpdiv = '<span style="font-size: 16px; color: gray;">ยังไม่มีระดับชั้นที่ลงทะเบียนเพิ่ม</span>'
                $('#SelectedLevList').append(newEmpdiv);
            }

            if ($('#RegisteredLevList').html() == '') {
                newEmpdiv = '<span style="font-size: 16px; color: gray;">ยังไม่เคยลงทะเบียนวิชานี้</span>'
                $('#RegisteredLevList').append(newEmpdiv);
            }

        }

        function SaveSubject() {
            openBlockUI();
            $('#deviceEnable').slideUp(1000);
            console.log('beforesave');
            saveMultiSubjectClass();
        }


        function TriggerConfirmMaxOnet() {
            console.log(TriggerConfirmMaxOnet);
            openBlockUI();
            $('#deviceEnable').slideUp(1000);
            console.log('beforesave');
            saveMultiSubjectClass();
        }

        function TriggerSelectSubject(e) {
            var obj = e.target || e.srcElement;
            var objParent = $(obj).parent();
            var sid = $(obj).attr('sid');
            console.log(sid);

            if (typeof sid !== "undefined") {

                $('.divPanelClass').show();
                $('.divSubjectImage').removeClass('subjectSelected');

                $(objParent).addClass('subjectSelected');
                $('.mainChooseSubject').removeClass('subjectSelected');

                clearCheckbox();

                subjectId = $(obj).attr('sid');

                var sid = $(obj).attr('sid');

                //disable checkbox ที่เคย เลือกไปแล้ว
                if (subjectRegistered != null) {
                    $.each(subjectRegistered, function (i, item) {
                        if (item.SubjectId.toUpperCase() == subjectId) {
                            $.each(item.ClassId, function (j, jtem) {
                                $('input[value="' + jtem.toUpperCase() + '"]').attr('disabled', true);
                            });
                        }
                    });
                }

                for (var i = 0; i < subjectClassSelected.length; i++) {
                    if (subjectId == subjectClassSelected[i].sId) {
                        for (var j = 0; j < subjectClassSelected[i].value.length; j++) {
                            $('input[value="' + subjectClassSelected[i].value[j] + '"]').attr('checked', true);
                        }
                        break;
                    }
                }
            }
        }

        function setTxtUnitAmount() {
            $('.txtUnitAmount').text('จำนวนเครดิต(ชั้นวิชา) ที่สามารถเลือกได้ ' + unitAmount + ' เครดิต');
        }

        function clearCheckbox() {
            $('input[type="checkbox"]:checked').each(function () {
                $(this).attr('checked', false);
            });
            $('input[type="checkbox"]:disabled').each(function () {
                $(this).removeAttr('disabled');
            });
        }

        function isLimitUnitAmount() {
            var amount = getUnitSelectedAmount();
            if (amount == unitAmount) {
                return true;
            }
            return false;
        }

        function getUnitSelectedAmount() {
            var amount = 0;
            $.each(subjectClassSelected, function (i, item) {
                amount = amount + item.value.length;
            });
            return amount;
        }

        function getSubjectSelected() {
            var listSubjectClass = [];
            $.each(subjectClassSelected, function (i, item) {
                if (item.value.length > 0) {
                    listSubjectClass.push(item);
                }
            });
            return listSubjectClass;
        }

        function callAlertDialog(titleName) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                $d.dialog('close');
            };
            $d.html('');
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleName);
        }

        function callConfirmDialog(titleName) {
            var $d = $('#dialog');
            var myBtn = {};
            myBtn["ตกลง"] = function () {
                window.location = "choosetestsetmaxonet.aspx?deviceUniqueId=" + deviceId + "&token=" + tokenId + "&IsNewRegister=False";
            };
            $d.html('');
            $d.dialog({ buttons: myBtn, draggable: false, resizable: false, modal: true }).dialog('option', 'title', titleName);
        }

        function RemoveConfirm() {
            $('input[type="radio"]').next().removeClass("confirm");
            $('.mainChooseSubject').removeClass("confirm");
            $('.textConfirm').hide();
            $('#btnBeginMaxOnet').show();
            $('#btnConfirmMaxOnet').hide();
        }

        function onBtnBackCredit() {
            console.log(isAddSubject);
            if (isAddSubject != "") {
                window.location = "choosetestsetmaxonet.aspx?deviceuniqueid=" + deviceId + "&token=" + tokenId + "&IsNewRegister=True";
            } else {
                $('#divAddCreditMaxoent').hide();
            }
        }

        function onBtnEdit() {
            $('#SelectedSubList').empty();
            $('#SelectedLevList').empty();
            $('#divRegisteredDetail').hide();
        }

        function onBtnAddCredit() {
            // check ว่ากรอกครบไหม
            var userName = $('#txtUserName').val();
            var pwd = $('#txtPassword').val();
            if (userName == "") {
                callAlertDialog("กรอกชื่อผู้ใช้ก่อนค่ะ");
                return 0;
            }
            if (pwd == "") {
                callAlertDialog("กรอกรหัสผ่านก่อนค่ะ");
                return 0;
            }

            var AddCreditResult = addCredit(userName, pwd);

            if (AddCreditResult == 1) {
                callConfirmDialog("เพิ่ม credit เรียบร้อยแล้วค่ะ");

            } else if (AddCreditResult == -1) {
                callAlertDialog("ชื่อผู้ใช้และรหัสผ่านไม่ถูกต้องค่ะ");
                return 0;
            } else if (AddCreditResult == -2) {
                callAlertDialog("รหัสนี้ใช้งานไปแล้วค่ะ");
                return 0;
            } else if (AddCreditResult == -3) {
                callAlertDialog("รหัสนี้หมดอายุแล้วค่ะ");
                return 0;
            }
            else if (AddCreditResult == -4) {
                callAlertDialog("มีข้อผิดพลาดในการลงทะเบียนค่ะ");
                return 0;
            }
        }

        function saveStudentSubject(studentClass, subjectLists) {

            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/MaxOnetService.asmx/RegisterSubjectMaxOnet",
                async: false,
                data: "{ studentId :  '" + studentId + "',studentClass :  '" + studentClass + "',subjectsId :  '" + subjectLists + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d === true) {
                        window.location = "choosetestsetmaxonet.aspx?deviceuniqueid=" + deviceId + "&token=" + tokenId + "&IsNewRegister=True";
                    }
                },
                error: function myfunction(request, status) {

                }
            });
        }


        function saveMultiSubjectClass() {
            console.log('saveMultiSubjectClass');
            var listSubjectClass = getSubjectSelected();
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/MaxOnetService.asmx/RegisterMultiSubjectClassMaxonet",
                async: false,
                data: "{ studentId :  '" + studentId + "',subjectClasslist :  '" + JSON.stringify(listSubjectClass) + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    if (msg.d === true) {
                        window.location = "choosetestsetmaxonet.aspx?deviceuniqueid=" + deviceId + "&token=" + tokenId + "&IsNewRegister=True";
                    }

                },
                error: function myfunction(request, status) {

                }
            });
        }

        function addCredit(userName, password) {

            var returnValue;
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>WebServices/MaxOnetService.asmx/AddCreditMaxonet",
                async: false,
                data: "{studentId : '" + studentId + "',tokenId : '" + tokenId + "',deviceId : '" + deviceId + "', userName :  '" + userName + "',password :  '" + password + "'}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (msg) {
                    returnValue = msg.d;
                },
                error: function myfunction(request, status) {

                }
            });
            return returnValue;
        }



    </script>

</asp:Content>
