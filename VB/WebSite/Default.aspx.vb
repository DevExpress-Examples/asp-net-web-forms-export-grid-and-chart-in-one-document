Imports System
Imports System.IO
Imports DevExpress.XtraCharts
Imports DevExpress.XtraCharts.Native
Imports DevExpress.XtraPrintingLinks
Imports DevExpress.XtraPrinting

Partial Public Class _Default
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		Chart.SeriesDataMember = "CategoryName"
		Chart.SeriesTemplate.ArgumentDataMember = "ProductSales"
		Chart.SeriesTemplate.ValueDataMembers.AddRange(New String() { "ProductSales" })
		Chart.SeriesTemplate.View = New StackedBarSeriesView()
	End Sub
	Protected Sub ExportButton_Click(ByVal sender As Object, ByVal e As EventArgs)
		Dim ps As New PrintingSystemBase()

		Dim link1 As New PrintableComponentLinkBase(ps)
		link1.Component = Grid

		Dim link2 As New PrintableComponentLinkBase(ps)
		Chart.DataBind()
		link2.Component = DirectCast(Chart, IChartContainer).Chart

		Dim compositeLink As New CompositeLinkBase(ps)
		compositeLink.Links.AddRange(New Object() { link1, link2 })

		compositeLink.CreateDocument()
		Using stream As New MemoryStream()
			compositeLink.PrintingSystemBase.ExportToXls(stream)
			Response.Clear()
			Response.Buffer = False
			Response.AppendHeader("Content-Type", "application/xls")
			Response.AppendHeader("Content-Transfer-Encoding", "binary")
			Response.AppendHeader("Content-Disposition", "attachment; filename=test.xls")
			Response.BinaryWrite(stream.ToArray())
			Response.End()
		End Using
		ps.Dispose()
	End Sub
End Class
