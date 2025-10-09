using CheckBoXIndexAPP.Modeli;
using ClosedXML.Excel;

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
        DateTime? sessionStartTime = null) // <-- optional
    {
        this.outputFolderPath = outputFolderPath;
        this.operatorName = operatorName;
        this.configData = configData;
        this.pdfFajloviTrenutneSesije = pdfFajloviTrenutneSesije;
        this.sessionStartTime = sessionStartTime ?? DateTime.MinValue;
    }

    public void GenerisiIzvestajExcel()
    {
        try
        {
            var trenutniFajlovi = pdfFajloviTrenutneSesije
                .Where(p => p.DatumObrade >= sessionStartTime)
                .ToList();

            if (trenutniFajlovi.Count == 0)
            {
                MessageBox.Show("Nema PDF fajlova koji su obrađeni u ovoj sesiji.",
                    "Obaveštenje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var workbookPath = Path.Combine(outputFolderPath, "izvestaj.xlsx");

            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string archiveFolder = Path.Combine(appDirectory, "SviIzvestaji");
            if (!Directory.Exists(archiveFolder))
                Directory.CreateDirectory(archiveFolder);

            var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet("Izvestaj");

            int red = 1;
            int kolona = 1;

            // --- HEADER ---
            worksheet.Cell(red, kolona++).Value = "Stari naziv fajla";
            worksheet.Cell(red, kolona++).Value = "Novi naziv fajla";

            int maxUnosa = trenutniFajlovi
                .Where(p => p.PoljaUnosi != null)
                .Select(p => p.PoljaUnosi.Count)
                .DefaultIfEmpty(0)
                .Max();

            for (int i = 1; i <= maxUnosa; i++)
            {
                worksheet.Cell(red, kolona++).Value = $"Naziv {i}";
                worksheet.Cell(red, kolona++).Value = $"Napomena {i}";
                worksheet.Cell(red, kolona++).Value = $"Opis {i}";
            }

            worksheet.Cell(red, kolona++).Value = "Datum obrade";
            worksheet.Cell(red, kolona).Value = "Operater";
            red++;

            // --- PODACI ---
            foreach (var pdf in trenutniFajlovi)
            {
                kolona = 1;
                worksheet.Cell(red, kolona++).Value = pdf.FileName;
                worksheet.Cell(red, kolona++).Value = string.IsNullOrWhiteSpace(pdf.NewFileName)
                    ? pdf.FileName
                    : pdf.NewFileName;

                if (pdf.PoljaUnosi != null && pdf.PoljaUnosi.Count > 0)
                {
                    foreach (var unos in pdf.PoljaUnosi)
                    {
                        worksheet.Cell(red, kolona++).Value = unos.NazivPolja;
                        worksheet.Cell(red, kolona++).Value = unos.Napomena;
                        worksheet.Cell(red, kolona++).Value = unos.Opis;
                    }
                }

                int praznih = maxUnosa - (pdf.PoljaUnosi?.Count ?? 0);
                for (int i = 0; i < praznih; i++)
                {
                    worksheet.Cell(red, kolona++).Value = "";
                    worksheet.Cell(red, kolona++).Value = "";
                    worksheet.Cell(red, kolona++).Value = "";
                }

                worksheet.Cell(red, kolona++).Value = pdf.DatumObrade.ToString("yyyy-MM-dd HH:mm:ss");
                worksheet.Cell(red, kolona).Value = operatorName;

                red++;
            }

            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(workbookPath);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string archiveFileName = $"Izvestaj_{timestamp}.xlsx";
            string archiveFilePath = Path.Combine(archiveFolder, archiveFileName);
            workbook.SaveAs(archiveFilePath);

            MessageBox.Show($"Izveštaj za trenutnu sesiju je uspešno generisan.\nKopija je sačuvana u folderu 'SviIzvestaji'.",
                "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = workbookPath,
                UseShellExecute = true
            });

            // Application.Exit(); // <-- preporučujem da ga ukloniš ili da ga pozivaš iz UI samo po potrebi
        }
        catch (Exception ex)
        {
            MessageBox.Show("Greška pri generisanju izveštaja: " + ex.Message,
                "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}