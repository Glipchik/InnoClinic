import { useDispatch, useSelector } from "react-redux";
import { useOffices } from "../../shared/hooks/useOffices";
import { useContext, useEffect, useState } from "react";
import { RootState } from "../../store/store";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import Button from "../../shared/ui/controls/Button";
import { fetchOfficesDataFailure, fetchOfficesDataRequest, fetchOfficesDataSuccess } from "../../store/slices/officesSlice";
import { DELETE } from '../../shared/api/officeApi';
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import { OfficeForm } from "./OfficeForm";
import Office from "../../entities/office";
import { Pagination } from "../../shared/ui/controls/Pagination";
import CreateOfficeModel from "./models/CreateOfficeModel";

export function OfficesList() {
  const [token, setToken] = useState<string | null>(null);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const pageSize = 2;

  const dispatch = useDispatch();
  const userManager = useContext(UserManagerContext);
  const { isUserAuthorized } = useSelector((state: RootState) => state.auth);

  const [editingOfficeId, setEditingOfficeId] = useState<string | null>(null);

  const { loading: officesLoading, error: officesError, officesData, fetchOffices, editOffice, createOffice } = useOffices(token);

  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser();
        setToken(user?.access_token ?? null);
      }
      fetchUser();
    }
  }, [userManager, isUserAuthorized]);

  useEffect(() => {
    if (token) {
      fetchOffices(pageIndex, pageSize);
    }
  }, [token, pageIndex]);

  const handleEdit = (id: string) => {
    setEditingOfficeId(id);
  };

  const handleDelete = async (id: string) => {
    if (token) {
      try {
        dispatch(fetchOfficesDataRequest());
        await DELETE(id, token);
        fetchOffices(pageIndex, pageSize);
        dispatch(fetchOfficesDataSuccess(null))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";
        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
        dispatch(fetchOfficesDataFailure(errorMessage));
      }
    }
  };

  return (
    <div className="my-auto">
      <h1 className="text-3xl my-4">Offices</h1>
      
      <div className="flex justify-end mb-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Office
        </Button>
      </div>

      {isCreating && (
        <OfficeForm
          office={{ id: "", address: "", registryPhoneNumber: "", isActive: true }}
          onCancel={() => setIsCreating(false)}
          onSubmit={async (createOfficeModel: CreateOfficeModel) => {
            await createOffice(createOfficeModel);
            setIsCreating(false);
            fetchOffices(pageIndex, pageSize);
          }}
        />
      )}

      {officesLoading && <Loading label="Loading offices..." />}
      {officesError && <ErrorBox value={officesError} />}
      
      {officesData && (
        <ul className="w-full flex flex-row justify-center items-center space-x-4">
          {officesData.items.map((office) => (
            <li key={office.id} className="p-4 w-[40%] flex flex-col rounded-xl space-y-3 bg-gray-200 justify-between items-center">
              {editingOfficeId === office.id ? (
                <OfficeForm
                  office={office}
                  onCancel={() => setEditingOfficeId(null)}
                  onSubmit={async (office: Office) => {
                    await editOffice(office);
                    setEditingOfficeId(null);
                    fetchOffices(pageIndex, pageSize);
                  }}
                />
              ) : (
                <>
                  <p className="place-self-start text-3xl font-semibold">Address: {office.address}</p>
                  <p className="place-self-start text-2xl">Phone number: {office.registryPhoneNumber}</p>
                  <p className="place-self-start text-2xl">Is active: {office.isActive ? 'Yes' : 'No'}</p>
                  <div className="flex space-x-4">
                    <Button onClick={() => handleEdit(office.id)}>Edit</Button>
                    {office.isActive && (
                      <Button onClick={() => handleDelete(office.id)} className="bg-red-600 hover:bg-red-700">
                        Delete
                      </Button>
                    )}
                  </div>
                </>
              )}
            </li>
          ))}
        </ul>
      )}

      {/* Pagination */}
      {officesData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={officesData.totalPages}
          hasPreviousPage={officesData.hasPreviousPage}
          hasNextPage={officesData.hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}