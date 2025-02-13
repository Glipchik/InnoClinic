import { useContext, useEffect, useState } from "react";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import { useSelector } from "react-redux";
import { RootState } from "../../store/store";
import { useServiceCategories } from "../../shared/hooks/useServiceCategories";
import Button from "../../shared/ui/controls/Button";
import { ServiceCategoryForm } from "../../features/serviceCategories/ServiceCategoryForm";
import CreateServiceCategoryModel from "../../features/serviceCategories/models/createServiceCategoryModel";
import ServiceCategory from "../../entities/serviceCategory";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import { ServiceCategoriesList } from "../../features/serviceCategories/ServiceCategoriesList";

function ServiceCategoriesPage() {
  const [token, setToken] = useState<string | null>(null)
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const { createServiceCategoryError, createServiceCategoryLoading, createServiceCategory, fetchServiceCategoriesWithPagination } = useServiceCategories(token);

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
        Service Categories
      </h1> 

      <div className="flex justify-end m-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Service Category
        </Button>
      </div>
      
      <div className="flex flex-col items-center m-4">
        {token && isCreating && <ServiceCategoryForm onCancel={() => setIsCreating(false)} onSubmit={(servicecategory : ServiceCategory) => {
          createServiceCategory(servicecategory as CreateServiceCategoryModel).then(() => {;
            setIsCreating(false);
          });
        }} serviceCategory={{ id: "", categoryName: "", isActive: true, timeSlotSize: "00:15:00" } as ServiceCategory} /> }

        {createServiceCategoryLoading && <Loading label="Creating ServiceCategory..." />}
        {createServiceCategoryError && <ErrorBox value={createServiceCategoryError} />}
      </div>

      <div className="min-h-dvh">
        {token && <ServiceCategoriesList token={token} />}
      </div>
    </>
  )
}

export { ServiceCategoriesPage };