<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CreateParentGraph.aspx.vb" Inherits="QuickTest.CreateParentGraph" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <telerik:RadChart ID="QualityChart" runat="server" Skin="Telerik">
            <PlotArea Appearance-Dimensions-Margins="60,20,75,80">
                <XAxis>
                    <AxisLabel Visible="true" Appearance-RotationAngle="360" Appearance-Position-AlignedPosition="Top">
                        <TextBlock Text="วันที่เข้าทำกิจกรรม" 
                            Appearance-TextProperties-Font="Angsana New, 13pt" 
                            Appearance-TextProperties-Color="darkSeaGreen" 
                            Appearance-Dimensions-Margins="20,0,0,0" 
                            Appearance-Dimensions-Paddings="0,0,10,0">

                        </TextBlock>
                    </AxisLabel>
                </XAxis>
                <YAxis>
                    <AxisLabel Visible="true" Appearance-RotationAngle="0" Appearance-Position-AlignedPosition="left">
                        <TextBlock Text="% คะแนนที่ทำได้" 
                            Appearance-TextProperties-Font="Angsana New, 13pt" 
                  
                            Appearance-TextProperties-Color="darkSeaGreen"   
                            Appearance-Dimensions-Paddings="0,0,0,0">
                        </TextBlock>
                    </AxisLabel>
                </YAxis>
            </PlotArea>
        </telerik:RadChart>


        <telerik:RadChart ID="QuantityChart" runat="server" Skin="Telerik">

            <PlotArea Appearance-Dimensions-Margins="60,20,75,80">
                <XAxis>
                    <AxisLabel Visible="true" Appearance-RotationAngle="360" 
                        Appearance-Position-AlignedPosition="Top">
                        <TextBlock Text="วันที่เข้าทำกิจกรรม" 
                            Appearance-TextProperties-Font="Angsana New, 13pt" 
                            Appearance-TextProperties-Color="darkSeaGreen" 
                            Appearance-Dimensions-Margins="20,0,2,0" 
                            Appearance-Dimensions-Paddings="0,0,0,0">

                        </TextBlock>
                    </AxisLabel>
                </XAxis>
                <YAxis>
                    <AxisLabel Visible="true" Appearance-RotationAngle="0" Appearance-Position-AlignedPosition="left">
                        <TextBlock Text="% จำนวนข้อที่เข้าทำ" 
                            Appearance-TextProperties-Font="Angsana New, 13pt" 
                  
                            Appearance-TextProperties-Color="darkSeaGreen"   
                            Appearance-Dimensions-Paddings="0,0,0,0">
                        </TextBlock>
                    </AxisLabel>
                </YAxis>
            </PlotArea>
        </telerik:RadChart>
    </div>
    </form>
</body>
</html>
