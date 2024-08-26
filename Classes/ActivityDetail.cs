namespace SupportUtil.Classes
{
    public class ActivityDetail
    {
        public string CurrentPeriod { get; set; }
        public string UserName { get; set; }
        public DateTime ShowResultDate { get; set; }
        public string LotteryInfoName { get; set; }
        public string SelectedNums { get; set; }
        public string IsWinStatus { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal WinMoney { get; set; }
        public decimal WinMoneyWithCapital { get; set; }
    }
}
