using System.Data;
using System.Windows.Forms.Design;
using System.Timers;
using Timer = System.Timers.Timer;
using System;

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

            //_apiService.GetOrders();

            //SyncCategories();
            SyncProducts();
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

        private async void SyncProducts()
        {
            int Created = 0;
            DataTable products = _databaseService.GetProducts();
            foreach (DataRow product in products.Rows)
            {
                var mapped_product = MapProduct(product);
                var res = await _apiService.SyncProduct(mapped_product);
                if (res == "Created") Created++;
                rtbResult.Text += res + "\n";
            }
            rtbResult.Text += Created + "\n";
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
            var DD= new Dictionary<string, object>
            {
                { "guid", row["guid"] },
                { "name", Convert.IsDBNull(row["NameE"]) ? row["NameA"] : row["NameE"] },
                { "name_ar", Convert.IsDBNull(row["NameA"]) ? "" : row["NameA"] },
                { "category", Convert.IsDBNull(row["MainCategory"]) ? "" : row["MainCategory"] },
                { "sub_category", Convert.IsDBNull(row["SubCategory"]) ? "" : row["SubCategory"] },
                { "tax", Convert.IsDBNull(row["VAT"]) ? 0 : row["VAT"] },
                { "price", Convert.IsDBNull(row["U1Price1"]) ? 0 : row["U1Price1"] },
                { "description", Convert.IsDBNull(row["Spec"]) ? "" : row["Spec"] },
                //{ "images", Convert.IsDBNull(row["Mat_Image"]) ? "" : row["Mat_Image"] }



                //{ "brand", Convert.IsDBNull(row["MatCompany_ID"]) ? "" : row["MatCompany_ID"] },
                //{ "label", Convert.IsDBNull(row["Code"]) ? "" : row["Code"] },
                //{ "shipping", Convert.IsDBNull(row["Location"]) ? "" : row["Location"] },
                //{ "sale_price", Convert.IsDBNull(row["U1Price1"]) ? 0 : row["U1Price1"] },
                //{ "stock", Convert.IsDBNull(row["MinQty"]) ? 0 : row["MinQty"] },
                //{ "low_stock_threshold", Convert.IsDBNull(row["OrderQty"]) ? 0 : row["OrderQty"] },
            };

            if (!Convert.IsDBNull(row["Mat_Image"]))
            {
                DD["images"] = new List<string>() { "data:image/png;base64," + Convert.ToBase64String((byte[])row["Mat_Image"]) };
            }

            return DD;
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
