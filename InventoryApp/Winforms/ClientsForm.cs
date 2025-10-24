using InventoryApp.Domain;
using InventoryApp.Repositories;
using System.Data;

namespace InventoryApp.WinForms
{
    public partial class ClientsForm : Form
    {
        private readonly IClientRepository _clientRepo;
        private DataTable _table = new();
        private readonly BindingSource _bs = new();
        private bool _persisting = false;

        public ClientsForm(IClientRepository clientRepo)
        {
            InitializeComponent();
            _clientRepo = clientRepo;
        }

        private async void ClientsForm_Load(object sender, EventArgs e)
        {
            await LoadTableAsync();
            SetupGrid();
            SetupContextMenu();

            dataGridClients.CellValidating += dataGridClients_CellValidating;
            dataGridClients.DataError += (s, ev) => { ev.ThrowException = false; };
            dataGridClients.CellValidated += dataGridClients_CellValidated;
            dataGridClients.UserDeletingRow += dataGridClients_UserDeletingRow;
        }

        private async System.Threading.Tasks.Task LoadTableAsync()
        {
            _table = BuildSchema();
            var clients = await _clientRepo.GetAllAsync();
            
            foreach (var c in clients)
            {
                var r = _table.NewRow();
                r["id"] = c.Id;
                r["nombre"] = c.Nombre;
                r["email"] = c.Email;
                r["telefono"] = c.Telefono;
                r["direccion"] = c.Direccion;
                _table.Rows.Add(r);
            }

            _table.AcceptChanges();
            _bs.DataSource = _table;
            dataGridClients.DataSource = _bs;
        }

        private static DataTable BuildSchema()
        {
            var t = new DataTable("cliente");
            var cId = t.Columns.Add("id", typeof(int));
            cId.AllowDBNull = true;
            cId.Unique = false;

            t.Columns.Add("nombre", typeof(string));
            t.Columns.Add("email", typeof(string));
            t.Columns.Add("telefono", typeof(string));
            t.Columns.Add("direccion", typeof(string));

            return t;
        }

        private void SetupGrid()
        {
            dataGridClients.AutoGenerateColumns = true;
            dataGridClients.AllowUserToAddRows = true;
            dataGridClients.AllowUserToDeleteRows = true;
            dataGridClients.MultiSelect = false;
            dataGridClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridClients.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridClients.Columns["id"] is DataGridViewColumn idCol)
            {
                idCol.HeaderText = "ID";
                idCol.ReadOnly = true;
                idCol.Width = 70;
            }
            if (dataGridClients.Columns["nombre"] is DataGridViewColumn nomCol)
            {
                nomCol.HeaderText = "Nombre Completo";
                nomCol.ReadOnly = false;
            }
            if (dataGridClients.Columns["email"] is DataGridViewColumn emailCol)
            {
                emailCol.HeaderText = "Email";
                emailCol.ReadOnly = false;
            }
            if (dataGridClients.Columns["telefono"] is DataGridViewColumn telCol)
            {
                telCol.HeaderText = "Teléfono";
                telCol.ReadOnly = false;
            }
            if (dataGridClients.Columns["direccion"] is DataGridViewColumn dirCol)
            {
                dirCol.HeaderText = "Dirección";
                dirCol.ReadOnly = false;
            }
        }

        private void SetupContextMenu()
        {
            var ctx = new ContextMenuStrip();
            var miEliminar = new ToolStripMenuItem("Eliminar");
            miEliminar.Click += async (s, ev) =>
            {
                if (dataGridClients.CurrentRow?.DataBoundItem is not DataRowView drv) return;
                await DeleteRowAsync(drv, confirm: true);
            };
            ctx.Items.Add(miEliminar);
            dataGridClients.ContextMenuStrip = ctx;
        }

        private void dataGridClients_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var colName = dataGridClients.Columns[e.ColumnIndex].Name;
            var value = e.FormattedValue?.ToString() ?? "";

            if (colName == "nombre")
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    e.Cancel = true;
                    dataGridClients.Rows[e.RowIndex].ErrorText = "El nombre es requerido.";
                }
                else dataGridClients.Rows[e.RowIndex].ErrorText = string.Empty;
            }
            else if (colName == "email")
            {
                if (!string.IsNullOrWhiteSpace(value) && !IsValidEmail(value))
                {
                    e.Cancel = true;
                    dataGridClients.Rows[e.RowIndex].ErrorText = "Formato de email inválido.";
                }
                else dataGridClients.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async void dataGridClients_CellValidated(object? sender, DataGridViewCellEventArgs e)
        {
            if (_persisting) return;
            if (e.RowIndex < 0 || e.RowIndex >= dataGridClients.Rows.Count) return;

            var gridRow = dataGridClients.Rows[e.RowIndex];
            if (gridRow.IsNewRow) return;

            dataGridClients.EndEdit();
            _bs.EndEdit();

            if (gridRow.DataBoundItem is not DataRowView drv) return;
            var row = drv.Row;

            if (IsNullOrEmpty(row, "nombre") && 
                IsNullOrEmpty(row, "email") && 
                IsNullOrEmpty(row, "telefono") && 
                IsNullOrEmpty(row, "direccion"))
                return;

            try
            {
                _persisting = true;

                if ((row.RowState == DataRowState.Added || row["id"] == DBNull.Value || ToInt(row["id"]) == 0)
                    && IsValidRow(row))
                {
                    var c = new Client
                    {
                        Nombre = row["nombre"]?.ToString() ?? "",
                        Email = row["email"]?.ToString() ?? "",
                        Telefono = row["telefono"]?.ToString() ?? "",
                        Direccion = row["direccion"]?.ToString() ?? ""
                    };

                    int newId = await _clientRepo.InsertAsync(c);
                    row["id"] = newId;
                    row.AcceptChanges();
                    return;
                }

                if (row.RowState == DataRowState.Modified && IsValidRow(row))
                {
                    int id = ToInt(row["id"]);
                    if (id > 0)
                    {
                        var c = new Client
                        {
                            Id = id,
                            Nombre = row["nombre"]?.ToString() ?? "",
                            Email = row["email"]?.ToString() ?? "",
                            Telefono = row["telefono"]?.ToString() ?? "",
                            Direccion = row["direccion"]?.ToString() ?? ""
                        };

                        var ok = await _clientRepo.UpdateAsync(c);
                        if (ok) row.AcceptChanges();
                        else row.RowError = "No se pudo actualizar en BD.";
                    }
                }
            }
            catch (Exception ex)
            {
                row.RowError = ex.Message;
                MessageBox.Show("Error al persistir: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _persisting = false;
            }
        }

        private async void dataGridClients_UserDeletingRow(object? sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row?.DataBoundItem is not DataRowView drv) return;

            var resp = MessageBox.Show("¿Eliminar este cliente?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) { e.Cancel = true; return; }

            var ok = await DeleteRowAsync(drv, confirm: false);
            if (!ok) e.Cancel = true;
        }

        private async System.Threading.Tasks.Task<bool> DeleteRowAsync(DataRowView drv, bool confirm)
        {
            if (_persisting) return false;
            var row = drv.Row;

            try
            {
                _persisting = true;

                if (row.RowState == DataRowState.Added || row["id"] == DBNull.Value || ToInt(row["id"]) == 0)
                {
                    row.Delete();
                    return true;
                }

                int id = ToInt(row["id"]);
                if (id <= 0) return false;

                if (confirm)
                {
                    var okConf = MessageBox.Show($"¿Eliminar el cliente #{id}?", "Confirmar",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    if (!okConf) return false;
                }

                var ok = await _clientRepo.DeleteAsync(id);
                if (ok)
                {
                    row.Delete();
                    return true;
                }

                MessageBox.Show("No se pudo eliminar en BD (¿referenciado en ventas?).",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                _persisting = false;
            }
        }

        private static bool IsValidRow(DataRow r)
            => !IsNullOrEmpty(r, "nombre");

        private static bool IsNullOrEmpty(DataRow r, string col)
            => !r.Table.Columns.Contains(col) || r[col] == DBNull.Value || string.IsNullOrWhiteSpace(r[col]?.ToString());

        private static int ToInt(object? o)
            => o == null || o == DBNull.Value ? 0 : int.TryParse(o.ToString(), out var i) ? i : 0;
    }
}