<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="SistemPendukungKeputusan.Web.WebForms.ReportViewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Report</title>
    <script lang="javaScript" type="text/javascript" src="/crystalreportviewers13/js/crviewer/crv.js">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnExportToPdf" runat="server" OnClick="btnExportToPdf_Click" Text="Export to Pdf" />
            <asp:Button ID="btnExportToXls" runat="server" OnClick="btnExportToXls_Click" Text="Export to Xls" />
            <asp:Button ID="btnExportSchema" runat="server" OnClick="btnExportSchema_Click" Text="Export Schema" />
            <CR:CrystalReportViewer ID="crViewer" runat="server" AutoDataBind="true" ReuseParameterValuesOnRefresh="True" HasCrystalLogo="False" HasPrintButton="False" HasSearchButton="False" HasToggleParameterPanelButton="False" EnableParameterPrompt="False" EnableDatabaseLogonPrompt="False" EnableDrillDown="False" HasToggleGroupTreeButton="False" ToolPanelView="None" HasExportButton="False" />
        </div>
    </form>
</body>
</html>
