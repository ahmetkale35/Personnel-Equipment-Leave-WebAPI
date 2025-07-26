using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repositories.Contracts;
using Services.Contracts;
using Microsoft.Extensions.Configuration;


namespace Services
{
    public class ServiceManager : IServiceManager
    {
        // Lazy yükleme kullanarak servisleri oluşturuyoruz
        private readonly Lazy<ILeaveService> _leaveService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
       // private readonly IEquipmentService _equipmentService;
        private readonly Lazy<IEquipmentService> _equipmentService;

        public ServiceManager(IRepositoryManager repositoryManager,    // Constructor injection ile bağımlılıkları alıyoruz
            ILoggerService logger,
            IMapper mapper,
            IConfiguration configuration, // IConfiguration'ı geçiyoruz
            UserManager<User> userManager) // UserManager<User> bağımlılığını alıyoruz
            // IEquipmentService equipmentService)   // Lazy yükleme ile EquipmentService'i başlatıyoruz


        {
            // Lazy yükleme ile LeaveService'i başlatıyoruz
            _leaveService = new Lazy<ILeaveService>(() => 
            new LeaveManager(repositoryManager, logger, mapper));


            _authenticationService = new Lazy<IAuthenticationService>(() =>
                new AuthenticationManager(mapper, userManager, configuration, logger)); // IConfiguration'ı null olarak geçiyoruz, gerçek uygulamada uygun şekilde geçmelisiniz
            //_equipmentService = equipmentService;

            _equipmentService = new Lazy<IEquipmentService>(() =>
                new EquipmentManager(repositoryManager, logger, mapper)); // Lazy yükleme ile EquipmentService'i başlatıyoruz
        }
        
        public ILeaveService Leave => _leaveService.Value;

        public IAuthenticationService Authentication => _authenticationService.Value;

        public IEquipmentService Equipment => _equipmentService.Value;
    }
}
