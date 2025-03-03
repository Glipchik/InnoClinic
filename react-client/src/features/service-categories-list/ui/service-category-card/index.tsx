import ServiceCategoryModel from "@models/serviceCategories/serviceCategoryModel";
import Button from "@shared/ui/controls/Button";
import { Link } from "react-router-dom";

interface ServiceCategoryCardProps {
  item: ServiceCategoryModel
  onDelete: () => void
}

const ServiceCategoryCard = ({ item: serviceCategoryModel, onDelete }: ServiceCategoryCardProps) => {
  return (
    <li key={serviceCategoryModel.id} className="flex flex-col p-6 bg-white rounded-lg shadow-lg hover:shadow-2xl transition-shadow">
      <div className="flex flex-col space-y-3">
        <div>
          <h3 className="text-xl font-semibold">Name: {serviceCategoryModel.categoryName}</h3>
        </div>
        <div className="flex flex-col space-y-1">
          <p className="text-lg">Time slot size: {serviceCategoryModel.timeSlotSize}</p>
        </div>
        <div className="flex justify-between mt-4">
            <Button>
            <Link to={`/service-categories/edit/${serviceCategoryModel.id}`}> Edit </Link>
            </Button>
          <Button onClick={onDelete} className="bg-red-600 hover:bg-red-700">
            Delete
          </Button>
        </div>
      </div>
    </li>
  )
}

export default ServiceCategoryCard