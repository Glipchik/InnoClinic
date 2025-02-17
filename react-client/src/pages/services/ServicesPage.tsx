import { ServicesList } from "../../features/services/ServicesList";
import { useContext, useEffect, useState } from "react";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import { useServices } from "../../shared/hooks/useServices";
import Button from "../../shared/ui/controls/Button";
import { ServiceForm } from "../../features/services/ServiceForm";
import CreateServiceModel from "../../models/services/CreateServiceModel";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import ServiceModel from "../../models/services/ServiceModel";

function ServicesPage() {
  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);

  const {
    fetchServicesError, fetchServicesLoading,
    createServiceError, createServiceLoading,
    editServiceError, editServiceLoading,
    deleteServiceLoading, deleteServiceError, createService } = useServices(token);

  const [listKey, setListKey] = useState(0);

  const userManager = useContext(UserManagerContext)
  const { isUserAuthorized } = useSelector(
    (state: RootState) => state.auth
  );
  
  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser()
        setToken(user?.access_token ?? null)
      }
      fetchUser()
    }
  }, [userManager, isUserAuthorized])

  return (
    <>
      <h1 className="text-4xl font-semibold m-4">
        Services
      </h1> 

      {fetchServicesLoading && <Loading label="Fetching Services: Loading..." />}
      {fetchServicesError && <ErrorBox value={`Fetching Error: ${fetchServicesError}`} />}

      {editServiceLoading && <Loading label="Editing Service: Editing..." />}
      {editServiceError && <ErrorBox value={`Editing Error: ${editServiceError}`} />}

      {deleteServiceLoading && <Loading label="Deleting Services: Deleting..." />}
      {deleteServiceError && <ErrorBox value={`Deleting Error: ${deleteServiceError}`} />}

      {createServiceLoading && <Loading label="Creating Services: Creating..." />}
      {createServiceError && <ErrorBox value={`Creating Error: ${createServiceError}`} />}

      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Service
        </Button>
      </div>
    
      <div className="flex flex-col items-center m-4">
        {token && isCreating && <ServiceForm onCancel={() => setIsCreating(false)} onSubmit={(createServiceModel : CreateServiceModel) => {
          createService(createServiceModel as CreateServiceModel).then(() => {
            setIsCreating(false)
            setListKey(prevKey => prevKey + 1)
          });
        }} createServiceModel={{ serviceName: "", price: 0, serviceCategoryId: "", specializationId: "", isActive: true } as CreateServiceModel} /> }
      </div>
      
      <div className="min-h-dvh m-4">
        {token && <ServicesList key={listKey} token={token} />}
      </div>
    </>
  )
}

export { ServicesPage };