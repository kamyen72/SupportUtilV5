using DupRecRemoval.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Reflection.Metadata;
using static DupRecRemoval.Controllers.APIController;

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

namespace DupRecRemoval.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public APIController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public class InputModel
        {
            public string InputText { get; set; }
        }

        public class ReturnModel
        {
            public string ReturnText { get; set; }
        }

        [EnableCors("AllowAll")]
        [Route("GetDBsCount")]
        [HttpPost]
        public IActionResult GetDBsCount([FromBody] InputModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.InputText))
            {
                return BadRequest("Input text is required.");
            }

            string inputText = model.InputText; // Extract inputText property from model

            DBList dbList = new DBList();

            ReturnModel returnModel = new ReturnModel();
            returnModel.ReturnText = dbList.dbs.Count.ToString();

            var rJason = JsonConvert.SerializeObject(dbList.dbs);

            //return Content("application/json", rJason);

            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetCurrentPeriods")]
        [HttpPost]
        public IActionResult GetCurrentPeriods([FromBody] DateRange model)
        {
            if (model == null)
            {
                return BadRequest("Input text is required.");
            }

            CurrentPeriodList cpl = new CurrentPeriodList();
            cpl.StartDate = model.StartDate + " 00:00:00";
            cpl.EndDate = model.EndDate + " 23:59:59";

            string sql = "";
            sql = "declare @startDate datetime, @endDate datetime; ";
            sql = sql + "set @startDate = '" + cpl.StartDate + "'; ";
            sql = sql + "set @endDate = '" + cpl.EndDate + "'; ";

            sql = sql + "select top 1000 ";
            sql = sql + "CurrentPeriod, count(*) as Recs ";
            sql = sql + "from MPlayer ";
            sql = sql + "where iswin is not null ";
            sql = sql + "and ShowResultDate >= @startDate and ShowResultDate <= @endDate ";
            sql = sql + "group by CurrentPeriod ";
            sql = sql + "order by CurrentPeriod ";

            cpl.CurrentPeriods = new List<CurrentPeriod>();
            SqlConnection connection = new SqlConnection(db_local.connStr);
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxrows = myDataRows.Rows.Count;
            for (int i = 0; i < maxrows; i++)
            {
                DataRow row = myDataRows.Rows[i];
                CurrentPeriod cp =  new CurrentPeriod();
                cp.currentperiod = row["CurrentPeriod"].ToString();
                cpl.CurrentPeriods.Add(cp);
            }

            string rJason = JsonConvert.SerializeObject(cpl.CurrentPeriods);

            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetDiffFromDBs")]
        [HttpPost]
        public IActionResult GetDiffFromDBs([FromBody] InputModel model)
        {
            if (model == null)
            {
                return BadRequest("Input text is required.");
            }

            var CurrentPeriod = model.InputText;

            CheckDBsDiff alldbschk = new CheckDBsDiff();
            List<db> AllDBsDiff = new List<db>();
            AllDBsDiff = alldbschk.CheckAllDBs4Diff(CurrentPeriod);

            string rJason = JsonConvert.SerializeObject(AllDBsDiff);

            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("DeleteDuplicate")]
        [HttpPost]
        public IActionResult DeleteDuplicate([FromBody] WrongGDMPlayer model)
        {
            if (model == null)
            {
                return BadRequest("Input text is required.");
            }

            WrongGDMPlayer wrec = new WrongGDMPlayer();
            wrec.CurrentPeriod = model.CurrentPeriod;
            wrec.SelectedNums = model.SelectedNums;
            wrec.GameDealerMemberID = model.GameDealerMemberID;
            wrec.ConnStr = model.ConnStr;

            string sql = "";

            sql = sql + "select top 1 * from GameDealerMPlayer ";
            sql = sql + "Where CurrentPeriod = '@dbCurrentPeriod' ";
            sql = sql + "and SelectedNums = '@dbSelectedNums' ";
            sql = sql + "and MemberID = @dbGameDealerMemberID ";
            sql = sql + "order by ID ";

            string sql2 = sql.Replace("@dbCurrentPeriod", wrec.CurrentPeriod)
                .Replace("@dbSelectedNums", wrec.SelectedNums)
                .Replace("@dbGameDealerMemberID", wrec.GameDealerMemberID);

            SqlConnection connection = new SqlConnection(wrec.ConnStr);
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxrows = myDataRows.Rows.Count;
            for (int i = 0; i < maxrows; i++)
            {
                DataRow row = myDataRows.Rows[i];
                wrec.IDtoKeep = int.Parse(row["ID"].ToString());
            }

            string sql3 = "delete from GameDealerMPlayer ";
            sql3 = sql3 + "Where CurrentPeriod = '@dbCurrentPeriod' ";
            sql3 = sql3 + "and SelectedNums = '@dbSelectedNums' ";
            sql3 = sql3 + "and MemberID = @dbGameDealerMemberID ";
            sql3 = sql3 + "and ID <> @dbIDtokeep ";
            

            string sql4 = sql3.Replace("@dbCurrentPeriod", wrec.CurrentPeriod)
            .Replace("@dbSelectedNums", wrec.SelectedNums)
            .Replace("@dbIDtokeep", wrec.IDtoKeep.ToString())
            .Replace("@dbGameDealerMemberID", wrec.GameDealerMemberID);

            SqlConnection connection2 = new SqlConnection(wrec.ConnStr);
            SqlCommand command2 = new SqlCommand(sql4, connection2);
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();

            ReturnModel rt = new ReturnModel();
            rt.ReturnText = "Success";

            string rJason = JsonConvert.SerializeObject(rt);

            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GenNewPlatformExcel")]
        [HttpPost]
        public IActionResult GenNewPlatformExcel([FromBody] NewPlatform model)
        {
            var wwwRootPath = _env.WebRootPath;
            string AppLocation = "";
            AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            AppLocation = AppLocation.Replace("file:\\", "");
            string date = DateTime.Now.ToShortDateString();
            date = date.Replace("/", "_");
            string filename = "NewPlatform_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            string folder = "ExcelFiles";
            string filepath = wwwRootPath + "\\" + folder + "\\" + filename;

            ExcelFile exfile = new ExcelFile();
            exfile.FileName = filename;
            exfile.PathName = filepath;

            GenExcelUtil exutil = new GenExcelUtil();
            exutil.GenerateTemplate(exfile, model);

            if (model == null)
            {
                return BadRequest("Input text is required.");
            }

            NewPlatform np = new NewPlatform();
            np.AgentName = model.AgentName;
            np.CompanyCode = model.CompanyCode;
            np.Platform = model.Platform;
            np.APID = model.APID;
            np.APIDomain = model.APIDomain;
            np.PlatformText = model.PlatformText;

            URLResponseList uRLResponseList = new URLResponseList();

            URLResponse res = new URLResponse();
            res.FileName = filename;
            res.FolderName = folder;

            uRLResponseList.Rows = new List<URLResponse>();
            uRLResponseList.Rows.Add(res);

            string rJason = JsonConvert.SerializeObject(uRLResponseList.Rows);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetActivityList")]
        [HttpPost]
        public IActionResult GetActivityList([FromBody] UserActivityInput model)
        {
            if (model == null)
            {
                return BadRequest("Input text is required.");
            }

            UserActivityInput cpl = new UserActivityInput();
            cpl.CurrentPeriod = model.CurrentPeriod;
            cpl.UserName = model.UserName;

            string sql = "";
            sql = sql + "declare @currentPeriod as nvarchar(max), @userName as nvarchar(max) ";
            sql = sql + "set @currentPeriod = '@dbCurrentPeriod' ";
            sql = sql + "set @userName = '%' + '@dbUserName' + '%' ";
            sql = sql + "select CurrentPeriod, UserName, sum(WinRec) as WinRecs, sum(LoseRec) as LoseRecs, sum(PendingRec) as PendingRecs ";
            sql = sql + "from ";
            sql = sql + "( ";
            sql = sql + "select ";
            sql = sql + "CurrentPeriod, ";
            sql = sql + "UserName, ";
            sql = sql + "WinRec = case ";
            sql = sql + "when IsWin = 1 then 1 ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ", LoseRec = case ";
            sql = sql + "when IsWin = 0 then 1 ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + ", PendingRec = case ";
            sql = sql + "when (iswin is null) then 1 ";
            sql = sql + "else 0 ";
            sql = sql + "end ";
            sql = sql + "from MPlayer ";
            sql = sql + "where currentperiod = @currentPeriod ";
            sql = sql + "and UserName like @userName ";
            sql = sql + ") as x ";
            sql = sql + "group by  ";
            sql = sql + "CurrentPeriod, UserName ";
            sql = sql + "order by  ";
            sql = sql + "CurrentPeriod, UserName ";

            var sql2 = sql.Replace("@dbCurrentPeriod", model.CurrentPeriod.ToString())
                          .Replace("@dbUserName", model.UserName.ToString());

            SqlConnection connection = new SqlConnection(db_ghl55.connStr);
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            UserActivitySummaryList ActivityList = new UserActivitySummaryList();
            ActivityList.Rows = new List<UserActivitySummary>();

            int maxrows = myDataRows.Rows.Count;
            for (int i = 0; i < maxrows; i++)
            {
                DataRow row = myDataRows.Rows[i];

                UserActivitySummary summary = new UserActivitySummary();
                summary.CurrentPeriod = row["CurrentPeriod"].ToString();
                summary.UserName = row["UserName"].ToString();
                summary.WinRecs = int.Parse(row["WinRecs"].ToString());
                summary.LoseRecs = int.Parse(row["LoseRecs"].ToString());
                summary.PendingRecs = int.Parse(row["PendingRecs"].ToString());
                ActivityList.Rows.Add(summary);
            }

            string rJason = JsonConvert.SerializeObject(ActivityList.Rows);

            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GenActivityExcel")]
        [HttpPost]
        public IActionResult GenActivityExcel([FromBody] jsonString model)
        {
            var test = model.Value.ToString();

            UserActivityInputList uail = new UserActivityInputList();
            uail.Rows = new List<UserActivityInput>();
            uail.Rows = JsonConvert.DeserializeObject<List<UserActivityInput>>(test.ToString());

            GenExcelUtil xutil = new GenExcelUtil();
            var wwwRootPath = _env.WebRootPath;

            URLResponse res = new URLResponse();
            res = xutil.GenBigExcel(wwwRootPath.ToString(), uail);

            URLResponseList uRLResponseList = new URLResponseList();

            uRLResponseList.Rows = new List<URLResponse>();
            uRLResponseList.Rows.Add(res);

            string rJason = JsonConvert.SerializeObject(uRLResponseList.Rows);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GenActivityExcelMPlayerAllFields")]
        [HttpPost]
        public IActionResult GenActivityExcelMPlayerAllFields([FromBody] jsonString model)
        {
            string username = model.Value;

            GenExcelUtil xutil = new GenExcelUtil();
            var wwwRootPath = _env.WebRootPath;

            URLResponse res = new URLResponse();
            res = xutil.CreateMPAllFieldsWB(wwwRootPath, username);

            // ----- template response for json response -----------------------

            URLResponseList uRLResponseList = new URLResponseList();

            uRLResponseList.Rows = new List<URLResponse>();
            uRLResponseList.Rows.Add(res);

            string rJason = JsonConvert.SerializeObject(uRLResponseList.Rows);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("ViewActivityMPlayerAllFields")]
        [HttpPost]
        public IActionResult ViewActivityMPlayerAllFields([FromBody] jsonString model)
        {
            string username = model.Value;

            List<db> dbList = new List<db>();

            db db = new db();
            db.connStr = db_ace99.connStr;
            db.ip = db_ace99.ip;
            db.userId = db_ace99.userId;
            db.password = db_ace99.password;
            db.dbfullname = db_ace99.dbfullname;
            db.MyID = db_ace99.MyID;
            dbList.Add(db);

            db = new db();
            db.connStr = db_king4d.connStr;
            db.ip = db_king4d.ip;
            db.userId = db_king4d.userId;
            db.password = db_king4d.password;
            db.dbfullname = db_king4d.dbfullname;
            db.MyID = db_king4d.MyID;
            dbList.Add(db);

            db = new db();
            db.connStr = db_bv.connStr;
            db.ip = db_bv.ip;
            db.userId = db_bv.userId;
            db.password = db_bv.password;
            db.dbfullname = db_bv.dbfullname;
            db.MyID = db_bv.MyID;
            dbList.Add(db);

            db = new db();
            db.connStr = db_wl.connStr;
            db.ip = db_wl.ip;
            db.userId = db_wl.userId;
            db.password = db_wl.password;
            db.dbfullname = db_wl.dbfullname;
            db.MyID = db_wl.MyID;
            dbList.Add(db);

            db = new db();
            db.connStr = db_ghl55.connStr;
            db.ip = db_ghl55.ip;
            db.userId = db_ghl55.userId;
            db.password = db_ghl55.password;
            db.dbfullname = db_ghl55.dbfullname;
            db.MyID = db_ghl55.MyID;
            dbList.Add(db);

            db = new db();
            db.connStr = db_tm.connStr;
            db.ip = db_tm.ip;
            db.userId = db_tm.userId;
            db.password = db_tm.password;
            db.dbfullname = db_tm.dbfullname;
            db.MyID = db_tm.MyID;
            dbList.Add(db);

            db = new db();
            db.connStr = db_tm2.connStr;
            db.ip = db_tm2.ip;
            db.userId = db_tm2.userId;
            db.password = db_tm2.password;
            db.dbfullname = db_tm2.dbfullname;
            db.MyID = db_tm2.MyID;
            dbList.Add(db);

            db = new db();
            db.connStr = db_ghlstaging.connStr;
            db.ip = db_ghlstaging.ip;
            db.userId = db_ghlstaging.userId;
            db.password = db_ghlstaging.password;
            db.dbfullname = db_ghlstaging.dbfullname;
            db.MyID = db_ghlstaging.MyID;
            dbList.Add(db);

            //---- search each db and write to excel worksheet

            int maxcount = dbList.Count;
            DBUtil dBUtil = new DBUtil();

            MPlayerAllList mainmplist = new MPlayerAllList();
            mainmplist.Rows = new List<MPlayerAll>();


            for (int x = 0; x < maxcount; x++)
            {
                db thisdb = dbList[x];
                //MPlayerAllList mplist = dBUtil.GetMPlayerAllFields(username, thisdb.MyID, ref mainmplist);
                dBUtil.InsertMPlayerAllFields(username, thisdb.MyID, ref mainmplist);
                var xx = mainmplist.Rows.Count;
            }

            var zz = mainmplist.Rows.Count;
            // ----- template response for json response -----------------------

            string rJason = JsonConvert.SerializeObject(mainmplist.Rows);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("CheckMissingMPlayer")]
        [HttpPost]
        public IActionResult CheckMissingMPlayer([FromBody] jsonString model)
        {
            string CurrentPeriod = model.Value;

            GameDealerMPlayerBaseList mainmplist = new GameDealerMPlayerBaseList();
            mainmplist.Rows = new List<GameDealerMPlayerBase>();

            DBUtil dbutil = new DBUtil();
            dbutil.GetGameDealerMPlayerBase(CurrentPeriod, ref mainmplist);

            MPlayerMinimumList mPlayerMinimumList = new MPlayerMinimumList();
            mPlayerMinimumList.Rows = new List<MPlayerMinimum>();

            GameDealerMPlayerBaseList finalGDMPlist = new GameDealerMPlayerBaseList();
            finalGDMPlist.Rows = new List<GameDealerMPlayerBase>();

            dbutil.GetMPlayerMinimumList(CurrentPeriod, ref mPlayerMinimumList);

            var yy = mPlayerMinimumList.Rows.Count;

            var zz = mainmplist.Rows.Count;

            DBList dblist = new DBList();

            for (int i = 0; i < zz; i++)
            {
                GameDealerMPlayerBase thisGDMP = mainmplist.Rows[i];
                int foundrec = 0;
                string dbname = "";
                for (int j = 0; j < yy; j++)
                {
                    MPlayerMinimum min = mPlayerMinimumList.Rows[j];
                    dbname = min.DBname;

                    if (thisGDMP.CurrentPeriod == min.CurrentPeriod && thisGDMP.MemberID == min.GameDealerMemberID && thisGDMP.SelectedNums == min.SelectedNums && thisGDMP.DBname == min.DBname)
                    {
                        foundrec++;
                        break;
                    }
                }

                GameDealerMPlayerBase newbase = new GameDealerMPlayerBase();
                newbase = mainmplist.Rows[i];
                var tt = mainmplist.Rows[i].MPlayer_Rec;

                newbase.MPlayer_Rec = foundrec;
                finalGDMPlist.Rows.Add(newbase);
            }

            GameDealerMPlayerBaseList outputGDMPlist = new GameDealerMPlayerBaseList();
            outputGDMPlist.Rows = new List<GameDealerMPlayerBase>();

            zz = finalGDMPlist.Rows.Count;

            for (int i = 0; i < zz; i++)
            {
                GameDealerMPlayerBase b = finalGDMPlist.Rows[i];

                if (b.MPlayer_Rec == 0)
                {
                    outputGDMPlist.Rows.Add(b);
                }
            }

            int max = outputGDMPlist.Rows.Count;
            string strMemberID = "";
            string strSelectedNums = "";
            

            for (int z = 0; z < max; z++)
            {
                GameDealerMPlayerBase g = outputGDMPlist.Rows[z];
                if (z == 0) {
                    strMemberID = strMemberID + g.MemberID;
                    strSelectedNums = strSelectedNums + "'" + g.SelectedNums + "'";
                }
                else
                {
                    strMemberID = strMemberID + ", " + g.MemberID;
                    strSelectedNums = strSelectedNums + ", '" + g.SelectedNums + "'";
                }
            }

            // ---- getting the unique db to be searched only ----
            int mx = outputGDMPlist.Rows.Count;

            List<string> list = new List<string>();
            for (int z = 0; z < mx; z++)
            {
                GameDealerMPlayerBase bs = outputGDMPlist.Rows[z];
                list.Add(bs.DBname);
            }

            int dbmx = list.Distinct().ToList().Count;

            List<db> dbtoSearchList = new List<db>();
            DBList alldbs = new DBList();

            for (int g = 0; g < dbmx; g++)
            {
                var mydbname = list[g];

                for (int h = 0; h < alldbs.dbs.Count; h++)
                {
                    db thisdb = alldbs.dbs[h];
                    if (thisdb.MyID == mydbname)
                    {
                        dbtoSearchList.Add(thisdb);
                    }
                }
            }


            GameDealerMPlayerBaseList outputGDMPlist2 = new GameDealerMPlayerBaseList();
            outputGDMPlist2.Rows = new List<GameDealerMPlayerBase>();

            

            dbutil.GetGameDealerMPlayerBaseWithIDs(strMemberID, strSelectedNums, CurrentPeriod, ref outputGDMPlist2, dbtoSearchList);

            

            string rJason = JsonConvert.SerializeObject(outputGDMPlist2.Rows);
            return Ok(rJason);
        }


        [EnableCors("AllowAll")]
        [Route("CreateMissingMPlayerByDB")]
        [HttpPost]
        public IActionResult CreateMissingMPlayerByDB([FromBody] CreateMPlayerInput model)
        {
            string AllIDs = model.allIDs;
            string dbName = model.dbname;

            DBUtil dBUtil = new DBUtil();
            dBUtil.CreateMissingMPlayerByDB(dbName, AllIDs);

            ReturnModel rJason = new ReturnModel();
            rJason.ReturnText = "Done Creation of MPlayer Records";
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("CheckMissingMPlayerByDB")]
        [HttpPost]
        public IActionResult CheckMissingMPlayerByDB([FromBody] MissingMPlayerInput model)
        {
            string CurrentPeriod = model.CurrentPeriod;
            string DBName = model.DBNametoSearch;
            DBUtil dbu = new DBUtil();

            string gdmpjson = dbu.GetGameDealerMPlayerBaseByDB(DBName, CurrentPeriod);
            string mpjson = dbu.GetMPlayerMinimumListByDB(DBName, CurrentPeriod);

            List<GameDealerMPlayerBase> gdmplist = new List<GameDealerMPlayerBase>();
            List<MPlayerMinimum> mplist = new List<MPlayerMinimum>();

            gdmplist = JsonConvert.DeserializeObject<List<GameDealerMPlayerBase>>(gdmpjson);
            mplist = JsonConvert.DeserializeObject<List<MPlayerMinimum>>(mpjson);

            CheckDiffClass checker = new CheckDiffClass();

            List<GameDealerMPlayerBase> MissingGDMPlist = new List<GameDealerMPlayerBase>();

            List<MissingList> mastermissinglist = new List<MissingList>();

            bool result = checker.CompareGDMP_MP(gdmplist, mplist, ref MissingGDMPlist);
            if (result)
            {
                MissingList mi = new MissingList();
                mi.dbname = DBName;
                mi.Rows = MissingGDMPlist;
                var tttt = MissingGDMPlist.Count;
                mastermissinglist.Add(mi);
            }

            /*
            ReturnModel returnModel = new ReturnModel();

            DBUtil dbu = new DBUtil();
            string MPjsonstr = dbu.GetMPlayerMinimumListByDB(DBName, CurrentPeriod);
            string GDMPjsonstr = dbu.GetGameDealerMPlayerBaseByDB(DBName, CurrentPeriod);

            List<GameDealerMPlayerBase> GDMPlist = JsonConvert.DeserializeObject<List<GameDealerMPlayerBase>>(GDMPjsonstr);
            List<MPlayerMinimum> MPlist = JsonConvert.DeserializeObject<List<MPlayerMinimum>>(MPjsonstr);

            int gdrecs = GDMPlist.Count;
            int mprecs = MPlist.Count;

            List<GameDealerMPlayerBase> missinglist = new List<GameDealerMPlayerBase>();
            for (int g = 0; g < gdrecs; g++)
            {
                GameDealerMPlayerBase gdmp = GDMPlist[g];
                int foundrec = 0;
                for (int m = 0; m < mprecs; m++)
                {
                    MPlayerMinimum mpm = MPlist[m];

                    if (gdmp.DBname == mpm.DBname && gdmp.CurrentPeriod == mpm.CurrentPeriod && gdmp.MemberID == mpm.GameDealerMemberID && gdmp.SelectedNums == mpm.SelectedNums)
                    {
                        foundrec++;
                        break;
                    }
                }
                if (foundrec == 0)
                {
                    // add to missinglist
                    missinglist.Add(gdmp);
                }
            }
            */

            string rJason = JsonConvert.SerializeObject(mastermissinglist);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("CheckMissingMPlayerAllDBs")]
        [HttpPost]
        public IActionResult CheckMissingMPlayerAllDBs([FromBody] InputModel model)
        {
            string CurrentPeriod = model.InputText;

            DBList dBList = new DBList();
            int mx = dBList.dbs.Count;
            DBUtil dbu = new DBUtil();
            string gdmpjson = "";
            string mpjson = "";
            List<GameDealerMPlayerBase> gdmplist = new List<GameDealerMPlayerBase>();
            List<MPlayerMinimum> mplist = new List<MPlayerMinimum>();

            List<GameDealerMPlayerBase> MissingGDMPlist = new List<GameDealerMPlayerBase>();
            CheckDiffClass checker = new CheckDiffClass();

            List<MissingList> mastermissinglist = new List<MissingList>();

            for (int i = 0; i < mx; i++) {
                db tdb = dBList.dbs[i];
                gdmpjson = dbu.GetGameDealerMPlayerBaseByDB(tdb.MyID, CurrentPeriod);
                mpjson = dbu.GetMPlayerMinimumListByDB(tdb.MyID, CurrentPeriod);

                gdmplist = JsonConvert.DeserializeObject<List<GameDealerMPlayerBase>>(gdmpjson);
                mplist = JsonConvert.DeserializeObject<List<MPlayerMinimum>>(mpjson);

                if (tdb.MyID == "db_ghl55")
                {
                    var xyz = "stop";
                }

                bool result = checker.CompareGDMP_MP(gdmplist, mplist, ref MissingGDMPlist);
                if (result)
                {
                    MissingList mi = new MissingList();
                    mi.dbname = tdb.MyID;
                    mi.Rows = MissingGDMPlist;
                    var tttt = MissingGDMPlist.Count;
                    mastermissinglist.Add(mi);
                }
            }

            var ttt = mastermissinglist.Count;

            string rJason = JsonConvert.SerializeObject(mastermissinglist);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetRoots")]
        [HttpPost]
        public IActionResult GetRoots([FromBody] InputModel model)
        {
            DBUtil dbu = new DBUtil();

            MenuRoots menuroots = new MenuRoots();
            menuroots = dbu.GetMenuRoots();

            ReturnModel rm = new ReturnModel();
            string rJason = JsonConvert.SerializeObject(menuroots.Roots);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("Test3")]
        [HttpPost]
        public IActionResult Test3([FromBody] InputModel model)
        {
            ReturnModel rm = new ReturnModel();
            rm.ReturnText = "what";

            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }

        [EnableCors("AllowAll")]
        [Route("GetMenuV3")]
        [HttpPost]
        public IActionResult GetMenuV3([FromBody] InputModel model)
        {
            DBUtil dbu = new DBUtil();

            var txt = dbu.GetMenuRootButtons();

            ReturnModel rm = new ReturnModel();
            rm.ReturnText = txt;
            string rJason = JsonConvert.SerializeObject(rm);
            return Ok(rJason);
        }

    }
}
