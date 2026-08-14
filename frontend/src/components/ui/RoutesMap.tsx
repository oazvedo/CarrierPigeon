import { Fragment } from "react";
import { MapContainer, Marker, Polyline, Popup } from "react-leaflet";
import { AppTileLayer, defaultIcon, FitBounds } from "./leafletShared";

export interface MapRoute {
  id: string;
  senderLatitude: number;
  senderLongitude: number;
  receiverLatitude: number;
  receiverLongitude: number;
  senderLabel: string;
  receiverLabel: string;
  text: string;
}

interface RoutesMapProps {
  routes: MapRoute[];
}

const BRAZIL_CENTER: [number, number] = [-14.235, -51.9253];

export function RoutesMap({ routes }: RoutesMapProps) {
  const points = routes.flatMap((route): [number, number][] => [
    [route.senderLatitude, route.senderLongitude],
    [route.receiverLatitude, route.receiverLongitude],
  ]);

  return (
    <MapContainer center={BRAZIL_CENTER} zoom={4} scrollWheelZoom className="map-container map-container-lg">
      <AppTileLayer />
      {routes.map((route) => {
        const sender: [number, number] = [route.senderLatitude, route.senderLongitude];
        const receiver: [number, number] = [route.receiverLatitude, route.receiverLongitude];
        return (
          <Fragment key={route.id}>
            <Marker position={sender} icon={defaultIcon}>
              <Popup>
                <strong>{route.senderLabel}</strong>
                <br />
                sent to {route.receiverLabel}: "{route.text}"
              </Popup>
            </Marker>
            <Marker position={receiver} icon={defaultIcon}>
              <Popup>
                <strong>{route.receiverLabel}</strong>
                <br />
                received from {route.senderLabel}: "{route.text}"
              </Popup>
            </Marker>
            <Polyline positions={[sender, receiver]} pathOptions={{ color: "#aa3bff", weight: 2.5, dashArray: "6 8" }} />
          </Fragment>
        );
      })}
      {points.length > 0 && <FitBounds points={points} />}
    </MapContainer>
  );
}
