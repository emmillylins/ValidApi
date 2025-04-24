using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ValidApi.Models;
using ValidApi.Services;

namespace ValidApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileParameterService _service;

        public ProfilesController(IProfileParameterService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna todos os perfis e seus parâmetros.
        /// </summary>
        /// <returns>Lista de perfis com seus parâmetros.</returns>
        /// <response code="200">Retorna a lista de perfis</response>
        /// <response code="500">Erro interno no servidor</response>
        [HttpGet]
        [SwaggerResponse(200, "Lista de perfis retornada com sucesso.", typeof(IEnumerable<ProfileParameter>))]
        [SwaggerResponse(500, "Erro interno ao buscar os perfis.")]
        public ActionResult<IEnumerable<ProfileParameter>> GetAll()
        {
            return Ok(_service.Parameters.Values);
        }

        /// <summary>
        /// Retorna um perfil específico e seus parâmetros.
        /// </summary>
        /// <param name="profileName">Nome do perfil a ser retornado.</param>
        /// <returns>Perfil específico com seus parâmetros.</returns>
        /// <response code="200">Perfil encontrado e retornado com sucesso</response>
        /// <response code="404">Perfil não encontrado</response>
        [HttpGet("{profileName}")]
        [SwaggerResponse(200, "Perfil encontrado", typeof(ProfileParameter))]
        [SwaggerResponse(404, "Perfil não encontrado")]
        public ActionResult<ProfileParameter> Get(string profileName)
        {
            if (_service.Parameters.TryGetValue(profileName, out var param))
                return Ok(param); // Retorna o perfil, se existir.

            return NotFound("Perfil não encontrado."); 
        }

        /// <summary>
        /// Cria um novo perfil com parâmetros.
        /// </summary>
        /// <param name="parameter">Parâmetros para o novo perfil.</param>
        /// <returns>Perfil criado com sucesso.</returns>
        /// <response code="201">Perfil criado com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="409">Perfil já existe</response>
        [HttpPost]
        [SwaggerResponse(201, "Perfil criado com sucesso.", typeof(ProfileParameter))]
        [SwaggerResponse(400, "Dados inválidos no request.")]
        [SwaggerResponse(409, "Perfil já existe.")]
        public IActionResult Create(ProfileParameter parameter)
        {
            if (_service.Parameters.ContainsKey(parameter.ProfileName))
                return Conflict("Perfil já existe."); // Impede duplicação.

            // gera a URL usando o nome do método 'Get'.
            var url = Url.Action(nameof(Get), new { profileName = parameter.ProfileName });

            // retorna a resposta HTTP 201 (Criado) com a URL gerada e o perfil recém-criado.
            return Created(url, parameter);

        }

        /// <summary>
        /// Atualiza os parâmetros de um perfil existente.
        /// </summary>
        /// <param name="profileName">Nome do perfil a ser atualizado.</param>
        /// <param name="parameter">Novo conjunto de parâmetros para o perfil.</param>
        /// <returns>Resultado da atualização do perfil.</returns>
        /// <response code="204">Perfil atualizado com sucesso</response>
        /// <response code="400">Dados inválidos fornecidos</response>
        /// <response code="404">Perfil não encontrado</response>
        [HttpPut("{profileName}")]
        [SwaggerResponse(204, "Perfil atualizado com sucesso.")]
        [SwaggerResponse(400, "Dados inválidos para atualização.")]
        [SwaggerResponse(404, "Perfil não encontrado.")]
        public IActionResult Update(string profileName, ProfileParameter parameter)
        {
            if (!_service.Parameters.ContainsKey(profileName))
                NotFound("Perfil não encontrado.");

            _service.Parameters[profileName] = parameter;
            return NoContent(); // Atualizado com sucesso.
        }

        /// <summary>
        /// Remove um perfil pelo nome.
        /// </summary>
        /// <param name="profileName">Nome do perfil a ser removido.</param>
        /// <returns>Resultado da remoção do perfil.</returns>
        /// <response code="204">Perfil removido com sucesso</response>
        /// <response code="404">Perfil não encontrado</response>
        [HttpDelete("{profileName}")]
        [SwaggerResponse(204, "Perfil removido com sucesso.")]
        [SwaggerResponse(404, "Perfil não encontrado.")]
        public IActionResult Delete(string profileName)
        {
            if (!_service.Parameters.Remove(profileName))
                NotFound("Perfil não encontrado.");

            return NoContent(); // Removido com sucesso.
        }

        /// <summary>
        /// Valida se um perfil tem permissão para uma ação específica.
        /// </summary>
        /// <param name="profileName">Nome do perfil.</param>
        /// <param name="action">Ação que deseja validar, ex: "CanEdit", "CanDelete".</param>
        /// <returns>Resultado da validação de permissão.</returns>
        /// <response code="200">Permissão válida</response>
        /// <response code="404">Perfil não encontrado</response>
        /// <response code="400">Ação inválida</response>
        [HttpGet("{profileName}/validate")]
        [SwaggerResponse(200, "Permissão validada com sucesso.")]
        [SwaggerResponse(400, "Ação inválida.")]
        [SwaggerResponse(404, "Perfil não encontrado.")]
        public IActionResult ValidatePermission(string profileName, [FromQuery] string action)
        {
            if (_service.Parameters.TryGetValue(profileName, out var param))
            {
                if (string.IsNullOrWhiteSpace(action))
                    return BadRequest("A ação não foi especificada.");

                var validActions = new[] { "CanEdit", "CanDelete" };
                if (!validActions.Contains(action, StringComparer.OrdinalIgnoreCase))
                    return BadRequest($"A ação '{action}' não é válida.");

                if (!param.Parameters.TryGetValue(action, out var value))
                    return BadRequest($"A ação '{action}' não está configurada para o perfil '{profileName}'.");

                bool hasPermission = value == "true";
                return Ok(new
                {
                    success = hasPermission,
                    message = hasPermission
                        ? $"O perfil '{profileName}' tem permissão para '{action}'."
                        : $"O perfil '{profileName}' não tem permissão para '{action}'."
                });
            }

            return NotFound("Perfil não encontrado."); // Perfil inexistente.
        }
    }
}
