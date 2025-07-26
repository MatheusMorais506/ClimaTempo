namespace ClimaAPI.Model
{
    public class RespostaClima
    {
        public string City { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double TemperatureCelsius { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public double TemperatureMaximaCelsius { get; set; }
        public double TemperatureMinimaCelsius { get; set; }
        public string CodigoPais { get; set; } = string.Empty;
    }
}
