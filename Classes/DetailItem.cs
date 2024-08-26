namespace SupportUtilV3.Classes
{
    public class DetailItem
    {
        public string text { get; set; }
        public string url { get; set; }
        public bool islink { get; set; }
        public List<DetailItem> Details { get; set; }

        public void init()
        {
            Details = new List<DetailItem>();
        }
    }
}
