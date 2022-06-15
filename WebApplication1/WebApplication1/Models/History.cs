namespace TestApp.Models
{
    public class History
    {
        public int ID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public virtual User User { get; set; }
        public int UserID { get; set; }
    }
}
