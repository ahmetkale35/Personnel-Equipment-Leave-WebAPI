using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repositories.Contracts;
using Services.Contracts;
using Microsoft.Extensions.Configuration;
using Entities.DataTransferObject.EquipmentDTO;
using Entities.DataTransferObject;


namespace Services
{
    public class ServiceManager : IServiceManager
    {
        // Lazy yükleme kullanarak servisleri oluşturuyoruz
        private readonly Lazy<ILeaveService> _leaveService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IEquipmentService> _equipmentService;

        // Constructor injection ile bağımlılıkları alıyoruz
        public ServiceManager(IRepositoryManager repositoryManager,    
            ILoggerService logger,
            IMapper mapper,
            IConfiguration configuration, // IConfiguration'ı geçiyoruz
            UserManager<User> userManager, 
            IDataShapper<EquipmentDto> shapperEquipment,
            IDataShapper<LeaveRequestDto> shapperLeave)

        {
            // Lazy yükleme ile LeaveService'i başlatıyoruz
            _leaveService = new Lazy<ILeaveService>(() => 
            new LeaveManager(repositoryManager, logger, mapper, shapperLeave));


            _authenticationService = new Lazy<IAuthenticationService>(() =>
                new AuthenticationManager(mapper, userManager, configuration, logger)); // IConfiguration'ı null olarak geçiyoruz, gerçek uygulamada uygun şekilde geçilmeli
            //_equipmentService = equipmentService;

            _equipmentService = new Lazy<IEquipmentService>(() =>
                new EquipmentManager(repositoryManager, logger, mapper, shapperEquipment)); // Lazy yükleme ile EquipmentService'i başlatıyoruz
        }
        
        public ILeaveService Leave => _leaveService.Value;

        public IAuthenticationService Authentication => _authenticationService.Value;

        public IEquipmentService Equipment => _equipmentService.Value;
    }
}
