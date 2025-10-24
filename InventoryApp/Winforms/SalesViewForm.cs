using InventoryApp.Repositories;
using MySql.Data.MySqlClient;
using System.Data;

namespace InventoryApp.WinForms
{
    public partial class SalesViewForm : Form
    {
        private readonly IClientRepository _clientRepo;
        private DataTable _salesTable = new();
        private DataTable _detailsTable = new();

        public SalesViewForm(IClientRepository clientRepo)
        {
            InitializeComponent();
            _clientRepo = clientRepo;
        }

        private async void SalesViewForm_Load(object sender, EventArgs e)
        {
            dateFrom.Value = DateTime.Now.AddDays(-30);
            dateTo.Value = DateTime.Now;

            await LoadClientsCombo();
            await LoadSales();
            SetupGrids();
        }

        private async System.Threading.Tasks.Task LoadClientsCombo()
        {
            var clients = await _clientRepo.GetAllAsync();
            cmbClientFilter.DataSource = clients;
            cmbClientFilter.DisplayMember = "Nombre";
            cmbClientFilter.ValueMember = "Id";
            cmbClientFilter.SelectedIndex = -1;
        }

        private async System.Threading.Tasks.Task LoadSales()
        {
            _salesTable = await GetSalesData();
            gridSales.DataSource = _salesTable;

            if (_salesTable.Rows.Count > 0)
            {
                gridSales.ClearSelection();
                gridDetails.DataSource = null;
            }
        }

        private async Task<DataTable> GetSalesData()
        {
            var table = new DataTable();
            using var con = new MySqlConnection("Server=localhost;Port=3306;Database=inventario_db;Uid=root;Pwd=Admin#2025;");

            var sql = new System.Text.StringBuilder(@"
                SELECT v.id, v.fecha, c.nombre as cliente, v.total 
                FROM venta v 
                INNER JOIN cliente c ON v.cliente_id = c.id 
                WHERE 1=1");

            var parameters = new List<MySqlParameter>();

            if (dateFrom.Checked)
            {
                sql.Append(" AND v.fecha >= @fechaFrom");
                parameters.Add(new MySqlParameter("@fechaFrom", dateFrom.Value.Date));
            }

            if (dateTo.Checked)
            {
                sql.Append(" AND v.fecha <= @fechaTo");
                parameters.Add(new MySqlParameter("@fechaTo", dateTo.Value.Date.AddDays(1).AddSeconds(-1)));
            }

            if (cmbClientFilter.SelectedValue != null && cmbClientFilter.SelectedValue is int clientId && clientId > 0)
            {
                sql.Append(" AND v.cliente_id = @clienteId");
                parameters.Add(new MySqlParameter("@clienteId", clientId));
            }

            sql.Append(" ORDER BY v.fecha DESC");

            using var cmd = new MySqlCommand(sql.ToString(), con);
            cmd.Parameters.AddRange(parameters.ToArray());

            using var adapter = new MySqlDataAdapter(cmd);
            await con.OpenAsync();
            adapter.Fill(table);

            return table;
        }

        private async Task<DataTable> GetSaleDetails(int saleId)
        {
            var table = new DataTable();
            using var con = new MySqlConnection("Server=localhost;Port=3306;Database=inventario_db;Uid=root;Pwd=Admin#2025;");

            var sql = @"
                SELECT p.nombre as producto, d.cantidad, d.precio_unit as precio, 
                       (d.cantidad * d.precio_unit) as subtotal
                FROM detalle_venta d
                INNER JOIN producto p ON d.producto_id = p.id
                WHERE d.venta_id = @ventaId";

            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@ventaId", saleId);

            using var adapter = new MySqlDataAdapter(cmd);
            await con.OpenAsync();
            adapter.Fill(table);

            return table;
        }

        private void SetupGrids()
        {
            gridSales.AutoGenerateColumns = true;
            gridSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridSales.ReadOnly = true;
            gridSales.MultiSelect = false;

            if (gridSales.Columns.Count > 0)
            {
                gridSales.Columns["id"].HeaderText = "ID Venta";
                gridSales.Columns["id"].Width = 80;
                gridSales.Columns["fecha"].HeaderText = "Fecha";
                gridSales.Columns["fecha"].Width = 150;
                gridSales.Columns["cliente"].HeaderText = "Cliente";
                gridSales.Columns["cliente"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gridSales.Columns["total"].HeaderText = "Total";
                gridSales.Columns["total"].DefaultCellStyle.Format = "N2";
                gridSales.Columns["total"].Width = 100;
            }

            gridDetails.AutoGenerateColumns = true;
            gridDetails.ReadOnly = true;
        }

        private async void gridSales_SelectionChanged(object sender, EventArgs e)
        {
            if (gridSales.CurrentRow?.DataBoundItem is DataRowView rowView)
            {
                int saleId = Convert.ToInt32(rowView["id"]);
                _detailsTable = await GetSaleDetails(saleId);
                gridDetails.DataSource = _detailsTable;

                if (gridDetails.Columns.Count > 0)
                {
                    gridDetails.Columns["producto"].HeaderText = "Producto";
                    gridDetails.Columns["producto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    gridDetails.Columns["cantidad"].HeaderText = "Cantidad";
                    gridDetails.Columns["cantidad"].Width = 80;
                    gridDetails.Columns["precio"].HeaderText = "Precio Unit.";
                    gridDetails.Columns["precio"].DefaultCellStyle.Format = "N2";
                    gridDetails.Columns["precio"].Width = 100;
                    gridDetails.Columns["subtotal"].HeaderText = "Subtotal";
                    gridDetails.Columns["subtotal"].DefaultCellStyle.Format = "N2";
                    gridDetails.Columns["subtotal"].Width = 100;
                }
            }
        }

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            await LoadSales();
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            dateFrom.Value = DateTime.Now.AddDays(-30);
            dateTo.Value = DateTime.Now;
            dateFrom.Checked = false;
            dateTo.Checked = false;
            cmbClientFilter.SelectedIndex = -1;
        }
    }
}