namespace SupportUtil.Classes
{
    public class GameDealerMPlayerBase
    {
        public string DBname { get; set; }
        public int? GDMP_ID { get; set; }
        public string? UserName { get; set; }
        public int? GameDealerMemberID { get; set; }
        public int? MemberID { get; set; }
        public string SelectedNums { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? GDMP_Recs { get; set; }
        public int? MPlayer_Rec { get; set; }
        public DateTime? MPUpdateDate { get; set; }
        public string CurrentPeriod { get; set; }


    }
}
