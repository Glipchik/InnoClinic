import { useDoctors } from "../../shared/hooks/useDoctors";
import { useEffect, useState } from "react";
import Button from "../../shared/ui/controls/Button";
import { Pagination } from "../../shared/ui/controls/Pagination";
import PaginatedList from "../../models/paginatedList";
import EditDoctorModel from "../../models/doctors/EditDoctorModel";
import DoctorModel from "../../models/doctors/DoctorModel";
import { EditDoctorForm } from "./index";
import { DoctorStatus } from "../../entities/enums/doctorStatus";
import Select from "../../shared/ui/forms/Select";
import Loading from "../../shared/ui/controls/Loading";
import Specialization from "../../entities/specialization";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { useOffices } from "../../shared/hooks/useOffices";
import Office from "../../entities/office";
import profile_pic from "../../assets/profile_pic.png"
import { GETProfilePictureUrl } from "../../shared/api/profileApi";

interface DoctorsListProps {
  token: string
}

export function DoctorsList({ token }: DoctorsListProps) {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [specializationId, setSpecializationId] = useState<string | null>(null);
  const [officeId, setOfficeId] = useState<string | null>(null);
  const { fetchSpecializationsLoading, fetchSpecializationsError, fetchSpecializationsData, fetchSpecializations } = useSpecializations(token)
  const { fetchOfficesLoading, fetchOfficesError, fetchOfficesData, fetchOffices } = useOffices(token)
  const pageSize = 2;

  const [photoUrls, setPhotoUrls] = useState<{ [key: string]: string | null }>({});

  const [editingDoctorId, setEditingDoctorId] = useState<string | null>(null);


  const {
    fetchDoctorsData,
    fetchDoctorsWithPagination, editDoctor, deleteDoctor } = useDoctors(token);

  useEffect(() => {
    if (token) {
      fetchDoctorsWithPagination(specializationId, officeId, pageIndex, pageSize);
      fetchSpecializations()
      fetchOffices()
    }
  }, [token, pageIndex]);

  useEffect(() => {
    if (fetchDoctorsData) {
      async function fetchPhotos() {
        const newPhotoUrls: { [key: string]: string | null } = {};

        for (const doctor of (fetchDoctorsData as PaginatedList<DoctorModel>).items) {
          newPhotoUrls[doctor.id] = (await GETProfilePictureUrl(doctor.account.id, token)).data;
        }

        setPhotoUrls(newPhotoUrls);
      }

      if (fetchDoctorsData) {
        fetchPhotos();
      }
    }
  }, [fetchDoctorsData]);

  const handleEdit = (id: string) => {
    setEditingDoctorId(id);
  };

  return (
    <div className="my-auto">

      {/* Specialization Select */}
      <div className="mb-4 flex flex-col">

        {fetchSpecializationsLoading && <Loading label="Loading specializations..." />}

        {fetchSpecializationsError && <p className="text-red-500">Error: {fetchSpecializationsError}</p>}

        {fetchSpecializationsData && <Select
          label="Specialization"
          id="specializationId"
          name="specializationId"
          onChange={(e) => {
            setSpecializationId(e.target.value);
            fetchDoctorsWithPagination(e.target.value, officeId, pageIndex, pageSize)
          }}
        >
          <option value="" label="Select specialization" />
          {fetchSpecializationsData &&
            (fetchSpecializationsData as Specialization[]).map((spec: Specialization) => (
              <option key={spec.id} value={spec.id} label={spec.specializationName} />
            ))}
        </Select>}

      </div>

      {/* Office Select */}
      <div className="mb-4 flex flex-col">

        {fetchOfficesLoading && <Loading label="Loading offices..." />}

        {fetchOfficesError && <p className="text-red-500">Error: {fetchOfficesError}</p>}

        {fetchOfficesData && <Select
          label="Office"
          id="officeId"
          name="officeId"
          onChange={(e) => {
            setOfficeId(e.target.value)
            fetchDoctorsWithPagination(specializationId, e.target.value, pageIndex, pageSize)
          }}
        >
          <option value="" label="Select office" />
          {fetchOfficesData &&
            (fetchOfficesData as Office[]).map((office: Office) => (
              <option key={office.id} value={office.id} label={office.address} />
            ))}
        </Select>}
      </div>

      {fetchDoctorsData && (
        <ul className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {(fetchDoctorsData as PaginatedList<DoctorModel>).items.map((doctorModel) => (
            <li key={doctorModel.id} className="flex flex-col p-6 bg-white rounded-lg shadow-lg hover:shadow-2xl transition-shadow">
              {editingDoctorId === doctorModel.id ? (
                <EditDoctorForm
                  token={token}
                  editDoctorModel={{
                    id: doctorModel.id,
                    careerStartYear: (doctorModel.careerStartYear).toString(),
                    dateOfBirth: (doctorModel.dateOfBirth).toString(),
                    firstName: doctorModel.firstName,
                    lastName: doctorModel.lastName,
                    middleName: doctorModel.middleName,
                    officeId: doctorModel.office.id,
                    status: doctorModel.status,
                    specializationId: doctorModel.specialization.id,
                    photo: null
                  } as EditDoctorModel}
                  onCancel={() => setEditingDoctorId(null)}
                  onSubmit={async (editDoctorModel: EditDoctorModel) => {
                    await editDoctor(editDoctorModel);
                    setEditingDoctorId(null);
                    fetchDoctorsWithPagination(specializationId, officeId, pageIndex, pageSize);
                  }}
                />
              ) : (
                <div className="flex flex-col space-y-3">
                  <div className="flex items-center space-x-4">
                    <img 
                      src={photoUrls[doctorModel.id] || profile_pic} 
                      alt={doctorModel.firstName} 
                      className="w-16 h-16 rounded-full object-cover"
                    />
                    <div>
                      <h3 className="text-xl font-semibold">{doctorModel.firstName} {doctorModel.lastName}</h3>
                      <p className="text-gray-500">{DoctorStatus[doctorModel.status]}</p>
                    </div>
                  </div>
                  <div className="flex flex-col space-y-1">
                    <p className="text-lg">Email: {doctorModel.account.email}</p>
                    <p className="text-lg">Phone: {doctorModel.account.phoneNumber}</p>
                    <p className="text-lg">Date of Birth: {doctorModel.dateOfBirth.toString()}</p>
                    <p className="text-lg">Career Start: {doctorModel.careerStartYear.toString()}</p>
                    <p className="text-lg">Specialization: {doctorModel.specialization.specializationName}</p>
                    <p className="text-lg">Office: {doctorModel.office.address}</p>
                    <p className="text-lg">Registry Phone: {doctorModel.office.registryPhoneNumber}</p>
                  </div>
                  <div className="flex justify-between mt-4">
                    <Button onClick={() => handleEdit(doctorModel.id)}>Edit</Button>
                    {doctorModel.status !== DoctorStatus.Inactive && (
                      <Button onClick={async () => {
                        await deleteDoctor(doctorModel.id);
                        setEditingDoctorId(null);
                        fetchDoctorsWithPagination(specializationId, officeId, pageIndex, pageSize);
                      }} className="bg-red-600 hover:bg-red-700">
                        Delete
                      </Button>
                    )}
                  </div>
                </div>
              )}
            </li>
          ))}
        </ul>
      )}

      {/* Pagination */}
      {fetchDoctorsData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={(fetchDoctorsData as PaginatedList<DoctorModel>).totalPages}
          hasPreviousPage={(fetchDoctorsData as PaginatedList<DoctorModel>).hasPreviousPage}
          hasNextPage={(fetchDoctorsData as PaginatedList<DoctorModel>).hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}
