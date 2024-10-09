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
    public class UtilController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UtilController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [EnableCors("AllowAll")]
        [Route("CreateNewRoot")]
        [HttpPost]
        public IActionResult CreateNewRoot([FromBody] MenuItemInput model)
        {
            var myText = model.text;
            var mySeq = model.Squence;
            var myurl = model.url;

            //MenuItemInput minp = new MenuItemInput();


            DBUtil dbu = new DBUtil();

            ReturnModel rm = new ReturnModel();
            rm.ReturnText = dbu.AddMenuRoot(model);

            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("DeleteMenuItem")]
        [HttpPost]
        public IActionResult DeleteMenuItem([FromBody] InputModel model)
        {
            var myID = model.InputText;

            DBUtil dbu = new DBUtil();

            ReturnModel rm = new ReturnModel();
            rm.ReturnText = dbu.DeleteMenuItem(myID);

            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("EditMenuRoot")]
        [HttpPost]
        public IActionResult EditMenuRoot([FromBody] MenuItemInput model)
        {
            DBUtil dbu = new DBUtil();

            var id = model.mID;

            ReturnModel rm = new ReturnModel();
            rm.ReturnText = dbu.EditMenuRoot(model);

            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetMenuChildItems")]
        [HttpPost]
        public IActionResult GetMenuChildItems([FromBody] InputModel model)
        {
            var myID = model.InputText;

            DBUtil dbu = new DBUtil();

            List<MenuItem> list = dbu.GetMenuChildItems(myID);

            string rJason = JsonConvert.SerializeObject(list);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("AddMenuChildItem")]
        [HttpPost]
        public IActionResult AddMenuChildItem([FromBody] MenuItemInput model)
        {
            var myText = model.text;
            var mySeq = model.Squence;
            var myurl = model.url;
            var myparentid = model.ParentID;

            //MenuItemInput minp = new MenuItemInput();


            DBUtil dbu = new DBUtil();

            ReturnModel rm = new ReturnModel();
            rm.ReturnText = dbu.AddMenuChildItem(model);

            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("CheckApid")]
        [HttpPost]
        public IActionResult CheckApid([FromBody] InputModel model)
        {
            string CurrentPeriod = model.InputText;

            DBUtil dbu = new DBUtil();

            ReturnModel rm = new ReturnModel();
            rm.ReturnText = dbu.GetApid(CurrentPeriod);
            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("ChangeStatusHkSyd")]
        [HttpPost]
        public IActionResult ChangeStatusHkSyd([FromBody] ApidDetails model)
        {
            string companyId = model.companyId;
            string status = model.status;


            DBUtil dBUtil = new DBUtil();
            dBUtil.ChangeStatusHkSyd(companyId, status);

            ReturnModel rJason = new ReturnModel();
            rJason.ReturnText = "Done Creation of MPlayer Records";
            return Ok(rJason);
        }
    }
}
