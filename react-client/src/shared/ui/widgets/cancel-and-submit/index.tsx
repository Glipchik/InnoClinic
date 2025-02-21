import Button from "../../controls/Button"

interface CancelAndSubmitProps {
  onCancel: () => void
}

const CancelAndSubmit = ({ onCancel }: CancelAndSubmitProps) => {
  return (
    <div className="flex flex-row space-x-4">
      <Button type="submit" className="w-full bg-blue-600 text-white hover:bg-blue-700">
        Submit
      </Button>
      <Button onClick={onCancel} className="w-full bg-gray-600 text-white hover:bg-gray-700">
        Cancel
      </Button>
    </div>
  )
}

export default CancelAndSubmit