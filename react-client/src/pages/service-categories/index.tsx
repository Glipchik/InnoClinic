import { CreateServiceCategory } from "@features/create-service-category-form";
import { ServiceCategoriesList } from "@features/service-categories-list";

const ServiceCategoriesPage = () => {
  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        ServiceCategorys
      </h1>
      <CreateServiceCategory />
      <div className="min-h-dvh m-4">
        <ServiceCategoriesList />
      </div>
    </>
  )
}

export { ServiceCategoriesPage };