namespace InventoryApp.WinForms
{
    partial class ClientsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panel1;
        private Label label_clientes;
        private DataGridView dataGridClients;

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
            label_clientes = new Label();
            dataGridClients = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridClients).BeginInit();
            SuspendLayout();
            
            // panel1
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label_clientes);
            panel1.Controls.Add(dataGridClients);
            panel1.Location = new Point(3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(1136, 596);
            panel1.TabIndex = 0;
            
            // label_clientes
            label_clientes.AutoSize = true;
            label_clientes.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_clientes.Location = new Point(22, 25);
            label_clientes.Name = "label_clientes";
            label_clientes.Size = new Size(158, 50);
            label_clientes.TabIndex = 5;
            label_clientes.Text = "Clientes";
            
            // dataGridClients
            dataGridClients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridClients.Location = new Point(0, 188);
            dataGridClients.Name = "dataGridClients";
            dataGridClients.Size = new Size(1133, 396);
            dataGridClients.TabIndex = 0;
            
            // ClientsForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1140, 598);
            Controls.Add(panel1);
            Name = "ClientsForm";
            Text = "Gestión de Clientes";
            Load += ClientsForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridClients).EndInit();
            ResumeLayout(false);
        }
    }
}