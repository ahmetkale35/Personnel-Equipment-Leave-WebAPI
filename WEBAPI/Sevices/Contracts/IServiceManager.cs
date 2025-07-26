
namespace Services.Contracts
{
    public interface IServiceManager
    {
        ILeaveService Leave { get; }
        IAuthenticationService Authentication { get; }

        IEquipmentService Equipment { get; }

    }
}
