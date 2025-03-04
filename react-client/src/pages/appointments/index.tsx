import CreateAppointment from "@features/create-appointment-form/ui";

const AppointmentsPage = () => {
  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Appointments
      </h1>
      <CreateAppointment />
    </>
  )
}

export { AppointmentsPage };