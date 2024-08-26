namespace SupportUtilV3.Classes
{
    public class RootItem
    {
        public string text { get; set; }
        public int squence { get; set; }
        public int menurootid { get; set; }
        public int isgroup { get; set; }
        public int myid { get; set; }
        public List<DetailItem> Details { get; set; }

        public void init()
        {
            Details = new List<DetailItem>();
        }
    }

    public class RootItems
    {
        public List<RootItem> Rows { get; set; }

        public void init()
        {
            Rows = new List<RootItem>();
        }
    }
}
