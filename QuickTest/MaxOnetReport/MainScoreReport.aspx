<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MainScoreReport.aspx.vb" Inherits="QuickTest.MainScoreReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

    <script src="../js/canvasjs.min.js"></script>

<head runat="server">
    <title></title>
        <style type="text/css">
            td {
                display:table-cell;
            }
            .tdRotate {
                background-color:antiquewhite; 
                   writing-mode:vertical-lr;
                -webkit-transform:rotate(270deg);   
            }
            .textAlignVer {
                display: block;
    filter: flipv fliph;
    -webkit-transform: rotate(-90deg);
    -moz-transform: rotate(-90deg);
    transform: rotate(-90deg);
    position: relative;
    width: 30px;
    white-space: nowrap;
    margin-bottom: 10px;
    left: 8px;
    text-align: center;
}
           
        </style>
    <script type="text/javascript">
window.onload = function () {

var chart = new CanvasJS.Chart("chartContainer", {
	exportEnabled: true,
	animationEnabled: true,
	title:{
		text: "State Operating Funds"
	},
	legend:{
		cursor: "pointer",
		itemclick: explodePie
	},
	data: [{
		type: "pie",
		showInLegend: true,
		toolTipContent: "{name}: <strong>{y}% </strong>",
		indexLabel: " {name} - {y}% ",
		dataPoints: [
			{ y: 22, name: " ไม่ตอบ" },
			{ y: 60, name: " ตอบผิด" },
			{ y: 12, name: " ตอบถูก" }
		]
	}]
});
chart.render();
}

function explodePie (e) {
	if(typeof (e.dataSeries.dataPoints[e.dataPointIndex].exploded) === "undefined" || !e.dataSeries.dataPoints[e.dataPointIndex].exploded) {
		e.dataSeries.dataPoints[e.dataPointIndex].exploded = true;
	} else {
		e.dataSeries.dataPoints[e.dataPointIndex].exploded = false;
	}
	e.chart.render();

}
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="divRangeAmount">
               
                <table border="1" cellspacing="0" cellpadding="0">
 <tbody>
       <tr>
        <td  style="border-style:none;"></td>
    <td align="center" colspan="4">หน่วยการเรียนรู้</td>
  </tr>
     <tr>
    <td align="center" style="border-style:none;">&nbsp;</td>
    <td align="center">1</td>
    <td align="center">2</td>
    <td align="center">3</td>
    <td align="center">4</td>
  </tr>
  <tr>
    <td width="120" align="center" style="border-style:none;">&nbsp;</td>
    <td height="150" width="45" valign="bottom"><span class="textAlignVer">มอม</span></td>
    <td height="150" width="45" valign="bottom"><span class="textAlignVer">ชนิดของคำ</span></td>
    <td height="150" width="45" valign="bottom"><span class="textAlignVer">กลอนดอกสร้อย</span></td>
    <td height="150" width="45" valign="bottom"><span class="textAlignVer">หัวใจนักรบ</span></td>
  </tr> 
<tr>
    <td align="left">จำนวนข้อ</td>
    <td align="center">20</td>
    <td align="center">20</td>
    <td align="center">20</td>
    <td align="center">20</td>
  </tr>
  <tr>
    <td align="left">คะแนนเต็ม</td>
    <td align="center">20</td>
    <td align="center">20</td>
    <td align="center">20</td>
    <td align="center">20</td>
  </tr>
  
  <tr>
    <td align="left">A (100% - 80%)</td>
    <td align="center">1</td>
    <td align="center">1</td>
    <td align="center">1</td>
    <td align="center">1</td>
  </tr>
  <tr>
    <td align="left">B (79% - 70%)</td>
    <td align="center">1</td>
    <td align="center">1</td>
    <td align="center">1</td>
    <td align="center">1</td>
  </tr>
</tbody></table>
             </div>  
        </div>

        <div>
            <div id="chartContainer" style="height: 370px; max-width: 920px; margin: 0px auto;"></div>
        </div>
        <br />
           <br />
           <br />
           <br />
        <div>
            <table>
                <tr class="headerRow">
                    <td> &nbsp;</td>
                    <asp:Repeater ID="rptHeader" runat="server">
                        <ItemTemplate>
                            <td class="header"><%# Container.DataItem("subName")%></td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <asp:Repeater ID="rptName" runat="server" OnItemDataBound="rptName_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td><%# Container.DataItem("stdName")%></td>
                             <asp:Repeater ID="rptScore" runat="server">
                                <ItemTemplate>
                                    <td class="header"><%# Container.DataItem("Quizscore").ToString %></td>
                        <%--            <td class="header"><%# DataBinder.Eval(Container.DataItem, "Quizscore") %>--%>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </form>
</body>
</html>
