interface LabelProps {
  value?: string;
  type: 'warning' | 'error' | 'success' | 'usual'
  className?: string;
}

const Label: React.FC<LabelProps> = ({ value, className, type }) => {
  const colorClasses = {
    error: "bg-red-200 text-red-700",
    success: "bg-green-200 text-green-700",
    warning: "bg-yellow-200 text-yellow-700",
    usual: "bg-gray-200 text-gray-700",
  };

  return (
    <div className={`flex ${colorClasses[type]} rounded-lg w-full p-3 ${className || ''}`}>
      {value}
    </div>
  );
};


export default Label;