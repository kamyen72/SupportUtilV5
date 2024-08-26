namespace SupportUtil.Classes
{
    public class MPlayerAll
    {
        public string Source { get; set; }
        public int ID { get; set; }
        public string UserName { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string LotteryInfoName { get; set; }
        public string SelectedNums { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public bool IsWin { get; set; }
        public DateTime ShowResultDate { get; set; }
        public decimal WinMoney { get; set; }
        public decimal WinMoneyWithCapital { get; set; }
        public int SecondMPlayerID { get; set; }
        public int MemberID { get; set; }
        public int GameDealerMemberID { get; set; }
        public int LotteryInfoID { get; set; }
        public int CompanyID { get; set; }
        public string CurrentPeriod { get; set; }
        public bool IsAfter { get; set; }
        public bool IsWinStop { get; set; }
        public string ManualBet { get; set; }
        public string Multiple { get; set; }
        public decimal RebatePro { get; set; }
        public int RebateProMoney { get; set; }
        public int ReferralPayType { get; set; }
        public int CashRebatePayType { get; set; }
        public int CashBackRebatePayType { get; set; }
        public int IsReferralWriteReport { get; set; }
        public int IsCashRebateWriteReport { get; set; }
        public int IsCashBackWriteReport { get; set; }
        public int IsReset { get; set; }
        public int CreateID { get; set; }
        public int UpdateID { get; set; }
    }
}
