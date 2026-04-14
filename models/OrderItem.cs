 public class OrderItem
 {
     [JsonPropertyName("ASIN")]
     public string ASIN { get; set; }

     [JsonPropertyName("OrderItemId")]
     public string OrderItemId { get; set; }

     [JsonPropertyName("SellerSKU")]
     public string SellerSKU { get; set; }

     [JsonPropertyName("Title")]
     public string Title { get; set; }

     [JsonPropertyName("QuantityOrdered")]
     public int? QuantityOrdered { get; set; }

     [JsonPropertyName("QuantityShipped")]
     public int? QuantityShipped { get; set; }

     [JsonPropertyName("ProductInfo")]
     public ProductInfo ProductInfo { get; set; }

     [JsonPropertyName("ItemPrice")]
     public Money? ItemPrice { get; set; }

     [JsonPropertyName("ItemTax")]
     public Money? ItemTax { get; set; }

     [JsonPropertyName("PromotionDiscount")]
     public Money? PromotionDiscount { get; set; }

     [JsonPropertyName("IsGift")]
     public string IsGift { get; set; }

     [JsonPropertyName("ConditionId")]
     public string ConditionId { get; set; }

     [JsonPropertyName("ConditionSubtypeId")]
     public string ConditionSubtypeId { get; set; }

     [JsonPropertyName("IsTransparency")]
     [JsonConverter(typeof(FlexibleBoolConverter))]
     public bool IsTransparency { get; set; }

     [JsonPropertyName("SerialNumberRequired")]
     [JsonConverter(typeof(FlexibleBoolConverter))]
     public bool SerialNumberRequired { get; set; }

     [JsonPropertyName("IossNumber")]
     public string IossNumber { get; set; }

     [JsonPropertyName("DeemedResellerCategory")]
     public string DeemedResellerCategory { get; set; }

     [JsonPropertyName("StoreChainStoreId")]
     public string StoreChainStoreId { get; set; }

     [JsonPropertyName("BuyerRequestedCancel")]
     public BuyerRequestedCancel BuyerRequestedCancel { get; set; }
 }
