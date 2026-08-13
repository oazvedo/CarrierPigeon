import { EmptyState } from "../components/ui/EmptyState";

export function MessagesPage() {
  return (
    <section>
      <div className="page-header">
        <div>
          <h1>Messages</h1>
          <p className="page-subtitle">Coming soon</p>
        </div>
      </div>
      <EmptyState
        title="Message sending isn't wired up yet"
        description="This page will let you send a message between users once the backend endpoints are ready."
      />
    </section>
  );
}
