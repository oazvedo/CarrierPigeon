import { useState, type FormEvent } from "react";
import { Button } from "../components/ui/Button";
import { EmptyState } from "../components/ui/EmptyState";
import { FormField } from "../components/ui/FormField";
import { IconButton } from "../components/ui/IconButton";
import { DeleteIcon, EditIcon } from "../components/ui/icons";
import { Modal } from "../components/ui/Modal";
import { Pagination } from "../components/ui/Pagination";
import { usePaginatedResource } from "../hooks/usePaginatedResource";
import type { BirdResponse } from "../api/types";

interface BirdFormState {
  name: string;
  description: string;
  velocity: string;
}

const EMPTY_FORM: BirdFormState = { name: "", description: "", velocity: "" };

export function BirdsPage() {
  const { items, page, setPage, pageSize, totalCount, loading, error, create, update, remove } =
    usePaginatedResource<BirdResponse>("/birds");

  const [editing, setEditing] = useState<BirdResponse | null>(null);
  const [creating, setCreating] = useState(false);
  const [form, setForm] = useState<BirdFormState>(EMPTY_FORM);
  const [submitting, setSubmitting] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  function openCreate() {
    setForm(EMPTY_FORM);
    setFormError(null);
    setCreating(true);
  }

  function openEdit(bird: BirdResponse) {
    setForm({ name: bird.name, description: bird.description, velocity: String(bird.velocity) });
    setFormError(null);
    setEditing(bird);
  }

  function closeModal() {
    setCreating(false);
    setEditing(null);
  }

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setFormError(null);

    const payload = {
      name: form.name,
      description: form.description,
      velocity: Number(form.velocity),
    };

    try {
      if (editing) {
        await update(editing.id, payload);
      } else {
        await create(payload);
      }
      closeModal();
    } catch (err) {
      setFormError(err instanceof Error ? err.message : "Something went wrong.");
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete(bird: BirdResponse) {
    if (!confirm(`Delete ${bird.name}?`)) return;
    await remove(bird.id);
  }

  return (
    <section>
      <div className="page-header">
        <div>
          <h1>Birds</h1>
          <p className="page-subtitle">{totalCount} total</p>
        </div>
        <Button variant="primary" onClick={openCreate}>
          New bird
        </Button>
      </div>

      {error && <EmptyState title="Failed to load birds" description={error} />}

      {!error && !loading && items.length === 0 && (
        <EmptyState title="No birds yet" description="Add a bird to start delivering messages." />
      )}

      {!error && items.length > 0 && (
        <div className="table-wrap">
          <table className="data-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Description</th>
                <th>Velocity (km/h)</th>
                <th aria-label="Actions" />
              </tr>
            </thead>
            <tbody>
              {items.map((bird) => (
                <tr key={bird.id}>
                  <td data-label="Name">{bird.name}</td>
                  <td data-label="Description">{bird.description}</td>
                  <td data-label="Velocity (km/h)" className="mono">
                    {bird.velocity}
                  </td>
                  <td className="table-actions">
                    <span className="table-actions-group">
                      <IconButton tone="edit" label="Edit bird" icon={<EditIcon />} onClick={() => openEdit(bird)} />
                      <IconButton tone="danger" label="Delete bird" icon={<DeleteIcon />} onClick={() => handleDelete(bird)} />
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <Pagination page={page} pageSize={pageSize} totalCount={totalCount} onPageChange={setPage} />

      {(creating || editing) && (
        <Modal title={editing ? "Edit bird" : "New bird"} onClose={closeModal}>
          <form className="form" onSubmit={handleSubmit}>
            <div className="form-row">
              <FormField label="Name" htmlFor="bird-name">
                <input
                  id="bird-name"
                  required
                  value={form.name}
                  onChange={(e) => setForm({ ...form, name: e.target.value })}
                />
              </FormField>
              <FormField label="Velocity (km/h)" htmlFor="bird-velocity">
                <input
                  id="bird-velocity"
                  type="number"
                  required
                  min={0}
                  step="0.1"
                  value={form.velocity}
                  onChange={(e) => setForm({ ...form, velocity: e.target.value })}
                />
              </FormField>
            </div>
            <FormField label="Description" htmlFor="bird-description">
              <textarea
                id="bird-description"
                required
                rows={3}
                value={form.description}
                onChange={(e) => setForm({ ...form, description: e.target.value })}
              />
            </FormField>

            {formError && <p className="form-error">{formError}</p>}

            <div className="form-actions">
              <Button type="button" variant="ghost" onClick={closeModal}>
                Cancel
              </Button>
              <Button type="submit" variant="primary" disabled={submitting}>
                {submitting ? "Saving…" : "Save"}
              </Button>
            </div>
          </form>
        </Modal>
      )}
    </section>
  );
}
