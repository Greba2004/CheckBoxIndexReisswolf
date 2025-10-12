using System;
using System.Collections.Generic;
using System.IO;

namespace CheckBoXIndexAPP.Modeli
{
    public class InputPdfFile
    {
        public string OriginalPath { get; set; }
        public string FileName => Path.GetFileName(OriginalPath);
        public string NewFileName { get; set; }
        public string OriginalFileName { get; set; }

        // Lista unosa iz checkboxova i polja opis/napomena
        public List<UnosNovaApp> PoljaUnosi { get; set; } = new List<UnosNovaApp>();

        public DateTime DatumObrade { get; set; } = DateTime.MinValue;

        public InputPdfFile(string path)
        {
            OriginalPath = path;
            NewFileName = FileName;
        }

        // Dodaj unos za polje, spaja ako polje već postoji
        public void DodajUnos(UnosNovaApp unos)
        {
            var postojece = PoljaUnosi.Find(u => u.NazivPolja == unos.NazivPolja);
            if (postojece != null)
            {
                postojece.Opis += " | " + unos.Opis;
                postojece.Napomena += " | " + unos.Napomena;
            }
            else
            {
                PoljaUnosi.Add(unos);
            }
        }
    }
}