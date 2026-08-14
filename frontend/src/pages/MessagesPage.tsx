import { useEffect, useState } from "react";
import { apiClient } from "../api/client";
import { Button } from "../components/ui/Button";
import { EmptyState } from "../components/ui/EmptyState";
import { IconButton } from "../components/ui/IconButton";
import { DeleteIcon, RouteIcon } from "../components/ui/icons";
import { MessageMap } from "../components/ui/MessageMap";
import { Modal } from "../components/ui/Modal";
import { Pagination } from "../components/ui/Pagination";
import { SendMessageModal } from "../components/messages/SendMessageModal";
import { usePaginatedResource } from "../hooks/usePaginatedResource";
import type { BirdResponse, MessageResponse, PaginatedResult, UserResponse } from "../api/types";

function userLabel(users: UserResponse[], id: string) {
  return users.find((user) => user.id === id)?.name ?? id;
}

function birdLabel(birds: BirdResponse[], id: string) {
  return birds.find((bird) => bird.id === id)?.name ?? id;
}

export function MessagesPage() {
  const { items, page, setPage, pageSize, totalCount, loading, error, create, remove } =
    usePaginatedResource<MessageResponse>("/messages");

  const [users, setUsers] = useState<UserResponse[]>([]);
  const [birds, setBirds] = useState<BirdResponse[]>([]);
  const [sending, setSending] = useState(false);
  const [viewingRoute, setViewingRoute] = useState<MessageResponse | null>(null);

  useEffect(() => {
    apiClient
      .get<PaginatedResult<UserResponse>>("/users?page=1&pageSize=100")
      .then((result) => setUsers(result.items))
      .catch(() => setUsers([]));

    apiClient
      .get<PaginatedResult<BirdResponse>>("/birds?page=1&pageSize=100")
      .then((result) => setBirds(result.items))
      .catch(() => setBirds([]));
  }, [items]);

  async function handleDelete(message: MessageResponse) {
    if (!confirm("Delete this message?")) return;
    await remove(message.id);
  }

  return (
    <section>
      <div className="page-header">
        <div>
          <h1>Messages</h1>
          <p className="page-subtitle">{totalCount} total</p>
        </div>
        <Button variant="primary" onClick={() => setSending(true)}>
          Send message
        </Button>
      </div>

      {error && <EmptyState title="Failed to load messages" description={error} />}

      {!error && !loading && items.length === 0 && (
        <EmptyState title="No messages yet" description="Send a message between users to see it here." />
      )}

      {!error && items.length > 0 && (
        <div className="table-wrap">
          <table className="data-table">
            <thead>
              <tr>
                <th>From</th>
                <th>To</th>
                <th>Bird</th>
                <th>Text</th>
                <th>Distance (km)</th>
                <th>ETA (min)</th>
                <th>Sent at</th>
                <th aria-label="Actions" />
              </tr>
            </thead>
            <tbody>
              {items.map((message) => (
                <tr key={message.id}>
                  <td data-label="From">{userLabel(users, message.senderId)}</td>
                  <td data-label="To">{userLabel(users, message.receiverId)}</td>
                  <td data-label="Bird">{birdLabel(birds, message.birdId)}</td>
                  <td data-label="Text">{message.text}</td>
                  <td data-label="Distance (km)" className="mono">
                    {message.distance.toFixed(2)}
                  </td>
                  <td data-label="ETA (min)" className="mono">
                    {message.estimatedDeliveryMinutes.toFixed(0)}
                  </td>
                  <td data-label="Sent at" className="mono">
                    {new Date(message.createdAt).toLocaleString()}
                  </td>
                  <td className="table-actions">
                    <span className="table-actions-group">
                      <IconButton tone="accent" label="View route" icon={<RouteIcon />} onClick={() => setViewingRoute(message)} />
                      <IconButton tone="danger" label="Delete message" icon={<DeleteIcon />} onClick={() => handleDelete(message)} />
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <Pagination page={page} pageSize={pageSize} totalCount={totalCount} onPageChange={setPage} />

      {sending && <SendMessageModal onClose={() => setSending(false)} onSubmit={create} />}

      {viewingRoute && (
        <Modal
          title={`Route: ${userLabel(users, viewingRoute.senderId)} → ${userLabel(users, viewingRoute.receiverId)}`}
          onClose={() => setViewingRoute(null)}
          className="modal-wide"
        >
          <p className="page-subtitle">
            {viewingRoute.distance.toFixed(2)} km · ~{viewingRoute.estimatedDeliveryMinutes.toFixed(0)} min by{" "}
            {birdLabel(birds, viewingRoute.birdId)}
          </p>
          <MessageMap
            senderLatitude={viewingRoute.senderLatitude}
            senderLongitude={viewingRoute.senderLongitude}
            receiverLatitude={viewingRoute.receiverLatitude}
            receiverLongitude={viewingRoute.receiverLongitude}
            senderLabel={userLabel(users, viewingRoute.senderId)}
            receiverLabel={userLabel(users, viewingRoute.receiverId)}
          />
        </Modal>
      )}
    </section>
  );
}
