using AutoMapper;
using Entities.DataTransferObject;
using Entities.DataTransferObject.EquipmentDTO;
using Entities.Models;


namespace WebAPI.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveRequestDtoForUpdate, LeaveRequest>();
            CreateMap<LeaveRequest, LeaveRequestDto>();
            CreateMap<LeaveRequestDtoInsertion, LeaveRequest>();

            CreateMap<UserForRegistirationDto, User>();

            CreateMap<EquipmentDtoForUpdate, EquipmentDto>();
            CreateMap<EquipmentDto, EquipmentDtoForUpdate>();
            CreateMap<EquipmentDtoForUpdate, EquipmentRequests>();
            CreateMap<EquipmentRequests, EquipmentDtoForUpdate>();
            CreateMap<EquipmentRequests, EquipmentDto>();
            CreateMap<EquipmentDtoInsertion, EquipmentRequests>();
            CreateMap<LeaveRequestDto, LeaveRequestDtoForUpdate>().ReverseMap();
        }
    }
}
