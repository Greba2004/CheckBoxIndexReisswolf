using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CheckBoXIndexAPP.Modeli;
using CheckBoXIndexAPP.Servisi;
using CheckBoXIndexAPP.Services;

namespace CheckBoXIndexAPP.Forms
{
    public partial class MainForma : Form
    {
        private readonly string inputPath;
        private readonly string outputPath;
        private readonly string imeOperatera;
        private readonly DateTime sessionStartTime = DateTime.Now;

        // Servisi
        private ConfigExcelServis configServis;
        private CsvServis csvServis;
        private PdfService pdfService;
        private IzvestajServis izvestajServis;
        private DataValidationService validationServis;


        // Podaci
        private List<CheckBoxConfig> configData;
        private List<InputPdfFile> pdfFajloviZajednicki = new List<InputPdfFile>();


        // Putanje u AppData
        private string configExcelPath;
        private string csvTempPath;
        private TextBox txtFilter;

        // Logika za checkboxove
        private CheckBox selektovaniCheckBox = null;
        private List<UnosNovaApp> unosPodaci = new List<UnosNovaApp>();

        // 🆕 Filter polje
        

        public MainForma()
        {
            InitializeComponent();
        }

        public MainForma(string inputPath, string outputPath, string imeOperatera) : this()
        {
            this.inputPath = inputPath ?? "";
            this.outputPath = outputPath ?? "";
            this.imeOperatera = imeOperatera ?? "";

            lblPdfNaziv.Text = "Naziv PDF fajla: (nije učitan)";

            configExcelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.xlsx");
            if (File.Exists(configExcelPath))
            {
                configServis = new ConfigExcelServis(configExcelPath);

                try
                {
                    configData = configServis.UcitajCheckBoxKonfiguraciju();
                    KreirajFilterTextBox(); // 🆕 prvo kreiraj filter
                    KreirajCheckBoxove(configData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri učitavanju konfiguracije: {ex.Message}");
                    configData = new List<CheckBoxConfig>();
                }
            }
            else
            {
                MessageBox.Show("Konfiguracioni fajl 'config.xlsx' nije pronađen u folderu aplikacije!");
                configData = new List<CheckBoxConfig>();
            }
        }

        private void MainForma_Load(object sender, EventArgs e)
        {
            try
            {
                string folderProjekta = AppDomain.CurrentDomain.BaseDirectory;
                configExcelPath = Path.Combine(folderProjekta, "config.xlsx");
                csvTempPath = Path.Combine(folderProjekta, "podaci_temp.csv");

                configServis = new ConfigExcelServis(configExcelPath);
                csvServis = new CsvServis(csvTempPath, outputPath);
                pdfService = new PdfService();
                validationServis = new DataValidationService();

                if (File.Exists(configExcelPath))
                {
                    try
                    {
                        configData = configServis.UcitajCheckBoxKonfiguraciju();
                        KreirajFilterTextBox(); // 🆕 filter se dodaje i pri loadu
                        KreirajCheckBoxove(configData);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greška pri učitavanju konfiguracije: " + ex.Message);
                        configData = new List<CheckBoxConfig>();
                    }
                }
                else
                {
                    MessageBox.Show($"Nije pronađen config fajl (config.xlsx) u folderu:\n{configExcelPath}");
                    configData = new List<CheckBoxConfig>();
                }

                try
                {
                    var ucitani = csvServis.UcitajPodatkeIzCsv();
                    pdfFajloviZajednicki = ucitani;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri učitavanju CSV-a: " + ex.Message);
                }

                pdfService.UcitajPdfFajlove(this.inputPath);

                if (pdfService.PdfFajlovi != null && pdfService.PdfFajlovi.Any())
                {
                    AzurirajUIPoslePromeneFajla();
                    pdfService.PrikaziTrenutniFajl(this.pdfPanel);
                }
                else
                {
                    lblPdfNaziv.Text = "Nema PDF fajlova u input folderu";
                }

                var configDataModel = new ConfigData { CheckBoxovi = configData };
                izvestajServis = new IzvestajServis(outputPath, imeOperatera, configDataModel, pdfFajloviZajednicki, sessionStartTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri startu forme: " + ex.Message);
            }
        }

        // 🆕 Dodato: kreiranje filter TextBoxa
        private void KreirajFilterTextBox()
        {
            if (txtFilter != null) return; // spreči dupliranje

            txtFilter = new TextBox
            {
                PlaceholderText = "Pretraži po nazivu ili ID...",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new System.Drawing.Font("Segoe UI", 10F)
            };
            txtFilter.TextChanged += TxtFilter_TextChanged;

            middlePanel.Controls.Add(txtFilter);
            txtFilter.BringToFront();
        }

        // 🆕 Logika filtriranja checkboxova
        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = txtFilter.Text.Trim().ToLower();

            foreach (Control ctrl in panelCheckBoxovi.Controls)
            {
                if (ctrl is CheckBox cb)
                {
                    string naziv = cb.Text.ToLower();
                    string id = cb.Tag?.ToString()?.ToLower() ?? "";

                    cb.Visible = string.IsNullOrEmpty(filter) ||
                                 naziv.Contains(filter) ||
                                 id.Contains(filter);
                }
            }
        }

        // Kreiranje checkboxova
        private void KreirajCheckBoxove(List<CheckBoxConfig> konfiguracija)
        {
            panelCheckBoxovi.Controls.Clear();
            int y = 10;

            foreach (var cfg in konfiguracija)
            {
                var cb = new CheckBox
                {
                    Text = cfg.Naziv + (cfg.Obavezno ? " *" : ""),
                    Tag = cfg.Id,
                    AutoSize = true,
                    Location = new System.Drawing.Point(10, y),
                    Font = new System.Drawing.Font("Segoe UI", 10F)
                };

                if (cfg.Obavezno)
                    cb.ForeColor = System.Drawing.Color.DarkRed;

                cb.CheckedChanged += CheckBox_CheckedChanged;
                panelCheckBoxovi.Controls.Add(cb);
                y += 30;
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null) return;

            if (cb.Checked)
            {
                if (selektovaniCheckBox != null && selektovaniCheckBox != cb)
                    selektovaniCheckBox.Checked = false;

                selektovaniCheckBox = cb;

                txtOpis.Visible = true;
                txtNapomena.Visible = true;
                btnSledeciUnos.Visible = true;

                txtOpis.Focus();
            }
            else
            {
                selektovaniCheckBox = null;
                txtOpis.Visible = false;
                txtNapomena.Visible = false;
                btnSledeciUnos.Visible = false;

                txtOpis.Clear();
                txtNapomena.Clear();
            }
        }

        private void btnSledeciUnos_Click(object sender, EventArgs e)
        {
            if (selektovaniCheckBox == null)
                return;

            string naziv = selektovaniCheckBox.Text;
            if (naziv.EndsWith(" *"))
                naziv = naziv.Substring(0, naziv.Length - 2);

            string opis = txtOpis.Text.Trim();
            string napomena = txtNapomena.Text.Trim();

            // Ako je polje obavezno, ne dozvoli prelazak bez opisa ili napomene
            var obaveznaPolja = configData
                .Where(c => c.Obavezno)
                .Select(c => c.Naziv)
                .ToList();

            if (obaveznaPolja.Contains(naziv) &&
                string.IsNullOrWhiteSpace(opis) &&
                string.IsNullOrWhiteSpace(napomena))
            {
                MessageBox.Show($"Polje '{naziv}' je obavezno i mora biti popunjeno pre prelaska na sledeći unos.",
                                "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ako ništa nije upisano, nema potrebe da se dodaje
            if (string.IsNullOrWhiteSpace(opis) && string.IsNullOrWhiteSpace(napomena))
            {
                MessageBox.Show("Morate uneti opis ili napomenu pre nego što pređete na sledeći unos.",
                                "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dodaj novi unos
            unosPodaci.Add(new UnosNovaApp
            {
                NazivPolja = naziv,
                Opis = opis,
                Napomena = napomena
            });

            // Resetuj UI
            selektovaniCheckBox.Checked = false;
            selektovaniCheckBox = null;
            txtOpis.Clear();
            txtNapomena.Clear();
            txtOpis.Visible = false;
            txtNapomena.Visible = false;
            btnSledeciUnos.Visible = false;
        }

        private void txtNapomena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSledeciUnos_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void AzurirajUIPoslePromeneFajla()
        {
            var trenutni = pdfService.TrenutniPdf;
            if (trenutni != null)
            {
                lblPdfNaziv.Text = trenutni.FileName;
                chkMenjajNaziv.Checked = false;
                txtNoviNaziv.Text = trenutni.NewFileName ?? trenutni.FileName;
            }
            else
            {
                lblPdfNaziv.Text = "Nema fajla";
            }

            txtOpis.Visible = false;
            txtNapomena.Visible = false;
            btnSledeciUnos.Visible = false;
        }

        private void btnPrethodni_Click(object sender, EventArgs e)
        {
            pdfService.PredjiNaPrethodniFajl();
            pdfService.PrikaziTrenutniFajl(this.pdfPanel);
            AzurirajUIPoslePromeneFajla();
        }

        private void btnSledeci_Click(object sender, EventArgs e)
        {
            pdfService.PredjiNaSledeciFajl();
            pdfService.PrikaziTrenutniFajl(this.pdfPanel);
            AzurirajUIPoslePromeneFajla();
        }

        private void btnIzvestaj_Click(object sender, EventArgs e)
        {
            try
            {
                var configDataModel = new ConfigData { CheckBoxovi = configData };
                izvestajServis = new IzvestajServis(outputPath, imeOperatera, configDataModel, pdfFajloviZajednicki);
                izvestajServis.GenerisiIzvestajExcel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška generisanja izveštaja: " + ex.Message);
            }
        }

        private void btnSacuvajPredji_Click(object sender, EventArgs e)
        {
            try
            {
                var trenutni = pdfService.TrenutniPdf;
                if (trenutni == null) return;

                // Ako postoji aktivan unos koji još nije dodat, dodaj ga pre provere
                if (selektovaniCheckBox != null &&
                    (!string.IsNullOrWhiteSpace(txtOpis.Text) || !string.IsNullOrWhiteSpace(txtNapomena.Text)))
                {
                    string nazivPolja = selektovaniCheckBox.Text;
                    if (nazivPolja.EndsWith(" *"))
                        nazivPolja = nazivPolja.Substring(0, nazivPolja.Length - 2);

                    var noviUnos = new UnosNovaApp
                    {
                        NazivPolja = nazivPolja,
                        Opis = txtOpis.Text.Trim(),
                        Napomena = txtNapomena.Text.Trim()
                    };

                    unosPodaci.Add(noviUnos);
                    selektovaniCheckBox.Checked = false;
                    selektovaniCheckBox = null;
                    txtOpis.Clear();
                    txtNapomena.Clear();
                    txtOpis.Visible = false;
                    txtNapomena.Visible = false;
                    btnSledeciUnos.Visible = false;
                }

                // Sjedinimo sve unose (posto neki dolaze iz trenutnog objekta, neki iz privremene liste)
                var sviUnosi = new List<UnosNovaApp>();
                if (trenutni.PoljaUnosi != null)
                    sviUnosi.AddRange(trenutni.PoljaUnosi);
                sviUnosi.AddRange(unosPodaci);

                // PROVERA OBAVEZNIH POLJA
                var obaveznaPolja = configData
                    .Where(c => c.Obavezno)
                    .Select(c => c.Naziv)
                    .ToList();

                var unesenaPolja = sviUnosi.Select(u => u.NazivPolja).Distinct().ToList();
                var nedostaju = obaveznaPolja.Except(unesenaPolja).ToList();

                if (nedostaju.Any())
                {
                    MessageBox.Show("Morate popuniti sva obavezna polja:\n" + string.Join(", ", nedostaju),
                                    "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Provera da ima bar jedan unos
                if (sviUnosi.Count == 0)
                {
                    MessageBox.Show("Niste uneli nijedan unos! Morate obraditi bar jedan check box pre čuvanja.",
                                    "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Dodaj sve privremene unose u trenutni PDF (metod DodajUnos spaja ponavljanja)
                foreach (var unos in unosPodaci)
                    trenutni.DodajUnos(unos);
                unosPodaci.Clear();

                // Novi naziv fajla ako je korisnik menjao
                if (chkMenjajNaziv.Checked && !string.IsNullOrWhiteSpace(txtNoviNaziv.Text))
                {
                    var novi = txtNoviNaziv.Text.Trim();
                    if (novi.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        novi = novi.Substring(0, novi.Length - 4);
                    trenutni.NewFileName = novi;
                }
                else
                {
                    trenutni.NewFileName = trenutni.FileName;
                }

                trenutni.DatumObrade = DateTime.Now;

                // Dodaj u zajedničku listu ako već nije tu
                if (!pdfFajloviZajednicki.Any(p => p.OriginalPath == trenutni.OriginalPath))
                    pdfFajloviZajednicki.Add(trenutni);

                // Premesti fajl u output folder
                pdfService.PremestiTrenutniPdfUFolder(outputPath);

                // Sačuvaj CSV odmah nakon premještanja
                csvServis.SacuvajPodatkeUCsv(pdfFajloviZajednicki, imeOperatera);

                // PROVERA: da li u input folderu ima još PDF fajlova
                bool imaJosPdfova = false;
                try
                {
                    if (!string.IsNullOrWhiteSpace(inputPath) && Directory.Exists(inputPath))
                    {
                        var files = Directory.GetFiles(inputPath, "*.pdf");
                        imaJosPdfova = files != null && files.Length > 0;
                    }
                }
                catch
                {
                    // ako ne možemo da čitamo folder, fallback na pdfService stanje
                    imaJosPdfova = pdfService.PdfFajlovi != null && pdfService.PdfFajlovi.Any(p => File.Exists(p.OriginalPath));
                }

                if (!imaJosPdfova)
                {
                    // Nema više fajlova — generiši izveštaj i obavesti korisnika
                    lblPdfNaziv.Text = "Nema više fajlova";
                    txtOpis.Visible = false;
                    txtNapomena.Visible = false;
                    btnSledeciUnos.Visible = false;

                    var izvestajServis = new IzvestajServis(
                        outputPath,
                        imeOperatera,
                        new ConfigData { CheckBoxovi = configData },
                        pdfFajloviZajednicki,
                        sessionStartTime);
                   

                    izvestajServis.GenerisiIzvestajExcel();

                    MessageBox.Show("Nema više PDF fajlova u input folderu.\nIzveštaj je automatski generisan.",
                                    "Kraj obrade", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                // Ako ima fajlova, pokušaj da pređemo na sledeći (postupak i prikaz)
                pdfService.PredjiNaSledeciFajl();

                if (pdfService.TrenutniPdf != null)
                {
                    pdfService.PrikaziTrenutniFajl(this.pdfPanel);
                    AzurirajUIPoslePromeneFajla();
                }
                else
                {
                    // Fallback: ako pdfService ne vrati sledeći iz nekog razloga, ponovo recheckaj input folder i eventualno generiši izveštaj
                    bool stillHave = false;
                    try
                    {
                        stillHave = !string.IsNullOrWhiteSpace(inputPath) && Directory.Exists(inputPath) &&
                                    Directory.GetFiles(inputPath, "*.pdf").Length > 0;
                    }
                    catch { }

                    if (!stillHave)
                    {
                        lblPdfNaziv.Text = "Nema više fajlova";
                        var izvestajServis = new IzvestajServis(
                            outputPath,
                            imeOperatera,
                            new ConfigData { CheckBoxovi = configData },
                            pdfFajloviZajednicki);
                        izvestajServis.GenerisiIzvestajExcel();

                        MessageBox.Show("Nema više PDF fajlova u input folderu.\nIzveštaj je automatski generisan.",
                                        "Kraj obrade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri čuvanju i prelasku: " + ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void chkMenjajNaziv_CheckedChanged(object sender, EventArgs e)
        {
            txtNoviNaziv.Visible = chkMenjajNaziv.Checked;
            if (chkMenjajNaziv.Checked)
                txtNoviNaziv.Focus();
        }

        private void MainForma_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                csvServis.SacuvajPodatkeUCsv(pdfFajloviZajednicki, imeOperatera);
                pdfService.OslobodiSvePdfResurse();
            }
            catch { }
        }
    }
}