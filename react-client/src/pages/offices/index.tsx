import { CreateOffice } from "@widgets/create-office";
import { OfficesList } from "@features/offices-list";

const OfficesPage = () => {
  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Offices
      </h1>
      <CreateOffice />
      <div className="min-h-dvh m-4">
        <OfficesList />
      </div>
    </>
  )
}

export { OfficesPage };