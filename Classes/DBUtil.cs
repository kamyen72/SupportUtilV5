using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using DupRecRemoval.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SupportUtilV3.Classes;
using SupportUtilV4.Classes;
using System.Data;
using System.Net;
using System.Xml.Linq;

namespace SupportUtil.Classes
{
    public class DBUtil
    {
        public ActivityDetailList GetUserActivityList(UserActivityInput input)
        {
            ActivityDetailList actdetlist = new ActivityDetailList();
            actdetlist.Rows = new List<ActivityDetail>();

            UserActivityInput aip = input;

            string sql = "";
            sql = sql + "select ";
            sql = sql + "CurrentPeriod, ";
            sql = sql + "UserName, ";
            sql = sql + "cast(a.UpdateDate as Date) as ShowResultDate ";
            sql = sql + ", b.LotteryInfoName ";
            sql = sql + ", a.SelectedNums ";
            sql = sql + ", IsWinStatus = case ";
            sql = sql + "when iswin = 1 then 'Win' ";
            sql = sql + "when iswin = 0 then 'Lose' ";
            sql = sql + "when iswin is null then 'Pending' ";
            sql = sql + "end ";
            sql = sql + ", [Price] = case ";
            sql = sql + "when a.Price is null then cast( 0 as decimal(34,4)) ";
            sql = sql + "else cast( a.Price as decimal(34,4)) ";
            sql = sql + "end ";
            sql = sql + ", [DiscountPrice] = cast( a.DiscountPrice as decimal(34,4)) ";
            sql = sql + ", [WinMoney] = case  ";
            sql = sql + "when a.WinMoney is null then  cast( 0 as decimal(34,4)) ";
            sql = sql + "else cast( a.WinMoney as decimal(34,4)) ";
            sql = sql + "end ";
            sql = sql + ", [WinMoneyWithCapital] = case  ";
            sql = sql + "when a.WinMoney is null then  cast( 0 as decimal(34,4)) ";
            sql = sql + "else cast( a.WinMoneyWithCapital as decimal(34,4)) ";
            sql = sql + "end ";
            sql = sql + "from mplayer a ";
            sql = sql + "inner join [LotteryInfo] b on a.LotteryInfoID = b.LotteryInfoID ";
            sql = sql + "where currentperiod = '@dbCurrentPeriod' ";
            sql = sql + "and UserName = '@dbUserName' ";

            string sql2 = sql.Replace("@dbCurrentPeriod", aip.CurrentPeriod.ToString())
                                .Replace("@dbUserName", aip.UserName.ToString());

            SqlConnection connection = new SqlConnection(db_ghl55.connStr);
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxrows = myDataRows.Rows.Count;
            for (int j = 0; j < maxrows; j++)
            {
                DataRow row = myDataRows.Rows[j];
                ActivityDetail det = new ActivityDetail();
                det.CurrentPeriod = row["CurrentPeriod"].ToString();
                det.UserName = row["UserName"].ToString();
                det.ShowResultDate = DateTime.Parse(row["ShowResultDate"].ToString());
                det.LotteryInfoName = row["LotteryInfoName"].ToString();
                det.SelectedNums = row["SelectedNums"].ToString();
                det.IsWinStatus = row["IsWinStatus"].ToString();
                det.Price = decimal.Parse(row["Price"].ToString());
                det.DiscountPrice = decimal.Parse(row["DiscountPrice"].ToString());
                det.WinMoney = decimal.Parse(row["WinMoney"].ToString());
                det.WinMoneyWithCapital = decimal.Parse(row["WinMoneyWithCapital"].ToString());
                actdetlist.Rows.Add(det);
            }

            return actdetlist;
        }

        public MPlayerAllList GetMPlayerAllFields(string UserName, string dbID, ref MPlayerAllList mainlist)
        {
            MPlayerAllList mplayerlist = new MPlayerAllList();
            mplayerlist.Rows = new List<MPlayerAll>();

            string sql = "";
            sql = sql + "select ";
            sql = sql + "'@dbID' as Source ";
            sql = sql + ", [ID] ";
            sql = sql + ",[UserName] ";
            sql = sql + ",[UpdateDate] ";
            sql = sql + ",[CreateDate] ";
            sql = sql + ",[LotteryInfoName] ";
            sql = sql + ",[SelectedNums] ";
            sql = sql + ",[DiscountPrice] = isnull(DiscountPrice, 0)  ";
            sql = sql + ",[Price] = isnull(Price, 0) ";
            sql = sql + ",[Qty] = isnull(Qty, 0) ";
            sql = sql + ",[IsWin] = isnull(IsWin, 0) ";
            sql = sql + ",[ShowResultDate] ";
            sql = sql + ",[WinMoney] = isnull(WinMoney, 0) ";
            sql = sql + ",[WinMoneyWithCapital] = isnull(WinMoneyWithCapital, 0) ";
            sql = sql + ",[SecondMPlayerID] = isnull(SecondMPlayerID, 0) ";
            sql = sql + ",[MemberID] = isnull(MemberID, 0) ";
            sql = sql + ",[GameDealerMemberID] = isnull(GameDealerMemberID, 0) ";
            sql = sql + ",[LotteryInfoID] = isnull(LotteryInfoID, 0) ";
            sql = sql + ",[CompanyID] = isnull(CompanyID, 0)  ";
            sql = sql + ",[CurrentPeriod] ";
            sql = sql + ",[IsAfter] = isnull(IsAfter, 0)   ";
            sql = sql + ",[IsWinStop] = isnull(IsWinStop, 0)   ";
            sql = sql + ",[ManualBet] ";
            sql = sql + ",[Multiple] ";
            sql = sql + ",[RebatePro] = isnull(RebatePro, 0)   ";
            sql = sql + ",[RebateProMoney] = isnull(RebateProMoney, 0)   ";
            sql = sql + ",[ReferralPayType] = isnull(ReferralPayType, 0)   ";
            sql = sql + ",[CashRebatePayType] = isnull(CashRebatePayType, 0)   ";
            sql = sql + ",[CashBackRebatePayType] = isnull(CashBackRebatePayType, 0)   ";
            sql = sql + ",[IsReferralWriteReport] = isnull(IsReferralWriteReport, 0)   ";
            sql = sql + ",[IsCashRebateWriteReport] = isnull(IsCashRebateWriteReport, 0)   ";
            sql = sql + ",[IsCashBackWriteReport] = isnull(IsCashBackWriteReport, 0)   ";
            sql = sql + ",[IsReset] = isnull(IsReset, 0)   ";
            sql = sql + ",[CreateID] = isnull(CreateID, 0)  ";
            sql = sql + ",[UpdateID] = isnull(UpdateID, 0)  ";
            sql = sql + "FROM [MPlayer]  ";
            sql = sql + "WHERE UserName like '%@dbUserName%' ";
            sql = sql + "order by UpdateDate  ";

            string sql2 = "";
            sql2 = sql.Replace("@dbID", dbID).Replace("@dbUserName", UserName);

            var localconnstr = "";
            DBList dBList = new DBList();
            int maxrec = dBList.dbs.Count;

            switch (dbID)
            {
                case "db_ghl55":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_ghl55")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;

                case "db_tm":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_tm")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_tm2":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_tm2")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_bv":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_bv")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_wl":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_wl")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_ace99":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_ace99")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_king4d":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_king4d")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_ghlstaging":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_ghlstaging")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
            }

            if (localconnstr != null && localconnstr != "")
            {
                SqlConnection connection = new SqlConnection(localconnstr);
                connection.Open();
                DataTable myDataRows = new DataTable();
                SqlCommand command = new SqlCommand(sql2, connection);
                command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(myDataRows);
                connection.Close();

                int maxcount = myDataRows.Rows.Count;

                for (int x = 0; x < maxcount; x++)
                {
                    DataRow r = myDataRows.Rows[x];
                    MPlayerAll m = new MPlayerAll();
                    m.Source = r["Source"].ToString();
                    m.ID = int.Parse(r["ID"].ToString());
                    m.UserName = r["UserName"].ToString();
                    m.UpdateDate = DateTime.Parse(r["UpdateDate"].ToString());
                    m.CreateDate = DateTime.Parse(r["CreateDate"].ToString());
                    m.LotteryInfoName = r["LotteryInfoName"].ToString();
                    m.SelectedNums = r["SelectedNums"].ToString();
                    m.DiscountPrice = decimal.Parse(r["DiscountPrice"].ToString());
                    m.Price = decimal.Parse(r["Price"].ToString());
                    m.Qty = decimal.Parse(r["Qty"].ToString());
                    m.IsWin = bool.Parse(r["IsWin"].ToString());
                    m.ShowResultDate = DateTime.Parse(r["ShowResultDate"].ToString());
                    m.WinMoney = decimal.Parse(r["WinMoney"].ToString());
                    m.WinMoneyWithCapital = decimal.Parse(r["WinMoneyWithCapital"].ToString());
                    m.SecondMPlayerID = int.Parse(r["SecondMPlayerID"].ToString());
                    m.MemberID = int.Parse(r["MemberID"].ToString());
                    m.GameDealerMemberID = int.Parse(r["GameDealerMemberID"].ToString());
                    m.LotteryInfoID = int.Parse(r["LotteryInfoID"].ToString());
                    m.CompanyID = int.Parse(r["CompanyID"].ToString());
                    m.CurrentPeriod = r["CurrentPeriod"].ToString();
                    m.IsAfter = bool.Parse(r["IsAfter"].ToString());
                    m.IsWinStop = bool.Parse(r["IsWinStop"].ToString());
                    m.ManualBet = r["ManualBet"].ToString();
                    m.Multiple = r["Multiple"].ToString();
                    m.RebatePro = int.Parse(r["RebatePro"].ToString());
                    m.RebateProMoney = int.Parse(r["RebateProMoney"].ToString());
                    m.ReferralPayType = int.Parse(r["ReferralPayType"].ToString());
                    m.CashRebatePayType = int.Parse(r["CashRebatePayType"].ToString());
                    m.CashBackRebatePayType = int.Parse(r["CashBackRebatePayType"].ToString());
                    m.IsReferralWriteReport = int.Parse(r["IsReferralWriteReport"].ToString());
                    m.IsCashRebateWriteReport = int.Parse(r["IsCashRebateWriteReport"].ToString());
                    m.IsCashBackWriteReport = int.Parse(r["IsCashBackWriteReport"].ToString());
                    m.IsReset = int.Parse(r["IsReset"].ToString());
                    m.CreateID = int.Parse(r["CreateID"].ToString());
                    m.UpdateID = int.Parse(r["UpdateID"].ToString());

                    mplayerlist.Rows.Add(m);
                    mainlist.Rows.Add(m);
                }

            }
            return mplayerlist;
        }

        public void InsertMPlayerAllFields(string UserName, string dbID, ref MPlayerAllList mainlist)
        {
            string sql = "";
            sql = sql + "select ";
            sql = sql + "'@dbID' as Source ";
            sql = sql + ", [ID] ";
            sql = sql + ",[UserName] ";
            sql = sql + ",[UpdateDate] ";
            sql = sql + ",[CreateDate] ";
            sql = sql + ",[LotteryInfoName] ";
            sql = sql + ",[SelectedNums] ";
            sql = sql + ",[DiscountPrice] = isnull(DiscountPrice, 0)  ";
            sql = sql + ",[Price] = isnull(Price, 0) ";
            sql = sql + ",[Qty] = isnull(Qty, 0) ";
            sql = sql + ",[IsWin] = isnull(IsWin, 0) ";
            sql = sql + ",[ShowResultDate] ";
            sql = sql + ",[WinMoney] = isnull(WinMoney, 0) ";
            sql = sql + ",[WinMoneyWithCapital] = isnull(WinMoneyWithCapital, 0) ";
            sql = sql + ",[SecondMPlayerID] = isnull(SecondMPlayerID, 0) ";
            sql = sql + ",[MemberID] = isnull(MemberID, 0) ";
            sql = sql + ",[GameDealerMemberID] = isnull(GameDealerMemberID, 0) ";
            sql = sql + ",[LotteryInfoID] = isnull(LotteryInfoID, 0) ";
            sql = sql + ",[CompanyID] = isnull(CompanyID, 0)  ";
            sql = sql + ",[CurrentPeriod] ";
            sql = sql + ",[IsAfter] = isnull(IsAfter, 0)   ";
            sql = sql + ",[IsWinStop] = isnull(IsWinStop, 0)   ";
            sql = sql + ",[ManualBet] ";
            sql = sql + ",[Multiple] ";
            sql = sql + ",[RebatePro] = isnull(RebatePro, 0)   ";
            sql = sql + ",[RebateProMoney] = isnull(RebateProMoney, 0)   ";
            sql = sql + ",[ReferralPayType] = isnull(ReferralPayType, 0)   ";
            sql = sql + ",[CashRebatePayType] = isnull(CashRebatePayType, 0)   ";
            sql = sql + ",[CashBackRebatePayType] = isnull(CashBackRebatePayType, 0)   ";
            sql = sql + ",[IsReferralWriteReport] = isnull(IsReferralWriteReport, 0)   ";
            sql = sql + ",[IsCashRebateWriteReport] = isnull(IsCashRebateWriteReport, 0)   ";
            sql = sql + ",[IsCashBackWriteReport] = isnull(IsCashBackWriteReport, 0)   ";
            sql = sql + ",[IsReset] = isnull(IsReset, 0)   ";
            sql = sql + ",[CreateID] = isnull(CreateID, 0)  ";
            sql = sql + ",[UpdateID] = isnull(UpdateID, 0)  ";
            sql = sql + "FROM [MPlayer]  ";
            sql = sql + "WHERE UserName like '%@dbUserName%' ";
            sql = sql + "order by UpdateDate  ";

            string sql2 = "";
            sql2 = sql.Replace("@dbID", dbID).Replace("@dbUserName", UserName);

            var localconnstr = "";
            DBList dBList = new DBList();
            int maxrec = dBList.dbs.Count;

            switch (dbID)
            {
                case "db_tm":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_tm")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_ghl55":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_ghl55")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_tm2":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_tm2")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_bv":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_bv")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_wl":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_wl")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_ace99":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_ace99")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_king4d":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_king4d")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
                case "db_ghlstaging":
                    for (int i = 0; i < maxrec; i++)
                    {
                        db thisdb = dBList.dbs[i];
                        if (thisdb.MyID.ToLower() == "db_ghlstaging")
                        {
                            localconnstr = thisdb.connStr;
                            break;
                        }
                    }
                    break;
            }

            if (localconnstr != null && localconnstr != "")
            {
                SqlConnection connection = new SqlConnection(localconnstr);
                connection.Open();
                DataTable myDataRows = new DataTable();
                SqlCommand command = new SqlCommand(sql2, connection);
                command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(myDataRows);
                connection.Close();

                int maxcount = myDataRows.Rows.Count;

                for (int x = 0; x < maxcount; x++)
                {
                    DataRow r = myDataRows.Rows[x];
                    MPlayerAll m = new MPlayerAll();
                    m.Source = r["Source"].ToString();
                    m.ID = int.Parse(r["ID"].ToString());
                    m.UserName = r["UserName"].ToString();
                    m.UpdateDate = DateTime.Parse(r["UpdateDate"].ToString());
                    m.CreateDate = DateTime.Parse(r["CreateDate"].ToString());
                    m.LotteryInfoName = r["LotteryInfoName"].ToString();
                    m.SelectedNums = r["SelectedNums"].ToString();
                    m.DiscountPrice = decimal.Parse(r["DiscountPrice"].ToString());
                    m.Price = decimal.Parse(r["Price"].ToString());
                    m.Qty = decimal.Parse(r["Qty"].ToString());
                    m.IsWin = bool.Parse(r["IsWin"].ToString());
                    m.ShowResultDate = DateTime.Parse(r["ShowResultDate"].ToString());
                    m.WinMoney = decimal.Parse(r["WinMoney"].ToString());
                    m.WinMoneyWithCapital = decimal.Parse(r["WinMoneyWithCapital"].ToString());
                    m.SecondMPlayerID = int.Parse(r["SecondMPlayerID"].ToString());
                    m.MemberID = int.Parse(r["MemberID"].ToString());
                    m.GameDealerMemberID = int.Parse(r["GameDealerMemberID"].ToString());
                    m.LotteryInfoID = int.Parse(r["LotteryInfoID"].ToString());
                    m.CompanyID = int.Parse(r["CompanyID"].ToString());
                    m.CurrentPeriod = r["CurrentPeriod"].ToString();
                    m.IsAfter = bool.Parse(r["IsAfter"].ToString());
                    m.IsWinStop = bool.Parse(r["IsWinStop"].ToString());
                    m.ManualBet = r["ManualBet"].ToString();
                    m.Multiple = r["Multiple"].ToString();
                    m.RebatePro = decimal.Parse(r["RebatePro"].ToString());
                    m.RebateProMoney = int.Parse(r["RebateProMoney"].ToString());
                    m.ReferralPayType = int.Parse(r["ReferralPayType"].ToString());
                    m.CashRebatePayType = int.Parse(r["CashRebatePayType"].ToString());
                    m.CashBackRebatePayType = int.Parse(r["CashBackRebatePayType"].ToString());
                    m.IsReferralWriteReport = int.Parse(r["IsReferralWriteReport"].ToString());
                    m.IsCashRebateWriteReport = int.Parse(r["IsCashRebateWriteReport"].ToString());
                    m.IsCashBackWriteReport = int.Parse(r["IsCashBackWriteReport"].ToString());
                    m.IsReset = int.Parse(r["IsReset"].ToString());
                    m.CreateID = int.Parse(r["CreateID"].ToString());
                    m.UpdateID = int.Parse(r["UpdateID"].ToString());

                    mainlist.Rows.Add(m);
                }

            }
        }

        public void GetGameDealerMPlayerBase(string CurrentPeriod, ref GameDealerMPlayerBaseList mainlist)
        {
            var localconnstr = "";
            DBList dBList = new DBList();
            int maxrec = dBList.dbs.Count;

            for (int i = 0; i < maxrec; i++)
            {
                db thisdb = dBList.dbs[i];
                localconnstr = thisdb.connStr;

                if (localconnstr != null && localconnstr != "")
                {
                    string sql = "";
                    sql = sql + "declare @CurrentPeriod nvarchar(max) ";
                    sql = sql + "set @CurrentPeriod = '@dbCurrentPeriod' ";
                    sql = sql + "drop table if exists #tempGDMPlayer ";
                    sql = sql + "create table #tempGDMPlayer ( ";
                    sql = sql + "DBname nvarchar(max) null ";
                    sql = sql + ", GDMP_ID int null ";
                    sql = sql + ", UserName nvarchar(max) null ";
                    sql = sql + ", GameDealerMemberID int null ";
                    sql = sql + ", MemberID int null ";
                    sql = sql + ", SelectedNums nvarchar(max) null ";
                    sql = sql + ", UpdateDate datetime null ";
                    sql = sql + ", GDMP_Recs int null ";
                    sql = sql + ", [MPlayer_Rec] int null ";
                    sql = sql + ", [MPUpdateDate] datetime null ";
                    sql = sql + ", CurrentPeriod nvarchar(max) null ";
                    sql = sql + ") ";
                    sql = sql + "insert into #tempGDMPlayer (dbname, MemberID, SelectedNums, UpdateDate, CurrentPeriod) ";
                    sql = sql + "select '@dbName', MemberID, SelectedNums, UpdateDate, CurrentPeriod ";
                    sql = sql + "from GameDealerMPlayer ";
                    sql = sql + "where CurrentPeriod = @CurrentPeriod and MemberID <> 0 and MemberID is not null ";
                    sql = sql + "Update #tempGDMPlayer ";
                    sql = sql + "set UserName = (select top 1 UserName from Mplayer where GamedealerMemberID = a.MemberID) ";
                    sql = sql + "from #tempGDMPlayer a ";
                    sql = sql + "select * from #tempGDMPlayer ";

                    string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod)
                                     .Replace("@dbName", thisdb.MyID);

                    SqlConnection connection = new SqlConnection(localconnstr);
                    connection.Open();
                    DataTable myDataRows = new DataTable();
                    SqlCommand command = new SqlCommand(sql2, connection);
                    command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(myDataRows);
                    connection.Close();

                    int maxcount = myDataRows.Rows.Count;

                    for (int x = 0; x < maxcount; x++)
                    {
                        DataRow r = myDataRows.Rows[x];
                        GameDealerMPlayerBase m = new GameDealerMPlayerBase();
                        m.DBname = r["DBname"].ToString();
                        m.UpdateDate = DateTime.Parse(r["UpdateDate"].ToString());
                        m.SelectedNums = r["SelectedNums"].ToString();
                        m.MemberID = int.Parse(r["MemberID"].ToString());
                        m.CurrentPeriod = r["CurrentPeriod"].ToString();

                        mainlist.Rows.Add(m);
                    }

                }
            }
        }

        public void GetGameDealerMPlayerBaseWithIDs(string tMemberIDs, string tSelectedNums, string tCurrentPeriod, ref GameDealerMPlayerBaseList mainlist2, List<db> dbtosearch)
        {
            var localconnstr = "";

            //List<db> dbtosearch = new List<db>();

            //db db = new db();
            //db.connStr = db_local.connStr;
            //db.ip = db_local.ip;
            //db.userId = db_local.userId;
            //db.password = db_local.password;
            //db.dbfullname = db_local.dbfullname;
            //db.MyID = db_local.MyID;
            //dbtosearch.Add(db);

            int maxrec = dbtosearch.Count;

            for (int i = 0; i < maxrec; i++)
            {
                db thisdb = dbtosearch[i];
                localconnstr = thisdb.connStr;

                if (localconnstr != null && localconnstr != "")
                {
                    string sql = "";
                    sql = sql + "select distinct ID as GDMP_ID, MemberID, SelectedNums, UpdateDate, CurrentPeriod ";
                    sql = sql + "from GameDealerMPlayer ";
                    sql = sql + "where CurrentPeriod = '@dbCurrentPeriod' ";
                    sql = sql + "and MemberID in (" + tMemberIDs + ") ";
                    sql = sql + "and SelectedNums in (" + tSelectedNums + ") ";

                    string sql2 = sql.Replace("@dbCurrentPeriod", tCurrentPeriod);

                    SqlConnection connection = new SqlConnection(localconnstr);
                    connection.Open();
                    DataTable myDataRows = new DataTable();
                    SqlCommand command = new SqlCommand(sql2, connection);
                    command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(myDataRows);
                    connection.Close();

                    int maxcount = myDataRows.Rows.Count;

                    for (int x = 0; x < maxcount; x++)
                    {
                        DataRow r = myDataRows.Rows[x];
                        GameDealerMPlayerBase m = new GameDealerMPlayerBase();
                        m.DBname = thisdb.MyID;
                        m.UpdateDate = DateTime.Parse(r["UpdateDate"].ToString());
                        m.SelectedNums = r["SelectedNums"].ToString();
                        m.MemberID = int.Parse(r["MemberID"].ToString());
                        m.CurrentPeriod = r["CurrentPeriod"].ToString();
                        m.GDMP_ID = int.Parse(r["GDMP_ID"].ToString());

                        mainlist2.Rows.Add(m);
                    }

                }
            }
        }

        public void GetMPlayerMinimumList(string CurrentPeriod, ref MPlayerMinimumList mainlist)
        {
            var localconnstr = "";
            DBList dBList = new DBList();
            int maxrec = dBList.dbs.Count;

            for (int i = 0; i < maxrec; i++)
            {
                db thisdb = dBList.dbs[i];
                localconnstr = thisdb.connStr;

                if (localconnstr != null && localconnstr != "")
                {
                    string sql = "";
                    sql = sql + "drop table if exists #tempMplayer ";
                    sql = sql + "create table #tempMplayer ( ";
                    sql = sql + "DBname nvarchar(max) null ";
                    sql = sql + ", CurrentPeriod nvarchar(max) null ";
                    sql = sql + ", UpdateDate datetime null ";
                    sql = sql + ", UserName nvarchar(max) null ";
                    sql = sql + ", GameDealerMemberID int null ";
                    sql = sql + ", SelectedNums nvarchar(max) null ";
                    sql = sql + ") ";
                    sql = sql + "insert into #tempMplayer (DBname, CurrentPeriod, UpdateDate, UserName, GameDealerMemberID, SelectedNums) ";
                    sql = sql + "select distinct '@dbName', CurrentPeriod, UpdateDate, UserName, GameDealerMemberID, SelectedNums ";
                    sql = sql + "from Mplayer where CurrentPeriod = '@dbCurrentPeriod' ";
                    sql = sql + "select * from #tempMplayer ";

                    string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod)
                                     .Replace("@dbName", thisdb.MyID);

                    SqlConnection connection = new SqlConnection(localconnstr);
                    connection.Open();
                    DataTable myDataRows = new DataTable();
                    SqlCommand command = new SqlCommand(sql2, connection);
                    command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(myDataRows);
                    connection.Close();

                    int maxcount = myDataRows.Rows.Count;

                    for (int x = 0; x < maxcount; x++)
                    {
                        DataRow r = myDataRows.Rows[x];
                        MPlayerMinimum m = new MPlayerMinimum();
                        m.DBname = r["DBname"].ToString();
                        m.UpdateDate = DateTime.Parse(r["UpdateDate"].ToString());
                        m.SelectedNums = r["SelectedNums"].ToString();
                        m.GameDealerMemberID = int.Parse(r["GameDealerMemberID"].ToString());
                        m.CurrentPeriod = r["CurrentPeriod"].ToString();
                        m.UserName = r["UserName"].ToString();


                        mainlist.Rows.Add(m);
                    }

                }
            }
        }

        public string GetUserName(int GameDealerMemberID, string dbname)
        {
            var localconnstr = "";
            DBList dBList = new DBList();
            int maxrec = dBList.dbs.Count;

            for (int x = 0; x < maxrec; x++)
            {
                db thisdb = dBList.dbs[x];
                if (thisdb.MyID.ToLower() == dbname.ToLower())
                {
                    localconnstr = thisdb.connStr;
                    break;
                }
            }

            string sql2 = "Select top 1 UserName from MPlayer where GameDealerMemberID = " + GameDealerMemberID.ToString() + "";

            SqlConnection connection = new SqlConnection(localconnstr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxcount = myDataRows.Rows.Count;

            for (int x = 0; x < maxcount; x++)
            {
                DataRow thisrow = myDataRows.Rows[x];
                return thisrow["UserName"].ToString();
            }

            return "";

        }

        public string GetGameDealerMPlayerBaseByDB(string DBName, string CurrentPeriod)
        {
            var localconnstr = "";
            DBList dBList = new DBList();
            int maxrec = dBList.dbs.Count;
            db thisdb = new db();
            string jsonString = "";

            for (int i = 0; i < maxrec; i++)
            {
                thisdb = dBList.dbs[i];

                if (thisdb.MyID == DBName) {
                    break;
                }
            }

            localconnstr = thisdb.connStr;

            if (localconnstr != null && localconnstr != "")
            {
                string sql = "";
                sql = sql + "declare @CurrentPeriod nvarchar(max) ";
                sql = sql + "set @CurrentPeriod = '@dbCurrentPeriod' ";
                sql = sql + "drop table if exists #tempGDMPlayer ";
                sql = sql + "create table #tempGDMPlayer ( ";
                sql = sql + "DBname nvarchar(max) null ";
                sql = sql + ", GDMP_ID int null ";
                sql = sql + ", UserName nvarchar(max) null ";
                sql = sql + ", GameDealerMemberID int null ";
                sql = sql + ", MemberID int null ";
                sql = sql + ", SelectedNums nvarchar(max) null ";
                sql = sql + ", UpdateDate datetime null ";
                sql = sql + ", GDMP_Recs int null ";
                sql = sql + ", [MPlayer_Rec] int null ";
                sql = sql + ", [MPUpdateDate] datetime null ";
                sql = sql + ", CurrentPeriod nvarchar(max) null ";
                sql = sql + ") ";
                sql = sql + "insert into #tempGDMPlayer (dbname, GDMP_ID, MemberID, SelectedNums, UpdateDate, CurrentPeriod) ";
                sql = sql + "select '@dbName', ID, MemberID, SelectedNums, UpdateDate, CurrentPeriod ";
                sql = sql + "from GameDealerMPlayer ";
                sql = sql + "where CurrentPeriod = @CurrentPeriod and MemberID <> 0 and MemberID is not null and isWin is null ";
                sql = sql + "Update #tempGDMPlayer ";
                sql = sql + "set UserName = (select top 1 UserName from Mplayer where GamedealerMemberID = a.MemberID) ";
                sql = sql + "from #tempGDMPlayer a ";
                sql = sql + "select * from #tempGDMPlayer ";

                string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod)
                                 .Replace("@dbName", thisdb.MyID);

                SqlConnection connection = new SqlConnection(localconnstr);
                connection.Open();
                DataTable myDataRows = new DataTable();
                SqlCommand command = new SqlCommand(sql2, connection);
                command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(myDataRows);
                connection.Close();

                jsonString = JsonConvert.SerializeObject(myDataRows, Formatting.Indented);
            }

            return jsonString;
        }

        public string GetMPlayerMinimumListByDB(string DBName, string CurrentPeriod)
        {
            var localconnstr = "";
            DBList dBList = new DBList();
            int maxrec = dBList.dbs.Count;
            db thisdb = new db();
            string jsonString = "";

            for (int i = 0; i < maxrec; i++)
            {
                thisdb = dBList.dbs[i];
                if (thisdb.MyID == DBName) {
                    break;
                }
            }

            localconnstr = thisdb.connStr;

            if (localconnstr != null && localconnstr != "")
            {
                string sql = "";
                sql = sql + "drop table if exists #tempMplayer ";
                sql = sql + "create table #tempMplayer ( ";
                sql = sql + "DBname nvarchar(max) null ";
                sql = sql + ", CurrentPeriod nvarchar(max) null ";
                sql = sql + ", UpdateDate datetime null ";
                sql = sql + ", UserName nvarchar(max) null ";
                sql = sql + ", GameDealerMemberID int null ";
                sql = sql + ", SelectedNums nvarchar(max) null ";
                sql = sql + ") ";
                sql = sql + "insert into #tempMplayer (DBname, CurrentPeriod, UpdateDate, UserName, GameDealerMemberID, SelectedNums) ";
                sql = sql + "select distinct '@dbName', CurrentPeriod, UpdateDate, UserName, GameDealerMemberID, SelectedNums ";
                sql = sql + "from Mplayer where CurrentPeriod = '@dbCurrentPeriod' and iswin is null ";
                sql = sql + "select * from #tempMplayer ";

                string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod)
                                 .Replace("@dbName", thisdb.MyID);

                SqlConnection connection = new SqlConnection(localconnstr);
                connection.Open();
                DataTable myDataRows = new DataTable();
                SqlCommand command = new SqlCommand(sql2, connection);
                command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(myDataRows);
                connection.Close();

                jsonString = JsonConvert.SerializeObject(myDataRows, Formatting.Indented);
            }
            return jsonString;
        }

        public void CreateMissingMPlayerByDB(string dbname, string allIDs)
        {
            DBList alldbs = new DBList();
            int mx = alldbs.dbs.Count;
            db thisdb = new db();
            for (int i = 0; i < mx; i++)
            {
                thisdb = alldbs.dbs[i];
                if (thisdb.MyID == dbname)
                {
                    break;
                }
            }

            string localconnstr = thisdb.connStr;

            if (localconnstr != null && localconnstr != "")
            {
                string sql = "";
                sql = sql + "INSERT INTO [dbo].[MPlayer] ([SecondMPlayerID],[MemberID],[GameDealerMemberID],[UserName],[LotteryInfoID],[CompanyID],[CurrentPeriod],[LotteryInfoName],[SelectedNums],[IsAfter],[IsWinStop],[ManualBet],[Multiple],[DiscountPrice],[Price],[Qty],[IsWin],[ShowResultDate],[RebatePro],[RebateProMoney],[WinMoney],[WinMoneyWithCapital],[ReferralPayType],[CashRebatePayType],[CashBackRebatePayType],[IsReferralWriteReport],[IsCashRebateWriteReport],[IsCashBackWriteReport],[IsReset],[CreateID],[CreateDate],[UpdateID],[UpdateDate])";
                sql = sql + "SELECT null,0,gm.MemberID,gp.UserName,[LotteryInfoID],gm.[CompanyID],[CurrentPeriod],[LotteryInfoName] ";
                sql = sql + ",[SelectedNums],[IsAfter],[IsWinStop],[ManualBet],[Multiple],[DiscountPrice],[Price],[Qty],[IsWin],getdate() ";
                sql = sql + ",[RebatePro],[RebateProMoney],[WinMoney],[WinMoneyWithCapital],0,0,0,0,0,0,0,[CreateID],gm.[CreateDate],[UpdateID],gm.[UpdateDate] ";
                sql = sql + "FROM GameDealerMPlayer gm ";
                sql = sql + "LEFT JOIN GameDealerMemberShip gp on gm.MemberID = gp.MemberID ";
                sql = sql + "WHERE gm.ID in ( @dbAllIDs ) ";

                string sql2 = sql.Replace("@dbAllIDs", allIDs);
                SqlConnection connection = new SqlConnection(localconnstr);
                connection.Open();
                SqlCommand command = new SqlCommand(sql2, connection);
                command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void CreateMissingGDMPlayerByDB(string dbname, string allIDs)
        {
            DBList alldbs = new DBList();
            int mx = alldbs.dbs.Count;
            db thisdb = new db();
            for (int i = 0; i < mx; i++)
            {
                thisdb = alldbs.dbs[i];
                if (thisdb.MyID == dbname)
                {
                    break;
                }
            }

            string localconnstr = thisdb.connStr;

            if (localconnstr != null && localconnstr != "")
            {
                string sql = "";
                sql = sql + "INSERT INTO [dbo].[GameDealerMPlayer] ";
                sql = sql + "([MemberID] ";
                sql = sql + ",[LotteryInfoID] ";
                sql = sql + ",[CompanyID] ";
                sql = sql + ",[CurrentPeriod] ";
                sql = sql + ",[LotteryInfoName] ";
                sql = sql + ",[SelectedNums] ";
                sql = sql + ",[IsAfter] ";
                sql = sql + ",[IsWinStop] ";
                sql = sql + ",[ManualBet] ";
                sql = sql + ",[Multiple] ";
                sql = sql + ",[DiscountPrice] ";
                sql = sql + ",[Price] ";
                sql = sql + ",[Qty] ";
                sql = sql + ",[IsWin] ";
                sql = sql + ",[RebatePro] ";
                sql = sql + ",[RebateProMoney] ";
                sql = sql + ",[WinMoney] ";
                sql = sql + ",[WinMoneyWithCapital] ";
                sql = sql + ",[IsWriteReport] ";
                sql = sql + ",[CreateID] ";
                sql = sql + ",[CreateDate] ";
                sql = sql + ",[UpdateID] ";
                sql = sql + ",[UpdateDate]) ";
                sql = sql + "select ";
                sql = sql + "GameDealerMemberID ";
                sql = sql + ", LotteryInfoID ";
                sql = sql + ", CompanyID ";
                sql = sql + ", CurrentPeriod ";
                sql = sql + ", LotteryInfoName ";
                sql = sql + ", SelectedNums ";
                sql = sql + ", IsAfter ";
                sql = sql + ", IsWinStop ";
                sql = sql + ", ManualBet ";
                sql = sql + ", Multiple ";
                sql = sql + ", DiscountPrice ";
                sql = sql + ", Price ";
                sql = sql + ", Qty ";
                sql = sql + ", IsWin ";
                sql = sql + ", RebatePro ";
                sql = sql + ", RebateProMoney ";
                sql = sql + ", WinMoney ";
                sql = sql + ", WinMoneyWithCapital ";
                sql = sql + ", 0 ";
                sql = sql + ", CreateID ";
                sql = sql + ", CreateDate ";
                sql = sql + ", UpdateID ";
                sql = sql + ", UpdateDate ";
                sql = sql + "from mplayer ";
                sql = sql + "where id in ( @dbAllIDs ) ";

                string sql2 = sql.Replace("@dbAllIDs", allIDs);
                SqlConnection connection = new SqlConnection(localconnstr);
                connection.Open();
                SqlCommand command = new SqlCommand(sql2, connection);
                command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public MenuRoots GetMenuRoots()
        {
            string sql = "select text, squence, menurootid from MenuRoot order by squence";

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            MenuRoots myRoots = new MenuRoots();
            myRoots.init();

            int mx = myDataRows.Rows.Count;
            for (int i = 0; i < mx; i++) {
                DataRow drow = myDataRows.Rows[i];

                RootItem rio = new RootItem();
                rio.text = drow["text"].ToString();
                rio.squence = int.Parse( drow["squence"].ToString() );
                rio.menurootid = int.Parse(drow["menurootid"].ToString());

                myRoots.Roots.Add(rio);
            }

            return myRoots;
        }

        //----------------------------------------------------------------------------------------------------------------
        public string GetMenuRootButtons()
        {
            string sql = "select * from mnItem where parentid is null order by squence";

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            MenuRoots myRoots = new MenuRoots();
            myRoots.init();

            int mx = myDataRows.Rows.Count;
            string output = "";
            int leftposition = 0;
            int heightpos = 80;

            for (int i = 0; i < mx; i++)
            {
                DataRow drow = myDataRows.Rows[i];

                output = output + "<button onclick='showdiv(\"div-" + drow["ID"].ToString() + "\")'  style='width:250px;height:80px;' ";
                output = output + "data-MenuRootID='" + drow["ID"].ToString() + "'>" + drow["text"].ToString() ;
                output = output + "</button>";
                output = output + "<label style='opacity:0;width:5px;'></label>";


                //heightpos = heightpos + 80;
                
                int mychild = 0;
                output = output + GetMenuItems(drow["ID"].ToString(), leftposition, 250, 1, ref mychild);

                leftposition = leftposition + 255;

            }

            //output = output + "<button>hide</button>";

            return output;
        }

        public string GetMenuItems(string myID, int leftpos, int wdth, int mylevel, ref int children)
        {
            string output = "";
            string sql = "";
            sql = sql + "select * from mnItem where parentid = " + myID + " order by squence ";

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            RootItems rootItems = new RootItems();
            rootItems.init();
            int toppos = 5;
            int mx = myDataRows.Rows.Count;

            children = mx;
            string zindex = "1";
            string bcolor = "#ff6666";
            string divprefix = "div-";
            int divtoppos = 85;
            if (mylevel == 2)
            {
                zindex = "999";
                bcolor = "#ff6677";
                leftpos = 250;
                divprefix = "l2div-";
                divtoppos = toppos;
            }
            
            output = "<div id='" + divprefix + myID + "' style='z-index:" + zindex + ";display:none;background:" + bcolor + ";color:#ffffff;padding-left:5px;padding-right:5px;padding-top:5px;padding-bottom:5px;position:absolute;top:" + divtoppos + "px;left:" + (leftpos + 5) + "px; width:" + wdth.ToString() + "px;height:" + (mx * 80 + 10) + "px;'>";
            for (int x = 0; x < mx; x++) {
                string buttonstring = "";
                DataRow row = myDataRows.Rows[x];
                buttonstring = buttonstring + "<button onclick='showdiv(\"l2div-" + row["ID"].ToString() + "\", this)' style='padding-left:5px;padding-right:5px;width:240px;height:80px;top:" + toppos.ToString() + "px' data-link='" + row["url"].ToString()  + "'>";
                buttonstring = buttonstring + row["text"].ToString();
                buttonstring = buttonstring + "</button>";
                int mychildren = 0;
                string mychildstring = GetMenuItems(row["ID"].ToString(), leftpos, 250, 2, ref mychildren);
                if (mychildren > 0)
                {
                    buttonstring = "<button onclick='showdiv(\"l2div-" + row["ID"].ToString() + "\", this)' style='background:#fb81dd;padding-left:5px;padding-right:5px;width:240px;height:80px;top:" + toppos.ToString() + "px'>";
                    buttonstring = buttonstring + row["text"].ToString();
                    buttonstring = buttonstring + "</button>";
                    output = output + buttonstring + mychildstring;
                }
                else
                {
                    output = output + buttonstring;
                }
                toppos = toppos + 80;
            }
            output = output + "</div>";
            

            return output;
        }

        // ----------------------------------------------------------------------------------------
        public string GetPendingRecsAllDBbyTicketNo(string ticketNo) {
            string output = "";

            string starttime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            string endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            DBUtil dbu = new DBUtil();
            
            // ---- prepare all 9 dbs for being checked -------

            List<db> dbs = new List<db>();

            db db = new db();
            db.connStr = db_ace99.connStr;
            db.MyID = db_ace99.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_king4d.connStr;
            db.MyID = db_king4d.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_togelking.connStr;
            db.MyID = db_togelking.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_ghl55.connStr;
            db.MyID = db_ghl55.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_tm.connStr;
            db.MyID = db_tm.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_tm2.connStr;
            db.MyID = db_tm2.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_tm3.connStr;
            db.MyID=db_tm3.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_bv.connStr;
            db.MyID = db_bv.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_wl.connStr;
            db.MyID = db_wl.MyID;
            dbs.Add(db);

            db = new db();
            db.connStr = db_local.connStr;
            db.MyID = db_local.MyID;
            dbs.Add(db);

            int mx = dbs.Count;

            CurrentPeriodLight cpl = new CurrentPeriodLight();

            cpl = dbu.GetCurrentPeriodLight(ticketNo);

            for (int x = 0; x < mx; x++)
            {
                db thisdb = dbs[x];

                switch (thisdb.MyID) {
                    case "db_ace99":
                        List<MPlayerLight> listace99 = new List<MPlayerLight>();
                        listace99 = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.ace99_m = listace99.Count;

                        List<GameDealerMPlayerLight> listgd_ace99 = new List<GameDealerMPlayerLight>();
                        listgd_ace99 = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.ace99_g = listgd_ace99.Count;
                        break;

                    case "db_king4d":
                        List<MPlayerLight> listdb_king4d = new List<MPlayerLight>();
                        listdb_king4d = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.king4d_m = listdb_king4d.Count;

                        List<GameDealerMPlayerLight> listgd_king4d = new List<GameDealerMPlayerLight>();
                        listgd_king4d = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.king4d_g = listgd_king4d.Count;
                        break;

                    case "db_togelking":
                        List<MPlayerLight> listdb_togelking = new List<MPlayerLight>();
                        listdb_togelking = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.togelking_m = listdb_togelking.Count;

                        List<GameDealerMPlayerLight> listgd_togelking = new List<GameDealerMPlayerLight>();
                        listgd_togelking = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.togelking_g = listgd_togelking.Count;
                        break;

                    case "db_ghl55":
                        List<MPlayerLight> listdb_ghl55 = new List<MPlayerLight>();
                        listdb_ghl55 = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.ghl55_m = listdb_ghl55.Count;

                        List<GameDealerMPlayerLight> listgd_ghl55 = new List<GameDealerMPlayerLight>();
                        listgd_ghl55 = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.ghl55_g = listgd_ghl55.Count;
                        break;

                    case "db_tm":
                        List<MPlayerLight> listdb_tm = new List<MPlayerLight>();
                        listdb_tm = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.tm_m = listdb_tm.Count;

                        List<GameDealerMPlayerLight> listgd_tm = new List<GameDealerMPlayerLight>();
                        listgd_tm = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.tm_g = listgd_tm.Count;
                        break;

                    case "db_tm2":
                        List<MPlayerLight> listdb_tm2 = new List<MPlayerLight>();
                        listdb_tm2 = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.tm2_m = listdb_tm2.Count;

                        List<GameDealerMPlayerLight> listgd_tm2 = new List<GameDealerMPlayerLight>();
                        listgd_tm2 = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.tm2_g = listgd_tm2.Count;
                        break;

                    case "db_tm3":
                        List<MPlayerLight> listdb_tm3 = new List<MPlayerLight>();
                        listdb_tm3 = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.tm3_m = listdb_tm3.Count;

                        List<GameDealerMPlayerLight> listgd_tm3 = new List<GameDealerMPlayerLight>();
                        listgd_tm3 = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.tm3_g = listgd_tm3.Count;
                        break;

                    case "db_bv":
                        List<MPlayerLight> listdb_bv = new List<MPlayerLight>();
                        listdb_bv = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.bv_m = listdb_bv.Count;

                        List<GameDealerMPlayerLight> listgd_bv = new List<GameDealerMPlayerLight>();
                        listgd_bv = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.bv_g = listgd_bv.Count;
                        break;

                    case "db_wl":
                        List<MPlayerLight> listdb_wl = new List<MPlayerLight>();
                        listdb_wl = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.wl_m = listdb_wl.Count;

                        List<GameDealerMPlayerLight> listgd_wl = new List<GameDealerMPlayerLight>();
                        listgd_wl = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.wl_g = listgd_wl.Count;
                        break;

                    case "db_local":
                        List<MPlayerLight> listdb_local = new List<MPlayerLight>();
                        listdb_local = dbu.GetMPlayerLightList(ticketNo, "", thisdb);
                        cpl.local_m = listdb_local.Count;

                        List<GameDealerMPlayerLight> listgd_local = new List<GameDealerMPlayerLight>();
                        listgd_local = dbu.GetGameDealerMPlayerLightList(ticketNo, "", thisdb);
                        cpl.local_g = listgd_local.Count;
                        break;
                }

                cpl.m_total = cpl.ace99_m + cpl.king4d_m + cpl.togelking_m + cpl.bv_m + cpl.wl_m + cpl.tm_m + cpl.tm2_m + cpl.tm3_m + cpl.ghl55_m + cpl.local_m;
                cpl.g_total = cpl.ace99_g + cpl.king4d_g + cpl.togelking_g + cpl.bv_g + cpl.wl_g + cpl.tm_g + cpl.tm2_g + cpl.tm3_g + cpl.ghl55_g + cpl.local_g;
            }

            if (cpl.IsOpen == null)
            {
                cpl.IsOpen = "";
            }

            output = "<table border=1 style='border-style:solid;boder-color:grey;border-width:3px;'>";
            output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TicketNo</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.CurrentPeriod + "</td></tr>";
            output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>Real Close Time</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.RealCloseTime.ToString() + "</td></tr>";
            output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>Is Open</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.IsOpen.ToString() + "</td></tr>";
            output = output + "<tr><td colspan='2'><label style='opacity:0;display:block;height:20px;'> </label></td></tr>";

            if (cpl.ace99_m != cpl.ace99_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_ace99'>ACE99 MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'>" + cpl.ace99_m + "</td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>ACE99 MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.ace99_m + "</td></tr>";
            }

            if (cpl.king4d_m != cpl.king4d_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_king4d'>King4D MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_king4d'>" + cpl.king4d_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>King4D MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.king4d_m + "</td></tr>";
            }

            if (cpl.togelking_m != cpl.togelking_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_togelking'>TogelKing MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_togelking'>" + cpl.togelking_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TogelKing MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.togelking_m + "</td></tr>";
            }

            if (cpl.ghl55_m != cpl.ghl55_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_ghl55'>GHL 55 MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_ghl55'>" + cpl.ghl55_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>GHL 55 MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.ghl55_m + "</td></tr>";
            }

            if (cpl.tm_m != cpl.tm_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm'>TM MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm'>" + cpl.tm_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TM MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.tm_m + "</td></tr>";
            }

            if (cpl.tm2_m != cpl.tm2_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm2'>TM2 MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm2'>" + cpl.tm2_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TM2 MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.tm2_m + "</td></tr>";
            }

            if (cpl.tm3_m != cpl.tm3_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm3'>TM3 MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm3'>" + cpl.tm3_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TM3 MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.tm3_m + "</td></tr>";
            }

            if (cpl.wl_m != cpl.wl_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_wl'>WL MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_wl'>" + cpl.wl_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>WL MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.wl_m + "</td></tr>";
            }

            if (cpl.bv_m != cpl.bv_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_bv'>BV MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_bv'>" + cpl.bv_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>BV MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.bv_m + "</td></tr>";
            }

            if (cpl.local_m != cpl.local_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_local'>Local MP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a  href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_local'>" + cpl.local_m + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>Local MP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.local_m + "</td></tr>";
            }
            output = output + "<tr><td colspan='2'><label style='opacity:0;display:block;height:20px;'> </label></td></tr>";

            //---- GameDealerMPlayer Portion -------------------------------------------------------------------------------------------------------------------------------------------

            if (cpl.ace99_m != cpl.ace99_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_ace99'>ACE99 GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_ace99'>" + cpl.ace99_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>ACE99 GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.ace99_g + "</td></tr>";
            }

            if (cpl.king4d_m != cpl.king4d_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_king4d'>King4D GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_king4d'>" + cpl.king4d_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>King4D GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.king4d_g + "</td></tr>";
            }

            if (cpl.togelking_m != cpl.togelking_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_togelking'>TogelKing GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_togelking'>" + cpl.togelking_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TogelKing GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.togelking_g + "</td></tr>";
            }

            if (cpl.ghl55_m != cpl.ghl55_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_ghl55'>GHL 55 GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_ghl55'>" + cpl.ghl55_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>GHL 55 GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.ghl55_g + "</td></tr>";
            }

            if (cpl.tm_m != cpl.tm_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm'>TM GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm'>" + cpl.tm_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TM GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.tm_g + "</td></tr>";
            }

            if (cpl.tm2_m != cpl.tm2_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm2'>TM2 GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm2'>" + cpl.tm2_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TM2 GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.tm2_g + "</td></tr>";
            }

            if (cpl.tm3_m != cpl.tm3_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm3'>TM3 GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_tm3'>" + cpl.tm3_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>TM3 GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.tm3_g + "</td></tr>";
            }

            if (cpl.wl_m != cpl.wl_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_wl'>WL GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_wl'>" + cpl.wl_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>WL GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.wl_g + "</td></tr>";
            }

            if (cpl.bv_m != cpl.bv_g)
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_bv'>BV GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_bv'>" + cpl.bv_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>BV GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.bv_g + "</td></tr>";
            }

            if (cpl.local_m != cpl.local_g)
            {
                output = output + "<tr><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_local'>Local GDMP</a></td><td class='diffclass' style='font-weight:bolder;font-size:16px;'><a href='SynchronizePlayers.html?ticket=" + cpl.CurrentPeriod + "&dbname=db_local'>" + cpl.local_g + "</a></td></tr>";
            }
            else
            {
                output = output + "<tr><td style='font-weight:bolder;font-size:16px;'>Local GDMP</td><td style='font-weight:bolder;font-size:16px;'>" + cpl.local_g + "</td></tr>";
            }
            

            output = output + "</table>";

            return output;
        }

        public List<MPlayerLight> GetMPlayerLightList(string CurrentPeriod, string isWinType, db myDB)
        {
            List<MPlayerLight> outlist = new List<MPlayerLight>();

            string sql = "";
            sql = sql + "select ";
            sql = sql + "ID, ";
            sql = sql + "UserName,  ";
            sql = sql + "ShowResultDate ";
            sql = sql + ", CurrentPeriod ";
            sql = sql + ", IsWin = case ";
            sql = sql + "when iswin is null then '' ";
            sql = sql + "when iswin = 0 then '0' ";
            sql = sql + "when iswin = 1 then '1' ";
            sql = sql + "end ";
            sql = sql + "from Mplayer ";
            sql = sql + "where CurrentPeriod = '@dbCurrentPeriod' ";
            if (isWinType == "")
            {
                sql = sql + "and IsWin is null ";
            }
            else
            {
                sql = sql + "and IsWin is not null ";
            }

            string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod);

            SqlConnection connection = new SqlConnection(myDB.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            outlist = myDataRows.ToList<MPlayerLight>();

            return outlist;
        }

        public List<GameDealerMPlayerLight> GetGameDealerMPlayerLightList(string CurrentPeriod, string isWinType, db myDB)
        {
            List<GameDealerMPlayerLight> outlist = new List<GameDealerMPlayerLight>();

            string sql = "";
            sql = sql + "select ";
            sql = sql + "ID ";
            //sql = sql + "UserName,  ";
            //sql = sql + "ShowResultDate ";
            sql = sql + ", CurrentPeriod ";
            sql = sql + ", IsWin = case ";
            sql = sql + "when iswin is null then '' ";
            sql = sql + "when iswin = 0 then '0' ";
            sql = sql + "when iswin = 1 then '1' ";
            sql = sql + "end ";
            sql = sql + "from GamedealerMplayer ";
            sql = sql + "where CurrentPeriod = '@dbCurrentPeriod' ";
            if (isWinType == "")
            {
                sql = sql + "and IsWin is null ";
            }
            else
            {
                sql = sql + "and IsWin is not null ";
            }

            string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod);

            SqlConnection connection = new SqlConnection(myDB.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            outlist = myDataRows.ToList<GameDealerMPlayerLight>();

            return outlist;
        }

        public CurrentPeriodLight GetCurrentPeriodLight(string CurrentPeriod)
        {
            CurrentPeriodLight cpl = new CurrentPeriodLight();

            string sql = "";
            sql = sql + "select distinct ";
            sql = sql + "CurrentPeriod ";
            sql = sql + ", RealCloseTime ";
            sql = sql + ", IsOpen = case ";
            sql = sql + "when isopen is null then '' ";
            sql = sql + "when isopen = 1 then '1' ";
            sql = sql + "when isopen = 0 then '0' ";
            sql = sql + "end ";
            sql = sql + "from OLottery ";
            sql = sql + "where CurrentPeriod = '@dbCurrentPeriod' ";

            string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod);

            SqlConnection connection = new SqlConnection(db_ghl33.connStr); // we always take from server 31 for olottery info
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int mx = myDataRows.Rows.Count;

            for (int i = 0; i < mx; i++)
            {
                DataRow row = myDataRows.Rows[i];
                cpl.CurrentPeriod = row["CurrentPeriod"].ToString();
                cpl.RealCloseTime = DateTime.Parse(row["RealCloseTime"].ToString());
                cpl.IsOpen = row["IsOpen"].ToString();
            }

            return cpl;
        }

        public List<CurrentPeriodLight> GetCurrentPeriodLightListByDates(string StartDate, string EndDate)
        {
            List<CurrentPeriodLight> outlist = new List<CurrentPeriodLight>();

            string sql = "";
            sql = sql + "select distinct ";
            sql = sql + "CurrentPeriod ";
            sql = sql + ", RealCloseTime ";
            sql = sql + ", IsOpen = case ";
            sql = sql + "when isopen is null then '' ";
            sql = sql + "when isopen = 1 then '1' ";
            sql = sql + "when isopen = 0 then '0' ";
            sql = sql + "end ";
            sql = sql + "from OLottery ";
            sql = sql + "where RealCloseTime >= '@dbStartDate' ";
            sql = sql + "and RealCloseTime <= '@dbEndDate' ";

            string sql2 = sql.Replace("@dbStartDate", StartDate)
                             .Replace("@dbEndDate", EndDate);

            SqlConnection connection = new SqlConnection(db_ghl33.connStr); // we always take from server 31 for olottery info
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            outlist = myDataRows.ToList<CurrentPeriodLight>();

            return outlist;
        }

        public string GetLastAgentCode()
        {
            string result = "";
            string sql = "select val from settings where id = 1";

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int mx = myDataRows.Rows.Count;
            for (int i = 0; i < mx; i++)
            {
                DataRow drow = myDataRows.Rows[i];

                result = drow["val"].ToString();
            }

            return result;
        }

        public string GetRecordDifferenceByDB(string dbname, string CurrentPeriod)
        {
            DBList alldbs = new DBList();
            int mx = alldbs.dbs.Count;
            db thisdb = new db();
            for (int i = 0; i < mx; i++)
            {
                thisdb = alldbs.dbs[i];
                if (thisdb.MyID == dbname)
                {
                    break;
                }
            }

            string localconnstr = thisdb.connStr;

            string txt = "";

            if (localconnstr != null && localconnstr != "")
            {
                string sql = "";
                sql = sql + "declare @currentPeriod nvarchar(max) ";
                sql = sql + "set @currentPeriod = '@dbCurrentPeriod' ";
                sql = sql + "drop table if exists #tempcompare ";
                sql = sql + "create table #tempcompare ";
                sql = sql + "( ";
                sql = sql + "rowid int identity(1,1) not null ";
                sql = sql + ", CurrentPeriod nvarchar(max) null ";
                sql = sql + ", UserName nvarchar(max) null ";
                sql = sql + ", SelectedNums nvarchar(max) null ";
                sql = sql + ", GameDealerMemberID int null ";
                sql = sql + ", MP_rec int null ";
                sql = sql + ", GDMP_rec int null ";
                sql = sql + ", mp_id int null ";
                sql = sql + ", gdmp_id int null ";
                sql = sql + ") ";
                sql = sql + "insert into #tempcompare (CurrentPeriod, UserName, SelectedNums, GameDealerMemberID) ";
                sql = sql + "select CurrentPeriod, UserName, SelectedNums, GamedealerMemberID ";
                sql = sql + "from ( ";
                sql = sql + "select CurrentPeriod, UserName, SelectedNums, GamedealerMemberID ";
                sql = sql + "from mplayer ";
                sql = sql + "where currentperiod = @currentPeriod ";
                sql = sql + "group by CurrentPeriod, UserName, SelectedNums, GamedealerMemberID ";
                sql = sql + "union ";
                sql = sql + "select CurrentPeriod, b.UserName, SelectedNums, a.MemberID as GameDealerMemberID ";
                sql = sql + "from gamedealermplayer a ";
                sql = sql + "left join GameDealerMemberShip b on a.MemberID = b.MemberID ";
                sql = sql + "where currentperiod = @currentPeriod ";
                sql = sql + "group by CurrentPeriod, b.UserName, SelectedNums, a.MemberID ";
                sql = sql + ") x ";
                sql = sql + "group by CurrentPeriod, UserName, SelectedNums, GamedealerMemberID ";
                sql = sql + "drop table if exists #tempgdmp ";
                sql = sql + "create table #tempgdmp ( ";
                sql = sql + "Id int null ";
                sql = sql + ", CurrentPeriod nvarchar(max) null ";
                sql = sql + ", SelectedNums nvarchar(max) null ";
                sql = sql + ", MemberID int null ";
                sql = sql + ") ";
                sql = sql + "insert into #tempgdmp (id, CurrentPeriod, SelectedNums, memberid) ";
                sql = sql + "select id, CurrentPeriod, SelectedNums, MemberID from gamedealermplayer where currentperiod = @currentPeriod ";
                sql = sql + "drop table if exists #tempmplayer  ";
                sql = sql + "create table #tempmplayer ";
                sql = sql + "( ";
                sql = sql + "ID int null ";
                sql = sql + ", CurrentPeriod nvarchar(max) null ";
                sql = sql + ", SelectedNums nvarchar(max) null ";
                sql = sql + ", GamedealerMemberID int null ";
                sql = sql + ") ";
                sql = sql + "insert into #tempmplayer (CurrentPeriod, SelectedNums, GamedealerMemberID, id) ";
                sql = sql + "select CurrentPeriod, SelectedNums, GamedealerMemberID, id ";
                sql = sql + "from mplayer where CurrentPeriod = @currentPeriod and IsWin is Null ";
                sql = sql + "update #tempcompare ";
                sql = sql + "set mp_rec = (select count(*) from #tempmplayer where CurrentPeriod = x.CurrentPeriod and SelectedNums = x.SelectedNums and GameDealerMemberID = x.GameDealerMemberID) ";
                sql = sql + ", GDMP_rec = (select count(*) from #tempgdmp where CurrentPeriod = x.CurrentPeriod and SelectedNums = x.SelectedNums and MemberID = x.GameDealerMemberID) ";
                sql = sql + "from #tempcompare x ";
                sql = sql + "update #tempcompare ";
                sql = sql + "set mp_id = isnull((select top 1 id from #tempmplayer where CurrentPeriod = a.currentPeriod and SelectedNums = a.selectedNums and GameDealerMemberID = a.GameDealerMemberID order by id asc), 0) ";
                sql = sql + "from #tempcompare a ";
                sql = sql + "update #tempcompare ";
                sql = sql + "set gdmp_id = isnull((select top 1 id from #tempgdmp where CurrentPeriod = a.currentPeriod and SelectedNums = a.selectedNums and MemberID = a.GameDealerMemberID order by id asc), 0) ";
                sql = sql + "from #tempcompare a ";
                sql = sql + "select * from #tempcompare where mp_rec <> gdmp_rec ";

                string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod);
                SqlConnection connection = new SqlConnection(localconnstr);
                connection.Open();
                DataTable myDataRows = new DataTable();
                SqlCommand command = new SqlCommand(sql2, connection);
                command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(myDataRows);
                //command.ExecuteNonQuery();
                connection.Close();

                var mx2 = myDataRows.Rows.Count;

                txt = "<table cellspacing=0 cellpadding=0>";
                txt = txt + "<tr>";
                txt = txt + "<td class='cell hdcell'>##</td>";
                txt = txt + "<td class='cell hdcell'>Current Period</td>";
                txt = txt + "<td class='cell hdcell'>User Name</td>";
                txt = txt + "<td class='cell hdcell'>Selected Nums</td>";
                txt = txt + "<td class='cell hdcell'>GamedealerMemberId</td>";
                txt = txt + "<td class='cell hdcell'>Mplayer Rec</td>";
                txt = txt + "<td class='cell hdcell'>GamedealerMplayer Rec</td>";
                txt = txt + "<td class='cell hdcell'>MPlayer ID to Keep</td>";
                txt = txt + "<td class='cell hdcell'>GDMP ID to Keep</td>";
                txt = txt + "<td class='cell hdcell'>Action</td>";
                txt = txt + "</tr>";

                for (int i = 0; i < mx2; i++)
                {
                    var thisrow = myDataRows.Rows[i];

                    int mprec = int.Parse(thisrow["MP_rec"].ToString());
                    int gdmprec = int.Parse(thisrow["GDMP_rec"].ToString());

                    var highlight = "";
                    var buttext = "";
                    if (mprec - gdmprec > 0 && gdmprec != 0)
                    {
                        highlight = "warningcell";
                        buttext = "Remove Dup MP";
                    }
                    else if (mprec - gdmprec < 0 && mprec != 0)
                    {
                        highlight = "bluecell"; // darkgreen
                        buttext = "Remove Dup GDMP";
                    }
                    else if (mprec - gdmprec > 0 && gdmprec == 0)
                    {
                        highlight = "purplecell";
                        buttext = "Create GDMP";
                    }
                    else if (mprec - gdmprec < 0 && mprec == 0)
                    {
                        highlight = "pinkcell";
                        buttext = "Create MP";

                        var gdmpid = int.Parse(thisrow["gdmp_id"].ToString());

                        CreateMissingMPlayerByDB(thisdb.MyID, gdmpid.ToString()); //temp fixed
                    }
                    txt = txt + "<tr>";
                    txt = txt + "<td class='cell " + highlight + "'>" + thisrow["rowid"].ToString() + "</td>";
                    txt = txt + "<td class='cell " + highlight+ "'>" + thisrow["CurrentPeriod"].ToString() + "</td>";
                    txt = txt + "<td class='cell " + highlight+ "'>" + thisrow["UserName"].ToString() + "</td>";

                    var rowid = thisrow["rowid"].ToString();
                    var selnum = thisrow["SelectedNums"].ToString();

                    txt = txt + "<td class='cell " + highlight + "'>" + thisrow["SelectedNums"].ToString() + "</td>";
                    txt = txt + "<td class='cell "+ highlight+"'>" + thisrow["GamedealerMemberId"].ToString() + "</td>";
                    txt = txt + "<td class='cell "+ highlight + "'>" + thisrow["MP_rec"].ToString() + "</td>";
                    txt = txt + "<td class='cell "+ highlight + "'>" + thisrow["GDMP_rec"].ToString() + "</td>";
                    txt = txt + "<td class='cell " + highlight+ "'>" + thisrow["mp_id"].ToString() + "</td>";
                    txt = txt + "<td class='cell "+ highlight + "'>" + thisrow["gdmp_id"].ToString() + "</td>";

                    if (buttext != "")
                    {
                        if (buttext == "Remove Dup GDMP")
                        {
                            txt = txt + "<td class='cell " + highlight + "'><span class='spanbutt' onclick='removedupgdmp(this)' ";
                            txt = txt + "data-CurrentPeriod='" + thisrow["CurrentPeriod"].ToString() + "' ";
                            txt = txt + "data-SelectedNums='" + thisrow["SelectedNums"].ToString() + "' ";
                            txt = txt + "data-GameDealerMemberId='" + thisrow["GamedealerMemberId"].ToString() + "' ";
                            txt = txt + "data-IDtoKeep='" + thisrow["gdmp_id"].ToString() + "' ";
                            txt = txt + "data-ConnStr='" + thisdb.connStr + "' ";
                            txt = txt + ">" + buttext + "</span></td>";
                        }
                        else if (buttext == "Remove Dup MP")
                        {
                            txt = txt + "<td class='cell " + highlight + "'><span class='spanbutt' onclick='removedupmp(this)' ";
                            txt = txt + "data-CurrentPeriod='" + thisrow["CurrentPeriod"].ToString() + "' ";
                            txt = txt + "data-SelectedNums='" + thisrow["SelectedNums"].ToString() + "' ";
                            txt = txt + "data-GameDealerMemberId='" + thisrow["GamedealerMemberId"].ToString() + "' ";
                            txt = txt + "data-IDtoKeep='" + thisrow["mp_id"].ToString() + "' ";
                            txt = txt + "data-ConnStr='" + thisdb.connStr + "' ";
                            txt = txt + ">" + buttext + "</span></td>";
                        }
                        else if (buttext == "Create MP")
                        {
                            txt = txt + "<td class='cell " + highlight + "'><span class='spanbutt' onclick='createmp(this)' ";
                            txt = txt + "data-AllIDs='" + thisrow["gdmp_id"].ToString() + "' ";
                            txt = txt + "data-dbname='" + thisdb.MyID + "' ";
                            txt = txt + ">" + buttext + "</span></td>";
                        }
                        else if (buttext == "Create GDMP")
                        {
                            txt = txt + "<td class='cell " + highlight + "'><span class='spanbutt' onclick='creategdmp(this)' ";
                            txt = txt + "data-AllIDs='" + thisrow["mp_id"].ToString() + "' ";
                            txt = txt + "data-dbname='" + thisdb.MyID + "' ";
                            txt = txt + ">" + buttext + "</span></td>";
                        }
                        else
                        {
                            txt = txt + "<td class='cell " + highlight + "'><span class='spanbutt'>" + buttext + "</span></td>";
                        }
                    }
                    else
                    {
                        txt = txt + "<td class='cell " + highlight + "'><span>" + buttext + "</span></td>";
                    }
                    txt = txt + "</tr>";
                }
                txt = txt + "</table>";
            }

            string returntext = txt;
            return returntext;
        }

        public List<MenuItem> GetMenuRootitems()
        {
            string sql = "select a.*, Children = (select count(*) from mnitem where parentId = a.ID) from mnItem a where parentid is null order by squence";

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            List<MenuItem> roots = new List<MenuItem>();

            for (int i = 0; i < myDataRows.Rows.Count; i++)
            {
                DataRow dr = myDataRows.Rows[i];
                MenuItem mi = new MenuItem();

                mi.ID = int.Parse(dr["ID"].ToString());
                mi.text = dr["text"].ToString();
                mi.url = dr["url"].ToString();

                bool test = false;
                if (dr["islink"] == null)
                {
                    mi.IsLink =false;
                }
                else
                {
                    bool.TryParse(dr["islink"].ToString(), out test);
                    if (test) { mi.IsLink =true; }
                    else { mi.IsLink =false; }
                    //mi.IsLink = bool.Parse(dr["islink"].ToString());
                }
                
                mi.Squence = int.Parse(dr["squence"].ToString());
                mi.Children = int.Parse(dr["children"].ToString());


                int pid = 0;

                bool fool = int.TryParse(dr["parentid"].ToString(), out pid);

                if (!fool)
                {
                    mi.ParentID = 0;
                }
                else
                {
                    mi.ParentID = pid;
                }
                

                roots.Add(mi);
            }

            return roots;
        }

        public List<MenuItem> GetMenuChildItems(string myid)
        {
            string sql = "select a.*, Children = (select count(*) from mnitem where parentId = a.ID) from mnItem a where parentid = " + myid + " order by squence";

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            List<MenuItem> items = new List<MenuItem>();

            for (int i = 0; i < myDataRows.Rows.Count; i++)
            {
                DataRow dr = myDataRows.Rows[i];
                MenuItem mi = new MenuItem();

                mi.ID = int.Parse(dr["ID"].ToString());
                mi.text = dr["text"].ToString();
                mi.url = dr["url"].ToString();

                bool test = false;
                if (dr["islink"] == null)
                {
                    mi.IsLink = false;
                }
                else
                {
                    bool.TryParse(dr["islink"].ToString(), out test);
                    if (test) { mi.IsLink = true; }
                    else { mi.IsLink = false; }
                    //mi.IsLink = bool.Parse(dr["islink"].ToString());
                }

                mi.Squence = int.Parse(dr["squence"].ToString());
                mi.Children = int.Parse(dr["children"].ToString());

                int pid = 0;

                test = int.TryParse(dr["parentid"].ToString(), out pid);

                if (!test)
                {
                    mi.ParentID = 0;
                }
                else
                {
                    mi.ParentID = pid;
                }

                items.Add(mi);
            }

            return items;
        }

        public string DeleteMenuItem(string ID)
        {
            string sql = "delete from mnitem where ID = " + ID;

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            command.ExecuteNonQuery();

            sql = "delete from mnitem where parentid = " + ID;
            command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            command.ExecuteNonQuery();

            connection.Close();

            return "Menu Item Deleted Successfully";
        }

        public string AddMenuRoot(MenuItemInput mi)
        {

            var txt = mi.text;
            var seq = mi.Squence;
            var url1 = mi.url;

            string sql = "insert into mnitem (text, squence) values ('" + txt + "', " + seq + ")";

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            command.ExecuteNonQuery();
            connection.Close();

            return "Menu Item Created Successfully";
        }

        public string AddMenuChildItem(MenuItemInput mi)
        {
            var txt = mi.text;
            var seq = mi.Squence;
            var url1 = mi.url;
            var parentid = mi.ParentID;

            string sql = "insert into mnitem (text, squence, url, parentid) values ('" + txt + "', " + seq + ", '" + url1 + "', " + parentid + ")";

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            command.ExecuteNonQuery();
            connection.Close();

            return "Menu Item Created Successfully";
        }

        public string EditMenuRoot(MenuItemInput mi)
        {
            var ID = mi.mID;
            var txt = mi.text;
            var seq = mi.Squence;
            var url1 = mi.url;
            var parentid = mi.ParentID;
            var islink = mi.IsLink;

            if (mi.ParentID == "0")
            {
                parentid = "NULL";
            }

            if (mi.IsLink == "")
            {
                islink = "0";
            }

            string sql = "update mnitem set text = '@dbText', Squence = @dbSeq, url = '@dbURL', ParentID = " + parentid + ", IsLink = " +islink + " where id = @dbID";
            string sql2 = sql.Replace("@dbText", txt).Replace("@dbSeq", seq).Replace("@dbURL", url1).Replace("@dbID", ID);

            SqlConnection connection = new SqlConnection(db_local_support.connStr);
            connection.Open();
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            command.ExecuteNonQuery();
            connection.Close();

            return "Menu Item Modified Successfully";
        }

        public string GetApid(string CurrentPeriod)
        {
            string result = "";
            string txt = "";
            string sql = "select * from platformsetting where Apid = '@dbCurrentPeriod'";
            string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod);
            SqlConnection connection = new SqlConnection(db_ghl33.connStr);
            connection.Open();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            DataTable myDataRows = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();
            int mx = myDataRows.Rows.Count;
            txt = "<table cellspacing=0 cellpadding=0>";
            txt = txt + "<tr>";
            txt = txt + "<td class='cell hdcell'>##</td>";
            txt = txt + "<td class='cell hdcell'>CompanyID</td>";
            txt = txt + "<td class='cell hdcell'>PlatformGroup</td>";
            txt = txt + "<td class='cell hdcell'>APID</td>";
            txt = txt + "<td class='cell hdcell'>Status</td>";
            txt = txt + "<td class='cell hdcell'>Action</td>";
            txt = txt + "</tr>";
            for (int i = 0; i < mx; i++)
            {
                var thisrow = myDataRows.Rows[i];

                string sqlStatus = "select * from LotteryTypeMaintain where companyID = '@dbcompanyID' and LotteryTypeID in (18,19)";
                string sqlStatus2 = sqlStatus.Replace("@dbcompanyID", thisrow["ID"].ToString());

                SqlConnection connection1 = new SqlConnection(db_tm.connStr);
                connection1.Open();

                SqlCommand command1 = new SqlCommand(sqlStatus2, connection1);
                command1.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                DataTable myDataRows1 = new DataTable();

                SqlDataAdapter adapter1 = new SqlDataAdapter(command1);
                adapter1.Fill(myDataRows1);
                connection1.Close();
                int thisrow1 = myDataRows1.Rows.Count;
                string status = "";
                string status2 = "";
                if (thisrow1 > 0)
                {
                    status = "Enabled";
                    status2 = "Disabled";
                }
                else
                {
                    status = "Disabled";
                    status2 = "Enabled";
                }

                // “Enabled” when there is no record can be found on ThirdMdatabase with above query. So, the only available option for next action is to “Disable”

                txt = txt + "<tr>";
                txt = txt + "<td class='cell'>" + i.ToString() + "</td>";
                txt = txt + "<td class='cell'>" + thisrow["ID"].ToString() + "</td>";
                txt = txt + "<td class='cell'>" + thisrow["PlatformGroup"].ToString() + "</td>";
                txt = txt + "<td class='cell'>" + thisrow["APID"].ToString() + "</td>";
                txt = txt + "<td class='cell'>" + status + "</td>";
                txt = txt + "<td class='cell'><span class='spanbutt' onclick='toggleStatus(this)' data-apid='" + thisrow["ID"].ToString() + "' data-status='" + status + "'>" + status2 + "</span></td>";
                txt = txt + "</tr>";
            }
            txt = txt + "</table>";

            string returntext = txt;
            return returntext;
        }

        public void ChangeStatusHkSyd(string companyId, string status)
        {
            Console.WriteLine(status == "Enabled");

            if (status == "Enabled")
            {
                string sql = "delete from LotteryTypeMaintain where LotteryTypeID in (18, 19) CompanyID = '@dbCurrentPeriod'";
                string sql2 = sql.Replace("@dbCurrentPeriod", companyId);

                SqlConnection connection = new SqlConnection(db_tm.connStr);
                connection.Open();

                SqlCommand command = new SqlCommand(sql2, connection);
                command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                command.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                string sqlHk = "insert into LotteryTypeMaintain (LotteryTypeID,CompanyID,Status,IsMaintain,IsCloseGame,CreateDate,CreateBy,UpdateDate,UpdateBy) VALUES (18, @dbCurrentPeriod, 0, 0, 0, CURRENT_TIMESTAMP, 0, CURRENT_TIMESTAMP, 0)";
                string sqlHk2 = sqlHk.Replace("@dbCurrentPeriod", companyId);

                SqlConnection connection = new SqlConnection(db_tm.connStr);
                connection.Open();

                SqlCommand command1 = new SqlCommand(sqlHk2, connection);
                command1.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                command1.ExecuteNonQuery();

                string sqlSyd = "insert into LotteryTypeMaintain (LotteryTypeID,CompanyID,Status,IsMaintain,IsCloseGame,CreateDate,CreateBy,UpdateDate,UpdateBy) VALUES (19, @dbCurrentPeriod, 0, 0, 0, CURRENT_TIMESTAMP, 0, CURRENT_TIMESTAMP, 0)";
                string sqlSyd2 = sqlSyd.Replace("@dbCurrentPeriod", companyId);

                SqlCommand command2 = new SqlCommand(sqlSyd2, connection);
                command2.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
                command2.ExecuteNonQuery();

                connection.Close();
            }

        }

        public string GetConnStr(string dbname)
        {
            DBList alldbs = new DBList();
            int mx = alldbs.dbs.Count;
            db thisdb = new db();
            for (int i = 0; i < mx; i++)
            {
                thisdb = alldbs.dbs[i];

                if (thisdb.MyID.IndexOf(dbname) != -1)
                {
                    break;
                }
            }

            string localconnstr = thisdb.connStr;

            return localconnstr;
        }
    }

    
}
