namespace ValidApi.Services
{
    public class ProfileParameterUpdater : BackgroundService
    {
        private readonly IProfileParameterService _profileService;
        private readonly ILogger<ProfileParameterUpdater> _logger;

        public ProfileParameterUpdater(IProfileParameterService profileService, ILogger<ProfileParameterUpdater> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Atualizando parâmetros de perfil...");
                AtualizarParametros();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        private void AtualizarParametros()
        {
            foreach (var perfil in _profileService.Parameters.Values)
            {
                if (perfil.Parameters.ContainsKey("CanEdit"))
                {
                    var valorAtual = perfil.Parameters["CanEdit"];
                    perfil.Parameters["CanEdit"] = valorAtual == "true" ? "false" : "true";
                }
            }
        }
    }
}
