using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Scooter
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; } = true;
        private HashSet<Reservation> _reservations = new HashSet<Reservation>();
        public IEnumerable<Reservation> Reservations => _reservations.ToList();
    }
}
