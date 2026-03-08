namespace MultiShop.Catalog.Dtos.FeatureSliderDtos
{
    public class UpdateFeatureSliderDto
    {
        public string FeatureSliderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        // UI'da en fazla 3 tane Feature bulunabilir. Bu yüzden active olanları belirlemek için status property'si ekledik. 
        public bool Status { get; set; }
    }
}
