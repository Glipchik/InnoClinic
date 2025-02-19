import { useReceptionists } from "../../shared/hooks/useReceptionists";
import { useEffect, useState } from "react";
import Button from "../../shared/ui/controls/Button";
import { Pagination } from "../../shared/ui/controls/Pagination";
import PaginatedList from "../../models/paginatedList";
import EditReceptionistModel from "../../models/receptionists/EditReceptionistModel";
import ReceptionistModel from "../../models/receptionists/ReceptionistModel";
import { EditReceptionistForm } from "./index";
import Select from "../../shared/ui/forms/Select";
import Loading from "../../shared/ui/controls/Loading";
import { useOffices } from "../../shared/hooks/useOffices";
import Office from "../../entities/office";
import profile_pic from "../../assets/profile_pic.png"
import { GETProfilePictureUrl } from "../../shared/api/profileApi";

interface ReceptionistsListProps {
  token: string
}

export function ReceptionistsList({ token }: ReceptionistsListProps) {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [officeId, setOfficeId] = useState<string | null>(null);
  const { fetchOfficesLoading, fetchOfficesError, fetchOfficesData, fetchOffices } = useOffices(token)
  const pageSize = 2;

  const [photoUrls, setPhotoUrls] = useState<{ [key: string]: string | null }>({});

  const [editingReceptionistId, setEditingReceptionistId] = useState<string | null>(null);


  const {
    fetchReceptionistsData,
    fetchReceptionistsWithPagination, editReceptionist, deleteReceptionist } = useReceptionists(token);

  useEffect(() => {
    if (token) {
      fetchReceptionistsWithPagination(officeId, pageIndex, pageSize);
      fetchOffices()
    }
  }, [token, pageIndex]);

  useEffect(() => {
    if (fetchReceptionistsData) {
      async function fetchPhotos() {
        const newPhotoUrls: { [key: string]: string | null } = {};

        for (const receptionist of (fetchReceptionistsData as PaginatedList<ReceptionistModel>).items) {
          newPhotoUrls[receptionist.id] = (await GETProfilePictureUrl(receptionist.account.id, token)).data;
        }

        setPhotoUrls(newPhotoUrls);
      }

      if (fetchReceptionistsData) {
        fetchPhotos();
      }
    }
  }, [fetchReceptionistsData]);

  const handleEdit = (id: string) => {
    setEditingReceptionistId(id);
  };

  return (
    <div className="my-auto">

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
            fetchReceptionistsWithPagination(e.target.value, pageIndex, pageSize)
          }}
        >
          <option value="" label="Select office" />
          {fetchOfficesData &&
            (fetchOfficesData as Office[]).map((office: Office) => (
              <option key={office.id} value={office.id} label={office.address} />
            ))}
        </Select>}
      </div>

      {fetchReceptionistsData && (
        <ul className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {(fetchReceptionistsData as PaginatedList<ReceptionistModel>).items.map((receptionistModel) => (
            <li key={receptionistModel.id} className="flex flex-col p-6 bg-white rounded-lg shadow-lg hover:shadow-2xl transition-shadow">
              {editingReceptionistId === receptionistModel.id ? (
                <EditReceptionistForm
                  token={token}
                  editReceptionistModel={{
                    id: receptionistModel.id,
                    firstName: receptionistModel.firstName,
                    lastName: receptionistModel.lastName,
                    middleName: receptionistModel.middleName,
                    officeId: receptionistModel.office.id,
                    photo: null
                  } as EditReceptionistModel}
                  onCancel={() => setEditingReceptionistId(null)}
                  onSubmit={async (editReceptionistModel: EditReceptionistModel) => {
                    await editReceptionist(editReceptionistModel);
                    setEditingReceptionistId(null);
                    fetchReceptionistsWithPagination(officeId, pageIndex, pageSize);
                  }}
                />
              ) : (
                <div className="flex flex-col space-y-3">
                  <div className="flex items-center space-x-4">
                    <img 
                      src={photoUrls[receptionistModel.id] || profile_pic} 
                      alt={receptionistModel.firstName} 
                      className="w-16 h-16 rounded-full object-cover"
                    />
                    <div>
                      <h3 className="text-xl font-semibold">{receptionistModel.firstName} {receptionistModel.lastName}</h3>
                    </div>
                  </div>
                  <div className="flex flex-col space-y-1">
                    <p className="text-lg">Email: {receptionistModel.account.email}</p>
                    <p className="text-lg">Phone: {receptionistModel.account.phoneNumber}</p>
                    <p className="text-lg">Office: {receptionistModel.office.address}</p>
                    <p className="text-lg">Registry Phone: {receptionistModel.office.registryPhoneNumber}</p>
                  </div>
                  <div className="flex justify-between mt-4">
                    <Button onClick={() => handleEdit(receptionistModel.id)}>Edit</Button>
                    <Button onClick={async () => {
                      await deleteReceptionist(receptionistModel.id);
                      setEditingReceptionistId(null);
                      fetchReceptionistsWithPagination(officeId, pageIndex, pageSize);
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
      {fetchReceptionistsData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={(fetchReceptionistsData as PaginatedList<ReceptionistModel>).totalPages}
          hasPreviousPage={(fetchReceptionistsData as PaginatedList<ReceptionistModel>).hasPreviousPage}
          hasNextPage={(fetchReceptionistsData as PaginatedList<ReceptionistModel>).hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}
