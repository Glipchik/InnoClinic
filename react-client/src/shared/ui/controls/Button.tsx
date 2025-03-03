import React from 'react';

interface ButtonProps {
  children: React.ReactNode;
  onClick?: () => void;
  className?: string;
  disabled?: boolean;
  type?: "submit" | "reset" | "button" | undefined;
  disabled?: boolean
}

const Button: React.FC<ButtonProps> = ({ children, onClick, className, type, disabled }) => {
  return (
    <button
      onClick={onClick}
      className={`px-4 py-2 bg-blue-500 text-white rounded disabled:bg-blue-300 hover:bg-blue-600 focus:outline-none cursor-pointer ${className}`}
      type={type}
      disabled={disabled}
    >
      {children}
    </button>
  );
};

export default Button;