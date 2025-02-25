interface LabelProps {
  value?: string;
  type: 'warning' | 'error' | 'success' | 'usual'
  className?: string;
}

const Label: React.FC<LabelProps> = ({ value, className, type='usual' }) => {
  const color = type === 'error' ? 'red' :
    type === 'success' ? 'green' : 
    type === 'warning' ? 'yellow' : 'gray'
  
  return (
    <div
      className={`flex bg-${color}-200 text-${color}-700 rounded-xl w-full p-4 ${className || ''}`}>
      {value}
    </div>
  );
}

export default Label;