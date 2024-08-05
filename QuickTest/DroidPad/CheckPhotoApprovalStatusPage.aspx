<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CheckPhotoApprovalStatusPage.aspx.vb" Inherits="QuickTest.CheckPhotoApprovalStatusPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js"></script>
    <script type="text/javascript">

        $(function () {

            $('#btnCancel').click(function () {
                parent.CloseFancyBox();
            });

            $('.Approve').click(function () {
                var id = $(this).attr('stdPtId');
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>DroidPad/CheckPhotoApprovalStatusPage.aspx/ApprovePhoto",
                    data: "{ StudentPhotoId: '" + id + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            if (data.d == 'Complete') {
                                $('#' + id).hide(500);
                            }
                        }
                       },
                       error: function myfunction(request, status) {
                           //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');	                
                       }
                   });
            });

            $('.NotApprove').click(function () {
                var id = $(this).attr('stdPtId');
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>DroidPad/CheckPhotoApprovalStatusPage.aspx/NotApprovePhoto",
                    data: "{ StudentPhotoId: '" + id + "'}",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            if (data.d == 'Complete') {
                                $('#' + id).hide(500);
                            }
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');	                
                    }
                  });
            });

            $('#btnApproveAll').click(function () {
                $.ajax({
                    type: "POST",
                    url: "<%=ResolveUrl("~")%>DroidPad/CheckPhotoApprovalStatusPage.aspx/ApproveAll",
                    contentType: "application/json; charset=utf-8", dataType: "json",
                    success: function (data) {
                        if (data.d != 0) {//session(UserId) หายจะ return 0 - Sefety function
                            if (data.d == 'Complete') {
                                alert('อนุมัติทั้งหมดเรียบร้อยแล้วค่ะ');
                                parent.CloseFancyBox();
                            }
                        }
                    },
                    error: function myfunction(request, status) {
                        //alert('ส่งข้อมูลไป CodeBehind ไม่ได้');	                
                    }
                });
              });

        });

    </script>

    <style type="text/css">

        #MainDiv {
        width:970px;
        height:680px;
        background-color:#f36523;
        margin-left:auto;
        margin-right:auto;
        }
        .frame {
            width:335px;
            height:215px;
            background-image:url('../Images/FrameStudent.png');
            background-size:cover;
            padding:25px;
            margin-left:62px;
            margin-top:20px;
            background-color:white;
            display:inline-block;
            vertical-align:top;
        }
        .SpnName {
        font-weight:bold;
        font-size:30px;
        }
        .SpnDetail {
        font-size:20px;
        }
        .SpnParent {
        font-weight:bold;
        font-size:25px;
        color:red;
        }
        .Top {
        height:150px;
        }
        .Approve {
        float:right;
        margin-top:10px;
        margin-right:10px;
        cursor:pointer;
        }
        .NotApprove {
        float:left;
        margin-top:10px;
        margin-left:10px;
        cursor:pointer;
        }

        .ForBtn {
            
            background-color: #F68500;
            list-style: none;
            padding: 0;
            text-shadow: 1px 1px #7E4D0E;
            -webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            -moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
            box-shadow: 0 1px 2px rgba(0,0,0,.2);
            color: #FFF;
            border: solid 1px #DA7C0C;
            background: #F78D1D;
            background: -webkit-gradient(linear, left top, left bottom, from(#FAA51A), to(#F47A20));
            background: -moz-linear-gradient(top,  #FAA51A,  #F47A20);
            -webkit-border-radius: 0.5em;
            -moz-border-radius: 0.5em;
            border-radius: 0.5em;
            width:160px;
            font-size:25px;
            height:60px;
            cursor:pointer;
        }

        #DivBtn {
        margin-left:auto;
        margin-right:auto;
        width:960px;
        text-align:center;
        margin-top:10px;
        }
        .Photo {
        margin-left:80px;
        width:160px;
        height:105px;
        margin-top:10px;
        }

    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div id="MainDiv">
            <div id="DivListApprove" style="height:590px;overflow:auto;" runat="server">

            <%--<div id="6F2D0501-9B9C-4D32-9A27-5EC5D51EC76D" class="frame">
                <div class="Top">
                    <span class="SpnName">ดช.สมรัก นามสกุลยาวๆๆ</span>
                    <br />
                    <span class="SpnDetail">เลขที่ 12 ป.5/99</span>
                    <br />
                    <img class="Photo" src="../UserData/1000001/{0c30703c-93e4-45f3-a0bc-9c45401a740c}/Id.png" />
                </div>
                <div class="Bottom">
                    <img class="Approve" stdId="6F2D0501-9B9C-4D32-9A27-5EC5D51EC76D" src="../Images/ApproveButton.png" />
                    <img class="NotApprove" stdId="6F2D0501-9B9C-4D32-9A27-5EC5D51EC76D" src="../Images/NotApproveButton.png" />
                </div>
            </div>
                     
            <div id="123" class="frame">
                <div class="Top">
                    <span class="SpnParent">แจ้งเป็นผู้ปกครอง</span>
                    <span style="margin-left:5px;">เลขที่ 12 ป.5/99</span>
                    <br />
                    <span class="SpnDetail">ดช.สมรัก นามสกุลยาวๆๆ</span>
                    <br />
                    <span style="font-size:18px;">โดย คุณแม่สมรัก นามสกุลยาวมากๆๆๆ</span>
                    <br />
                    <img class="Photo" src="../UserData/1000001/{0c30703c-93e4-45f3-a0bc-9c45401a740c}/Id.png" />
                </div>
                <div class="Bottom">
                     <img class="Approve" stdId="123" src="../Images/ApproveButton.png" />
                    <img class="NotApprove" stdId="123" src="../Images/NotApproveButton.png" />
                </div>
            </div>--%>
                      
            <%--<div id="124" class="frame">
                <div class="Top">
                    <span class="SpnName">ดช.สมรัก นามสกุลยาวๆๆ</span>
                    <br />
                    <span>เลขที่ 12 ป.5/99</span>
                </div>
                <div class="Bottom">
                    <img class="Approve" stdId="124" src="../Images/ApproveButton.png" />
                    <img class="NotApprove" stdId="124" src="../Images/NotApproveButton.png" />
                </div>
            </div>
                       
            <div class="frame">
                <div class="Top">
                    <span>ดช.สมรัก นามสกุลยาวๆๆ</span>
                    <br />
                    <span>เลขที่ 12 ป.5/99</span>
                </div>
                <div class="Bottom">
                     <img class="Approve" src="../Images/ApproveButton.png" />
                    <img class="NotApprove" src="../Images/NotApproveButton.png" />
                </div>
            </div>
                         
            <div class="frame">
                <div class="Top">
                    <span class="SpnName">ดช.สมรัก นามสกุลยาวๆๆ</span>
                    <br />
                    <span>เลขที่ 12 ป.5/99</span>
                </div>
                <div class="Bottom">
                    <img class="Approve" src="../Images/ApproveButton.png" />
                    <img class="NotApprove" src="../Images/NotApproveButton.png" />
                </div>
            </div>
                      
            <div class="frame">
                <div class="Top">
                    <span>ดช.สมรัก นามสกุลยาวๆๆ</span>
                    <br />
                    <span>เลขที่ 12 ป.5/99</span>
                </div>
                <div class="Bottom">
                     <img class="Approve" src="../Images/ApproveButton.png" />
                    <img class="NotApprove" src="../Images/NotApproveButton.png" />
                </div>
            </div>--%>
                </div>

            <div id="DivBtn">
                <input type="button" class="ForBtn" id="btnCancel" value="พักไว้ก่อน" />
                <input type="button" style="margin-left:95px;" class="ForBtn" id="btnApproveAll" value="อนุมัติทั้งหมด" />
            </div>

        </div>
    </form>
</body>
</html>
