using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CheckBoXIndexAPP.Modeli;

namespace CheckBoXIndexAPP.Servisi
{
    public class IzvestajServis
    {
        private readonly string outputFolderPath;
        private readonly string operatorName;
        private readonly ConfigData configData;
        private readonly List<InputPdfFile> pdfFajloviZajednicki;

        public IzvestajServis(
            string outputFolderPath,
            string operatorName,
            ConfigData configData,
            List<InputPdfFile> pdfFajloviZajednicki)
        {
            this.outputFolderPath = outputFolderPath;
            this.operatorName = operatorName;
            this.configData = configData;
            this.pdfFajloviZajednicki = pdfFajloviZajednicki;
        }

        public void GenerisiIzvestajExcel()
        {
            try
            {
                var workbookPath = Path.Combine(outputFolderPath, "izvestaj.xlsx");

                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string archiveFolder = Path.Combine(appDirectory, "SviIzvestaji");
                if (!Directory.Exists(archiveFolder))
                    Directory.CreateDirectory(archiveFolder);

                var workbook = new XLWorkbook();
                var worksheet = workbook.AddWorksheet("Izvestaj");

                int red = 1;

                // --- HEADER ---
                int kolona = 1;
                worksheet.Cell(red, kolona++).Value = "Stari naziv fajla";
                worksheet.Cell(red, kolona++).Value = "Novi naziv fajla";

                // Za svaki check box dodajemo Naziv - Opis - Napomena
                foreach (var cb in configData.CheckBoxovi)
                {
                    worksheet.Cell(red, kolona++).Value = cb.Naziv + " - Opis";
                    worksheet.Cell(red, kolona++).Value = cb.Naziv + " - Napomena";
                }

                worksheet.Cell(red, kolona).Value = "Ime operatera"; // poslednja kolona
                red++;

                // --- PODACI ---
                foreach (var pdf in pdfFajloviZajednicki)
                {
                    kolona = 1;
                    worksheet.Cell(red, kolona++).Value = pdf.FileName;
                    worksheet.Cell(red, kolona++).Value = string.IsNullOrWhiteSpace(pdf.NewFileName) ? pdf.FileName : pdf.NewFileName;

                    // Merge logika: ako je isti naziv polja više puta, spajamo Opis i Napomenu
                    var unosi = pdf.PoljaUnosi;
                    if (unosi != null && unosi.Count > 0)
                    {
                        // Grupisanje po NazivPolja
                        var grupisani = unosi.GroupBy(u => u.NazivPolja);
                        foreach (var grupa in grupisani)
                        {
                            string naziv = grupa.Key;
                            string opis = string.Join(" | ", grupa.Select(x => x.Opis));
                            string napomena = string.Join(" | ", grupa.Select(x => x.Napomena));

                            worksheet.Cell(red, kolona++).Value = naziv;
                            worksheet.Cell(red, kolona++).Value = opis;
                            worksheet.Cell(red, kolona++).Value = napomena;
                        }
                    }

                    red++;
                }

                // Ime operatera samo u poslednjem redu poslednje kolone
                if (red > 2)
                {
                    worksheet.Cell(red - 1, kolona).Value = operatorName;
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(workbookPath);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string archiveFileName = $"Izvestaj_{timestamp}.xlsx";
                string archiveFilePath = Path.Combine(archiveFolder, archiveFileName);
                workbook.SaveAs(archiveFilePath);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = workbookPath,
                    UseShellExecute = true
                });

                MessageBox.Show($"Izveštaj je uspešno generisan i otvoren.\nKopija je sačuvana u folderu 'SviIzvestaji'.",
                                "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri generisanju izveštaja: " + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
