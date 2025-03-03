import CreateService from "@features/create-service-form/ui";
import { ServicesList } from "@features/services-list/ui";

const ServicesPage = () => {
  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Services
      </h1>
      <CreateService />
      <div className="min-h-dvh m-4">
        <ServicesList />
      </div>
    </>
  )
}

export { ServicesPage };