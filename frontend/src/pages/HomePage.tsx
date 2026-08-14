import { useCallback, useEffect, useState } from "react";
import { apiClient } from "../api/client";
import { Button } from "../components/ui/Button";
import { EmptyState } from "../components/ui/EmptyState";
import { RoutesMap, type MapRoute } from "../components/ui/RoutesMap";
import { SendMessageModal } from "../components/messages/SendMessageModal";
import type { CreateMessageRequest, MessageResponse, PaginatedResult, UserResponse } from "../api/types";

export function HomePage() {
  const [messages, setMessages] = useState<MessageResponse[]>([]);
  const [users, setUsers] = useState<UserResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [sending, setSending] = useState(false);

  const load = useCallback(() => {
    setLoading(true);
    setError(null);

    Promise.all([
      apiClient.get<PaginatedResult<MessageResponse>>("/messages?page=1&pageSize=50"),
      apiClient.get<PaginatedResult<UserResponse>>("/users?page=1&pageSize=100"),
    ])
      .then(([messagesResult, usersResult]) => {
        setMessages(messagesResult.items);
        setUsers(usersResult.items);
      })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  useEffect(() => {
    load();
  }, [load]);

  function userLabel(id: string) {
    return users.find((user) => user.id === id)?.name ?? id;
  }

  const routes: MapRoute[] = messages.map((message) => ({
    id: message.id,
    senderLatitude: message.senderLatitude,
    senderLongitude: message.senderLongitude,
    receiverLatitude: message.receiverLatitude,
    receiverLongitude: message.receiverLongitude,
    senderLabel: userLabel(message.senderId),
    receiverLabel: userLabel(message.receiverId),
    text: message.text,
  }));

  async function handleSend(payload: CreateMessageRequest) {
    await apiClient.post("/messages", payload);
    load();
  }

  return (
    <section className="home-full">
      <div className="home-toolbar">
        <div>
          <h1>Flight map</h1>
          <p className="page-subtitle">
            {loading ? "Loading routes…" : `${routes.length} route${routes.length === 1 ? "" : "s"} in flight`}
          </p>
        </div>
        <Button variant="primary" onClick={() => setSending(true)}>
          Send message
        </Button>
      </div>

      {error && (
        <div className="home-empty">
          <EmptyState title="Failed to load the map" description={error} />
        </div>
      )}

      {!error && !loading && routes.length === 0 && (
        <div className="home-empty">
          <EmptyState
            title="No routes yet"
            description="Send your first message to see its flight path on the map."
          />
        </div>
      )}

      {!error && (loading || routes.length > 0) && (
        <div className="home-map-fill">
          <RoutesMap routes={routes} />
        </div>
      )}

      {sending && <SendMessageModal onClose={() => setSending(false)} onSubmit={handleSend} />}
    </section>
  );
}
