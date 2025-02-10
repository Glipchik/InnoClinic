interface ErrorBoxProps {
  value?: string;
  className?: string;
}

const ErrorBox: React.FC<ErrorBoxProps> = ({ value, className }) => (
  <div 
    className={`flex bg-red-200 rounded-2xl w-full p-4 ${className || ''}`}>
    { value }
  </div>
);

export default ErrorBox;