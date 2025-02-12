import ServiceCategory from "./serviceCategory";
import Specialization from "./specialization";

export default interface Service {
  id: string,
  serviceName: string,
  serviceCategory: ServiceCategory,
  specialization: Specialization,
  price: number,
  isActive: boolean
}