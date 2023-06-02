using ContentHub_RestApi_Assets.Models;

namespace ContentHub_RestApi_Assets
{
    public interface IContentHubService
    {
        Task<bool> GetAssets(string query, List<AssetModelDetail> assets);
    }
}
