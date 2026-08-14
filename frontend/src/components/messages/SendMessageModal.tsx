import { useEffect, useState, type FormEvent } from "react";
import { apiClient } from "../../api/client";
import type { AttachmentType, BirdResponse, CreateMessageRequest, PaginatedResult, UserResponse } from "../../api/types";
import { Button } from "../ui/Button";
import { FormField } from "../ui/FormField";
import { Modal } from "../ui/Modal";

interface MessageFormState {
  senderId: string;
  receiverId: string;
  birdId: string;
  text: string;
  attachmentUrl: string;
  attachmentType: AttachmentType | "";
}

const EMPTY_FORM: MessageFormState = {
  senderId: "",
  receiverId: "",
  birdId: "",
  text: "",
  attachmentUrl: "",
  attachmentType: "",
};

interface SendMessageModalProps {
  onClose: () => void;
  onSubmit: (payload: CreateMessageRequest) => Promise<void>;
}

export function SendMessageModal({ onClose, onSubmit }: SendMessageModalProps) {
  const [users, setUsers] = useState<UserResponse[]>([]);
  const [birds, setBirds] = useState<BirdResponse[]>([]);
  const [form, setForm] = useState<MessageFormState>(EMPTY_FORM);
  const [submitting, setSubmitting] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  useEffect(() => {
    apiClient
      .get<PaginatedResult<UserResponse>>("/users?page=1&pageSize=100")
      .then((result) => setUsers(result.items))
      .catch(() => setUsers([]));

    apiClient
      .get<PaginatedResult<BirdResponse>>("/birds?page=1&pageSize=100")
      .then((result) => setBirds(result.items))
      .catch(() => setBirds([]));
  }, []);

  const canSubmit = users.length >= 2 && birds.length >= 1;

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setFormError(null);

    const payload: CreateMessageRequest = {
      senderId: form.senderId,
      receiverId: form.receiverId,
      birdId: form.birdId,
      text: form.text,
      attachmentUrl: form.attachmentUrl || null,
      attachmentType: form.attachmentType || null,
    };

    try {
      await onSubmit(payload);
      onClose();
    } catch (err) {
      setFormError(err instanceof Error ? err.message : "Something went wrong.");
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <Modal title="Send a message" subtitle="Route a message between two users by carrier bird." onClose={onClose} className="modal-wide">
      {!canSubmit && (
        <p className="form-hint">
          You need at least two users and one bird registered before you can send a message.
        </p>
      )}
      <form className="form" onSubmit={handleSubmit}>
        <div className="form-row">
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
        </div>
        <FormField label="Bird" htmlFor="message-bird">
          <select
            id="message-bird"
            required
            value={form.birdId}
            onChange={(e) => setForm({ ...form, birdId: e.target.value })}
          >
            <option value="" disabled>
              Select a bird
            </option>
            {birds.map((bird) => (
              <option key={bird.id} value={bird.id}>
                {bird.name}
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
        <div className="form-row">
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
        </div>

        {formError && <p className="form-error">{formError}</p>}

        <div className="form-actions">
          <Button type="button" variant="ghost" onClick={onClose}>
            Cancel
          </Button>
          <Button type="submit" variant="primary" disabled={submitting || !canSubmit}>
            {submitting ? "Sending…" : "Send"}
          </Button>
        </div>
      </form>
    </Modal>
  );
}
