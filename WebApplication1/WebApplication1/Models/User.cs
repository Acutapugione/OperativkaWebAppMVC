namespace TestApp.Models
{
    public class User
    {
        public int ID { get; set; }
        public string TicketType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<History> Histories { get; set; }
    }
}
