interface ModalProps {
  isOpen: boolean,
  children: React.ReactNode;
}

const Modal = ({ isOpen, children }: ModalProps) => {
  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 flex
                    items-center justify-center
                    bg-black/50">
      <div className="bg-white rounded-lg
                      shadow-lg p-6 max-w-md
                      w-full relative">
        {children}
      </div>
    </div>
  );
};

export default Modal