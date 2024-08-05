<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="HomeWorkManagerPage.aspx.vb" Inherits="QuickTest.HomeWorkManagerPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadStyleContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<div id="main">
 
        <div id="site_content">
            <div class="content" style="width: 930px;">
                <section id="select">
		<center>
		<h2 style="margin: 15px 0 0 0;">ต้องการจัดการบ้านใหม่ หรือ เลือกการบ้านเก่า</h2>
        
		<div id="div-1" style='text-align:center'><h3 ><label id="newTestset" class="new" <%--onclick="document.location.href ='step2.aspx';"--%>>จัดการบ้านใหม่</label></h3> </div>
		
        <div id="div-1"  class="ListingFixedHeightContainer" style='text-align:center; '>
          <h3 ><label class="old"  onclick="ToggleBox();">แก้ไขการบ้านเก่า</label></h3>
	

		  <div id="divListing"  class="ListingContent" style='text-align:center; position: relative;overflow:scroll'>
          <%--<div id="divListing" style='text-align:center; width:100%; height:100%; overflow:auto;'>--%>
          <asp:Repeater id="Listing" runat="server">
            <HeaderTemplate>
                  <table class="bordered" style="width:100%; border-spacing:4; margin-top: 0px; "><thead>
                  <tr ><th >ชื่อ</th><th >วันที่สร้าง</th></tr>
                  </thead>
            </HeaderTemplate>
            <ItemTemplate>
             <tr ><td  style="background: #FFFFCC;"><a class="aTestset" href="HomeworkAssignPage.aspx?Id=<%# Container.DataItem("TestSet_Id")%>&Name=<%# Container.DataItem("TestSet_Name")%>"><%# Container.DataItem("TestSet_Name")%></a><%--<img id='imdDeleteTestSet' class='RubberHide' src="../Images/Delete-icon.png" onclick='UpdateTestSetId("<%# Container.DataItem("TestSet_Id")%>",this)' style=' float:right; cursor: pointer' />--%></td><td style="background: #FFFFCC;"> <%# Container.DataItem("LastUpdate")%></td></tr>
            </ItemTemplate>

            <AlternatingItemTemplate>
             <tr><td style="background: #FFFFFF;"><a class="aTestset" href="HomeworkAssignPage.aspx?Id=<%# Container.DataItem("TestSet_Id")%>&Name=<%# Container.DataItem("TestSet_Name")%>"><%# Container.DataItem("TestSet_Name")%></a><%--<img id='imdDeleteTestSet' class='RubberHide' src="../Images/Delete-icon.png" onclick='UpdateTestSetId("<%# Container.DataItem("TestSet_Id")%>",this)' style='float:right;cursor: pointer'/>--%></td><td style="background: #FFFFFF;"> <%# Container.DataItem("LastUpdate")%></td></tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
             </table>
            </FooterTemplate>
          </asp:Repeater>
          

              </div>   
        	</div>

		</center>
		  </section>
            </div>
        </div>

    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ExtraScript" runat="server">
    <script src="../js/jquery-1.7.1.js" type="text/javascript"></script>
<script type="text/javascript">


    $(function () {
        ToggleBox();


        $('#newTestset').click(function(){
        
              $.ajax({ type: "POST",
        	            url: "<%=ResolveUrl("~")%>Module/HomeWorkManagerPage.aspx/InsertNewTestSetForHomeWork",
        	            //data: "{ VbQsetId: '" + QsetId + "'}",
        	            contentType: "application/json; charset=utf-8", dataType: "json",   
        	            success: function (msg) {
                        if (msg.d != '') {
                            var TestSetId = msg.d.TestSetId;
                            var TestSetName = msg.d.CategoryName;
                            window.location = '../Module/HomeworkAssignPage.aspx?Id=' + TestSetId + '&Name=' + TestSetName; 
                        }
        	            },
        	            error: function myfunction(request, status)  {
                        alert('บางอย่างผิดพลาดโปรดลองอีกครั้ง')
        	            }
        	        });
        
        });



})

        function ToggleBox() {

            $("#divListing").slideToggle("slow");
            if ($.browser.msie) {
            if ($.browser.version <= 7) {
                $('#divListing').css('overflow','auto');
                }
            }
            
      }

      </script>

</asp:Content>
