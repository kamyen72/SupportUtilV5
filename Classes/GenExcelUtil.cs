using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Web;
using System.IO;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using DupRecRemoval.Classes;
using System.Net;

namespace SupportUtil.Classes
{
    public class GenExcelUtil
    {
        private readonly IWebHostEnvironment _env;

        private int global_r = 0;
        
        public void GenerateTemplate(ExcelFile exfile, NewPlatform np)
        {
            PlatformList platformList = new PlatformList();
            platformList.LoadPlatforms();

            NewPlatform model = np;

            using (XLWorkbook wb = new XLWorkbook())
            {
                // creating the HL worksheet ---------------------------------------

                var selws = 0;

                for (int i = 0; i < platformList.platforms.Count; i++)
                {
                    NewPlatform pf = platformList.platforms[i];

                    if (model.Platform == pf.Platform)
                    {
                        selws = i;
                    }

                    var ws = wb.Worksheets.Add(pf.Platform);

                    int r = 1; // row number 1 is the Report Title
                    int c = 1;

                    ws.Cell(r, c).Value = "Platform";
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = true;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Column(c).Width = 11;

                    c++;
                    ws.Cell(r, c).Value = "Agent Name";
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = true;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Column(c).Width = 24;

                    c++;
                    ws.Cell(r, c).Value = "Agent ID";
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = true;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Column(c).Width = 11;

                    c++;
                    ws.Cell(r, c).Value = "Company Code";
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = true;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Column(c).Width = 17;

                    c++;
                    ws.Cell(r, c).Value = "APID";
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = true;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Column(c).Width = 13;

                    c++;
                    ws.Cell(r, c).Value = "API Domain";
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = true;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Column(c).Width = 15;

                    c++;
                    ws.Cell(r, c).Value = pf.APIDomain;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = true;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Column(c).Width = 35;

                    c++;
                    c++;
                    ws.Cell(r, c).Value = pf.PlatformText;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = true;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Column(c).Width = 18;

                    //--------------- filling in the entry value from ui -------------------

                    if (model.Platform == pf.Platform)
                    {
                        r = 2;
                        c = 1;

                        ws.Cell(r, c).Value = model.Platform;
                        ws.Cell(r, c).Style.Font.SetFontSize(12)
                                                .Fill.BackgroundColor = XLColor.Yellow;
                        ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(r, c).Style.Font.SetFontName("Arial");
                        ws.Cell(r, c).Style.Font.Bold = false;

                        c++;
                        ws.Cell(r, c).Value = model.AgentName;
                        ws.Cell(r, c).Style.Font.SetFontSize(12)
                                                .Fill.BackgroundColor = XLColor.Yellow;
                        ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(r, c).Style.Font.SetFontName("Arial");
                        ws.Cell(r, c).Style.Font.Bold = false;
                        ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                        c++;
                        ws.Cell(r, c).Value = "";
                        ws.Cell(r, c).Style.Font.SetFontSize(12)
                                                .Fill.BackgroundColor = XLColor.NoColor;
                        ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(r, c).Style.Font.SetFontName("Arial");
                        ws.Cell(r, c).Style.Font.Bold = false;
                        ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                        c++;
                        ws.Cell(r, c).Value = model.CompanyCode;
                        ws.Cell(r, c).Style.Font.SetFontSize(12)
                                                .Fill.BackgroundColor = XLColor.Yellow;
                        ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(r, c).Style.Font.SetFontName("Arial");
                        ws.Cell(r, c).Style.Font.Bold = false;
                        ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                        c++;
                        ws.Cell(r, c).Value = model.APID;
                        ws.Cell(r, c).Style.Font.SetFontSize(12)
                                                .Fill.BackgroundColor = XLColor.Yellow;
                        ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                        ws.Cell(r, c).Style.Font.SetFontName("Arial");
                        ws.Cell(r, c).Style.Font.Bold = false;
                        ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        ws.Column(c).Width = 20;

                        c++;

                        c++;
                        r = 1;
                        ws.Cell(r, c).Style.Font.SetFontSize(12)
                                                .Fill.BackgroundColor = XLColor.Yellow;
                        ws.Cell(r, c).Style.Font.Bold = false;
                    }
                }

                var filepath = exfile.PathName;

                // -------- final saving to workbook --------
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                wb.SaveAs(filepath);
            }
        }

        public URLResponse GenBigExcel(string rootpath, UserActivityInputList highlist)
        {
            XLWorkbook wb = new XLWorkbook();
            
            int r = 1;
            int c = 1;

            int UserCount = highlist.Rows.Count;
            for (int x = 0; x < UserCount; x++)
            {
                var myname = highlist.Rows[x].UserName;
                    
                var ws = wb.Worksheets.Add(highlist.Rows[x].UserName); // each sheet contain many rows of details

                DBUtil myutil = new DBUtil();
                ActivityDetailList myactivitylist = myutil.GetUserActivityList(highlist.Rows[x]);


                r = 1;
                c = 1;
                ws.Cell(r, c).Value = "Current Period";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = "User Name";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 25;

                c++;
                ws.Cell(r, c).Value = "Update Date";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = "Lottery Info Name";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 25;

                c++;
                ws.Cell(r, c).Value = "Selected Nums";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 20;

                c++;
                ws.Cell(r, c).Value = "Wining Status";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = "Price";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = "Discount Price";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = "Win Money";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = "Win Money with Capital";
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = true;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                ws.Cell(r, c).Style.Alignment.Vertical = XLAlignmentVerticalValues.Bottom;
                ws.Cell(r, c).Style.Alignment.WrapText = true;
                ws.Column(c).Width = 18;
                r = 2;
                c = 1;
                int maxrow = myactivitylist.Rows.Count;
                for (int i = 0; i < maxrow; i++)
                {
                    ActivityDetail det = myactivitylist.Rows[i];

                    ws.Cell(r, c).Value = det.CurrentPeriod;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        

                    c++;
                    ws.Cell(r, c).Value = det.UserName;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        

                    c++;
                    ws.Cell(r, c).Value = det.ShowResultDate;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        

                    c++;
                    ws.Cell(r, c).Value = det.LotteryInfoName;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        

                    c++;
                    ws.Cell(r, c).Value = det.SelectedNums;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        

                    c++;
                    ws.Cell(r, c).Value = det.IsWinStatus;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        

                    c++;
                    ws.Cell(r, c).Value = det.Price;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Cell(r, c).Style.NumberFormat.Format = "$ #,##0.00";


                    c++;
                    ws.Cell(r, c).Value = det.DiscountPrice;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Cell(r, c).Style.NumberFormat.Format = "$ #,##0.00";


                    c++;
                    ws.Cell(r, c).Value = det.WinMoney;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Cell(r, c).Style.NumberFormat.Format = "$ #,##0.00";


                    c++;
                    ws.Cell(r, c).Value = det.WinMoneyWithCapital;
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                    ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                    ws.Cell(r, c).Style.Font.SetFontName("Arial");
                    ws.Cell(r, c).Style.Font.Bold = false;
                    ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Cell(r, c).Style.Alignment.Vertical = XLAlignmentVerticalValues.Bottom;
                    ws.Cell(r, c).Style.Alignment.WrapText = true;
                    ws.Cell(r, c).Style.NumberFormat.Format = "$ #,##0.00";


                    r++;
                    c = 1;
                }

            }

            var wwwRootPath = rootpath;
            string date = DateTime.Now.ToShortDateString();
            date = date.Replace("/", "_");
            string filename = "UserActivityList_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            string folder = "ExcelFiles";
            string filepath = wwwRootPath + "\\" + folder + "\\" + filename;
            wb.SaveAs(filepath);

            URLResponse res = new URLResponse();
            res.FileName = filename;
            res.FolderName = filepath;

            return res;
        }

        public URLResponse CreateMPAllFieldsWB(string rootpath, string username)
        {
            XLWorkbook wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("MPlayer");

            WriteToMPhdWS(ref ws);

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
            db.connStr = db_ghl55.connStr;
            db.ip = db_ghl55.ip;
            db.userId = db_ghl55.userId;
            db.password = db_ghl55.password;
            db.dbfullname = db_ghl55.dbfullname;
            db.MyID = db_ghl55.MyID;
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
            global_r = 2;
            for (int x = 0; x < maxcount; x++)
            {
                db thisdb = dbList[x];
                var tt = thisdb.MyID;

                if (tt == "db_ghl55")
                {
                    var y = "wait";
                }

                //MPlayerAllList mplist = dBUtil.GetMPlayerAllFields(username, thisdb.MyID,ref mainmplist);
                dBUtil.InsertMPlayerAllFields(username, thisdb.MyID, ref mainmplist);
            }

            var xmen = mainmplist.Rows.Count;

            if (mainmplist != null && mainmplist.Rows.Count != 0)
            {
                var rr = mainmplist.Rows.Count;
                WriteToMPdetWS(mainmplist, ref ws);
            }

            var wwwRootPath = rootpath;
            string date = DateTime.Now.ToShortDateString();
            date = date.Replace("/", "_");
            string filename = "UserActivityListFull_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            string folder = "ExcelFiles";
            string filepath = wwwRootPath + "\\" + folder + "\\" + filename;
            wb.SaveAs(filepath);

            URLResponse fres = new URLResponse();
            fres.FileName = filename;
            fres.FolderName = filepath;

            return fres;
        }

        public void WriteToMPdetWS(MPlayerAllList mplist, ref IXLWorksheet ws)
        {
            int r = global_r;
            int c = 1;

            int maxrows = mplist.Rows.Count;

            for (int i = 0; i < maxrows; i++)
            {
                var mprow = mplist.Rows[i];

                ws.Cell(r, c).Value = mprow.Source;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.ID;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.UserName;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.UpdateDate;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(r, c).Style.NumberFormat.Format = "yyyy-mm-dd hh:mm:ss";
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.CreateDate;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(r, c).Style.NumberFormat.Format = "yyyy-mm-dd hh:mm:ss";
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.LotteryInfoName;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.CurrentPeriod;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.SelectedNums;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.DiscountPrice;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(r, c).Style.NumberFormat.Format = "#,##0.00_);[Red](#,##0.00)";
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.Price;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(r, c).Style.NumberFormat.Format = "#,##0.00_);[Red](#,##0.00)";
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.Qty;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.IsWin;
                if (mprow.IsWin == true)
                {
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.Pink;
                }
                else
                {
                    ws.Cell(r, c).Style.Font.SetFontSize(12)
                                            .Fill.BackgroundColor = XLColor.NoColor;
                }
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.ShowResultDate;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(r, c).Style.NumberFormat.Format = "yyyy-mm-dd hh:mm:ss";
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.WinMoney;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(r, c).Style.NumberFormat.Format = "#,##0.00_);[Red](#,##0.00)";
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.WinMoneyWithCapital;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Cell(r, c).Style.NumberFormat.Format = "#,##0.00_);[Red](#,##0.00)";
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.SecondMPlayerID;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.MemberID;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.GameDealerMemberID;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.LotteryInfoID;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.CompanyID;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.IsAfter;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.IsWinStop;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.ManualBet;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.Multiple;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.RebatePro;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.RebateProMoney;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.ReferralPayType;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.CashRebatePayType;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.CashBackRebatePayType;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.IsReferralWriteReport;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.IsCashRebateWriteReport;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.IsCashBackWriteReport;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.IsReset;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.CreateID;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                c++;
                ws.Cell(r, c).Value = mprow.UpdateID;
                ws.Cell(r, c).Style.Font.SetFontSize(12)
                                        .Fill.BackgroundColor = XLColor.NoColor;
                ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
                ws.Cell(r, c).Style.Font.SetFontName("Arial");
                ws.Cell(r, c).Style.Font.Bold = false;
                ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                ws.Column(c).Width = 18;

                r++;
                c = 1;
                global_r = r;
            }
            ws.Columns().AdjustToContents();
        }

        public void WriteToMPhdWS(ref IXLWorksheet ws)
        {
            int r = 1;
            int c = 1;

            string test = ws.Name;

            ws.Cell(r, c).Value = "Source";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "ID";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "UserName";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "UpdateDate";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "CreateDate";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "LotteryInfoName";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "CurrentPeriod";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "SelectedNums";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "DiscountPrice";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "Price";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "Qty";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "IsWin";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "ShowResultDate";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "WinMoney";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "WinMoneyWithCapital";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "SecondMPlayerID";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "MemberID";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "GameDealerMemberID";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "LotteryInfoID";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "CompanyID";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "IsAfter";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "IsWinStop";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "ManualBet";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "Multiple";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "RebatePro";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "RebateProMoney";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "ReferralPayType";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "CashRebatePayType";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "CashBackRebatePayType";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "IsReferralWriteReport";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "IsCashRebateWriteReport";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "IsCashBackWriteReport";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "IsReset";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "CreateID";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;

            c++;
            ws.Cell(r, c).Value = "UpdateID";
            ws.Cell(r, c).Style.Font.SetFontSize(12)
                                    .Fill.BackgroundColor = XLColor.NoColor;
            ws.Cell(r, c).Style.Font.FontColor = XLColor.Black;
            ws.Cell(r, c).Style.Font.SetFontName("Arial");
            ws.Cell(r, c).Style.Font.Bold = true;
            ws.Cell(r, c).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Column(c).Width = 18;
        }
    }

}
