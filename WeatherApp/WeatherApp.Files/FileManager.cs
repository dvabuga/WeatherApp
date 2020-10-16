using System;
using System.IO;
using ClosedXML.Excel;
using WeatherApp.Files.SharedModels;

namespace WeatherApp.Files
{
    public class FileManager
    {
        public static Stream GetXlsx(ChartModel faults)
        {
            var stream = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Sample Sheet");

                ws.Column(2).Width = 20;
                var k = 0;

                for (var i = 0; i < faults.ChartData.Count; i++)
                {
                    ws.Cell("C4").Value = $"Погрешности измерений на {DateTime.Now}";
                    ws.Range("C4:G4").Row(1).Merge();

                    ws.Cell($"B{5 + k}").Value = $"Интервал - {faults.ChartData[i].interval}";
                    ws.Cell($"B{6 + k}").Value = "Интервал";
                    ws.Cell($"B{7 + k}").Value = "Погрешность";

                    for (var j = 0; j < faults.ChartData[i].Item1.Count; j++)
                    {
                        ws.Column(3 + j).Width = 15;
                        ws.Cell(6 + k, 3 + j).Value = faults.ChartData[i].Item1[j].date;
                    }

                    for (var j = 0; j < faults.ChartData[i].Item1.Count; j++)
                    {
                        ws.Cell(7 + k, 3 + j).Value = faults.ChartData[i].Item1[j].temp;
                    }

                    k += 4;

                }

                workbook.SaveAs(stream);
                stream.Position = 0;
            }

            return stream;
        }


    }
}
