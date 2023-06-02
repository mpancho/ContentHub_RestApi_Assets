namespace ContentHub_RestApi_Assets.Models
{
    public class AssetModel
    {
        public List<AssetModelDetail> Items { get; set; }
        public int Total_Items { get; set; }
        public int Returned_Items { get; set; }
        public AssetModelNavigation Next { get; set; }
        public AssetModelNavigation Previous { get; set; }
        public AssetModelNavigation Self { get; set; }
    }
}
