namespace ContentHub_RestApi_Assets.Models
{
    public class AssetPropertyModel
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public AssetFileProperty FileProperties { get; set; }
    }
}
