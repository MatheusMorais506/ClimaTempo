import countries from "i18n-iso-countries";
import pt from "i18n-iso-countries/langs/pt.json";
import "./ClimaCard.css";

countries.registerLocale(pt);

export default function ClimaCard({ weather }) {
  if (!weather) return null;

  return (
    <div className="weather-card">
      <h2>{weather.city}</h2>
      <p>{weather.description}</p>
      <p>🌡️ {weather.temperatureCelsius} °C</p>
      <p>💧 Umidade: {weather.humidity}%</p>
      <p>💨 Vento: {weather.windSpeed} m/s</p>
      <p>⬆ Máxima: {weather.temperatureMaximaCelsius} °C</p>
      <p>⬇ Mínima: {weather.temperatureMinimaCelsius} °C</p>
      <p>🌍 País: {countries.getName(weather.codigoPais, "pt")}</p>
    </div>
  );
}
