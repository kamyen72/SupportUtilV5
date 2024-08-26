namespace DupRecRemoval.Classes
{
    public class CheckDBsDiff
    {
        public List<db> CheckAllDBs4Diff(string CurrentPeriod)
        {
            DBList dBList = new DBList();

            List<db> DBWithDiff = new List<db>();

            int maxcount = dBList.dbs.Count;

            for (int i = 0; i < maxcount; i++)
            {
                db ldb = dBList.dbs[i];
                var dbname = ldb.dbfullname;

                CheckDiffClass cdf = new CheckDiffClass();
                List<DiffRec> diffs = cdf.CheckDiff(CurrentPeriod, ldb.connStr);

                var diffreccount = diffs.Count;

                for (int j = 0; j < diffs.Count; j++)
                {
                    db newdb = new db();
                    DiffRec diff = diffs[j];

                    ldb.MPlayer_Recs = diff.mplayer_recs;
                    ldb.GDMPlayer_Recs = diff.gdmplayer_recs;
                    ldb.Diff = diff.diff;

                    newdb.dbfullname = ldb.dbfullname;
                    newdb.connStr = ldb.connStr;
                    newdb.ip = ldb.ip;
                    newdb.CurrentPeriod = CurrentPeriod;
                    newdb.SelectedNums = diff.selectednums;
                    newdb.UserName = diff.username;
                    newdb.GameDealerMemberID = diff.gamedealermemberid;

                    newdb.MPlayer_Recs = ldb.MPlayer_Recs;
                    newdb.GDMPlayer_Recs = ldb.GDMPlayer_Recs;
                    newdb.Diff = ldb.Diff;

                    DBWithDiff.Add(newdb);
                }
            }

            return DBWithDiff;
        }
    }
}
