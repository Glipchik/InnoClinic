import { useState } from "react";
import Button from "@shared/ui/controls/Button";

interface CreateEntityProps {
  buttonText: string;
  renderForm: (close: () => void) => JSX.Element;
}

export const CreateEntity = ({ buttonText, renderForm }: CreateEntityProps) => {
  const [isCreating, setIsCreating] = useState<boolean>(false);

  return (
    <>
      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>{buttonText}</Button>
      </div>
      <div className="flex flex-col items-center">
        {isCreating && renderForm(() => setIsCreating(false))}
      </div>
    </>
  );
};