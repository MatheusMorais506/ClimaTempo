import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import "leaflet/dist/leaflet.css";
import L from "leaflet";

// Corrige ícones do Leaflet
delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl:
    "https://unpkg.com/leaflet@1.7/dist/images/marker-icon-2x.png",
  iconUrl:
    "https://unpkg.com/leaflet@1.7/dist/images/marker-icon.png",
  shadowUrl:
    "https://unpkg.com/leaflet@1.7/dist/images/marker-shadow.png",
});

export default function MapaView({ weather }) {
  if (!weather) return null;

  return (
    <MapContainer
      center={[weather?.latitude || 0, weather?.longitude || 0]}
      zoom={weather ? 6 : 2}
      style={{ 
        height: "96%", 
        width: "95%" , 
        borderRadius: "2px" , 
        position: "relative" ,
        outline: "none",
        border: "1px solid #000",
        boxShadow: "0 6px 15px rgba(0,0,0,0.2)"}}
    >
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
      />
      <Marker position={[weather.latitude, weather.longitude]}>
        <Popup>{weather.city}</Popup>
      </Marker>
    </MapContainer>
  );
}
