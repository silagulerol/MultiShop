namespace MultiShop.RapidApi.Models
{
    public class ECommerceViewModel
    {

        public class Rootobject
        {
            public string status { get; set; }
            public string request_id { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Product[] products { get; set; }
            public object[] sponsored_products { get; set; }
            public Filter[] filters { get; set; }
        }

        public class Product
        {
            public string product_id { get; set; }
            public string product_title { get; set; }
            public object product_description { get; set; }
            public object[] product_photos { get; set; }
            public object[] product_videos { get; set; }
            public Product_Attributes product_attributes { get; set; }
            public object product_rating { get; set; }
            public string product_page_url { get; set; }
            public object product_num_reviews { get; set; }
            public object product_num_offers { get; set; }
            public string[] typical_price_range { get; set; }
            public Current_Product_Variant_Properties current_product_variant_properties { get; set; }
            public Product_Variants product_variants { get; set; }
            public Reviews_Insights reviews_insights { get; set; }
            public object offer { get; set; }
        }

        public class Product_Attributes
        {
        }

        public class Current_Product_Variant_Properties
        {
        }

        public class Product_Variants
        {
        }

        public class Reviews_Insights
        {
        }

        public class Filter
        {
            public string title { get; set; }
            public bool multivalue { get; set; }
            public Value[] values { get; set; }
        }

        public class Value
        {
            public string title { get; set; }
            public string q { get; set; }
            public string shoprs { get; set; }
        }

    }
}
