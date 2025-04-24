using ValidApi.Models;

namespace ValidApi.Services
{
    public class ProfileParameterService : IProfileParameterService
    {
        public Dictionary<string, ProfileParameter> Parameters { get; private set; } = new();

        // Carrega os parâmetros de perfil 'mockados'.
        public void LoadParameters()
        {
            Parameters = new Dictionary<string, ProfileParameter>
            {
                ["Admin"] = new ProfileParameter
                { 
                    ProfileName = "Admin",
                    Parameters = new Dictionary<string, string>
                    {
                        { "CanEdit", "true" },
                        { "CanDelete", "true" }
                    }
                },
                ["User"] = new ProfileParameter
                {
                    ProfileName = "User",
                    Parameters = new Dictionary<string, string>
                    {
                        { "CanEdit", "false" },
                        { "CanDelete", "false" }
                    }
                }
            };
        }

    }
}
