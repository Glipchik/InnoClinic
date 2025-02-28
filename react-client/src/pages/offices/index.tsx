import { OfficesList } from "@features/offices-list/ui";

const OfficesPage = () => {
  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Offices
      </h1>
      <div className="min-h-dvh m-4">
        <OfficesList />
      </div>
    </>
  )
}

export { OfficesPage };