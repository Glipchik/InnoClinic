import EditSpecializationModel from "@features/edit-specialization-form/models/editSpecializationModel";
import SpecializationModel from "@models/specializations/specializationModel";
import servicesAxiosInstance from "@shared/api/clients/services";
import tokenInterceptor from "@shared/api/interceptors/tokenInterceptor";

servicesAxiosInstance.interceptors.request.use(tokenInterceptor)

const specializationsApi = {
  edit: async (editSpecializationModel: EditSpecializationModel) => {
    return await servicesAxiosInstance.put<EditSpecializationModel[]>('Specializations', editSpecializationModel);
  },
  getById: async (specializationId?: string) => {
    return await servicesAxiosInstance.get<SpecializationModel>(`Specializations/${specializationId}`);
  },
}

export default specializationsApi