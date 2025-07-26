using Entities.Models;

namespace Entities.DataTransferObject.EquipmentDTO
{
    public record EquipmentDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int EquipmentItemId { get; set; }

        public int Adet { get; set; }
        public string Açıklama { get; set; }
        public string Durum { get; set; }

        // foreign key for Onaylayan
        public string? OnaylayanId { get; set; }

        // navigation property for Onaylayan
        public User? Onaylayan { get; set; }

        // navigation property for User
        public User User { get; set; }


        public DateTime TalepTarihi { get; set; }

        // navigation property for EquipmentItem
        public EquipmentItem EquipmentItem { get; set; }

    }
}
