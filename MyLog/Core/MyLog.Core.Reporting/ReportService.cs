using Microsoft.Reporting.NETCore;
using MyLog.Core.Contracts;
using MyLog.Core.Contracts.Models;

namespace MyLog.Core.Reporting;

public class ReportService: IReportService
{
    public async Task CreateMovementReportAsync(List<MovementDto> data)
    {
        // Path to the .rdlc report file
        var path = Path.GetDirectoryName(GetType().Assembly.Location);
        string reportPath = Path.Combine(path, "MovementListe.rdlc");

        // Load the report
        var report = new LocalReport();
        report.ReportPath = reportPath;

        // Add the data source to the report
        report.DataSources.Clear();
        report.DataSources.Add(new ReportDataSource("ds_Data", data));
        report.SetParameters(new ReportParameter("UserName", "bob"));

        // Render the report to a PDF file
        string deviceInfo =
            @"<DeviceInfo>
            <OutputFormat>PDF</OutputFormat>
            <PageWidth>21.03cm</PageWidth>
            <PageHeight>29.7cm</PageHeight>
            <MarginTop>0.5cm</MarginTop>
            <MarginLeft>1cm</MarginLeft>
            <MarginRight>1cm</MarginRight>
            <MarginBottom>1cm</MarginBottom>
            </DeviceInfo>";

        byte[] byteLabel;
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;

        byteLabel = report.Render(
                "PDF", deviceInfo, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

        // Save the PDF file to the file system
        await File.WriteAllBytesAsync(Path.Combine(path, "MovementReport.pdf"), byteLabel);
    }
}
