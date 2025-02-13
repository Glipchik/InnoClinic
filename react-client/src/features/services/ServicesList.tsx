import { useServices } from "../../shared/hooks/useServices";
import { useEffect, useState } from "react";
import Button from "../../shared/ui/controls/Button";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import { ServiceForm } from "./ServiceForm";
import Service from "../../entities/service";
import { Pagination } from "../../shared/ui/controls/Pagination";
import CreateServiceModel from "./models/CreateServiceModel";
import PaginatedList from "../../models/paginatedList";
import ServiceModel from "./models/ServiceModel";

interface ServicesListProps {
  token: string
}

export function ServicesList( { token }: ServicesListProps ) {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const pageSize = 2;

  const [editingServiceId, setEditingServiceId] = useState<string | null>(null);


  const {
    fetchServicesData, fetchServicesError, fetchServicesLoading,
    createServiceError, createServiceLoading,
    editServiceError, editServiceLoading,
    deleteServiceLoading, deleteServiceError,
    fetchServicesWithPagination, editService, createService, deleteService } = useServices(token);

  useEffect(() => {
    if (token) {
      fetchServicesWithPagination(pageIndex, pageSize);
    }
  }, [token, pageIndex]);

  const handleEdit = (id: string) => {
    setEditingServiceId(id);
  };

  return (
    <div className="my-auto">
      {fetchServicesLoading && <Loading label="Fetching Services: Loading..." />}
      {fetchServicesError && <ErrorBox value={`Fetching Error: ${fetchServicesError}`} />}

      {editServiceLoading && <Loading label="Editing Service: Editing..." />}
      {editServiceError && <ErrorBox value={`Editing Error: ${editServiceError}`} />}

      {deleteServiceLoading && <Loading label="Deleting Services: Deleting..." />}
      {deleteServiceError && <ErrorBox value={`Deleting Error: ${deleteServiceError}`} />}

      {createServiceLoading && <Loading label="Creating Services: Creating..." />}
      {createServiceError && <ErrorBox value={`Creating Error: ${createServiceError}`} />}

      {isCreating && (
        <ServiceForm
          serviceModel={{ id: "", serviceName: "", isActive: true, serviceCategoryId: "", specializationId: "", price: 0 } as ServiceModel}
          onCancel={() => setIsCreating(false)}
          onSubmit={async (createServiceModel: CreateServiceModel) => {
            await createService(createServiceModel);
            setIsCreating(false);
            fetchServicesWithPagination(pageIndex, pageSize);
          }}
        />
      )}
      
      {fetchServicesData && (
        <ul className="w-full flex flex-row justify-center items-center space-x-4">
          {(fetchServicesData as PaginatedList<Service>).items.map((service) => (
            <li key={service.id} className="p-4 w-[40%] flex flex-col rounded-xl space-y-3 bg-gray-200 justify-between items-center">
              {editingServiceId === service.id ? (
              <ServiceForm
                serviceModel={{
                id: service.id,
                serviceName: service.serviceName,
                isActive: service.isActive,
                serviceCategoryId: service.serviceCategory.id,
                specializationId: service.specialization.id,
                price: service.price
                } as ServiceModel}
                onCancel={() => setEditingServiceId(null)}
                onSubmit={async (serviceModel: ServiceModel) => {
                  await editService(serviceModel);
                  setEditingServiceId(null);
                  fetchServicesWithPagination(pageIndex, pageSize);
                }}
              />
              ) : (
              <>
                <p className="place-self-start text-3xl font-semibold">Service Name: {service.serviceName}</p>
                <br/>
                <p className="place-self-start text-2xl">Category Name: {service.serviceCategory.categoryName}</p>
                <p className="place-self-start text-2xl">Category Time Slot Size: {service.serviceCategory.timeSlotSize}</p>
                <br/>
                <p className="place-self-start text-2xl">Specialization Name: {service.specialization.specializationName}</p>
                <br/>
                <p className="place-self-start text-2xl">Price: {service.price}</p>
                <p className="place-self-start text-2xl">Is active: {service.isActive ? 'Yes' : 'No'}</p>
                <div className="flex space-x-4">
                <Button onClick={() => handleEdit(service.id)}>Edit</Button>
                {service.isActive && (
                  <Button onClick={async () => {
                  await deleteService(service.id);
                  setEditingServiceId(null);
                  fetchServicesWithPagination(pageIndex, pageSize);
                  }} className="bg-red-600 hover:bg-red-700">
                  Delete
                  </Button>
                )}
                </div>
              </>
              )}
            </li>
          ))}
        </ul>
      )}

      {/* Pagination */}
      {fetchServicesData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={(fetchServicesData as PaginatedList<Service>).totalPages}
          hasPreviousPage={(fetchServicesData as PaginatedList<Service>).hasPreviousPage}
          hasNextPage={(fetchServicesData as PaginatedList<Service>).hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}