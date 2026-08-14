import { useEffect, useState, type FormEvent } from "react";
import { apiClient } from "../api/client";
import { Button } from "../components/ui/Button";
import { EmptyState } from "../components/ui/EmptyState";
import { FormField } from "../components/ui/FormField";
import { Modal } from "../components/ui/Modal";
import { Pagination } from "../components/ui/Pagination";
import { usePaginatedResource } from "../hooks/usePaginatedResource";
import type { AttachmentType, MessageResponse, PaginatedResult, UserResponse } from "../api/types";

interface MessageFormState {
  senderId: string;
  receiverId: string;
  text: string;
  attachmentUrl: string;
  attachmentType: AttachmentType | "";
  senderLatitude: string;
  senderLongitude: string;
  receiverLatitude: string;
  receiverLongitude: string;
}

const EMPTY_FORM: MessageFormState = {
  senderId: "",
  receiverId: "",
  text: "",
  attachmentUrl: "",
  attachmentType: "",
  senderLatitude: "",
  senderLongitude: "",
  receiverLatitude: "",
  receiverLongitude: "",
};

function userLabel(users: UserResponse[], id: string) {
  return users.find((user) => user.id === id)?.name ?? id;
}

export function MessagesPage() {
  const { items, page, setPage, pageSize, totalCount, loading, error, create, remove } =
    usePaginatedResource<MessageResponse>("/messages");

  const [users, setUsers] = useState<UserResponse[]>([]);
  const [creating, setCreating] = useState(false);
  const [form, setForm] = useState<MessageFormState>(EMPTY_FORM);
  const [submitting, setSubmitting] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  useEffect(() => {
    apiClient
      .get<PaginatedResult<UserResponse>>("/users?page=1&pageSize=100")
      .then((result) => setUsers(result.items))
      .catch(() => setUsers([]));
  }, []);

  function openCreate() {
    setForm(EMPTY_FORM);
    setFormError(null);
    setCreating(true);
  }

  function closeModal() {
    setCreating(false);
  }

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setFormError(null);

    const payload = {
      senderId: form.senderId,
      receiverId: form.receiverId,
      text: form.text,
      attachmentUrl: form.attachmentUrl || null,
      attachmentType: form.attachmentType || null,
      senderLatitude: Number(form.senderLatitude),
      senderLongitude: Number(form.senderLongitude),
      receiverLatitude: Number(form.receiverLatitude),
      receiverLongitude: Number(form.receiverLongitude),
    };

    try {
      await create(payload);
      closeModal();
    } catch (err) {
      setFormError(err instanceof Error ? err.message : "Something went wrong.");
    } finally {
      setSubmitting(false);
    }
  }

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
        <Button variant="primary" onClick={openCreate} disabled={users.length < 2}>
          New message
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
                <th>Text</th>
                <th>Sent at</th>
                <th aria-label="Actions" />
              </tr>
            </thead>
            <tbody>
              {items.map((message) => (
                <tr key={message.id}>
                  <td data-label="From">{userLabel(users, message.senderId)}</td>
                  <td data-label="To">{userLabel(users, message.receiverId)}</td>
                  <td data-label="Text">{message.text}</td>
                  <td data-label="Sent at" className="mono">
                    {new Date(message.createdAt).toLocaleString()}
                  </td>
                  <td className="table-actions">
                    <Button variant="ghost" onClick={() => handleDelete(message)}>
                      Delete
                    </Button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <Pagination page={page} pageSize={pageSize} totalCount={totalCount} onPageChange={setPage} />

      {creating && (
        <Modal title="New message" onClose={closeModal}>
          <form className="form" onSubmit={handleSubmit}>
            <FormField label="Sender" htmlFor="message-sender">
              <select
                id="message-sender"
                required
                value={form.senderId}
                onChange={(e) => setForm({ ...form, senderId: e.target.value })}
              >
                <option value="" disabled>
                  Select a user
                </option>
                {users.map((user) => (
                  <option key={user.id} value={user.id}>
                    {user.name}
                  </option>
                ))}
              </select>
            </FormField>
            <FormField label="Receiver" htmlFor="message-receiver">
              <select
                id="message-receiver"
                required
                value={form.receiverId}
                onChange={(e) => setForm({ ...form, receiverId: e.target.value })}
              >
                <option value="" disabled>
                  Select a user
                </option>
                {users.map((user) => (
                  <option key={user.id} value={user.id}>
                    {user.name}
                  </option>
                ))}
              </select>
            </FormField>
            <FormField label="Text" htmlFor="message-text">
              <textarea
                id="message-text"
                required
                rows={3}
                value={form.text}
                onChange={(e) => setForm({ ...form, text: e.target.value })}
              />
            </FormField>
            <FormField label="Attachment URL (optional)" htmlFor="message-attachment-url">
              <input
                id="message-attachment-url"
                value={form.attachmentUrl}
                onChange={(e) => setForm({ ...form, attachmentUrl: e.target.value })}
              />
            </FormField>
            <FormField label="Attachment type" htmlFor="message-attachment-type">
              <select
                id="message-attachment-type"
                value={form.attachmentType}
                onChange={(e) => setForm({ ...form, attachmentType: e.target.value as AttachmentType | "" })}
              >
                <option value="">None</option>
                <option value="Photo">Photo</option>
                <option value="File">File</option>
              </select>
            </FormField>
            <FormField label="Sender latitude" htmlFor="message-sender-lat">
              <input
                id="message-sender-lat"
                type="number"
                required
                step="0.000001"
                value={form.senderLatitude}
                onChange={(e) => setForm({ ...form, senderLatitude: e.target.value })}
              />
            </FormField>
            <FormField label="Sender longitude" htmlFor="message-sender-lon">
              <input
                id="message-sender-lon"
                type="number"
                required
                step="0.000001"
                value={form.senderLongitude}
                onChange={(e) => setForm({ ...form, senderLongitude: e.target.value })}
              />
            </FormField>
            <FormField label="Receiver latitude" htmlFor="message-receiver-lat">
              <input
                id="message-receiver-lat"
                type="number"
                required
                step="0.000001"
                value={form.receiverLatitude}
                onChange={(e) => setForm({ ...form, receiverLatitude: e.target.value })}
              />
            </FormField>
            <FormField label="Receiver longitude" htmlFor="message-receiver-lon">
              <input
                id="message-receiver-lon"
                type="number"
                required
                step="0.000001"
                value={form.receiverLongitude}
                onChange={(e) => setForm({ ...form, receiverLongitude: e.target.value })}
              />
            </FormField>

            {formError && <p className="form-error">{formError}</p>}

            <div className="form-actions">
              <Button type="button" variant="ghost" onClick={closeModal}>
                Cancel
              </Button>
              <Button type="submit" variant="primary" disabled={submitting}>
                {submitting ? "Sending…" : "Send"}
              </Button>
            </div>
          </form>
        </Modal>
      )}
    </section>
  );
}
