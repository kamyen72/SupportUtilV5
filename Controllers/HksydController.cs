using DupRecRemoval.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Reflection.Metadata;
using static DupRecRemoval.Controllers.UtilController;

using Microsoft.AspNetCore.Hosting;
using System.Web;
using DupRecRemoval.Classes;

using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using SupportUtil.Classes;
using SupportUtilV2.Classes;
using SupportUtilV3.Classes;
using DocumentFormat.OpenXml.Spreadsheet;
using SupportUtilV4.Classes;

namespace DupRecRemoval.Controllers
{
    [Route("/[controller]")]
    //[UtilController]
    public class HksydController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public HksydController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [EnableCors("AllowAll")]
        [Route("haziqtest2")]
        [HttpPost]
        public IActionResult haziqtest2([FromBody] InputModel model)
        {
            ReturnModel rm = new ReturnModel();
            rm.ReturnText = "Haziq Testing";

            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetConnStr")]
        [HttpPost]
        public IActionResult GetConnStr([FromBody] InputModel model)
        {
            var dbname = model.InputText;
            DBUtil dbu = new DBUtil();
            ReturnModel rm = new ReturnModel();
            rm.ReturnText = dbu.GetConnStr(dbname);

            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }


    }
}
