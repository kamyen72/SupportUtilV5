namespace SupportUtilV4.Classes
{
    public class MenuItem
    {
        public int ID { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public string imageurl { get; set; }
        public bool IsLink { get; set; }
        public int Squence { get; set; }
        public int ParentID { get; set; }
        public int Children { get; set; }
    }


    public class MenuItemInput
    {
        public string mID { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public string imageurl { get; set; }
        public string IsLink { get; set; }
        public string Squence { get; set; }
        public string ParentID { get; set; }
    }
}
