export default interface CreateServiceModel {
  id: string,
  serviceName: string,
  serviceCategoryId: string,
  specializationId: string,
  price: number,
  isActive: boolean
}