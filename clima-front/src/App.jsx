import { useState, useEffect } from "react";
import axios from "axios";
import Container from "./components/Container/Container";
import ClimaCard from "./components/ClimaCard/ClimaCard";
import MapaView from "./components/MapaView/MapaView";
import ConsultaCidade from "./models/ConsultaCidade";
import "./App.css";
import api_options from './services/api';

export default function App() {
  const [city, setCity] = useState("");
  const [weather, setWeather] = useState(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  var metodoApi = api_options();
  var apiUrl = metodoApi.params.Api_Url;
  
  const ConsultarCidade = async (cityName) => {
    try {
      setError("");
      setWeather(null);
      setLoading(true);
      const res = await axios.get(
        `${apiUrl}/api/Clima?city=${encodeURIComponent(cityName)}`
      );

      setWeather(res.data);
    } catch {
      setError("Não foi possível obter os dados para a cidade informada.");
    } finally {
      setLoading(false);
    }
  };

  const ConsultarCEP = async (cep) => {
    try {
      setError("");
      setWeather(null);
      setLoading(true);
      const res = await axios.get(
        `${apiUrl}/api/Clima/ConsultarCep?cep=${encodeURIComponent(cep)}`
      );

      setWeather(res.data);
    } catch {
      setError("Não foi possível obter os dados para o CEP informado.");
    } finally {
      setLoading(false);
    }
  };

  const ConsultarLatitudeLongitude = async (lat, lon) => {
    try {

      var parametros = new ConsultaCidade();
      parametros.Latitude = lat;
      parametros.Longitude = lon;

      const res = await axios.post(
        `${apiUrl}/api/Clima/ConsultarLatitudeLongitude`,
        parametros
      );

      return res.data.city || null;
    } catch {
      return null;
    }
  };
  
  useEffect(() => {
    if (navigator.geolocation) {
      setLoading(true);
      navigator.geolocation.getCurrentPosition(
        async (position) => {
          const { latitude, longitude } = position.coords;
          const cityName = await ConsultarLatitudeLongitude(latitude, longitude);

          if (cityName) {
            setCity(cityName);
            ConsultarCidade(cityName);
          } else {
            setError("Não foi possível identificar a sua cidade.");
            setLoading(false);
          }
        },
        () => {
          setError("Não foi possível acessar sua localização. Digite a cidade.");
          setLoading(false);
        }
      );
    }
  }, []);

  return (
    <Container
      city={city}
      setCity={setCity}
      ConsultarCidade={ConsultarCidade}
      ConsultarCEP={ConsultarCEP}
      error={error}
      loading={loading}
      weather={weather}
      ClimaCardComponent={ClimaCard}
      MapaViewComponent={MapaView}
    />
  );
}
