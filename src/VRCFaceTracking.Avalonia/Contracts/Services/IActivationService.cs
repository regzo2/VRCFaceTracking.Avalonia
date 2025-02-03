using System.Threading.Tasks;

namespace VRCFaceTracking.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
