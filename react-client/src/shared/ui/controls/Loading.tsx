import React from 'react';

interface LoadingProps {
  className?: string;
  label?: string;
}

const Loading: React.FC<LoadingProps> = ({ className, label }) => (
  <div 
    className={`flex flex-col items-center justify-center bg-gray-200 rounded-2xl w-full p-4 ${className || ''}`}
  >
    <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-gray-500 mb-2"></div>
    <p className="text-gray-700 font-medium">{label ?? 'Loading...'}</p>
  </div>
);

export default Loading;