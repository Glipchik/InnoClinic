import ServiceCategoryModel from "@models/serviceCategories/serviceCategoryModel";
import servicesAxiosInstance from "../clients/services";
import tokenInterceptor from "../interceptors/tokenInterceptor";
import PaginatedList from "@models/paginatedList";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const serviceCategoriesApi = {
  getAll: async () => {
    return await servicesAxiosInstance.get<ServiceCategoryModel[]>('ServiceCategories');
  },
  getAllWithPagination: async (pageIndex?: number, pageSize?: number) => {
    return await servicesAxiosInstance.get<PaginatedList<ServiceCategoryModel>>('ServiceCategories/with-pagination', { params: {
      pageIndex: pageIndex ?? 1,
      pageSize: pageSize ?? 2
    }});
  },
  getById: async (id: string) => {
    return await servicesAxiosInstance.get<ServiceCategoryModel[]>(`ServiceCategories/${id}`);
  }
}

export default serviceCategoriesApi