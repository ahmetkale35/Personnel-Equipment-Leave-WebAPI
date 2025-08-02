using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<ILeaveRepository> _leaveRepository;
        private readonly Lazy<IEquipmentRepository> _equipmentRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _leaveRepository = new Lazy<ILeaveRepository>(() => new LeaveRepository(_context));
            _equipmentRepository = new Lazy<IEquipmentRepository>(() => new EquipmentRepository(_context));
        }
        public ILeaveRepository Leave => _leaveRepository.Value;

        public IEquipmentRepository Equipment => _equipmentRepository.Value;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
