using System;
using System.Drawing;
using System.Windows.Forms;
using PdfiumViewer;

namespace CheckBoXIndexAPP.Forms
{
    partial class MainForma
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // SplitContainer
            this.splitContainer1 = new SplitContainer();
            this.splitContainer1.Dock = DockStyle.Fill;

            // Left Table (3 rows)
            this.leftTable = new TableLayoutPanel();
            this.leftTable.ColumnCount = 1;
            this.leftTable.RowCount = 3;
            this.leftTable.Dock = DockStyle.Fill;
            this.leftTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 18F)); // Top
            this.leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 57F)); // Middle
            this.leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25F)); // Bottom

            // Top panel
            this.topPanel = new Panel();
            this.topPanel.Dock = DockStyle.Fill;

            this.lblPdfNaziv = new Label();
            this.lblPdfNaziv.Dock = DockStyle.Top;
            this.lblPdfNaziv.Height = 36;
            this.lblPdfNaziv.Text = "Naziv PDF fajla: (nije učitan)";
            this.lblPdfNaziv.TextAlign = ContentAlignment.MiddleLeft;
            this.lblPdfNaziv.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            this.chkMenjajNaziv = new CheckBox();
            this.chkMenjajNaziv.Dock = DockStyle.Top;
            this.chkMenjajNaziv.Height = 28;
            this.chkMenjajNaziv.Text = "Menjaj naziv";
            this.chkMenjajNaziv.CheckedChanged += new EventHandler(this.chkMenjajNaziv_CheckedChanged);

            this.txtNoviNaziv = new TextBox();
            this.txtNoviNaziv.Dock = DockStyle.Top;
            this.txtNoviNaziv.Height = 32;
            this.txtNoviNaziv.PlaceholderText = "Unesi novi naziv (bez .pdf)";
            this.txtNoviNaziv.Visible = false;

            this.topPanel.Controls.Add(this.txtNoviNaziv);
            this.topPanel.Controls.Add(this.chkMenjajNaziv);
            this.topPanel.Controls.Add(this.lblPdfNaziv);

            // Middle panel
            this.middlePanel = new Panel();
            this.middlePanel.Dock = DockStyle.Fill;

            // Panel za check boxove (scrollable)
            this.panelCheckBoxovi = new Panel();
            this.panelCheckBoxovi.Dock = DockStyle.Top;
            this.panelCheckBoxovi.Height = 150;
            this.panelCheckBoxovi.AutoScroll = true;

            // ComboBox za nazive polja
            this.cmbNaziviPolja = new ComboBox();
            this.cmbNaziviPolja.Dock = DockStyle.Top;
            this.cmbNaziviPolja.Height = 36;
            this.cmbNaziviPolja.DropDownStyle = ComboBoxStyle.DropDownList;

            // TextBox za opis i napomenu
            this.txtOpis = new TextBox();
            this.txtOpis.Dock = DockStyle.Top;
            this.txtOpis.Height = 36;
            this.txtOpis.PlaceholderText = "Opis...";
            this.txtOpis.Visible = false;

            this.txtNapomena = new TextBox();
            this.txtNapomena.Dock = DockStyle.Top;
            this.txtNapomena.Height = 36;
            this.txtNapomena.PlaceholderText = "Napomena...";
            this.txtNapomena.Visible = false;

            // Dugme sledeći unos
            this.btnSledeciUnos = new Button();
            this.btnSledeciUnos.Dock = DockStyle.Top;
            this.btnSledeciUnos.Height = 44;
            this.btnSledeciUnos.Text = "Sledeći unos (Enter)";
            styleBigButton(this.btnSledeciUnos);
            this.btnSledeciUnos.Visible = false;
            this.btnSledeciUnos.Click += new EventHandler(this.btnSacuvajPredji_Click);

            // Dodaj kontrole u middle panel
            this.middlePanel.Controls.Add(this.btnSledeciUnos);
            this.middlePanel.Controls.Add(this.txtNapomena);
            this.middlePanel.Controls.Add(this.txtOpis);
            this.middlePanel.Controls.Add(this.cmbNaziviPolja);
            this.middlePanel.Controls.Add(this.panelCheckBoxovi);

            // Bottom panel (2x2 buttons)
            this.bottomPanel = new TableLayoutPanel();
            this.bottomPanel.ColumnCount = 2;
            this.bottomPanel.RowCount = 2;
            this.bottomPanel.Dock = DockStyle.Fill;
            this.bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            this.bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.bottomPanel.Padding = new Padding(10);

            this.btnPrethodni = new Button();
            this.btnSledeci = new Button();
            this.btnIzvestaj = new Button();
            this.btnSacuvajPredji = new Button();

            styleBigButton(this.btnPrethodni);
            this.btnPrethodni.Text = "⟵ Prethodni";
            this.btnPrethodni.Dock = DockStyle.Fill;
            this.btnPrethodni.Click += new EventHandler(this.btnPrethodni_Click);

            styleBigButton(this.btnSledeci);
            this.btnSledeci.Text = "Sledeći ⟶";
            this.btnSledeci.Dock = DockStyle.Fill;
            this.btnSledeci.Click += new EventHandler(this.btnSledeci_Click);

            styleBigButton(this.btnIzvestaj);
            this.btnIzvestaj.Text = "Izveštaj";
            this.btnIzvestaj.Dock = DockStyle.Fill;
            this.btnIzvestaj.Click += new EventHandler(this.btnIzvestaj_Click);

            styleBigButton(this.btnSacuvajPredji);
            this.btnSacuvajPredji.Text = "Sačuvaj i pređi";
            this.btnSacuvajPredji.Dock = DockStyle.Fill;
            this.btnSacuvajPredji.Click += new EventHandler(this.btnSacuvajPredji_Click);

            this.bottomPanel.Controls.Add(this.btnPrethodni, 0, 0);
            this.bottomPanel.Controls.Add(this.btnSledeci, 1, 0);
            this.bottomPanel.Controls.Add(this.btnIzvestaj, 0, 1);
            this.bottomPanel.Controls.Add(this.btnSacuvajPredji, 1, 1);

            // PDF Panel
            this.pdfPanel = new Panel();
            this.pdfPanel.Dock = DockStyle.Fill;

            this.pdfViewer = new PdfiumViewer.PdfViewer();
            this.pdfViewer.Dock = DockStyle.Fill;
            this.pdfPanel.Controls.Add(this.pdfViewer);

            // Assemble leftTable
            this.leftTable.Controls.Add(this.topPanel, 0, 0);
            this.leftTable.Controls.Add(this.middlePanel, 0, 1);
            this.leftTable.Controls.Add(this.bottomPanel, 0, 2);

            // SplitContainer Panels
            this.splitContainer1.Panel1.Controls.Add(this.leftTable);
            this.splitContainer1.Panel2.Controls.Add(this.pdfPanel);

            // Main form
            this.ClientSize = new Size(1200, 800);
            this.Controls.Add(this.splitContainer1);
            this.Text = "CheckBox Index APP - Nova Forma";
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(this.MainForma_Load);
        }

        #endregion

        #region Helpers & Fields

        private void styleBigButton(Button btn)
        {
            btn.BackColor = Color.MidnightBlue;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btn.Margin = new Padding(8);
        }

        private SplitContainer splitContainer1;
        private TableLayoutPanel leftTable;
        private Panel topPanel;
        private Panel middlePanel;
        private Panel panelCheckBoxovi;
        private TableLayoutPanel bottomPanel;
        private Panel pdfPanel;

        private Label lblPdfNaziv;
        private CheckBox chkMenjajNaziv;
        private TextBox txtNoviNaziv;

        private ComboBox cmbNaziviPolja;
        private TextBox txtOpis;
        private TextBox txtNapomena;
        private Button btnSledeciUnos;

        private Button btnPrethodni;
        private Button btnSledeci;
        private Button btnIzvestaj;
        private Button btnSacuvajPredji;

        private PdfiumViewer.PdfViewer pdfViewer;

        #endregion
    }
}