using ClimaAPI.Model;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;

namespace ClimaAPI.Services
{
    public class ClimaService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public ClimaService(HttpClient httpClient, IMemoryCache cache, IConfiguration config)
        {
            _httpClient = httpClient;
            _cache = cache;
            //_apiKey = config["OpenWeatherMap:ApiKey"];
            _apiKey = Environment.GetEnvironmentVariable("OPENWEATHERMAP_APIKEY"); //Chave da API cadatrada como variável de ambiente no Azure
            _baseUrl = config["OpenWeatherMap:BaseUrl"];
        }

        public async Task<RespostaClima> ConsultarCepViaCep(string cep)
        {
            // 1️⃣ Consultar ViaCEP
            var viaCepUrl = $"https://viacep.com.br/ws/{cep}/json/";
            var response = await _httpClient.GetAsync(viaCepUrl);

            if (!response.IsSuccessStatusCode)
            {
                var weatherCepResp = await ConsultarCepOpenWeatherMap(cep);
                return weatherCepResp;
            }

            var viaCepResp = await response.Content.ReadAsStringAsync();
            var endereco = JsonConvert.DeserializeObject<RespostaViaCep>(viaCepResp);

            if (endereco == null || endereco.Erro)
            {
                var weatherCepResp = await ConsultarCepOpenWeatherMap(cep);
                return weatherCepResp;

            }

            var cidade = endereco.Localidade;

            var respostaClima = await ConsultarCidadeOpenWeatherMap(cidade);

            return respostaClima;
        }

        public async Task<RespostaClima> ConsultarCidadeOpenWeatherMap(string city)
        {
            if (_cache.TryGetValue(city, out RespostaClima cached))
                return cached;

            var url = $"{_baseUrl}?q={city}&appid={_apiKey}&units=metric&lang=pt_br";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var root = doc.RootElement;

            var weatherResponse = new RespostaClima
            {
                City = root.GetProperty("name").GetString(),
                Description = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                TemperatureCelsius = root.GetProperty("main").GetProperty("temp").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                WindSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble(),
                TemperatureMaximaCelsius = root.GetProperty("main").GetProperty("temp_max").GetDouble(),
                TemperatureMinimaCelsius = root.GetProperty("main").GetProperty("temp_min").GetDouble(),
                longitude = root.GetProperty("coord").GetProperty("lon").GetDouble(),
                latitude = root.GetProperty("coord").GetProperty("lat").GetDouble(),
                CodigoPais = root.GetProperty("sys").GetProperty("country").GetString()
            };

            _cache.Set(city, weatherResponse, TimeSpan.FromMinutes(10));

            return weatherResponse;
        }

        public async Task<RespostaClima> ConsultarCepOpenWeatherMap(string cep)
        {
            if (_cache.TryGetValue(cep, out RespostaClima cached))
                return cached;

            var url = $"{_baseUrl}?zip={cep},br&appid={_apiKey}&units=metric&lang=pt_br";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var root = doc.RootElement;

            var weatherResponse = new RespostaClima
            {
                City = root.GetProperty("name").GetString(),
                Description = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                TemperatureCelsius = root.GetProperty("main").GetProperty("temp").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                WindSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble(),
                TemperatureMaximaCelsius = root.GetProperty("main").GetProperty("temp_max").GetDouble(),
                TemperatureMinimaCelsius = root.GetProperty("main").GetProperty("temp_min").GetDouble(),
                longitude = root.GetProperty("coord").GetProperty("lon").GetDouble(),
                latitude = root.GetProperty("coord").GetProperty("lat").GetDouble(),
                CodigoPais = root.GetProperty("sys").GetProperty("country").GetString()
            };

            _cache.Set(cep, weatherResponse, TimeSpan.FromMinutes(10));

            return weatherResponse;
        }

        public async Task<RespostaClima> ConsultarLatitudeLongitude(ConsultaCidade consultaCidade)
        {
            var latitude = consultaCidade.Latitude;
            var longitude = consultaCidade.Longitude;

            if (_cache.TryGetValue(consultaCidade, out RespostaClima cached))
                return cached;

            var url = $"{_baseUrl}?lat={latitude}&lon={longitude}&limit=1&appid={_apiKey}&lang=pt_br";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            var root = doc.RootElement;

            var weatherResponse = new RespostaClima
            {
                City = root.GetProperty("name").GetString(),
                Description = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                TemperatureCelsius = root.GetProperty("main").GetProperty("temp").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                WindSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble(),
                TemperatureMaximaCelsius = root.GetProperty("main").GetProperty("temp_max").GetDouble(),
                TemperatureMinimaCelsius = root.GetProperty("main").GetProperty("temp_min").GetDouble(),
                longitude = root.GetProperty("coord").GetProperty("lon").GetDouble(),
                latitude = root.GetProperty("coord").GetProperty("lat").GetDouble(),
                CodigoPais = root.GetProperty("sys").GetProperty("country").GetString()
            };

            _cache.Set(consultaCidade, weatherResponse, TimeSpan.FromMinutes(10));

            return weatherResponse;
        }
    }
}
