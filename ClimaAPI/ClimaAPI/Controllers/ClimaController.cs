using ClimaAPI.Model;
using ClimaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClimaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClimaController : ControllerBase
    {
        private readonly ClimaService _weatherService;

        public ClimaController(ClimaService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("Cidade é obrigatória");

            var result = await _weatherService.ConsultarCidadeOpenWeatherMap(city);
            return Ok(result);
        }

        [HttpGet]
        [Route("ConsultarCep")]
        public async Task<IActionResult> ConsultarCep([FromQuery] string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return BadRequest("Cep é obrigatório");

            var result = await _weatherService.ConsultarCepViaCep(cep);
            return Ok(result);
        }

        [HttpPost]
        [Route("ConsultarLatitudeLongitude")]
        public async Task<IActionResult> ConsultarLatitudeLongitude([FromBody] ConsultaCidade consultaCidade)
        {
            var result = await _weatherService.ConsultarLatitudeLongitude(consultaCidade);
            return Ok(result);
        }
    }
}
