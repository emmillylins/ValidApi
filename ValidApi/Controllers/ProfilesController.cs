using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public ActionResult<IEnumerable<ProfileParameter>> GetAll() => Ok(_service.Parameters.Values);

        [HttpGet("{profileName}")]
        public ActionResult<ProfileParameter> Get(string profileName)
        {
            if (_service.Parameters.TryGetValue(profileName, out var param))
                return Ok(param);
            return NotFound();
        }

        [HttpPost]
        public IActionResult Create(ProfileParameter parameter)
        {
            if (_service.Parameters.ContainsKey(parameter.ProfileName))
                return Conflict("Perfil já existe.");

            _service.Parameters[parameter.ProfileName] = parameter;
            return CreatedAtAction(nameof(Get), new { profileName = parameter.ProfileName }, parameter);
        }

        [HttpPut("{profileName}")]
        public IActionResult Update(string profileName, ProfileParameter parameter)
        {
            if (!_service.Parameters.ContainsKey(profileName))
                return NotFound();

            _service.Parameters[profileName] = parameter;
            return NoContent();
        }

        [HttpDelete("{profileName}")]
        public IActionResult Delete(string profileName)
        {
            if (!_service.Parameters.Remove(profileName))
                return NotFound();

            return NoContent();
        }

        [HttpGet("{profileName}/validate")]
        public IActionResult ValidatePermission(string profileName, [FromQuery] string action)
        {
            if (_service.Parameters.TryGetValue(profileName, out var param))
            {
                if (param.Parameters.TryGetValue(action, out var value) && value == "true")
                    return Ok(true);
                return Ok(false);
            }

            return NotFound();
        }
    }
}
