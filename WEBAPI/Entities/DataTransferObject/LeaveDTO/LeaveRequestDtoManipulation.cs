
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObject
{
    public abstract record LeaveRequestDtoManipulation
    {
        //[Required(ErrorMessage = "UserId alanı zorunludur.")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "LeaveTypeId alanı zorunludur.")]
        public int LeaveTypeId { get; init; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih giriniz.")]
        public DateTime BaslangicTarihi { get; init; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih giriniz.")]
        public DateTime BitisTarihi { get; init; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Aciklama { get; init; }

        [Required(ErrorMessage = "Durum alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Durum en fazla 50 karakter olabilir.")]
        public string Durum { get; set; }
    }
}
