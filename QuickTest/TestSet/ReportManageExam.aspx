<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReportManageExam.aspx.vb" Inherits="QuickTest.ReportManageExam" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div id='MainDiv'> 
   <telerik:RadGrid ID='ReportGrid'  runat="server" AutoGenerateColumns="false"  ExportSettings-ExportOnlyData="true" >
   
   <MasterTableView CommandItemDisplay="Top">
   <CommandItemSettings ShowExportToExcelButton="true" />
   <CommandItemSettings ShowAddNewRecordButton="false" />
   <CommandItemSettings ShowRefreshButton="false" />
   <Columns>
   
   <telerik:GridBoundColumn DataField='IsComplete' HeaderText='เสร็จสมบูรณ์' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='QCategory_Name' HeaderText='ชื่อหน่วยการเรียนรู้' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='QSet_Name' HeaderText='ชื่อชุดข้อสอบ' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='QAmount' HeaderText='จำนวนข้อ' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='IsExplainQuestion' HeaderText='อธิบายโจทย์แล้ว(ข้อ)' >
   </telerik:GridBoundColumn>

    <telerik:GridBoundColumn DataField='IsExplainAnswer' HeaderText='อธิบายคำตอบแล้ว(ตัวเลือก)/จาก(ตัวเลือก)' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='NewEvaluation' HeaderText='เลือก "ตัวชี้วัด" แล้ว' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='KPA' HeaderText='เลือก "KPA" แล้ว' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='InterExam' HeaderText='เลือก "แบบทดสอบระดับชาติ" แล้ว' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='Difficult' HeaderText='เลือก"ระดับความยากง่าย" แล้ว' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='ProveNewEvaluation' HeaderText='อนุมัติ "ระดับความยากง่าย" แล้ว' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='ProveKPA' HeaderText='อนุมัติ "KPA" แล้ว' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='ProveInterExam' HeaderText='อนุมัติ "แบบทดสอบระดับชาติ" แล้ว' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='ProveDifficult' HeaderText='อนุมัติ "ระดับความยากง่าย" แล้ว' >
   </telerik:GridBoundColumn>

   <telerik:GridBoundColumn DataField='Book_Syllabus' HeaderText='ปี' >
   </telerik:GridBoundColumn>


   </Columns>
   </MasterTableView>

   </telerik:RadGrid>
    </div>
    </form>
</body>
</html>
