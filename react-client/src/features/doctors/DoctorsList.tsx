import { useDoctors } from "../../shared/hooks/useDoctors";
import { useEffect, useState } from "react";
import Button from "../../shared/ui/controls/Button";
import { Pagination } from "../../shared/ui/controls/Pagination";
import PaginatedList from "../../models/paginatedList";
import EditDoctorModel from "../../models/doctors/EditDoctorModel";
import DoctorModel from "../../models/doctors/DoctorModel";
import { EditDoctorForm } from "./EditDoctorForm";
import { DoctorStatus } from "../../entities/enums/doctorStatus";
import Select from "../../shared/ui/forms/Select";
import Loading from "../../shared/ui/controls/Loading";
import Specialization from "../../entities/specialization";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { useOffices } from "../../shared/hooks/useOffices";
import Office from "../../entities/office";

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

  const handleEdit = (id: string) => {
    setEditingDoctorId(id);
  };

  return (
    <div className="my-auto">

      {/* Specialization Select */}
      <div className="flex flex-col">

        {fetchSpecializationsLoading && <Loading label="Loading specializations..." />}

        {fetchSpecializationsError && <p className="text-red-500">Error: {fetchSpecializationsError}</p>}

        <Select
          disabled={false}
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

        </Select>

      </div>

      {/* Office Select */}
      <div className="flex flex-col">

        {fetchOfficesLoading && <Loading label="Loading offices..." />}

        {fetchOfficesError && <p className="text-red-500">Error: {fetchOfficesError}</p>}

        <Select
          disabled={false}
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
            (fetchOfficesData as Office[]).map((spec: Office) => (
              <option key={spec.id} value={spec.id} label={spec.address} />
            ))}

        </Select>
      </div>

      {fetchDoctorsData && (
        <ul className="w-full flex flex-row justify-center items-center space-x-4">
          {(fetchDoctorsData as PaginatedList<DoctorModel>).items.map((doctorModel) => (
            <li key={doctorModel.id} className="p-4 w-[40%] flex flex-col rounded-xl space-y-3 bg-gray-200 justify-between items-center">
              {editingDoctorId === doctorModel.id ? (
                <EditDoctorForm
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
                <>
                  <h2 className="text-2xl font-bold">Account info</h2>
                  <p className="place-self-start text-xl font-semibold">Email: {doctorModel.account.email}</p>
                  <p className="place-self-start text-xl font-semibold">Phone Number: {doctorModel.account.phoneNumber}</p>
                  <h2 className="text-2xl font-bold">Personal info</h2>
                  <p className="place-self-start text-xl font-semibold">First Name: {doctorModel.firstName}</p>
                  <p className="place-self-start text-xl font-semibold">Last Name: {doctorModel.lastName}</p>
                  {doctorModel.middleName && <p className="place-self-start text-xl font-semibold">Middle Name: {doctorModel.middleName}</p>}
                  <p className="place-self-start text-xl font-semibold">Date Of Birth: {doctorModel.dateOfBirth.toString()}</p>
                  <p className="place-self-start text-xl font-semibold">Career Start: {doctorModel.careerStartYear.toString()}</p>
                  <h2 className="text-2xl font-bold">Specialization info</h2>
                  <p className="place-self-start text-xl">Specialization Name: {doctorModel.specialization.specializationName}</p>
                  <h2 className="text-2xl font-bold">Office info</h2>
                  <p className="place-self-start text-xl">Address: {doctorModel.office.address}</p>
                  <p className="place-self-start text-xl">Registry Phone Number: {doctorModel.office.registryPhoneNumber}</p>
                  <br />
                  <p className="place-self-start text-xl">Status: {DoctorStatus[doctorModel.status]}</p>
                  <div className="flex space-x-4">
                    <Button onClick={() => handleEdit(doctorModel.id)}>Edit</Button>
                    {doctorModel.status != DoctorStatus.Inactive && (<Button onClick={async () => {
                      await deleteDoctor(doctorModel.id);
                      setEditingDoctorId(null);
                      fetchDoctorsWithPagination(specializationId, officeId, pageIndex, pageSize);
                    }} className="bg-red-600 hover:bg-red-700">
                      Delete
                    </Button>)}
                  </div>
                </>
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