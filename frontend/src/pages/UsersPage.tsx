import { useState, type FormEvent } from "react";
import { Badge } from "../components/ui/Badge";
import { Button } from "../components/ui/Button";
import { EmptyState } from "../components/ui/EmptyState";
import { FormField } from "../components/ui/FormField";
import { IconButton } from "../components/ui/IconButton";
import { DeleteIcon, EditIcon } from "../components/ui/icons";
import { Modal } from "../components/ui/Modal";
import { Pagination } from "../components/ui/Pagination";
import { usePaginatedResource } from "../hooks/usePaginatedResource";
import type { Role, UserResponse } from "../api/types";

interface UserFormState {
  name: string;
  email: string;
  password: string;
  role: Role;
}

const EMPTY_FORM: UserFormState = { name: "", email: "", password: "", role: "User" };

export function UsersPage() {
  const { items, page, setPage, pageSize, totalCount, loading, error, create, update, remove } =
    usePaginatedResource<UserResponse>("/users");

  const [editing, setEditing] = useState<UserResponse | null>(null);
  const [creating, setCreating] = useState(false);
  const [form, setForm] = useState<UserFormState>(EMPTY_FORM);
  const [submitting, setSubmitting] = useState(false);
  const [formError, setFormError] = useState<string | null>(null);

  function openCreate() {
    setForm(EMPTY_FORM);
    setFormError(null);
    setCreating(true);
  }

  function openEdit(user: UserResponse) {
    setForm({ name: user.name, email: user.email, password: "", role: user.role });
    setFormError(null);
    setEditing(user);
  }

  function closeModal() {
    setCreating(false);
    setEditing(null);
  }

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setFormError(null);

    try {
      if (editing) {
        await update(editing.id, form);
      } else {
        await create(form);
      }
      closeModal();
    } catch (err) {
      setFormError(err instanceof Error ? err.message : "Something went wrong.");
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete(user: UserResponse) {
    if (!confirm(`Delete ${user.name}?`)) return;
    await remove(user.id);
  }

  return (
    <section>
      <div className="page-header">
        <div>
          <h1>Users</h1>
          <p className="page-subtitle">{totalCount} total</p>
        </div>
        <Button variant="primary" onClick={openCreate}>
          New user
        </Button>
      </div>

      {error && <EmptyState title="Failed to load users" description={error} />}

      {!error && !loading && items.length === 0 && (
        <EmptyState title="No users yet" description="Create your first user to get started." />
      )}

      {!error && items.length > 0 && (
        <div className="table-wrap">
          <table className="data-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Role</th>
                <th aria-label="Actions" />
              </tr>
            </thead>
            <tbody>
              {items.map((user) => (
                <tr key={user.id}>
                  <td data-label="Name">{user.name}</td>
                  <td data-label="Email" className="mono">
                    {user.email}
                  </td>
                  <td data-label="Role">
                    <Badge tone={user.role === "Admin" ? "accent" : "neutral"}>{user.role}</Badge>
                  </td>
                  <td className="table-actions">
                    <span className="table-actions-group">
                      <IconButton tone="edit" label="Edit user" icon={<EditIcon />} onClick={() => openEdit(user)} />
                      <IconButton tone="danger" label="Delete user" icon={<DeleteIcon />} onClick={() => handleDelete(user)} />
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
        <Modal title={editing ? "Edit user" : "New user"} onClose={closeModal}>
          <form className="form" onSubmit={handleSubmit}>
            <div className="form-row">
              <FormField label="Name" htmlFor="user-name">
                <input
                  id="user-name"
                  required
                  value={form.name}
                  onChange={(e) => setForm({ ...form, name: e.target.value })}
                />
              </FormField>
              <FormField label="Email" htmlFor="user-email">
                <input
                  id="user-email"
                  type="email"
                  required
                  value={form.email}
                  onChange={(e) => setForm({ ...form, email: e.target.value })}
                />
              </FormField>
            </div>
            <div className="form-row">
              <FormField label="Password" htmlFor="user-password">
                <input
                  id="user-password"
                  type="password"
                  required
                  minLength={6}
                  value={form.password}
                  onChange={(e) => setForm({ ...form, password: e.target.value })}
                  placeholder={editing ? "Re-enter password to confirm changes" : undefined}
                />
              </FormField>
              <FormField label="Role" htmlFor="user-role">
                <select
                  id="user-role"
                  value={form.role}
                  onChange={(e) => setForm({ ...form, role: e.target.value as Role })}
                >
                  <option value="User">User</option>
                  <option value="Admin">Admin</option>
                </select>
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
