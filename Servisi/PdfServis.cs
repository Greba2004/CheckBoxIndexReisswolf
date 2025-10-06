using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PdfiumViewer;
using CheckBoXIndexAPP.Modeli;

namespace CheckBoXIndexAPP.Services
{
    public class PdfService
    {
        private PdfViewer pdfViewer;
        private List<InputPdfFile> pdfFajlovi = new List<InputPdfFile>();
        private int trenutniIndex = 0;

        public List<InputPdfFile> PdfFajlovi => pdfFajlovi;
        public int TrenutniIndex => trenutniIndex;

        // NOVO: property za trenutni PDF fajl
        public InputPdfFile TrenutniPdf
        {
            get
            {
                if (trenutniIndex >= 0 && trenutniIndex < pdfFajlovi.Count)
                    return pdfFajlovi[trenutniIndex];
                return null;
            }
        }

        /// <summary>
        /// Učitaj PDF fajlove iz ulaznog foldera.
        /// </summary>
        public void UcitajPdfFajlove(string inputFolderPath)
        {
            if (string.IsNullOrEmpty(inputFolderPath) || !Directory.Exists(inputFolderPath))
            {
                MessageBox.Show("Ulazni folder nije validan!");
                return;
            }

            var fajlovi = Directory.GetFiles(inputFolderPath, "*.pdf");
            pdfFajlovi = fajlovi.Select(f => new InputPdfFile(f)).ToList();

            if (pdfFajlovi.Count == 0)
                MessageBox.Show("Nema PDF fajlova u izabranom folderu.");
        }

        /// <summary>
        /// Prikaži trenutni PDF u panelu.
        /// </summary>
        public void PrikaziTrenutniFajl(Panel panel)
        {
            OslobodiPdfViewer();

            if (TrenutniPdf == null)
                return;

            OtvoriPdf(TrenutniPdf.OriginalPath, panel);
        }

        /// <summary>
        /// Otvori PDF u PdfViewer-u.
        /// </summary>
        private void OtvoriPdf(string putanjaPdf, Panel panel)
        {
            if (string.IsNullOrEmpty(putanjaPdf) || !File.Exists(putanjaPdf))
                return;

            OslobodiPdfViewer();

            pdfViewer = new PdfViewer
            {
                Dock = DockStyle.Fill,
                Document = PdfDocument.Load(putanjaPdf)
            };

            panel.Controls.Clear();
            panel.Controls.Add(pdfViewer);
        }

        /// <summary>
        /// Oslobodi PdfViewer resurse.
        /// </summary>
        private void OslobodiPdfViewer()
        {
            if (pdfViewer != null)
            {
                pdfViewer.Document?.Dispose();
                pdfViewer.Dispose();
                pdfViewer = null;
            }
        }

        /// <summary>
        /// Oslobodi sve PDF resurse i listu fajlova.
        /// </summary>
        public void OslobodiSvePdfResurse()
        {
            OslobodiPdfViewer();
            pdfFajlovi.Clear();
            trenutniIndex = 0;
        }

        /// <summary>
        /// Premesti trenutni PDF u output folder i ažuriraj putanju.
        /// </summary>
        public void PremestiTrenutniPdfUFolder(string outputFolderPath)
        {
            if (TrenutniPdf == null)
                return;

            string nazivFajla = string.IsNullOrWhiteSpace(TrenutniPdf.NewFileName)
                ? Path.GetFileName(TrenutniPdf.OriginalPath)
                : TrenutniPdf.NewFileName;

            if (!nazivFajla.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                nazivFajla += ".pdf";

            string novaPutanja = Path.Combine(outputFolderPath, nazivFajla);

            if (File.Exists(TrenutniPdf.OriginalPath))
            {
                OslobodiPdfViewer();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                File.Move(TrenutniPdf.OriginalPath, novaPutanja);
                TrenutniPdf.OriginalPath = novaPutanja;
            }
        }

        /// <summary>
        /// Idi na sledeći PDF fajl.
        /// </summary>
        public void PredjiNaSledeciFajl()
        {
            if (trenutniIndex < pdfFajlovi.Count - 1)
                trenutniIndex++;
        }

        /// <summary>
        /// Idi na prethodni PDF fajl.
        /// </summary>
        public void PredjiNaPrethodniFajl()
        {
            if (trenutniIndex > 0)
                trenutniIndex--;
        }
    }
}
