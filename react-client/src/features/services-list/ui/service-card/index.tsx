import ServiceModel from "@models/services/serviceModel";
import Button from "@shared/ui/controls/Button";
import { Link } from "react-router-dom";

interface ServiceCardProps {
  item: ServiceModel
  onDelete: () => void
}

const ServiceCard = ({ item: serviceModel, onDelete }: ServiceCardProps) => {
  return (
    <li key={serviceModel.id} className="flex flex-col p-6 bg-white rounded-lg shadow-lg hover:shadow-2xl transition-shadow">
      <div className="flex flex-col space-y-3">
        <div>
          <h3 className="text-xl font-semibold">Name: {serviceModel.serviceName}</h3>
        </div>
        <div className="flex flex-col space-y-1">
          <p className="text-lg">Category Name: {serviceModel.serviceCategory.categoryName}</p>
          <p className="text-lg">Category Time Slot Size: {serviceModel.serviceCategory.timeSlotSize}</p>
          <p className="text-lg">Specialization Name: {serviceModel.specialization.specializationName}</p>
          <p className="text-lg">Price: {serviceModel.price}</p>
          <p className="text-lg">Is active: {serviceModel.isActive ? 'Yes' : 'No'}</p>
        </div>
        <div className="flex justify-between mt-4">
          <Button>
            <Link to={`/services/edit/${serviceModel.id}`}> Edit </Link>
          </Button>
          <Button onClick={onDelete} className="bg-red-600 hover:bg-red-700">
            Delete
          </Button>
        </div>
      </div>
    </li>
  )
}

export default ServiceCard