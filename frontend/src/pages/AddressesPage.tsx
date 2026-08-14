import { useEffect, useState, type FormEvent } from "react";
import { apiClient } from "../api/client";
import { Button } from "../components/ui/Button";
import { EmptyState } from "../components/ui/EmptyState";
import { FormField } from "../components/ui/FormField";
import { IconButton } from "../components/ui/IconButton";
import { DeleteIcon, EditIcon } from "../components/ui/icons";
import { Modal } from "../components/ui/Modal";
import { Pagination } from "../components/ui/Pagination";
import { usePaginatedResource } from "../hooks/usePaginatedResource";
import type { AddressResponse, PaginatedResult, UserResponse } from "../api/types";

interface AddressFormState {
  userId: string;
  cep: string;
  number: string;
}

const EMPTY_FORM: AddressFormState = { userId: "", cep: "", number: "" };

function userLabel(users: UserResponse[], id: string) {
  return users.find((user) => user.id === id)?.name ?? id;
}

export function AddressesPage() {
  const { items, page, setPage, pageSize, totalCount, loading, error, create, update, remove } =
    usePaginatedResource<AddressResponse>("/addresses");

  const [users, setUsers] = useState<UserResponse[]>([]);
  const [editing, setEditing] = useState<AddressResponse | null>(null);
  const [creating, setCreating] = useState(false);
  const [form, setForm] = useState<AddressFormState>(EMPTY_FORM);
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

  function openEdit(address: AddressResponse) {
    setForm({ userId: address.userId, cep: address.cep, number: address.number ?? "" });
    setFormError(null);
    setEditing(address);
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
      userId: form.userId,
      cep: form.cep,
      number: form.number || null,
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

  async function handleDelete(address: AddressResponse) {
    if (!confirm("Delete this address?")) return;
    await remove(address.id);
  }

  return (
    <section>
      <div className="page-header">
        <div>
          <h1>Addresses</h1>
          <p className="page-subtitle">{totalCount} total</p>
        </div>
        <Button variant="primary" onClick={openCreate} disabled={users.length === 0}>
          New address
        </Button>
      </div>

      {error && <EmptyState title="Failed to load addresses" description={error} />}

      {!error && !loading && items.length === 0 && (
        <EmptyState title="No addresses yet" description="Add an address to a user to enable delivery estimates." />
      )}

      {!error && items.length > 0 && (
        <div className="table-wrap">
          <table className="data-table">
            <thead>
              <tr>
                <th>User</th>
                <th>CEP</th>
                <th>Street</th>
                <th>Neighborhood</th>
                <th>City</th>
                <th>UF</th>
                <th aria-label="Actions" />
              </tr>
            </thead>
            <tbody>
              {items.map((address) => (
                <tr key={address.id}>
                  <td data-label="User">{userLabel(users, address.userId)}</td>
                  <td data-label="CEP" className="mono">
                    {address.cep}
                  </td>
                  <td data-label="Street">
                    {address.street ?? "—"}
                    {address.number ? `, ${address.number}` : ""}
                  </td>
                  <td data-label="Neighborhood">{address.neighborhood ?? "—"}</td>
                  <td data-label="City">{address.local ?? "—"}</td>
                  <td data-label="UF">{address.uf ?? "—"}</td>
                  <td className="table-actions">
                    <span className="table-actions-group">
                      <IconButton tone="edit" label="Edit address" icon={<EditIcon />} onClick={() => openEdit(address)} />
                      <IconButton tone="danger" label="Delete address" icon={<DeleteIcon />} onClick={() => handleDelete(address)} />
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
        <Modal title={editing ? "Edit address" : "New address"} onClose={closeModal}>
          <form className="form" onSubmit={handleSubmit}>
            <FormField label="User" htmlFor="address-user">
              <select
                id="address-user"
                required
                value={form.userId}
                onChange={(e) => setForm({ ...form, userId: e.target.value })}
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
            <div className="form-row">
              <FormField label="CEP" htmlFor="address-cep">
                <input
                  id="address-cep"
                  required
                  placeholder="00000-000"
                  value={form.cep}
                  onChange={(e) => setForm({ ...form, cep: e.target.value })}
                />
              </FormField>
              <FormField label="Number (optional)" htmlFor="address-number">
                <input
                  id="address-number"
                  value={form.number}
                  onChange={(e) => setForm({ ...form, number: e.target.value })}
                />
              </FormField>
            </div>

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
