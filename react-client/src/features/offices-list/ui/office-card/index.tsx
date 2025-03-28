import OfficeModel from "@shared/models/offices/officeModel"
import Button from "@shared/ui/controls/Button";
import { Link } from "react-router-dom";

interface OfficeCardProps {
  item: OfficeModel
  onDelete: () => void
}

const OfficeCard = ({ item: officeModel, onDelete }: OfficeCardProps) => {
  return (
    <li data-testid="office-card" key={officeModel.id} className="flex flex-col p-6 bg-white rounded-lg shadow-lg hover:shadow-2xl transition-shadow">
      <div className="flex flex-col space-y-3">
        <div>
          <h3 className="text-xl font-semibold">Address: {officeModel.address}</h3>
        </div>
        <div className="flex flex-col space-y-1">
          <p className="text-lg">Phone: {officeModel.registryPhoneNumber}</p>
          <p className="text-lg">Is active: {officeModel.isActive ? 'Yes' : 'No'}</p>
        </div>
        <div className="flex justify-between mt-4">
            <Link to={`/offices/edit/${officeModel.id}`}>
              <Button data_testid="edit-button"> 
                Edit
              </Button>
            </Link>
          <Button data_testid="deactivate-button" onClick={onDelete} className="bg-red-600 hover:bg-red-700 disabled:bg-red-800 disabled:text-gray-100" disabled={!officeModel.isActive}>
            Deactivate
          </Button>
        </div>
      </div>
    </li>
  )
}

export default OfficeCard