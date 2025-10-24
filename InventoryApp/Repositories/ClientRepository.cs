using InventoryApp.Domain;
using InventoryApp.Infrastructure;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace InventoryApp.Repositories
{
    public class ClientRepository : IClientRepository
    {
        public async Task<List<Client>> GetAllAsync()
        {
            var list = new List<Client>();
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("SELECT id, nombre, email, telefono, direccion, nit FROM cliente ORDER BY nombre", con);
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Client
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Email = rd.IsDBNull(rd.GetOrdinal("email")) ? "" : rd.GetString("email"),
                    Telefono = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono"),
                    Direccion = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion"),
                    Nit = rd.IsDBNull(rd.GetOrdinal("nit")) ? "" : rd.GetString("nit")
                });
            }
            return list;
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("SELECT id, nombre, email, telefono, direccion, nit FROM cliente WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Client 
                { 
                    Id = rd.GetInt32("id"), 
                    Nombre = rd.GetString("nombre"),
                    Email = rd.IsDBNull(rd.GetOrdinal("email")) ? "" : rd.GetString("email"),
                    Telefono = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono"),
                    Direccion = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion"),
                    Nit = rd.IsDBNull(rd.GetOrdinal("nit")) ? "" : rd.GetString("nit")
                };
            }
            return null;
        }

        public async Task<int> InsertAsync(Client c)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "INSERT INTO cliente (nombre, email, telefono, direccion, nit) VALUES (@n, @e, @t, @d, @nit); SELECT LAST_INSERT_ID();", con);
            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@e", string.IsNullOrEmpty(c.Email) ? DBNull.Value : c.Email);
            cmd.Parameters.AddWithValue("@t", string.IsNullOrEmpty(c.Telefono) ? DBNull.Value : c.Telefono);
            cmd.Parameters.AddWithValue("@d", string.IsNullOrEmpty(c.Direccion) ? DBNull.Value : c.Direccion);
            cmd.Parameters.AddWithValue("@nit", string.IsNullOrEmpty(c.Nit) ? DBNull.Value : c.Nit);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<bool> UpdateAsync(Client c)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(
                "UPDATE cliente SET nombre=@n, email=@e, telefono=@t, direccion=@d, nit=@nit WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@n", c.Nombre);
            cmd.Parameters.AddWithValue("@e", string.IsNullOrEmpty(c.Email) ? DBNull.Value : c.Email);
            cmd.Parameters.AddWithValue("@t", string.IsNullOrEmpty(c.Telefono) ? DBNull.Value : c.Telefono);
            cmd.Parameters.AddWithValue("@d", string.IsNullOrEmpty(c.Direccion) ? DBNull.Value : c.Direccion);
            cmd.Parameters.AddWithValue("@nit", string.IsNullOrEmpty(c.Nit) ? DBNull.Value : c.Nit);
            cmd.Parameters.AddWithValue("@id", c.Id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("DELETE FROM cliente WHERE id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<Client?> GetByNitAsync(string nit)
        {
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand("SELECT id, nombre, email, telefono, direccion, nit FROM cliente WHERE nit=@nit", con);
            cmd.Parameters.AddWithValue("@nit", nit);
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Client 
                { 
                    Id = rd.GetInt32("id"), 
                    Nombre = rd.GetString("nombre"),
                    Email = rd.IsDBNull(rd.GetOrdinal("email")) ? "" : rd.GetString("email"),
                    Telefono = rd.IsDBNull(rd.GetOrdinal("telefono")) ? "" : rd.GetString("telefono"),
                    Direccion = rd.IsDBNull(rd.GetOrdinal("direccion")) ? "" : rd.GetString("direccion"),
                    Nit = rd.IsDBNull(rd.GetOrdinal("nit")) ? "" : rd.GetString("nit")
                };
            }
            return null;
        }
    }
}