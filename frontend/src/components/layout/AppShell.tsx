import { useState } from "react";
import { NavLink, Outlet } from "react-router-dom";

const NAV_ITEMS = [
  { to: "/users", label: "Users" },
  { to: "/birds", label: "Birds" },
  { to: "/messages", label: "Messages" },
];

export function AppShell() {
  const [navOpen, setNavOpen] = useState(false);

  return (
    <div className="shell">
      <header className="topbar">
        <button
          type="button"
          className="nav-toggle"
          aria-label="Toggle navigation"
          aria-expanded={navOpen}
          onClick={() => setNavOpen((open) => !open)}
        >
          <span />
          <span />
          <span />
        </button>
        <span className="brand">BirdMessage</span>
      </header>

      <nav className={`sidebar${navOpen ? " sidebar-open" : ""}`}>
        <span className="brand sidebar-brand">BirdMessage</span>
        <ul>
          {NAV_ITEMS.map((item) => (
            <li key={item.to}>
              <NavLink
                to={item.to}
                className={({ isActive }) => (isActive ? "nav-link nav-link-active" : "nav-link")}
                onClick={() => setNavOpen(false)}
              >
                {item.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>

      {navOpen && <div className="sidebar-backdrop" onClick={() => setNavOpen(false)} />}

      <main className="content">
        <Outlet />
      </main>
    </div>
  );
}
