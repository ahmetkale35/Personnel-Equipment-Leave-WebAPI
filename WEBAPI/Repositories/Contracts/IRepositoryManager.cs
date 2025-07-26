
namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        ILeaveRepository Leave {  get; }

        IEquipmentRepository Equipment { get; }
        void Save();
    }
}
