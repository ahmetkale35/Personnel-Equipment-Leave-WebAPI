
using System.ComponentModel.DataAnnotations;


namespace Entities.DataTransferObject.EquipmentDTO
{
    public abstract record EquipmentDtoManipulation
    {
        public string? UserId { get; set; }  // Zorunlu değil
        public string? OnaylayanId { get; set; }  // Zorunlu değil

        [Required(ErrorMessage = "EquipmentItemId alanı zorunludur.")]
        public int EquipmentItemId { get; init; }

        [Required(ErrorMessage = "Adet alanı zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Adet en az 1 olmalıdır.")]
        public int Adet { get; init; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Açıklama { get; init; }

        [Required(ErrorMessage = "Durum alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Durum en fazla 50 karakter olabilir.")]
        public string Durum { get; set; }

        [Required(ErrorMessage = "Talep tarihi zorunludur.")]
        [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih giriniz.")]
        public DateTime TalepTarihi { get; init; }
    }
}
