import { CreateSpecialization } from "@features/create-specialization-form";
import { SpecializationsList } from "@features/specializations-list";

const SpecializationsPage = () => {
  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Specializations
      </h1>
      <CreateSpecialization />
      <div className="min-h-dvh m-4">
        <SpecializationsList />
      </div>
    </>
  )
}

export { SpecializationsPage };