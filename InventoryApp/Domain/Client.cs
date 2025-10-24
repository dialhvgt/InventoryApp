namespace InventoryApp.Domain
{
    public class Client
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Nit { get; set; } = "";
    }
}