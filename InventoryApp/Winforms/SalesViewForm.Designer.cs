namespace InventoryApp.WinForms
{
    partial class SalesViewForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panel1;
        private Label label1;
        private DataGridView gridSales;
        private DataGridView gridDetails;
        private GroupBox groupBox1;
        private DateTimePicker dateFrom;
        private DateTimePicker dateTo;
        private Label label2;
        private Label label3;
        private ComboBox cmbClientFilter;
        private Label label4;
        private Button btnFilter;
        private Button btnClearFilter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            btnClearFilter = new Button();
            btnFilter = new Button();
            cmbClientFilter = new ComboBox();
            label4 = new Label();
            dateTo = new DateTimePicker();
            label3 = new Label();
            dateFrom = new DateTimePicker();
            label2 = new Label();
            gridDetails = new DataGridView();
            gridSales = new DataGridView();
            label1 = new Label();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridDetails).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridSales).BeginInit();
            SuspendLayout();
            
            // panel1
            panel1.BackColor = Color.White;
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(gridDetails);
            panel1.Controls.Add(gridSales);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(2, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1200, 700);
            panel1.TabIndex = 0;
            
            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(22, 25);
            label1.Name = "label1";
            label1.Size = new Size(279, 50);
            label1.TabIndex = 0;
            label1.Text = "Visualizar Ventas";
            
            // gridSales
            gridSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridSales.Location = new Point(22, 100);
            gridSales.Name = "gridSales";
            gridSales.Size = new Size(1150, 200);
            gridSales.TabIndex = 1;
            gridSales.SelectionChanged += gridSales_SelectionChanged;
            
            // gridDetails
            gridDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridDetails.Location = new Point(22, 350);
            gridDetails.Name = "gridDetails";
            gridDetails.Size = new Size(1150, 300);
            gridDetails.TabIndex = 2;
            
            // groupBox1
            groupBox1.Controls.Add(btnClearFilter);
            groupBox1.Controls.Add(btnFilter);
            groupBox1.Controls.Add(cmbClientFilter);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(dateTo);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(dateFrom);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new Point(400, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(772, 80);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filtros";
            
            // label2
            label2.AutoSize = true;
            label2.Location = new Point(15, 25);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 0;
            label2.Text = "Desde:";
            
            // dateFrom
            dateFrom.Format = DateTimePickerFormat.Short;
            dateFrom.Location = new Point(60, 21);
            dateFrom.Name = "dateFrom";
            dateFrom.Size = new Size(100, 23);
            dateFrom.TabIndex = 1;
            
            // label3
            label3.AutoSize = true;
            label3.Location = new Point(175, 25);
            label3.Name = "label3";
            label3.Size = new Size(40, 15);
            label3.TabIndex = 2;
            label3.Text = "Hasta:";
            
            // dateTo
            dateTo.Format = DateTimePickerFormat.Short;
            dateTo.Location = new Point(220, 21);
            dateTo.Name = "dateTo";
            dateTo.Size = new Size(100, 23);
            dateTo.TabIndex = 3;
            
            // label4
            label4.AutoSize = true;
            label4.Location = new Point(335, 25);
            label4.Name = "label4";
            label4.Size = new Size(47, 15);
            label4.TabIndex = 4;
            label4.Text = "Cliente:";
            
            // cmbClientFilter
            cmbClientFilter.FormattingEnabled = true;
            cmbClientFilter.Location = new Point(385, 21);
            cmbClientFilter.Name = "cmbClientFilter";
            cmbClientFilter.Size = new Size(200, 23);
            cmbClientFilter.TabIndex = 5;
            
            // btnFilter
            btnFilter.Location = new Point(600, 20);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(75, 23);
            btnFilter.TabIndex = 6;
            btnFilter.Text = "Filtrar";
            btnFilter.UseVisualStyleBackColor = true;
            btnFilter.Click += btnFilter_Click;
            
            // btnClearFilter
            btnClearFilter.Location = new Point(680, 20);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(75, 23);
            btnClearFilter.TabIndex = 7;
            btnClearFilter.Text = "Limpiar";
            btnClearFilter.UseVisualStyleBackColor = true;
            btnClearFilter.Click += btnClearFilter_Click;
            
            // SalesViewForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1204, 704);
            Controls.Add(panel1);
            Name = "SalesViewForm";
            Text = "Visualización de Ventas";
            Load += SalesViewForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridDetails).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridSales).EndInit();
            ResumeLayout(false);
        }
    }
}