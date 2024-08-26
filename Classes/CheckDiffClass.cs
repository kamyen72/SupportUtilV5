using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SupportUtil.Classes;
using SupportUtilV2.Classes;
using System.Data;

namespace DupRecRemoval.Classes
{
    public class CheckDiffClass
    {
        // this class will process each db for difference

        public List<DiffRec> CheckDiff(string CurrentPeriod, string dbconnstr)
        {
            string sql = "";
            sql = sql + "declare @CurrentPeriod nvarchar(max); ";
            sql = sql + "set @CurrentPeriod = '@dbCurrentPeriod'; ";
            sql = sql + "drop table if exists #tempcompare ";
            sql = sql + "create table #tempcompare ( ";
            sql = sql + "	id int identity(1,1) not null, ";
            sql = sql + "	CurrentPeriod nvarchar(max) null, ";
            sql = sql + "	SelectedNums nvarchar(max) null, ";
            sql = sql + "	UserName nvarchar(max) null, ";
            sql = sql + "	GameDealerMemberID int null, ";
            sql = sql + "	MPlayer_Recs int null, ";
            sql = sql + "	GDMPlayer_Recs int null, ";
            sql = sql + "	Diff int null ";
            sql = sql + ") ";
            sql = sql + "insert into #tempcompare (CurrentPeriod, SelectedNums, UserName, GameDealerMemberID, MPlayer_Recs, GDMPlayer_Recs) ";
            sql = sql + "select CurrentPeriod, SelectedNums, UserName, GameDealerMemberID, count(*) as Recs ";
            sql = sql + ", (select count(*)  ";
            sql = sql + "from GameDealerMPlayer where CurrentPeriod = a.CurrentPeriod and SelectedNums = a.SelectedNums and MemberID = a.GameDealerMemberID and iswin is null) ";
            sql = sql + "from Mplayer a ";
            sql = sql + "where CurrentPeriod = @CurrentPeriod and iswin is null ";
            sql = sql + "group by CurrentPeriod, SelectedNums, UserName, GameDealerMemberID ";
            sql = sql + "order by count(*) desc ";
            sql = sql + "update #tempcompare set diff = MPlayer_Recs - GDMPlayer_Recs ";
            sql = sql + "select * from #tempcompare  where diff <> 0 order by CurrentPeriod ";

            string sql2 = sql.Replace("@dbCurrentPeriod", CurrentPeriod);

            SqlConnection connection = new SqlConnection(dbconnstr);
            DataTable myDataRows = new DataTable();
            SqlCommand command = new SqlCommand(sql2, connection);
            command.CommandTimeout = 300; // 5 minutes (60 seconds X 5)
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(myDataRows);
            connection.Close();

            int maxrows = myDataRows.Rows.Count;
            List<DiffRec> diffrecs = new List<DiffRec>();
            for (int i = 0; i < maxrows; i++)
            {
                DataRow row = myDataRows.Rows[i];
                DiffRec drec = new DiffRec();
                drec.currentperiod = row["CurrentPeriod"].ToString();
                drec.selectednums = row["SelectedNums"].ToString();
                drec.username = row["UserName"].ToString();
                drec.gamedealermemberid = row["GameDealerMemberID"].ToString();
                drec.mplayer_recs = int.Parse(row["MPlayer_Recs"].ToString());
                drec.gdmplayer_recs = int.Parse(row["GDMPlayer_Recs"].ToString());
                drec.diff = int.Parse(row["Diff"].ToString());
                diffrecs.Add(drec);
            }

            return diffrecs;
        }

        public bool CompareGDMP_MP(List<GameDealerMPlayerBase> gdmplist, List<MPlayerMinimum> mplist, ref List<GameDealerMPlayerBase> gdmpfilterlist)
        {
            int gdpmax = gdmplist.Count;
            int mpmax = mplist.Count;
            bool isMissingItem = true;
            int recfound = 0;
            int missedrec = 0;
            var bb = "";
            for (int g = 0; g < gdpmax; g++)
            {
                GameDealerMPlayerBase gbase = gdmplist[g];
                
                int missingrec = 0;

                bb = gbase.DBname;

                var resultlist = mplist.Where(m => m.DBname == bb && m.GameDealerMemberID == gbase.MemberID && m.CurrentPeriod == gbase.CurrentPeriod && m.SelectedNums == gbase.SelectedNums).ToList<MPlayerMinimum>();
                var chk = resultlist.Count;
                if (chk != 0)
                {
                    recfound++;
                }
                else
                {
                    missedrec++;

                    gbase.MPlayer_Rec = 0;
                    gdmpfilterlist.Add(gbase);
                }

            }
            if (missedrec == 0) {
                isMissingItem = false;
            }

            return isMissingItem;
        }
    }
}
