namespace DupRecRemoval.Classes
{
    public class db_master
    {
        public static string connStr = "Server=192.82.60.148;Database=MasterGHL;User Id=MasterUser;Password=@master85092212;TrustServerCertificate=true;";
        public static string dbfullname = "MasterGHL";
        public static string ip = "192.82.60.148";
        public static string userId = "MasterUser";
        public static string password = "@master85092212";
        public static string MyID = "db_master";
    }

    public class db_ghl33
    {
		public static string connStr = "Server=192.82.60.31;Database=GHL;User Id=GHLUser;Password=@golden85092212;TrustServerCertificate=true;";
		public static string dbfullname = "GHL";
		public static string ip = "192.82.60.31";
		public static string userId = "GHLUser";
		public static string password = "@golden85092212";
        public static string MyID = "db_ghl33";
    }

    public class db_ghl55
    {
        public static string connStr = "Server=192.82.60.55;Database=GHL;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=true;";
        public static string dbfullname = "GHL";
        public static string ip = "192.82.60.55";
        public static string userId = "sa";
        public static string password = "p@ssw0rd";
        public static string MyID = "db_ghl55";
    }

    public class db_tm
    {
        public static string connStr = "Server=192.82.60.55;Database=ThirdM;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=true;";
        public static string dbfullname = "ThirdM";
        public static string ip = "192.82.60.55";
        public static string userId = "sa";
        public static string password = "p@ssw0rd";
        public static string MyID = "db_tm";
    }

    public class db_tm2
    {
        public static string connStr = "Server=192.82.60.55;Database=ThirdM2;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=true;";
        public static string dbfullname = "ThirdM2";
        public static string ip = "192.82.60.55";
        public static string userId = "sa";
        public static string password = "p@ssw0rd";
        public static string MyID = "db_tm2";
    }

    public class db_tm3
    {
        public static string connStr = "Server=192.82.60.55;Database=ThirdM3;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=true;";
        public static string dbfullname = "ThirdM3";
        public static string ip = "192.82.60.55";
        public static string userId = "sa";
        public static string password = "p@ssw0rd";
        public static string MyID = "db_tm3";
    }

    public class db_bv
    {
        public static string connStr = "Server=192.82.60.55;Database=BV;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=true;";
        public static string dbfullname = "BV";
        public static string ip = "192.82.60.55";
        public static string userId = "sa";
        public static string password = "p@ssw0rd";
        public static string MyID = "db_bv";
    }

    public class db_wl
    {
        public static string connStr = "Server=192.82.60.55;Database=WL;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=true;";
        public static string dbfullname = "WL";
        public static string ip = "192.82.60.55";
        public static string userId = "sa";
        public static string password = "p@ssw0rd";
        public static string MyID = "db_wl";
    }

    public class db_ace99
    {
        public static string connStr = "Server=192.82.60.149;Database=ACE99;User Id=ACE99User;Password=@ace9985092212;TrustServerCertificate=true;";
        public static string dbfullname = "ACE99";
        public static string ip = "192.82.60.149";
        public static string userId = "ACE99User";
        public static string password = "@ace9985092212";
        public static string MyID = "db_ace99";
    }

    public class db_king4d
    {
        public static string connStr = "Server=192.82.60.149;Database=King4D;User Id=King4DUser;Password=@king4D85092212;TrustServerCertificate=true;";
        public static string dbfullname = "King4D";
        public static string ip = "192.82.60.149";
        public static string userId = "King4DUser";
        public static string password = "@king4D85092212";
        public static string MyID = "db_king4d";
    }

    public class db_togelking
    {
        public static string connStr = "Server=192.82.60.149;Database=TogelKing;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=true;";
        public static string dbfullname = "TogelKing";
        public static string ip = "192.82.60.149";
        public static string userId = "sa";
        public static string password = "p@ssw0rd";
        public static string MyID = "db_togelking";
    }


    public class db_local
    {
        public static string connStr = "Server=localhost;Database=ThirdM;User Id=sa;Password=Kamyen@72;TrustServerCertificate=true;";
        public static string dbfullname = "ThirdM";
        public static string ip = "localhost";
        public static string userId = "sa";
        public static string password = "Kamyen@72";
        public static string MyID = "db_local";
    }

    public class db_local_support
    {
        public static string connStr = "Server=localhost;Database=SupportDB;User Id=sa;Password=Kamyen@72;TrustServerCertificate=true;";
        public static string dbfullname = "SupportDB";
        public static string ip = "localhost";
        public static string userId = "sa";
        public static string password = "Kamyen@72";
        public static string MyID = "db_local_support";
    }

    public class db_ghlstaging
    {
        public static string connStr = "Server=118.107.201.247;Database=GhlStaging;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=true;";
        public static string dbfullname = "GhlStaging";
        public static string ip = "118.107.201.247";
        public static string userId = "sa";
        public static string password = "p@ssw0rd";
        public static string MyID = "db_ghlstaging";
    }

    public class db
    {
        public string connStr {  get; set; }
        public string dbfullname { get; set; }
        public string ip { get; set; }
        public string UserName { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string CurrentPeriod { get; set; }
        public string SelectedNums { get; set; }
        public string GameDealerMemberID { get; set; }
        public int MPlayer_Recs { get; set; }
        public int GDMPlayer_Recs { get; set; }
        public int Diff {  get; set; }

        public string MyID { get; set; }
    }
}
