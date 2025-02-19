using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SyncApp
{
    public class AppResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Order> data { get; set; }
    }
    public class stock
    {
        public string product_code { get; set; }
        public int product_stock { get; set; }
    }




    public class OrderResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public List<Order> Data { get; set; }
    }

    public class Order
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("product_order_id")]
        public string ProductOrderId { get; set; }

        [JsonPropertyName("order_date")]
        public string OrderDate { get; set; }

        [JsonPropertyName("customer_id")]
        public int CustomerId { get; set; }

        [JsonPropertyName("is_guest")]
        public int IsGuest { get; set; }

        [JsonPropertyName("product_json")]
        public string ProductJson { get; set; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("product_price")]
        public decimal ProductPrice { get; set; }

        [JsonPropertyName("coupon_price")]
        public decimal CouponPrice { get; set; }

        [JsonPropertyName("delivery_price")]
        public decimal DeliveryPrice { get; set; }

        [JsonPropertyName("tax_price")]
        public decimal TaxPrice { get; set; }

        [JsonPropertyName("final_price")]
        public decimal FinalPrice { get; set; }

        [JsonPropertyName("return_price")]
        public decimal ReturnPrice { get; set; }

        [JsonPropertyName("payment_type")]
        public string PaymentType { get; set; }

        [JsonPropertyName("payment_status")]
        public string PaymentStatus { get; set; }

        [JsonPropertyName("delivery_id")]
        public int DeliveryId { get; set; }

        [JsonPropertyName("delivered_status")]
        public int DeliveredStatus { get; set; }

        [JsonPropertyName("delivery_date")]
        public string DeliveryDate { get; set; }

        [JsonPropertyName("return_status")]
        public int ReturnStatus { get; set; }

        [JsonPropertyName("return_date")]
        public string ReturnDate { get; set; }

        [JsonPropertyName("reward_points")]
        public int RewardPoints { get; set; }

        [JsonPropertyName("theme_id")]
        public string ThemeId { get; set; }

        [JsonPropertyName("store_id")]
        public int StoreId { get; set; }

        [JsonPropertyName("items")]
        public List<OrderItem> Items { get; set; }

        [JsonPropertyName("order_id_string")]
        public string OrderIdString { get; set; }

        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
    }

    public class OrderItem
    {
        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        //[JsonPropertyName("image")]
        //public string Image { get; set; }

        [JsonPropertyName("qty")]
        public int Quantity { get; set; }

        //[JsonPropertyName("orignal_price")]
        //public decimal OriginalPrice { get; set; }

        //[JsonPropertyName("final_price")]
        //public decimal FinalPrice { get; set; }

        //[JsonPropertyName("total_orignal_price")]
        //public decimal TotalOriginalPrice { get; set; }

        //[JsonPropertyName("variant_id")]
        //public int VariantId { get; set; }

        //[JsonPropertyName("variant_name")]
        //public string VariantName { get; set; }
    }


}
