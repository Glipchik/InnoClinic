import Button from "@shared/ui/controls/Button";
import Modal from "@shared/ui/modal";

interface ConfirmationModalProps {
  isOpen: boolean,
  onClose: () => void,
  onConfirm: () => void,
  text: string
}

const ConfirmationModal = ({ isOpen, onClose, onConfirm, text }: ConfirmationModalProps) => {
  return (
    <Modal isOpen={isOpen}>
      <h2 className="text-lg font-bold"> 
        Confirm Action
      </h2>
      <p className="text-gray-700">
        {text}
      </p>
      <div className="flex justify-end
                      space-x-4 mt-4">
        <Button onClick={onClose}>
          Cancel
        </Button>
        <Button onClick={onConfirm}>
          Confirm
        </Button>
      </div>
    </Modal>
  );
};

export default ConfirmationModal