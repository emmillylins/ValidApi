using ValidApi.Models;

namespace ValidApi.Services
{
    public interface IProfileParameterService
    {
        Dictionary<string, ProfileParameter> Parameters { get; }
        void LoadParameters();
    }
}
