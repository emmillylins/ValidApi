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

        // Executa a atualização periódica a cada 5 minutos enquanto a tarefa não for cancelada.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Atualizando parâmetros de perfil...");
                AtualizarParametros();  // Chama o método que atualiza os parâmetros.
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Aguarda 5 minutos antes de continuar.
            }
        }

        // Atualiza o valor do parâmetro "CanEdit", alternando entre "true" e "false".
        private void AtualizarParametros()
        {
            foreach (var perfil in _profileService.Parameters.Values)
            {
                if (perfil.Parameters.ContainsKey("CanEdit"))
                {
                    var valorAtual = perfil.Parameters["CanEdit"];
                    perfil.Parameters["CanEdit"] = valorAtual == "true" ? "false" : "true"; // Alterna o valor.
                }
            }
        }
    }
}
