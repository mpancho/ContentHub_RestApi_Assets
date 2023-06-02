using ClosedXML.Excel;
using ContentHub_RestApi_Assets.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContentHub_RestApi_Assets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentHubController : ControllerBase
    {
        private readonly IContentHubService _contentHubService;

        public ContentHubController(IContentHubService contentHubService)
        {
            _contentHubService = contentHubService;
        }

        [HttpGet(Name = "GetReportAssets")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var workbookResult = new XLWorkbook();
                List<AssetModelDetail> assets = new List<AssetModelDetail>();
                var worksheetResult = workbookResult.Worksheets.Add("Content Hub");
                int counter = 1;
                //Set the Excel column names
                worksheetResult.Cell("A" + counter).Value = "ID";
                worksheetResult.Cell("B" + counter).Value = "FileName";
                worksheetResult.Cell("C" + counter).Value = "Title";
                worksheetResult.Cell("D" + counter).Value = "Extension";

                var resultProcess = await _contentHubService.GetAssets("/api/entities/query?query=Definition.Name=='M.Asset'", assets);
                //Return true when the assets were retrieved sucessfully
                if(resultProcess)
                {
                    //Get the assets with value on the extension
                    var assetsWithExtension = assets.Where(item => item.Properties.FileProperties != null && item.Properties.FileProperties.Properties != null && item.Properties.FileProperties.Properties.Extension != null);
                    foreach (var asset in assetsWithExtension)
                    {
                        //For each asset that has extension, get the properties to populate the Excel object
                        counter++;
                        worksheetResult.Cell("A" + counter).Value = asset.Id;
                        worksheetResult.Cell("B" + counter).Value = asset.Properties.FileName;
                        worksheetResult.Cell("C" + counter).Value = asset.Properties.Title;
                        worksheetResult.Cell("D" + counter).Value = asset.Properties.FileProperties.Properties.Extension;
                    }
                    var stream = new MemoryStream();
                    workbookResult.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AssetsContentHubRESTApi.xlsx");
                }
                else
                {
                    return BadRequest("No Assets were retrieved");
                }               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occured: {ex.Message}");
                return BadRequest(ex);
            }            
        }
    }
}