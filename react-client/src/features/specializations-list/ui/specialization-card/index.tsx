import SpecializationModel from "@models/specializations/specializationModel";
import Button from "@shared/ui/controls/Button";
import { Link } from "react-router-dom";

interface SpecializationCardProps {
  item: SpecializationModel
  onDelete: () => void
}

const SpecializationCard = ({ item: specializationModel, onDelete }: SpecializationCardProps) => {
  return (
    <li key={specializationModel.id} className="flex flex-col p-6 bg-white rounded-lg shadow-lg hover:shadow-2xl transition-shadow">
      <div className="flex flex-col space-y-3">
        <div>
          <h3 className="text-xl font-semibold">Name: {specializationModel.specializationName}</h3>
        </div>
        <div className="flex flex-col space-y-1">
          <p className="text-lg">Is active: {specializationModel.isActive ? 'Yes' : 'No'}</p>
        </div>
        <div className="flex justify-between mt-4">
            <Button>
            <Link to={`/specializations/edit/${specializationModel.id}`}> Edit </Link>
            </Button>
          <Button onClick={onDelete} className="bg-red-600 hover:bg-red-700">
            Delete
          </Button>
        </div>
      </div>
    </li>
  )
}

export default SpecializationCard