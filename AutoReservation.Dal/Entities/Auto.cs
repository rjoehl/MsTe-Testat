using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AutoReservation.Dal.Entities
{
    public abstract class Auto
    {
        public int Id { get; set; }
        public string Marke { get; set; }
        public byte[] RowVersion { get; set; }
        public int Tagestarif { get; set; }
        public virtual DbSet<Reservation> Reservationen { get; set; }
    }

    public class LuxusklasseAuto : Auto
    {
        public int BasisTarif { get; set; }
    }

    public class MittelKlasseAuto : Auto { }

    public class StandardAuto : Auto { }

}

