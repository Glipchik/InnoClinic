import { useDispatch, useSelector } from "react-redux";
import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { useContext, useEffect, useState } from "react";
import { RootState } from "../../store/store";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext";
import Button from "../../shared/ui/controls/Button";
import { fetchSpecializationsDataFailure, fetchSpecializationsDataRequest, fetchSpecializationsDataSuccess } from "../../store/slices/specializationsSlice";
import { DELETE } from '../../shared/api/specializationApi';
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import { SpecializationForm } from "./SpecializationForm";
import Specialization from "../../entities/specialization";
import { Pagination } from "../../shared/ui/controls/Pagination";
import CreateSpecializationModel from "./models/createSpecializationModel";

export function SpecializationsList() {
  const [token, setToken] = useState<string | null>(null);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const pageSize = 2;

  const dispatch = useDispatch();
  const userManager = useContext(UserManagerContext);
  const { isUserAuthorized } = useSelector((state: RootState) => state.auth);

  const [editingSpecializationId, setEditingSpecializationId] = useState<string | null>(null);

  const { loading: specializationsLoading, error: specializationsError, paginatedSpecializationsData, fetchSpecializationsWithPagination, editSpecialization, createSpecialization } = useSpecializations(token);

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
      fetchSpecializationsWithPagination(pageIndex, pageSize);
    }
  }, [token, pageIndex]);

  const handleEdit = (id: string) => {
    setEditingSpecializationId(id);
  };

  const handleDelete = async (id: string) => {
    if (token) {
      try {
        dispatch(fetchSpecializationsDataRequest());
        await DELETE(id, token);
        fetchSpecializationsWithPagination(pageIndex, pageSize);
        dispatch(fetchSpecializationsDataSuccess(null))
      } catch (error: unknown) {
        let errorMessage = "An unknown error occurred";
        if (error.response && error.response.data) {
          const problemDetails = error.response.data;
          errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        }
        dispatch(fetchSpecializationsDataFailure(errorMessage));
      }
    }
  };

  return (
    <div className="my-auto">
      <h1 className="text-3xl my-4">Specializations</h1>
      
      <div className="flex justify-end mb-4">
        <Button onClick={() => setIsCreating(true)}>
          Create New Specialization
        </Button>
      </div>

      {isCreating && (
        <SpecializationForm
          specialization={{ id: "", specializationName: "", isActive: true } as Specialization}
          onCancel={() => setIsCreating(false)}
          onSubmit={async (createSpecializationModel: CreateSpecializationModel) => {
            await createSpecialization(createSpecializationModel);
            setIsCreating(false);
            fetchSpecializationsWithPagination(pageIndex, pageSize);
          }}
        />
      )}

      {specializationsLoading && <Loading label="Loading specializations..." />}
      {specializationsError && <ErrorBox value={specializationsError} />}
      
      {paginatedSpecializationsData && (
        <ul className="w-full flex flex-row justify-center items-center space-x-4">
          {paginatedSpecializationsData.items.map((specialization) => (
            <li key={specialization.id} className="p-4 w-[40%] flex flex-col rounded-xl space-y-3 bg-gray-200 justify-between items-center">
              {editingSpecializationId === specialization.id ? (
                <SpecializationForm
                  specialization={specialization}
                  onCancel={() => setEditingSpecializationId(null)}
                  onSubmit={async (specialization: Specialization) => {
                    await editSpecialization(specialization);
                    setEditingSpecializationId(null);
                    fetchSpecializationsWithPagination(pageIndex, pageSize);
                  }}
                />
              ) : (
                <>
                  <p className="place-self-start text-3xl font-semibold">Address: {specialization.specializationName}</p>
                  <p className="place-self-start text-2xl">Is active: {specialization.isActive ? 'Yes' : 'No'}</p>
                  <div className="flex space-x-4">
                    <Button onClick={() => handleEdit(specialization.id)}>Edit</Button>
                    {specialization.isActive && (
                      <Button onClick={() => handleDelete(specialization.id)} className="bg-red-600 hover:bg-red-700">
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
      {paginatedSpecializationsData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={paginatedSpecializationsData.totalPages}
          hasPreviousPage={paginatedSpecializationsData.hasPreviousPage}
          hasNextPage={paginatedSpecializationsData.hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}