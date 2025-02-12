import { useDispatch, useSelector } from "react-redux";
import { useServices } from "../../shared/hooks/useServices";
import { useContext, useEffect, useState } from "react";
import { RootState } from "../../store/store";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import Button from "../../shared/ui/controls/Button";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import { ServiceForm } from "./ServiceForm";
import Service from "../../entities/service";
import { Pagination } from "../../shared/ui/controls/Pagination";
import CreateServiceModel from "./models/createServiceModel";
import PaginatedList from "../../models/paginatedList";

export function ServicesList() {
  const [token, setToken] = useState<string | null>(null);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const pageSize = 2;

  const userManager = useContext(UserManagerContext);
  const { isUserAuthorized } = useSelector((state: RootState) => state.auth);

  const [editingServiceId, setEditingServiceId] = useState<string | null>(null);


  const {
    fetchServicesData, fetchServicesError, fetchServicesLoading,
    createServiceError, createServiceLoading,
    editServiceError, editServiceLoading,
    deleteServiceLoading, deleteServiceError,
    fetchServicesWithPagination, editService, createService, deleteService } = useServices(token);

  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser();
        setToken(user?.access_token ?? null);
      }
      fetchUser();
    }
  }, [userManager, isUserAuthorized]);

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
      <h1 className="text-3xl my-4">Services</h1>

      {fetchServicesLoading && <Loading label="Fetching Services: Loading..." />}
      {fetchServicesError && <ErrorBox value={`Fetching Error: ${fetchServicesError}`} />}

      {editServiceLoading && <Loading label="Editing Service: Editing..." />}
      {editServiceError && <ErrorBox value={`Editing Error: ${editServiceError}`} />}

      {deleteServiceLoading && <Loading label="Deleting Services: Deleting..." />}
      {deleteServiceError && <ErrorBox value={`Deleting Error: ${deleteServiceError}`} />}

      {createServiceLoading && <Loading label="Creating Services: Creating..." />}
      {createServiceError && <ErrorBox value={`Creating Error: ${createServiceError}`} />}
      
      <div className="flex justify-end mb-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Service
        </Button>
      </div>

      {isCreating && (
        <ServiceForm
          service={{ id: "", serviceName: "", isActive: true, serviceCategoryId: "", specializationId: "", price: 0 } as Service}
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
                  service={service}
                  onCancel={() => setEditingServiceId(null)}
                  onSubmit={async (service: Service) => {
                    await editService(service);
                    setEditingServiceId(null);
                    fetchServicesWithPagination(pageIndex, pageSize);
                  }}
                />
              ) : (
                <>
                  <p className="place-self-start text-3xl font-semibold">Service Name: {service.serviceName}</p>
                  <p className="place-self-start text-2xl">Is active: {service.isActive ? 'Yes' : 'No'}</p>
                  <p className="place-self-start text-2xl">Is active: {service.isActive ? 'Yes' : 'No'}</p>
                  <p className="place-self-start text-2xl">Is active: {service.isActive ? 'Yes' : 'No'}</p>
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