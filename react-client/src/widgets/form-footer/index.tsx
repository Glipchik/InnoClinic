import Button from "@shared/ui/controls/Button"

interface FormFooterProps {
  onCancel: () => void,
  submitDisabled?: boolean
}

const FormFooter = ({ onCancel, submitDisabled }: FormFooterProps) => {
  return (
    <div className="flex flex-row space-x-4">
      <Button type="submit" className="w-full bg-blue-600 text-white hover:bg-blue-700" disabled={submitDisabled}>
        Submit
      </Button>
      <Button onClick={onCancel} type="button" className="w-full bg-gray-600 text-white hover:bg-gray-700">
        Cancel
      </Button>
    </div>
  )
}

export default FormFooter