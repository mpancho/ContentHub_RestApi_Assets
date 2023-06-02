using Newtonsoft.Json;
using ContentHub_RestApi_Assets.Models;

namespace ContentHub_RestApi_Assets
{
    public class ContentHubService: IContentHubService
    {
        private static readonly HttpClient client;

        static ContentHubService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://sitecoresandbox.cloud"),
                
            };
            client.DefaultRequestHeaders.Add("x-auth-token", "xauthtokenstring");
        }

        /// <summary>
        /// Get the assets from the REST API executing a query with pagination (25 items by default). Execute recursively the method to get all assets
        /// </summary>
        /// <param name="query"></param>
        /// <param name="assets">List with the assets retrieved from the API Service</param>
        /// <returns>True for process sucessed otherwise false </returns>
        public async Task<bool> GetAssets(string query, List<AssetModelDetail> assets)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                {
                    var assetInitial = await GetAssetsInner(query);

                    if (assetInitial != null && assetInitial?.Next?.Href != null)
                    {
                        assets.AddRange(assetInitial.Items);
                        await GetAssets(assetInitial.Next.Href, assets);
                    }
                    return true;
                }
                else
                { 
                    return false; 
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Execute the query and convert to the AssetModel object
        /// </summary>
        /// <param name="query">Query to get the assets</param>
        /// <returns>Asset Model object</returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task<AssetModel> GetAssetsInner(string query)
        {
            try
            {
                //Execute the query
                var response = await client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync(); 
                    //Convert the response to an AssetModel object
                    return JsonConvert.DeserializeObject<AssetModel>(stringResponse);
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }        
    }
}
