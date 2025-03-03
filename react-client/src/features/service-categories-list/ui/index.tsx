import { RootState } from "@app/store";
import { fetchServiceCategoriesWithPaginationRequest } from "@shared/store/fetch-service-categories-with-pagination";
import { List } from "@widgets/list";
import { deleteServiceCategoryRequest } from "../store/delete-service-category";
import ServiceCategoryCard from "./service-category-card";

export const ServiceCategoriesList = () => {
  return (
    <List
      fetchStateSelector={(state: RootState) => state.fetchServiceCategoriesWithPagination}
      deleteStateSelector={(state: RootState) => state.deleteServiceCategory}
      fetchAction={fetchServiceCategoriesWithPaginationRequest}
      deleteAction={deleteServiceCategoryRequest}
      CardComponent={ServiceCategoryCard}
      entityName="serviceCategory"
    />
  );
}
