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

            //SetupSyncTimer();
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

            _apiService.GetOrders();

            //SyncCategories();
            //SyncProducts();
            //SyncOrders();
        }

        private void SyncCategories()
        {
            DataTable categories = _databaseService.GetCategories();
            foreach (DataRow category in categories.Rows)
            {
                _apiService.SyncCategory(MapCategory(category));
            }
        }

        private void SyncProducts()
        {
            DataTable products = _databaseService.GetProducts();
            foreach (DataRow product in products.Rows)
            {
                _apiService.SyncProduct(MapProduct(product));
            }
        }

        private void SyncOrders()
        {
            DataTable orders = _databaseService.GetOrders();
            foreach (DataRow order in orders.Rows)
            {
                _apiService.SyncOrder(MapOrder(order));
            }
        }

        private Dictionary<string, object> MapCategory(DataRow row)
        {
            return new Dictionary<string, object>
            {
                //{ "id", row["CategoryID"] },
                { "name", row["CategoryName"] },
                { "description", row["Description"] }
            };
        }

        private Dictionary<string, object> MapProduct(DataRow row)
        {
            return new Dictionary<string, object>
            {
                { "id", row["ProductID"] },
                { "name", row["ProductName"] },
                { "category", row["Category"] },//name
                { "sub_category", row["SubCategory"] },
                { "brand", row["Brand"] },
                { "label", row["Label"] },
                { "shipping", row["Shipping"] },
                { "tax", row["Tax"] },
                { "price", row["Price"] },
                { "sale_price", row["SalePrice"] },
                { "description", row["Description"] },
                { "stock", row["StockQuantity"] },
                { "low_stock_threshold", row["LowStockThreshold"] },
                { "images", row["Images"] }
            };
        }

        private Dictionary<string, object> MapOrder(DataRow row)
        {
            return new Dictionary<string, object>
            {
                { "id", row["OrderID"] },
                { "product_order_id", row["ProductOrderID"] },
                { "order_date", row["OrderDate"] },
                { "customer_id", row["CustomerID"] },
                { "is_guest", row["IsGuest"] },
                { "product_id", row["ProductID"] },
                { "product_price", row["ProductPrice"] },
                { "coupon_price", row["CouponPrice"] },
                { "delivery_price", row["DeliveryPrice"] },
                { "tax_price", row["TaxPrice"] },
                { "final_price", row["FinalPrice"] },
                { "payment_type", row["PaymentType"] },
                { "payment_status", row["PaymentStatus"] },
                { "delivery_status", row["DeliveredStatus"] },
                { "delivery_date", row["DeliveryDate"] },
                { "return_status", row["ReturnStatus"] },
                { "return_date", row["ReturnDate"] },
                { "store_id", row["StoreID"] },
                { "created_at", row["CreatedAt"] },
                { "updated_at", row["UpdatedAt"] }
            };
        }
    }
}
