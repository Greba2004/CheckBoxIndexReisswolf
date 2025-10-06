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

        public List<UnosNovaApp> PoljaUnosi { get; set; } = new List<UnosNovaApp>();

        public DateTime DatumObrade { get; set; } = DateTime.MinValue;

        public InputPdfFile(string path)
        {
            OriginalPath = path;
            NewFileName = FileName;
        }
    }
}
