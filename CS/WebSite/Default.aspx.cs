using System;
using System.IO;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraPrinting;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        Chart.SeriesDataMember = "CategoryName";
        Chart.SeriesTemplate.ArgumentDataMember = "ProductSales";
        Chart.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "ProductSales" });
        Chart.SeriesTemplate.View = new StackedBarSeriesView();
    }
    protected void ExportButton_Click(object sender, EventArgs e) {
        PrintingSystemBase ps = new PrintingSystemBase();

        PrintableComponentLinkBase link1 = new PrintableComponentLinkBase(ps);
        link1.Component = Grid;

        PrintableComponentLinkBase link2 = new PrintableComponentLinkBase(ps);
        Chart.DataBind();
        link2.Component = ((IChartContainer)Chart).Chart;

        CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
        compositeLink.Links.AddRange(new object[] { link1, link2 });

        compositeLink.CreateDocument();
        using (MemoryStream stream = new MemoryStream()) {
            compositeLink.PrintingSystemBase.ExportToXls(stream);
            Response.Clear();
            Response.Buffer = false;
            Response.AppendHeader("Content-Type", "application/xls");
            Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Response.AppendHeader("Content-Disposition", "attachment; filename=test.xls");
            Response.BinaryWrite(stream.ToArray());
            Response.End();
        }
        ps.Dispose();
    }
}
