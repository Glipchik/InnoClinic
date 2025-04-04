import React from "react";

interface ButtonProps {
  children?: React.ReactNode;
  onClick?: () => void;
  className?: string;
  disabled?: boolean;
  type?: "submit" | "reset" | "button" | undefined;
  data_testid?: string;
}

const Button: React.FC<ButtonProps> = ({
  children,
  onClick,
  className,
  type,
  disabled,
  data_testid,
}) => {
  return (
    <button
      onClick={onClick}
      className={`px-4 py-2 bg-blue-500 text-white rounded disabled:bg-blue-900 disabled:text-gray-400 disabled:hover:cursor-not-allowed hover:bg-blue-600 focus:outline-none cursor-pointer ${className}`}
      type={type}
      disabled={disabled}
      data-testid={data_testid}
    >
      {children}
    </button>
  );
};

export default Button;
