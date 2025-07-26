

namespace Entities.Models
{
    public class LeaveRequest
    {
        public int id { get; set; }

        public string? UserId { get; set; }

      
        public int LeaveTypeId { get; set; }

       
        public DateTime BaslangicTarihi { get; set; }

        
        public DateTime BitisTarihi { get; set; }

        
        public string Aciklama { get; set; }

        public string Durum { get; set; }

        public User? User { get; set; }             // ✔ nullable yaptık
        public LeaveType? LeaveType { get; set; }
    }
}
