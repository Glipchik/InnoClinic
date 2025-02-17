import { useServiceCategories } from "../../shared/hooks/useServiceCategories";
import { useEffect, useState } from "react";
import Button from "../../shared/ui/controls/Button";
import { ServiceCategoryForm } from "./ServiceCategoryForm";
import ServiceCategory from "../../entities/serviceCategory";
import { Pagination } from "../../shared/ui/controls/Pagination";
import CreateServiceCategoryModel from "../../models/serviceCategories/createServiceCategoryModel";
import PaginatedList from "../../models/paginatedList";
import EditServiceCategoryModel from "../../models/serviceCategories/editServiceCategoryModel";

interface ServiceCategoriesListProps {
  token: string
}

export function ServiceCategoriesList({ token }: ServiceCategoriesListProps) {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const pageSize = 2;

  const [editingServiceCategoryId, setEditingServiceCategoryId] = useState<string | null>(null);

  const {
    fetchServiceCategoriesData,
    fetchServiceCategoriesWithPagination, 
    editServiceCategory, 
    createServiceCategory, 
    deleteServiceCategory 
  } = useServiceCategories(token);

  useEffect(() => {
    if (token) {
      fetchServiceCategoriesWithPagination(pageIndex, pageSize);
    }
  }, [token, pageIndex]);

  const handleEdit = (id: string) => {
    setEditingServiceCategoryId(id);
  };

  return (
    <div className="my-auto">
      {isCreating && (
        <ServiceCategoryForm
          createServiceCategoryModel={{ categoryName: "", timeSlotSize: "00:05:00" } as ServiceCategory}
          onCancel={() => setIsCreating(false)}
          onSubmit={async (createServiceCategoryModel: CreateServiceCategoryModel) => {
            await createServiceCategory(createServiceCategoryModel);
            setIsCreating(false);
            fetchServiceCategoriesWithPagination(pageIndex, pageSize);
          }}
        />
      )}
      
      {fetchServiceCategoriesData && (
        <ul className="w-full flex flex-row justify-center items-center space-x-4">
          {(fetchServiceCategoriesData as PaginatedList<ServiceCategory>).items.map((serviceCategory) => (
            <li key={serviceCategory.id} className="p-4 w-[40%] flex flex-col rounded-xl space-y-3 bg-gray-200 justify-between items-center">
              {editingServiceCategoryId === serviceCategory.id ? (
                <ServiceCategoryForm
                  createServiceCategoryModel={serviceCategory as CreateServiceCategoryModel}
                  onCancel={() => setEditingServiceCategoryId(null)}
                  onSubmit={async (createServiceCategoryModel: CreateServiceCategoryModel) => {
                    await editServiceCategory({ ...createServiceCategoryModel, id: serviceCategory.id } as EditServiceCategoryModel);
                    setEditingServiceCategoryId(null);
                    fetchServiceCategoriesWithPagination(pageIndex, pageSize);
                  }}
                />
              ) : (
                <>
                  <p className="place-self-start text-3xl font-semibold">Category Name: {serviceCategory.categoryName}</p>
                  <p className="place-self-start text-2xl">Time Slot Size: {serviceCategory.timeSlotSize}</p>
                  <div className="flex space-x-4">
                    <Button onClick={() => handleEdit(serviceCategory.id)}>Edit</Button>
                    <Button onClick={ async () => {
                      await deleteServiceCategory(serviceCategory.id);
                      setEditingServiceCategoryId(null);
                      fetchServiceCategoriesWithPagination(pageIndex, pageSize);
                    }} className="bg-red-600 hover:bg-red-700">
                      Delete
                    </Button>
                  </div>
                </>
              )}
            </li>
          ))}
        </ul>
      )}

      {/* Pagination */}
      {fetchServiceCategoriesData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={(fetchServiceCategoriesData as PaginatedList<ServiceCategory>).totalPages}
          hasPreviousPage={(fetchServiceCategoriesData as PaginatedList<ServiceCategory>).hasPreviousPage}
          hasNextPage={(fetchServiceCategoriesData as PaginatedList<ServiceCategory>).hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}