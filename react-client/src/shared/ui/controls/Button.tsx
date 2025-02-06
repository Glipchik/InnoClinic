import React from 'react';

interface ButtonProps {
  children: React.ReactNode;
  onClick?: () => void;
  className?: string;
  type?: "submit" | "reset" | "button" | undefined;
}

const Button: React.FC<ButtonProps> = ({ children, onClick, className, type }) => {
  return (
    <button
      onClick={onClick}
      className={`px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 focus:outline-none cursor-pointer ${className}`}
      type={type}
    >
      {children}
    </button>
  );
};

export default Button;