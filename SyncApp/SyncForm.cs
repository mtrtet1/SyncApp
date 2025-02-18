using System.Data;
using System.Windows.Forms.Design;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SyncApp
{
    public partial class SyncForm : Form
    {
        private readonly DatabaseService _databaseService;
        private readonly ApiService _apiService;
        private readonly ConfigService _configService;
        private Timer _syncTimer;
        public SyncForm()
        {
            InitializeComponent();
            _configService = ConfigService.LoadConfig();
            _databaseService = new DatabaseService(_configService.DbConnectionString);
            _apiService = new ApiService(_configService.ApiUrl, _configService.ApiToken);

            SetupSyncTimer();
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            StartSync();
        }

        private void SetupSyncTimer()
        {
            _syncTimer = new Timer(_configService.SyncInterval * 60000); // Convert minutes to milliseconds
            _syncTimer.Elapsed += (s, e) => StartSync();
            _syncTimer.Start();
        }

        private void StartSync()
        {
            SyncCategories();
            SyncProducts();
            SyncOrders();
        }

        private void SyncCategories()
        {
            DataTable categories = _databaseService.GetCategories();
            foreach (DataRow category in categories.Rows)
            {
                _apiService.SyncCategory(category);
            }
        }

        private void SyncProducts()
        {
            DataTable products = _databaseService.GetProducts();
            foreach (DataRow product in products.Rows)
            {
                _apiService.SyncProduct(product);
            }
        }

        private void SyncOrders()
        {
            DataTable orders = _databaseService.GetOrders();
            foreach (DataRow order in orders.Rows)
            {
                _apiService.SyncOrder(order);
            }
        }
    }
}
