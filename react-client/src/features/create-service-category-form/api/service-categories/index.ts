import CreateServiceCategoryModel from "@features/create-service-category-form/models/createServiceCategoryModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const serviceCategoriesApi = {
  post: async (createServiceCategoryModel: CreateServiceCategoryModel) => {
    return await servicesAxiosInstance.post<CreateServiceCategoryModel>('ServiceCategories', createServiceCategoryModel);
  }
}

export default serviceCategoriesApi