
using Entities.Models;

namespace Entities.DataTransferObject
{
    // eğer get set kullanmazsak kullanmak zorunndayız , postmande kötü outpu çıkabilir (BackingFiled) gibi daha düzgün formatta cıktı için kaldırabiliriz fakat ozaman da get set yazmamız gerekir bu da immutable yapıyı bozabilir XML Desteği için yaptık , CSV desteği için custom bir formmatter gerekiyor textoutputformatter üst sınfından yapılıyor ,CSV istekleri için (accept) 406 kodu dönuyor
    // [Serializable]

    public record LeaveRequestDto
    {
        public int Id { get; init; }
        public string? UserId { get; set; }
        public int LeaveTypeId { get; init; }
        public DateTime BaslangicTarihi { get; init; }
        public DateTime BitisTarihi { get; init; }
        public string Aciklama { get; init; }
        public string Durum { get; set; }


        // navigation properties
        public User? User { get; set; }             //  nullable yaptık
        public LeaveType? LeaveType { get; set; }


    }
}






