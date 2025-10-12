using CheckBoXIndexAPP.Modeli;
using CheckBoXIndexAPP.Servisi;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

public class IzvestajServis
{
    private readonly string outputFolderPath;
    private readonly string operatorName;
    private readonly ConfigData configData;
    private readonly List<InputPdfFile> pdfFajloviTrenutneSesije;
    private readonly DateTime sessionStartTime;

    public IzvestajServis(
        string outputFolderPath,
        string operatorName,
        ConfigData configData,
        List<InputPdfFile> pdfFajloviTrenutneSesije,
        DateTime? sessionStartTime = null)
    {
        this.outputFolderPath = outputFolderPath;
        this.operatorName = operatorName;
        this.configData = configData;
        this.pdfFajloviTrenutneSesije = pdfFajloviTrenutneSesije ?? new List<InputPdfFile>();
        this.sessionStartTime = sessionStartTime ?? DateTime.MinValue;
    }

    public void GenerisiIzvestajExcel()
    {
        try
        {
            var trenutniFajlovi = pdfFajloviTrenutneSesije
                .Where(p => p.DatumObrade >= sessionStartTime)
                .OrderBy(p => p.DatumObrade)
                .ToList();

            if (!trenutniFajlovi.Any())
            {
                MessageBox.Show("Nema PDF fajlova koji su obrađeni u ovoj sesiji.",
                    "Obaveštenje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string archiveFolder = Path.Combine(appDirectory, "SviIzvestaji");
            if (!Directory.Exists(archiveFolder))
                Directory.CreateDirectory(archiveFolder);

            string workbookPath = Path.Combine(outputFolderPath, "izvestaj.xlsx");

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.AddWorksheet("Izvestaj");

                int red = 1;
                int kolona = 1;

                // 🔹 Fiksne kolone
                ws.Cell(red, kolona++).Value = "Stari naziv fajla";
                ws.Cell(red, kolona++).Value = "Novi naziv fajla";
                ws.Cell(red, kolona++).Value = "Operater";
                ws.Cell(red, kolona++).Value = "Datum obrade";

                var checkBoxovi = configData?.CheckBoxovi ?? new List<CheckBoxConfig>();
                foreach (var cb in checkBoxovi)
                {
                    ws.Cell(red, kolona++).Value = $"Naziv {cb.Naziv}";
                    ws.Cell(red, kolona++).Value = $"Opis {cb.Naziv}";
                    ws.Cell(red, kolona++).Value = $"Napomena {cb.Naziv}";
                }

                red++;

                // 🔹 Popunjavanje redova po PDF fajlu
                foreach (var pdf in trenutniFajlovi)
                {
                    kolona = 1;
                    ws.Cell(red, kolona++).Value = pdf.OriginalFileName; // originalni naziv
                    ws.Cell(red, kolona++).Value = string.IsNullOrWhiteSpace(pdf.NewFileName)
                        ? pdf.OriginalFileName
                        : pdf.NewFileName; // novi naziv
                    ws.Cell(red, kolona++).Value = operatorName;
                    ws.Cell(red, kolona++).Value = pdf.DatumObrade.ToString("yyyy-MM-dd HH:mm:ss");

                    foreach (var cb in checkBoxovi)
                    {
                        var unos = pdf.PoljaUnosi?.FirstOrDefault(u => u.IdPolja == cb.Id);
                        ws.Cell(red, kolona++).Value = unos?.NazivPolja ?? "";
                        ws.Cell(red, kolona++).Value = Formatiraj(unos?.Opis);
                        ws.Cell(red, kolona++).Value = Formatiraj(unos?.Napomena);
                    }

                    red++;
                }

                ws.Columns().AdjustToContents();
                workbook.SaveAs(workbookPath);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string archivePath = Path.Combine(archiveFolder, $"Izvestaj_{timestamp}.xlsx");
                workbook.SaveAs(archivePath);
            }

            MessageBox.Show("Izveštaj je uspešno generisan i sačuvan u 'SviIzvestaji'.",
                "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = workbookPath,
                    UseShellExecute = true
                });
            }
            catch { }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Greška pri generisanju izveštaja: " + ex.Message,
                "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string Formatiraj(string tekst)
    {
        if (string.IsNullOrWhiteSpace(tekst))
            return "";

        var delovi = tekst
            .Split(new[] { ',', ';', '|', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return string.Join(" // ", delovi);
    }
}