interface ErrorBoxProps {
  value?: string;
  className?: string;
}

const ErrorBox: React.FC<ErrorBoxProps> = ({ value, className }) => (
  <div 
    className={`flex bg-red-200 text-red-700 rounded-xl w-full p-4 ${className || ''}`}>
    { value }
  </div>
);

export default ErrorBox;