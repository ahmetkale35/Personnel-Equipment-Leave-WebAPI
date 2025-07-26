


namespace Entities.DataTransferObject
{


    //public class LeaveRequestDtoForUpdate
    //{
    //    [JsonConstructor]
    //    public LeaveRequestDtoForUpdate(int id, int userId, int leaveTypeId, DateTime baslangicTarihi,
    //        DateTime bitisTarihi, string aciklama, string durum)
    //    {
    //        Id = id;
    //        UserId = userId;
    //        LeaveTypeId = leaveTypeId;
    //        BaslangicTarihi = baslangicTarihi;
    //        BitisTarihi = bitisTarihi;
    //        Aciklama = aciklama;
    //        Durum = durum;
    //    }

    //    public int Id { get; }
    //    public int UserId { get; }
    //    public int LeaveTypeId { get; }
    //    public DateTime BaslangicTarihi { get; }
    //    public DateTime BitisTarihi { get; }
    //    public string Aciklama { get; }
    //    public string Durum { get; }
    //}
    public record LeaveRequestDtoForUpdate : LeaveRequestDtoManipulation
    {
       // [Required(ErrorMessage = "Id alanı zorunludur.")]
        
      //  public int Id { get; init; }
        //public LeaveRequestDtoForUpdate(int id, int userId, int leaveTypeId, DateTime baslangicTarihi,
        //    DateTime bitisTarihi, string aciklama, string durum)
        //{
        //    Id = id;
        //    UserId = userId;
        //    LeaveTypeId = leaveTypeId;
        //    BaslangicTarihi = baslangicTarihi;
        //    BitisTarihi = bitisTarihi;
        //    Aciklama = aciklama;
        //    Durum = durum;
        //}
    }
}

