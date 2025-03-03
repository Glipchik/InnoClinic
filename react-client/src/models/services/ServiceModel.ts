import ServiceCategoryModel from "@models/serviceCategories/serviceCategoryModel";
import SpecializationModel from "@models/specializations/specializationModel";

export default interface ServiceModel {
  id: string,
  serviceName: string,
  serviceCategory: ServiceCategoryModel,
  specialization: SpecializationModel,
  price: number,
  isActive: boolean
}