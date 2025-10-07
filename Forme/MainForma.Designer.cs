using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckBoXIndexAPP.Forms
{
    partial class MainForma
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Glavni SplitContainer
            this.splitContainer1 = new SplitContainer();
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Orientation = Orientation.Vertical;
            this.splitContainer1.SplitterDistance = 450;

            // Leva strana
            this.leftTable = new TableLayoutPanel();
            this.leftTable.ColumnCount = 1;
            this.leftTable.RowCount = 3;
            this.leftTable.Dock = DockStyle.Fill;
            this.leftTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 18F));
            this.leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 57F));
            this.leftTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

            // Gornji panel
            this.topPanel = new Panel();
            this.topPanel.Dock = DockStyle.Fill;
            this.topPanel.Padding = new Padding(10);

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

            // Srednji panel
            this.middlePanel = new Panel();
            this.middlePanel.Dock = DockStyle.Fill;

            // TableLayoutPanel za middlePanel (filter + razmak + checkboxovi + unos)
            TableLayoutPanel middleTable = new TableLayoutPanel();
            middleTable.Dock = DockStyle.Fill;
            middleTable.RowCount = 4;
            middleTable.ColumnCount = 1;
            middleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F)); // filter panel
            middleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F)); // razmak
            middleTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // checkbox panel
            middleTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F)); // input panel
            middleTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            // Filter panel
            this.filterPanel = new Panel();
            this.filterPanel.Dock = DockStyle.Fill;
            this.filterPanel.Padding = new Padding(10, 5, 10, 5);

            // Panel sa checkboxovima
            this.panelCheckBoxovi = new Panel();
            this.panelCheckBoxovi.Dock = DockStyle.Fill;
            this.panelCheckBoxovi.AutoScroll = true;

            // Input panel
            Panel inputPanel = new Panel();
            inputPanel.Dock = DockStyle.Fill;
            inputPanel.Padding = new Padding(5);

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
            this.txtNapomena.KeyDown += new KeyEventHandler(this.txtNapomena_KeyDown);

            this.btnSledeciUnos = new Button();
            this.btnSledeciUnos.Dock = DockStyle.Bottom;
            this.btnSledeciUnos.Height = 44;
            this.btnSledeciUnos.Text = "Sledeći unos (Enter)";
            StyleBigButton(this.btnSledeciUnos);
            this.btnSledeciUnos.Visible = false;
            this.btnSledeciUnos.Click += new EventHandler(this.btnSledeciUnos_Click);

            inputPanel.Controls.Add(this.btnSledeciUnos);
            inputPanel.Controls.Add(this.txtNapomena);
            inputPanel.Controls.Add(this.txtOpis);

            middleTable.Controls.Add(this.filterPanel, 0, 0);
            middleTable.Controls.Add(this.panelCheckBoxovi, 0, 2);
            middleTable.Controls.Add(inputPanel, 0, 3);

            this.middlePanel.Controls.Add(middleTable);

            // Donji panel
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

            StyleBigButton(this.btnPrethodni);
            this.btnPrethodni.Text = "⟵ Prethodni";
            this.btnPrethodni.Dock = DockStyle.Fill;
            this.btnPrethodni.Click += new EventHandler(this.btnPrethodni_Click);

            StyleBigButton(this.btnSledeci);
            this.btnSledeci.Text = "Sledeći ⟶";
            this.btnSledeci.Dock = DockStyle.Fill;
            this.btnSledeci.Click += new EventHandler(this.btnSledeci_Click);

            StyleBigButton(this.btnIzvestaj);
            this.btnIzvestaj.Text = "Izveštaj";
            this.btnIzvestaj.Dock = DockStyle.Fill;
            this.btnIzvestaj.Click += new EventHandler(this.btnIzvestaj_Click);

            StyleBigButton(this.btnSacuvajPredji);
            this.btnSacuvajPredji.Text = "Sačuvaj i pređi";
            this.btnSacuvajPredji.Dock = DockStyle.Fill;
            this.btnSacuvajPredji.Click += new EventHandler(this.btnSacuvajPredji_Click);

            this.bottomPanel.Controls.Add(this.btnPrethodni, 0, 0);
            this.bottomPanel.Controls.Add(this.btnSledeci, 1, 0);
            this.bottomPanel.Controls.Add(this.btnIzvestaj, 0, 1);
            this.bottomPanel.Controls.Add(this.btnSacuvajPredji, 1, 1);

            // Desni panel
            this.pdfPanel = new Panel();
            this.pdfPanel.Dock = DockStyle.Fill;
            this.pdfPanel.BackColor = Color.DarkGray;

            // Povezivanje layouta
            this.splitContainer1.Panel1.Controls.Add(this.leftTable);
            this.splitContainer1.Panel2.Controls.Add(this.pdfPanel);

            this.leftTable.Controls.Add(this.topPanel, 0, 0);
            this.leftTable.Controls.Add(this.middlePanel, 0, 1);
            this.leftTable.Controls.Add(this.bottomPanel, 0, 2);

            // Glavna forma
            this.ClientSize = new Size(1100, 700);
            this.Controls.Add(this.splitContainer1);
            this.Text = "CheckBox Index APP";
            this.FormClosing += new FormClosingEventHandler(this.MainForma_FormClosing);
            this.Load += new EventHandler(this.MainForma_Load);
        }

        private void StyleBigButton(Button btn)
        {
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btn.BackColor = Color.LightSteelBlue;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Height = 44;
            btn.Margin = new Padding(5);
        }

        #endregion

        // Deklaracije kontrola
        private SplitContainer splitContainer1;
        private TableLayoutPanel leftTable;
        private Panel topPanel;
        private Panel middlePanel;
        private TableLayoutPanel bottomPanel;
        private Panel pdfPanel;

        private Label lblPdfNaziv;
        private CheckBox chkMenjajNaziv;
        private TextBox txtNoviNaziv;

        private Panel filterPanel;
        private Panel panelCheckBoxovi;
        private TextBox txtOpis;
        private TextBox txtNapomena;
        private Button btnSledeciUnos;

        private Button btnPrethodni;
        private Button btnSledeci;
        private Button btnIzvestaj;
        private Button btnSacuvajPredji;
    }
}