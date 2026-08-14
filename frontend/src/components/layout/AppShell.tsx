import { useState, type ReactNode } from "react";
import { NavLink, Outlet, useLocation } from "react-router-dom";
import { useTheme } from "../../contexts/ThemeContext";

const NAV_ITEMS: { to: string; label: string; end?: boolean; icon: ReactNode }[] = [
  {
    to: "/",
    label: "Home",
    end: true,
    icon: (
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
        <path d="M3 11.5 12 4l9 7.5" />
        <path d="M5.5 10v9a1 1 0 0 0 1 1h4v-6h3v6h4a1 1 0 0 0 1-1v-9" />
      </svg>
    ),
  },
  {
    to: "/users",
    label: "Users",
    icon: (
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
        <circle cx="12" cy="8" r="3.4" />
        <path d="M5 20c1.2-3.6 4-5.5 7-5.5s5.8 1.9 7 5.5" />
      </svg>
    ),
  },
  {
    to: "/addresses",
    label: "Addresses",
    icon: (
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
        <path d="M12 21s-7-6.1-7-11.5A7 7 0 0 1 19 9.5C19 14.9 12 21 12 21Z" />
        <circle cx="12" cy="9.5" r="2.4" />
      </svg>
    ),
  },
  {
    to: "/birds",
    label: "Birds",
    icon: (
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
        <path d="M4 13c3-6 8-8 13-6 1.4.5 2.5 1.5 3 2.5-1.3.2-2.3.7-3 1.5 1 .3 1.8 1 2.3 2-1.4.3-2.6.1-3.6-.5-1.7 3.3-5.4 6-9.7 6.5" />
        <path d="M9 12.5c-1.8.3-3.4 1.2-5 3" />
      </svg>
    ),
  },
  {
    to: "/messages",
    label: "Messages",
    icon: (
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
        <path d="M4 5h16v11H8l-4 4V5Z" />
        <path d="M8 9h8M8 12.5h5" />
      </svg>
    ),
  },
];

function SunIcon() {
  return (
    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
      <circle cx="12" cy="12" r="4.2" />
      <path d="M12 2.5v2.4M12 19.1v2.4M4.4 4.4l1.7 1.7M17.9 17.9l1.7 1.7M2.5 12h2.4M19.1 12h2.4M4.4 19.6l1.7-1.7M17.9 6.1l1.7-1.7" />
    </svg>
  );
}

function MoonIcon() {
  return (
    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
      <path d="M20 14.5A8.5 8.5 0 1 1 9.5 4a6.7 6.7 0 0 0 10.5 10.5Z" />
    </svg>
  );
}

export function AppShell() {
  const [navOpen, setNavOpen] = useState(false);
  const { theme, toggleTheme } = useTheme();
  const location = useLocation();
  const isFullBleed = location.pathname === "/";

  return (
    <div className="shell">
      <header className="topbar">
        <div className="topbar-left">
          <NavLink to="/" className="brand" onClick={() => setNavOpen(false)}>
            🕊️ BirdMessage
          </NavLink>
          <nav className={`nav${navOpen ? " nav-open" : ""}`}>
            <ul>
              {NAV_ITEMS.map((item) => (
                <li key={item.to}>
                  <NavLink
                    to={item.to}
                    end={item.end}
                    className={({ isActive }) => (isActive ? "nav-link nav-link-active" : "nav-link")}
                    onClick={() => setNavOpen(false)}
                  >
                    <span className="nav-icon">{item.icon}</span>
                    {item.label}
                  </NavLink>
                </li>
              ))}
            </ul>
          </nav>
        </div>

        <div className="topbar-actions">
          <button
            type="button"
            className="theme-toggle"
            aria-label={theme === "dark" ? "Switch to light theme" : "Switch to dark theme"}
            onClick={toggleTheme}
          >
            {theme === "dark" ? <SunIcon /> : <MoonIcon />}
          </button>
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
        </div>
      </header>

      {navOpen && <div className="sidebar-backdrop" onClick={() => setNavOpen(false)} />}

      <main className={isFullBleed ? "content-full" : "content"}>
        <Outlet />
      </main>
    </div>
  );
}
