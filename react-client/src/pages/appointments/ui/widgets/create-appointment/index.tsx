import { CreateAppointmentForm } from "../../../../../features/create-appointment-form/ui"
import Button from "../../../../../shared/ui/controls/Button"

interface CreateAppointmentProps {
  onCancel: () => void,
  onClick: () => void,
  isDisabled: boolean
}

const CreateAppointment = ({ onCancel, onClick, isDisabled }: CreateAppointmentProps) => {
  return (
    <>
      <div className="flex justify-end m-4">
        <Button onClick={onClick}>
          Create New Appointment
        </Button>
      </div>
      <div className="flex flex-col items-center">
        {isDisabled && <CreateAppointmentForm onCancel={onCancel} />}
      </div>
    </>
  )
}

export default CreateAppointment