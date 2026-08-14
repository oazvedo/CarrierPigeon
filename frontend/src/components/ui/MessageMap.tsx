import { MapContainer, Marker, Polyline, Popup } from "react-leaflet";
import { AppTileLayer, defaultIcon, FitBounds } from "./leafletShared";

interface MessageMapProps {
  senderLatitude: number;
  senderLongitude: number;
  receiverLatitude: number;
  receiverLongitude: number;
  senderLabel: string;
  receiverLabel: string;
}

export function MessageMap({
  senderLatitude,
  senderLongitude,
  receiverLatitude,
  receiverLongitude,
  senderLabel,
  receiverLabel,
}: MessageMapProps) {
  const sender: [number, number] = [senderLatitude, senderLongitude];
  const receiver: [number, number] = [receiverLatitude, receiverLongitude];

  return (
    <MapContainer center={sender} zoom={13} scrollWheelZoom className="map-container">
      <AppTileLayer />
      <Marker position={sender} icon={defaultIcon}>
        <Popup>{senderLabel}</Popup>
      </Marker>
      <Marker position={receiver} icon={defaultIcon}>
        <Popup>{receiverLabel}</Popup>
      </Marker>
      <Polyline positions={[sender, receiver]} pathOptions={{ color: "#aa3bff", weight: 3, dashArray: "6 8" }} />
      <FitBounds points={[sender, receiver]} />
    </MapContainer>
  );
}
