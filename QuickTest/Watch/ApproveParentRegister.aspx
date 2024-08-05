<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ApproveParentRegister.aspx.vb" Inherits="QuickTest.ApproveParentRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>

    <script type="text/javascript">


        function ApproveRequest(InputId) {
            $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Watch/ApproveParentRegister.aspx/ApproveParentRegister",
                data: "{ RPADID: '" + InputId + "' }",
                 contentType: "application/json; charset=utf-8", dataType: "json",
                 success: function (msg) {
                     if (msg.d !== '-1') {
                         $('#' + InputId).hide();
                     }
                 },
                 error: function myfunction(request, status) {
                     //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                 }
             });
        }

        function NotApproveRequest(InputId) {
                $.ajax({
                type: "POST",
                url: "<%=ResolveUrl("~")%>Watch/ApproveParentRegister.aspx/NotApproveParentRegister",
                data: "{ RPADID: '" + InputId + "' }",
                 contentType: "application/json; charset=utf-8", dataType: "json",
                 success: function (msg) {
                     if (msg.d !== '-1') {
                         $('#' + InputId).hide();
                     }
                 },
                 error: function myfunction(request, status) {
                     //alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
                 }
             });
        }
        
    </script>


    <style type="text/css">

        #MainDiv {
        width:100%;
        }
        .DivPic {
       width:20%;
       text-align:center;
       
        }
        .DivInFo {
        width:54%;
        border-left:1px solid;
        border-right:1px solid;
        }
        .DivApprove {
        text-align:center;width:25%; 
        }
        .ForDiv {
        display:inline-block;
        }
        .ForParentInfo {
        border-bottom:1px solid;text-align:center;padding:15px;
        }
        .ForStudentInfo {
        text-align:center;
        }
        .ForDivRequest {
        width:95%;
        margin-left:auto;
        margin-right:auto;
        border:1px solid;
        }
        img {
        cursor:pointer;
        }
        span {
        font-size:30px;
        margin-left:10px;
        margin-right:10px;
        }
        .imgApprove {
        position:relative;
        top:-25px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">

    <div id="MainDiv" runat="server">
    
  <%--      <div id="1"  class="ForDivRequest">

            <div id="DivPic" class="ForDiv DivPic" >
                <img src="../Images/Student.png" style="width:150px;" />
            </div>

            <div id="DivInfo" class="ForDiv DivInFo" >

                
                            <div id="ParentInFo" class="ForParentInfo" >
                                <span>ผู้ปกครอง</span>
                                <br />
                    <span id="spnParentName">สมรัก พรรคเพื่อเก้ง</span>
                    <span id="spnRequestDate">15/5/2553 12:50</span>
                </div>
                       
                            <div id="StudentInfo" class="ForStudentInfo" >
                                <span>นักเรียน</span>
                                <br />
                    <span id="spnStudentName">ลูกสมรัก พรรคเพื่อเก้ง</span>
                                <br />
                    <span id="spnStudentClassRoom">ป.3/1</span>
                    <span id="spnStudentCurrentNoInroom">เลขที่ 25</span>
                </div>
                     

            </div>

            <div id="DivApprove" class="ForDiv DivApprove" >
                <div>
                <img src="../Images/Right Tick/check.jpeg" class="imgApprove" style="width:50px;" onclick="ApproveRequest('1')" />
                    </div>
                <div>
                <img src="../Images/Close.png" onclick="NotApproveRequest('1')" />
                    </div>
            </div>

        </div>


         <div id="2" class="ForDivRequest">

            <div id="Div2" class="ForDiv DivPic" >
                <img src="../Images/Student.png" style="width:150px;" />
            </div>

            <div id="Div3" class="ForDiv DivInFo" >

                
                            <div id="Div4" class="ForParentInfo" >
                                <span>ผู้ปกครอง</span>
                                <br />
                    <span id="Span1">สมรัก พรรคเพื่อเก้ง2</span>
                    <span id="Span2">15/5/2553 12:50</span>
                </div>
                       
                            <div id="Div5" class="ForStudentInfo" >
                                <span>นักเรียน</span>
                                <br />
                    <span id="Span3">ลูกสมรัก พรรคเพื่อเก้ง</span>
                                <br />
                    <span id="Span4">ป.3/1</span>
                    <span id="Span5">เลขที่ 25</span>
                </div>
                     

            </div>

            <div id="Div6" class="ForDiv DivApprove" >
                <div>
                <img src="../Images/Right Tick/check.jpeg" class="imgApprove" style="width:50px;" onclick="ApproveRequest('2')" />
                    </div>
                <div>
                <img src="../Images/Close.png" onclick="NotApproveRequest('2')" />
                    </div>
            </div>

        </div>


         <div id="3" class="ForDivRequest">

            <div id="Div8" class="ForDiv DivPic" >
                <img src="../Images/Student.png" style="width:150px;" />
            </div>

            <div id="Div9" class="ForDiv DivInFo" >

                
                            <div id="Div10" class="ForParentInfo" >
                                <span>ผู้ปกครอง</span>
                                <br />
                    <span id="Span6">สมรัก พรรคเพื่อเก้ง3</span>
                    <span id="Span7">15/5/2553 12:50</span>
                </div>
                       
                            <div id="Div11" class="ForStudentInfo" >
                                <span>นักเรียน</span>
                                <br />
                    <span id="Span8">ลูกสมรัก พรรคเพื่อเก้ง</span>
                                <br />
                    <span id="Span9">ป.3/1</span>
                    <span id="Span10">เลขที่ 25</span>
                </div>
                     

            </div>

            <div id="Div12" class="ForDiv DivApprove" >
                <div>
                <img src="../Images/Right Tick/check.jpeg" class="imgApprove" style="width:50px;" onclick="ApproveRequest('3')" />
                    </div>
                <div>
                <img src="../Images/Close.png" onclick="NotApproveRequest('3')" />
                    </div>
            </div>

        </div>--%>



    </div>

    </form>
</body>
</html>
