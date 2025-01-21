using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Reservation
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScooterId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal AmountCharged { get; set; }
        public ScooterStatus Status{ get; set; } = ScooterStatus.Active;

        public override bool Equals(object? obj)
        {
            return obj is Reservation reservation &&
                   Id == reservation.Id &&
                   StartTime == reservation.StartTime &&
                   EndTime == reservation.EndTime &&
                   AmountCharged == reservation.AmountCharged &&
                   Status == reservation.Status;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, StartTime, EndTime, AmountCharged, Status);
        }
    }
}
