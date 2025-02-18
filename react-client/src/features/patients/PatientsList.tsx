import { usePatients } from "../../shared/hooks/usePatients";
import { useEffect, useState } from "react";
import Button from "../../shared/ui/controls/Button";
import { Pagination } from "../../shared/ui/controls/Pagination";
import PaginatedList from "../../models/paginatedList";
import EditPatientModel from "../../models/patients/EditPatientModel";
import PatientModel from "../../models/patients/PatientModel";
import { EditPatientForm } from "./index";
import profile_pic from "../../assets/profile_pic.png"
import { GETProfilePictureUrl } from "../../shared/api/profileApi";

interface PatientsListProps {
  token: string
}

export function PatientsList({ token }: PatientsListProps) {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const pageSize = 2;

  const [photoUrls, setPhotoUrls] = useState<{ [key: string]: string | null }>({});

  const [editingPatientId, setEditingPatientId] = useState<string | null>(null);


  const {
    fetchPatientsData,
    fetchPatientsWithPagination, editPatient, deletePatient } = usePatients(token);

  useEffect(() => {
    if (token) {
      fetchPatientsWithPagination(pageIndex, pageSize);
    }
  }, [token, pageIndex]);

  useEffect(() => {
    if (fetchPatientsData) {
      async function fetchPhotos() {
        const newPhotoUrls: { [key: string]: string | null } = {};

        for (const patient of (fetchPatientsData as PaginatedList<PatientModel>).items) {
          newPhotoUrls[patient.id] = (await GETProfilePictureUrl(patient.account.id, token)).data;
        }

        setPhotoUrls(newPhotoUrls);
      }

      if (fetchPatientsData) {
        fetchPhotos();
      }
    }
  }, [fetchPatientsData]);

  const handleEdit = (id: string) => {
    setEditingPatientId(id);
  };

  return (
    <div className="my-auto">

      {fetchPatientsData && (
        <ul className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {(fetchPatientsData as PaginatedList<PatientModel>).items.map((patientModel) => (
            <li key={patientModel.id} className="flex flex-col p-6 bg-white rounded-lg shadow-lg hover:shadow-2xl transition-shadow">
              {editingPatientId === patientModel.id ? (
                <EditPatientForm
                  editPatientModel={{
                    id: patientModel.id,
                    dateOfBirth: (patientModel.dateOfBirth).toString(),
                    firstName: patientModel.firstName,
                    lastName: patientModel.lastName,
                    middleName: patientModel.middleName,
                    photo: null
                  } as EditPatientModel}
                  onCancel={() => setEditingPatientId(null)}
                  onSubmit={async (editPatientModel: EditPatientModel) => {
                    await editPatient(editPatientModel);
                    setEditingPatientId(null);
                    fetchPatientsWithPagination(pageIndex, pageSize);
                  }}
                />
              ) : (
                <div className="flex flex-col space-y-3">
                  <div className="flex items-center space-x-4">
                    <img 
                      src={photoUrls[patientModel.id] || profile_pic} 
                      alt={patientModel.firstName} 
                      className="w-16 h-16 rounded-full object-cover"
                    />
                    <div>
                      <h3 className="text-xl font-semibold">{patientModel.firstName} {patientModel.lastName}</h3>
                    </div>
                  </div>
                  <div className="flex flex-col space-y-1">
                    <p className="text-lg">Email: {patientModel.account.email}</p>
                    <p className="text-lg">Phone: {patientModel.account.phoneNumber}</p>
                    <p className="text-lg">Date of Birth: {patientModel.dateOfBirth.toString()}</p>
                  </div>
                  <div className="flex justify-between mt-4">
                    <Button onClick={() => handleEdit(patientModel.id)}>Edit</Button>
                    <Button onClick={async () => {
                        await deletePatient(patientModel.id);
                        setEditingPatientId(null);
                        fetchPatientsWithPagination(pageIndex, pageSize);
                      }} className="bg-red-600 hover:bg-red-700">
                      Delete
                    </Button>
                  </div>
                </div>
              )}
            </li>
          ))}
        </ul>
      )}

      {/* Pagination */}
      {fetchPatientsData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={(fetchPatientsData as PaginatedList<PatientModel>).totalPages}
          hasPreviousPage={(fetchPatientsData as PaginatedList<PatientModel>).hasPreviousPage}
          hasNextPage={(fetchPatientsData as PaginatedList<PatientModel>).hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}
