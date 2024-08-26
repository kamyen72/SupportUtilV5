using SupportUtil.Classes;

namespace SupportUtilV2.Classes
{
    public class MissingList
    {
        public string dbname { get; set; }
        public List<GameDealerMPlayerBase> Rows { get; set; }
        public int MissingCount {  get; set; }
    }
}
