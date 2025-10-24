using InventoryApp.Repositories;
using InventoryApp.Services;
using InventoryApp.WinForms;

namespace InventoryApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var productRepo = new ProductRepository();
            var clientRepo = new ClientRepository();
            var saleRepo = new SaleRepository();
            var salesService = new SalesService(productRepo, saleRepo);

            Application.Run(new MainForm(productRepo, clientRepo, salesService));
        }
    }
}