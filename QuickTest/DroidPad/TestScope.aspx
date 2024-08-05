<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestScope.aspx.vb" Inherits="QuickTest.TestScope" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript">
    $(function(){

   //------------------------------------------------- Method RegisterWithSchool
     $('#Button1').click(function(){
       var DeviceId = $('#Text1').val();
     var SchoolID = $('#Text2').val();
     var SchoolPass = $('#Text3').val();

     $.ajax({ type: "POST",
	            url: "<%=ResolveUrl("~")%>DroidPad/install.aspx/RegisterWithSchool",
                //url: "<%=ResolveUrl("~")%>DroidPad/StudentAction.aspx/NextAction",
	            data: "{ DeviceUniqueID: '" + DeviceId + "', SchoolID: '" + SchoolID + "',SchoolPassword: '" + SchoolPass + "'}",  //" 
	            contentType: "application/json; charset=utf-8", dataType: "json",   
	            success: function (data) {
                  alert(data.d);

                    //$('#Button2').css('display','block');
	            },
	            error: function myfunction(request, status)  {
                alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                //alert('shin' + request.statusText + status);    
	            }
	        });
     });
     //---------------------------------------------------------------------------- Method MoveToNewSchool
     $('#Button2').click(function(){
                      var DeviceId2 = $('#Text1').val();
                      var SchoolID2 = $('#Text2').val();
                      var SchoolPass2 = $('#Text3').val();
                        $.ajax({ type: "POST",
	                        url: "<%=ResolveUrl("~")%>DroidPad/install.aspx/MoveToNewSchool",
	                        data: "{DeviceUniqueID: '" + DeviceId2 + "', SchoolID: '" + SchoolID2 + "',SchoolPassword: '" + SchoolPass2 + "'}",  //" 
	                        contentType: "application/json; charset=utf-8", dataType: "json",   
            	            success: function (data) {
                                alert(data.d);
	                        },
	                        error: function myfunction(request, status)  {
                            alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                        }
	                       });
                    });
        //--------------------------------------------- Method RegisterTeacher
        $('#Button3').click(function(){
        var DeviceId3 = $('#Text4').val();
        var TeacherFirstName = $('#Text5').val();
        var TeacherLastName = $('#Text6').val();
        var TeacherClass = $('#Text7').val();
        var TeacherRoom = $('#Text8').val();
        var TeacherSubject = $('#Text9').val();
          $.ajax({ type: "POST",
	                        url: "<%=ResolveUrl("~")%>DroidPad/install.aspx/RegisterTeacher",
	                        data: "{DeviceUniqueID: '" + DeviceId3 + "', FirstName: '" + TeacherFirstName + "',LastName: '" + TeacherLastName + "',TeacherClass:'" + TeacherClass + "',Room: '" + TeacherRoom + "',Subject: '" + TeacherSubject + "'}",  //" 
	                        contentType: "application/json; charset=utf-8", dataType: "json",   
            	            success: function (data) {
                                alert(data.d);
	                        },
	                        error: function myfunction(request, status)  {
                            alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                        }
	                       });
        });

        //------------------------------------------------- Method MoveToNewTeacher
        $('#Button4').click(function(){
        var DeviceId4 = $('#Text4').val();
        var TeacherFirstName2 = $('#Text5').val();
        var TeacherLastName2 = $('#Text6').val();
        var TeacherClass2 = $('#Text7').val();
        var TeacherRoom2 = $('#Text8').val();
        var TeacherSubject2 = $('#Text9').val();
         $.ajax({ type: "POST",
	                        url: "<%=ResolveUrl("~")%>DroidPad/install.aspx/MoveToNewTeacher",
	                        data: "{DeviceUniqueID: '" + DeviceId4 + "', FirstName: '" + TeacherFirstName2 + "',LastName: '" + TeacherLastName2 + "',TeacherClass:'" + TeacherClass2 + "',Room: '" + TeacherRoom2 + "',Subject: '" + TeacherSubject2 + "'}",  //" 
	                        contentType: "application/json; charset=utf-8", dataType: "json",   
            	            success: function (data) {
                                alert(data.d);
	                        },
	                        error: function myfunction(request, status)  {
                            alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                        }
	                       });

        });

        //------------------------------------------------------- Method RegisterStudent

        $('#Button5').click(function(){
        var DeviceId5 = $('#Text10').val();
        var StudentFirstName = $('#Text11').val();
        var StudentLastName = $('#Text12').val();
        var StudentClass = $('#Text13').val();
        var StudentRoom = $('#Text14').val();
        var StudentCode = $('#Text15').val();
        var StudentNumberInRoom = $('#Text16').val();
         $.ajax({ type: "POST",
	                        url: "<%=ResolveUrl("~")%>DroidPad/install.aspx/RegisterStudent",
	                        data: "{DeviceUniqueID: '" + DeviceId5 + "', FirstName: '" + StudentFirstName + "',LastName: '" + StudentLastName + "',StudentClass:'" + StudentClass + "',Room: '" + StudentRoom + "',StudentCode: '" + StudentCode + "',NumberInRoom: '" + StudentNumberInRoom + "'}",  //" 
	                        contentType: "application/json; charset=utf-8", dataType: "json",   
            	            success: function (data) {
                                alert(data.d);
	                        },
	                        error: function myfunction(request, status)  {
                            alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                        }
	                       });
        });

        //------------------------------------------------------- Method MoveToNewStudent
             
               $('#Button6').click(function(){
        var DeviceId6 = $('#Text10').val();
        var StudentFirstName2 = $('#Text11').val();
        var StudentLastName2 = $('#Text12').val();
        var StudentClass2 = $('#Text13').val();
        var StudentRoom2 = $('#Text14').val();
        var StudentCode2 = $('#Text15').val();
        var StudentNumberInRoom2 = $('#Text16').val();
         $.ajax({ type: "POST",
	                        url: "<%=ResolveUrl("~")%>DroidPad/install.aspx/MoveToNewStudent",
	                        data: "{DeviceUniqueID: '" + DeviceId6 + "', FirstName: '" + StudentFirstName2 + "',LastName: '" + StudentLastName2 + "',StudentClass:'" + StudentClass2 + "',Room: '" + StudentRoom2 + "',StudentCode: '" + StudentCode2 + "',NumberInRoom: '" + StudentNumberInRoom2 + "'}",  //" 
	                        contentType: "application/json; charset=utf-8", dataType: "json",   
            	            success: function (data) {
                                alert(data.d);
	                        },
	                        error: function myfunction(request, status)  {
                            alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                        }
	                       });
        });

        $('#BtnMute').click(function(){
        var DeviceId = $('#txtUnique').val();
        var Mute = $('#txtMute').val();
             $.ajax({ type: "POST",
	                        url: "<%=ResolveUrl("~")%>DroidPad/TeacherAction.aspx/MuteAll",
	                        data: "{DeviceUniqueID: '" + DeviceId + "', NeedMute: '" + Mute + "'}",  //" 
	                        contentType: "application/json; charset=utf-8", dataType: "json",   
            	            success: function (data) {
                                alert(data.d);
	                        },
	                        error: function myfunction(request, status)  {
                            alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                        }
	                       });
        });

         $('#BtnHide').click(function(){
           var DeviceId = $('#txtUnique').val();
        var Hide = $('#txtHide').val();
             $.ajax({ type: "POST",
	                        url: "<%=ResolveUrl("~")%>DroidPad/TeacherAction.aspx/HideAll",
	                        data: "{DeviceUniqueID: '" + DeviceId + "', NeedHide: '" + Hide + "'}",  //" 
	                        contentType: "application/json; charset=utf-8", dataType: "json",   
            	            success: function (data) {
                                alert(data.d);
	                        },
	                        error: function myfunction(request, status)  {
                            alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                        }
	                       });
        });


             $('#BtnLock').click(function(){
             var DeviceId = $('#txtUnique').val();
        var Lock = $('#txtLock').val();
             $.ajax({ type: "POST",
	                        url: "<%=ResolveUrl("~")%>DroidPad/TeacherAction.aspx/LockAll",
	                        data: "{DeviceUniqueID: '" + DeviceId + "', NeedLock: '" + Lock + "'}",  //" 
	                        contentType: "application/json; charset=utf-8", dataType: "json",   
            	            success: function (data) {
                                alert(data.d);
	                        },
	                        error: function myfunction(request, status)  {
                            alert('ส่งข้อมูลไป CodeBehind ไม่ได้');
	                        }
	                       });
        });





    });
       
    </script>




</head>
<body>
    <form id="form1" runat="server">
    <div style='background-color:Red;'>
        DeviceId<input id="Text1" type="text" value='' />
    SchoolId<input id="Text2" type="text"  value=''/>
      SchoolPass<input id="Text3" type="text"  />
        <input id="Button1" type="button" value="LogIn" />

        <input type="button" id='Button2' value='ย้ายโรงเรียน' style='display:block;' />
    </div>
    <hr />
    <br />
    <div style='background-color:Red;'>
    DeViceId<input type="text" id='Text4' />
    FirstName<input type="text" id='Text5' />
    LastName<input type="text" id='Text6' />
    <br />
    Class<input type="text" id='Text7' />
    Room<input type="text" id='Text8' />
    Subject<input type="text" id='Text9' />
    <input type="button" id='Button3'  value='ok'/>
    <br />
    <input type="button" id='Button4' value='ย้ายครู' />
    </div>
    <hr />
    <br />
     <div>
    DeViceId<input type="text" id='Text10' />
    FirstName<input type="text" id='Text11' />
    LastName<input type="text" id='Text12' />
    <br />
    StudentClass<input type="text" id='Text13' />
    Room<input type="text" id='Text14' />
    StudentCode<input type="text" id='Text15' />
    NumberInRoom<input type="text" id='Text16' />
    <input type="button" id='Button5'  value='ok'/>
    <br />
    <input type="button" id='Button6' value='ย้ายนักเรียน' />
    </div>
    <hr />
    <div>
    <input type="text" id='txtUnique' />
    <br />
     <input type="text" id='txtMute' />
    <input type="button" id='BtnMute' value='Mute' />
   <br />
       <input type="text" id='txtHide' />
    <input type="button" id='BtnHide' value='Hide' />
    <br />
     <input type="text" id='txtLock' />
    <input type="button" id='BtnLock' value='Lock' />
   
    </div>
    </form>


</body>
</html>
