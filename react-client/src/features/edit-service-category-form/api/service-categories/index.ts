import EditServiceCategoryModel from "@features/edit-service-category-form/models/editServiceCategoryModel";
import ServiceCategoryModel from "@models/serviceCategories/serviceCategoryModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const servicecategorysApi = {
  edit: async (editServiceCategoryModel: EditServiceCategoryModel) => {
    return await servicesAxiosInstance.put<EditServiceCategoryModel[]>('ServiceCategories', editServiceCategoryModel);
  },
  getById: async (servicecategoryId?: string) => {
    return await servicesAxiosInstance.get<ServiceCategoryModel>(`ServiceCategories/${servicecategoryId}`);
  },
}

export default servicecategorysApi