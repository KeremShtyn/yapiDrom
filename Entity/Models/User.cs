namespace Entity.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
