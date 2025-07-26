import "./Container.css";
import Footer from "../Footer/Footer";
import Navbar from "../Navbar/Navbar";

export default function Container({ 
  city, 
  setCity, 
  ConsultarCidade,
  ConsultarCEP, 
  error, 
  loading, 
  weather, 
  ClimaCardComponent,
  MapaViewComponent  
}) {
  return (
    <div className="container">

      <Navbar />

      {/* Topo: busca */}
      <div className="search-container">
        <input
          type="text"
          placeholder="Digite a cidade ou cep(Brasil)"
          value={city}
          onChange={(e) => setCity(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === "Enter") {
              const valor = city.trim();
            if (/^\d+$/.test(valor)) {
              ConsultarCEP(valor);
            } else {
              ConsultarCidade(valor);
          }
        }
      }}
        />
          <button
            onClick={() => {
            const valor = city.trim();
            if (/^\d+$/.test(valor)) {
              ConsultarCEP(valor);
            } else {
              ConsultarCidade(valor);
          }
        }}
      >
            <span className="material-icons">search</span>
          </button>
      </div>

      {/* Meio: WeatherCard */}
      <div className="middle-container">
        {error && <p className="error">{error}</p>}

        {loading && (
          <div className="loader-container">
            <div className="loader"></div>
            <p>Carregando...</p>
          </div>
        )}

        {!loading && <ClimaCardComponent weather={weather} />}
      </div>

      {/* Embaixo: MapView */}
      <div className="bottom-container">
        <MapaViewComponent weather={weather} />
      </div>

        <Footer />
    </div>
  );
}
