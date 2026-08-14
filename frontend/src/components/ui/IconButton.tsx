import type { ButtonHTMLAttributes, ReactNode } from "react";

type Tone = "edit" | "danger" | "accent" | "neutral";

interface IconButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  tone?: Tone;
  label: string;
  icon: ReactNode;
}

export function IconButton({ tone = "neutral", label, icon, className = "", ...props }: IconButtonProps) {
  const classes = ["icon-btn", `icon-btn-${tone}`, className].filter(Boolean).join(" ");
  return (
    <button type="button" className={classes} aria-label={label} title={label} {...props}>
      {icon}
    </button>
  );
}
