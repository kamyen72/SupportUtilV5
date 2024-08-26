using DupRecRemoval.Classes;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SupportUtilV3.Classes;
using System.Data;
using System.Net;

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
                sql = sql + "where CurrentPeriod = @CurrentPeriod and MemberID <> 0 and MemberID is not null and isWin is null";
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
                sql = sql + "INSERT INTO MPlayer ";
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
    }

    
}
