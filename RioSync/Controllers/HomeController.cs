using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RioSync.Models;


namespace RioSync.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Index(FormModel model)
        {
            IFormFile file = model.file;

           string Path = "";


            StringBuilder sb = new StringBuilder();
            GetProdRioData("5633", "5947");


            if (file.Length > 0)
            {
                string sFileExtension = ".xls";
                 ISheet sheet;
                string fullPath = file.FileName;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    sb.Append("<table  id='datatable'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }
                        sb.AppendLine("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            ViewBag.HtmlStr = sb.ToString();
            return View();
            
        }
        private RioLocation GetProdRioData(string storenumber, string brand)
        {
            var response = new RioLocation();
            var rioGetLocationUri = "https://sxl1yffjr2.execute-api.us-east-1.amazonaws.com/prod/locations/5633?did=5947";
            var rioDid = "5947";
            var rioAPiKey = "UkuDT8mUt05NwBE903Jgy6F3VSrELdkRHTBJDm1i";
            string riourl = string.Format(rioGetLocationUri, storenumber, rioDid);
            string responseMessage = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(riourl);
            request.PreAuthenticate = true;
            request.Method = "GET";
            request.ContentType = "application/json;charset=utf-8";
            request.Headers.Add("x-api-key", rioAPiKey);
            WebResponse apiresponse = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = apiresponse.GetResponseStream();
            // Read the content.
            if (((HttpWebResponse)apiresponse).StatusDescription.ToLower() == "OK".ToLower())
            {
                using (var stream = apiresponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    responseMessage = reader.ReadToEnd();
                    response = JsonConvert.DeserializeObject<RioLocation>(responseMessage);
                }
            }
            return response;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
