using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CheckBoXIndexAPP.Modeli;

namespace CheckBoXIndexAPP.Servisi
{
    public class CsvServis
    {
        private readonly string csvPath;
        private readonly string outputFolderPath;

        public CsvServis(string csvPath, string outputFolderPath)
        {
            this.csvPath = csvPath;
            this.outputFolderPath = outputFolderPath;
        }

        /// <summary>
        /// Sačuvaj listu PDF fajlova u CSV fajl.
        /// </summary>
        public void SacuvajPodatkeUCsv(List<InputPdfFile> pdfFajlovi, string operatorName)
        {
            try
            {
                var pdfZaCuvanje = pdfFajlovi
                    .Where(p => File.Exists(p.OriginalPath) && p.OriginalPath.StartsWith(outputFolderPath))
                    .ToList();

                using (var writer = new StreamWriter(csvPath, false))
                {
                    foreach (var pdf in pdfZaCuvanje)
                    {
                        // Merge logika: spajanje svih unosa
                        var poljaString = pdf.PoljaUnosi
                            .Select(u => $"{u.NazivPolja}|{u.Opis}|{u.Napomena}")
                            .ToArray();

                        string linija = string.Join(";", new string[]
                        {
                            pdf.FileName,
                            pdf.NewFileName ?? "",
                            string.Join(",", poljaString),
                            pdf.DatumObrade == DateTime.MinValue ? "" : pdf.DatumObrade.ToString("o"),
                            operatorName
                        });

                        writer.WriteLine(linija);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri čuvanju CSV fajla: " + ex.Message);
            }
        }

        /// <summary>
        /// Učitaj PDF fajlove iz CSV fajla.
        /// </summary>
        public List<InputPdfFile> UcitajPodatkeIzCsv()
        {
            var pdfFajlovi = new List<InputPdfFile>();
            try
            {
                if (!File.Exists(csvPath)) return pdfFajlovi;

                var lines = File.ReadAllLines(csvPath);
                foreach (var line in lines)
                {
                    var delovi = line.Split(';');
                    if (delovi.Length >= 5)
                    {
                        var pdf = new InputPdfFile(delovi[0])
                        {
                            NewFileName = delovi[1]
                        };

                        // Učitavanje PoljaUnosi
                        var unosiString = delovi[2].Split(',');
                        foreach (var unosStr in unosiString)
                        {
                            var deloviUnosa = unosStr.Split('|');
                            if (deloviUnosa.Length == 3)
                            {
                                pdf.DodajUnos(new UnosNovaApp
                                {
                                    NazivPolja = deloviUnosa[0],
                                    Opis = deloviUnosa[1],
                                    Napomena = deloviUnosa[2]
                                });
                            }
                        }

                        if (DateTime.TryParse(delovi[3], out var dt))
                            pdf.DatumObrade = dt;

                        pdfFajlovi.Add(pdf);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju CSV fajla: " + ex.Message);
            }

            return pdfFajlovi;
        }
    }
}
