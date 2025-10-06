using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CheckBoXIndexAPP.Modeli;


namespace CheckBoXIndexAPP.Servisi
{
    public class ConfigExcelServis
    {
        private readonly string excelPath;

        public ConfigExcelServis(string excelPath)
        {
            this.excelPath = excelPath;
        }

        public List<CheckBoxConfig> UcitajCheckBoxKonfiguraciju()
        {
            if (string.IsNullOrEmpty(excelPath) || !File.Exists(excelPath))
                throw new FileNotFoundException("Excel konfiguracioni fajl nije pronađen!", excelPath);

            var listaCheckBoxova = new List<CheckBoxConfig>();

            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet(1); // prvi sheet

                int row = 2; // preskačemo header

                while (!worksheet.Cell(row, 1).IsEmpty())
                {
                    try
                    {
                        var redniBroj = worksheet.Cell(row, 1).GetValue<int>();
                        var id = worksheet.Cell(row, 2).GetValue<int>();
                        var obaveznoText = worksheet.Cell(row, 3).GetValue<string>().Trim().ToUpper();
                        var naziv = worksheet.Cell(row, 4).GetValue<string>().Trim();

                        listaCheckBoxova.Add(new CheckBoxConfig
                        {
                            RedniBroj = redniBroj,
                            Id = id,
                            Obavezno = obaveznoText == "DA",
                            Naziv = naziv
                        });
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Greška u čitanju reda {row}: {ex.Message}");
                    }

                    row++;
                }
            }

            // sortiramo po rednom broju
            return listaCheckBoxova.OrderBy(c => c.RedniBroj).ToList();
        }
    }
    public class CheckBoxConfig
    {
        public int RedniBroj { get; set; }
        public int Id { get; set; }
        public bool Obavezno { get; set; }
        public string Naziv { get; set; }
    }
}