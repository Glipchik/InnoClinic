import React from 'react';

interface LoadingProps {
  className?: string;
  label?: string;
}

const Loading: React.FC<LoadingProps> = ({ className, label }) => (
  <div 
    className={`flex flex-col items-center justify-center bg-gray-300 rounded-xl w-full p-4 ${className || ''}`}
  >
    <div className="rounded-full h-12 w-12 border-4 border-t-4 border-t-gray-500 border-gray-300 animate-spin mb-2"></div>
    <p className="text-gray-700 font-medium">{label ?? 'Loading...'}</p>
  </div>
);

export default Loading;