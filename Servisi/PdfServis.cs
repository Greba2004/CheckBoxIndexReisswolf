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

        private int trenutnaStranica = 1;
        private int ukupnoStranica = 1;

        public List<InputPdfFile> PdfFajlovi => pdfFajlovi;
        public int TrenutniIndex => trenutniIndex;

        public InputPdfFile TrenutniPdf
        {
            get
            {
                if (trenutniIndex >= 0 && trenutniIndex < pdfFajlovi.Count)
                    return pdfFajlovi[trenutniIndex];
                return null;
            }
        }
        public PdfViewer PdfViewerInstance => pdfViewer;
        public int TrenutnaStranica => trenutnaStranica;

        // Učitaj PDF fajlove iz foldera
        public void UcitajPdfFajlove(string inputFolderPath)
        {
            if (string.IsNullOrEmpty(inputFolderPath) || !Directory.Exists(inputFolderPath))
            {
                MessageBox.Show("Ulazni folder nije validan!");
                return;
            }

            var fajlovi = Directory.GetFiles(inputFolderPath, "*.pdf");
            pdfFajlovi = fajlovi.Select(f =>
            {
                var pdf = new InputPdfFile(f)
                {
                    OriginalFileName = Path.GetFileNameWithoutExtension(f)
                };
                return pdf;
            }).ToList();

            if (pdfFajlovi.Count == 0)
                MessageBox.Show("Nema PDF fajlova u izabranom folderu.");
        }

        // Prikaz PDF-a u panelu
        public void PrikaziTrenutniFajl(Panel panel)
        {
            OslobodiPdfViewer();

            if (TrenutniPdf == null)
                return;

            OtvoriPdf(TrenutniPdf.OriginalPath, panel);
        }

        private void OtvoriPdf(string putanjaPdf, Panel panel)
        {
            if (string.IsNullOrEmpty(putanjaPdf) || !File.Exists(putanjaPdf))
                return;

            OslobodiPdfViewer();

            var dokument = PdfDocument.Load(putanjaPdf);
            pdfViewer = new PdfViewer
            {
                Dock = DockStyle.Fill,
                Document = dokument
            };

            ukupnoStranica = dokument.PageCount;
            if (trenutnaStranica > ukupnoStranica)
                trenutnaStranica = 1;

            pdfViewer.Renderer.Page = trenutnaStranica - 1;

            panel.Controls.Clear();
            panel.Controls.Add(pdfViewer);
        }

        private void OslobodiPdfViewer()
        {
            if (pdfViewer != null)
            {
                pdfViewer.Document?.Dispose();
                pdfViewer.Dispose();
                pdfViewer = null;
            }
        }

        public void OslobodiSvePdfResurse()
        {
            OslobodiPdfViewer();
            pdfFajlovi.Clear();
            trenutniIndex = 0;
        }

        // Premeštanje fajla
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

        public void PredjiNaSledeciFajl()
        {
            if (trenutniIndex < pdfFajlovi.Count - 1)
            {
                trenutniIndex++;
                trenutnaStranica = 1;
            }
        }

        public void PredjiNaPrethodniFajl()
        {
            if (trenutniIndex > 0)
            {
                trenutniIndex--;
                trenutnaStranica = 1;
            }
        }

        // Novi delovi 👇
        public bool ImaJosStranica()
        {
            return trenutnaStranica < ukupnoStranica;
        }

        public void PredjiNaSledecuStranicu()
        {
            if (ImaJosStranica())
                trenutnaStranica++;
        }
    }
}
