import { useSpecializations } from "../../shared/hooks/useSpecializations";
import { useEffect, useState } from "react";
import Button from "../../shared/ui/controls/Button";
import Loading from "../../shared/ui/controls/Loading";
import ErrorBox from "../../shared/ui/containers/ErrorBox";
import { SpecializationForm } from "./SpecializationForm";
import Specialization from "../../entities/specialization";
import { Pagination } from "../../shared/ui/controls/Pagination";
import CreateSpecializationModel from "../../models/specializations/createSpecializationModel";
import PaginatedList from "../../models/paginatedList";

interface SpecializationsListProps {
  token: string
}

export function SpecializationsList({ token }: SpecializationsListProps) {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [isCreating, setIsCreating] = useState<boolean>(false);
  const pageSize = 2;


  const [editingSpecializationId, setEditingSpecializationId] = useState<string | null>(null);

  const {
    fetchSpecializationsData, fetchSpecializationsError, fetchSpecializationsLoading,
    createSpecializationError, createSpecializationLoading,
    editSpecializationError, editSpecializationLoading,
    deleteSpecializationLoading, deleteSpecializationError,
    fetchSpecializationsWithPagination, editSpecialization, createSpecialization, deleteSpecialization } = useSpecializations(token);

  useEffect(() => {
    if (token) {
      fetchSpecializationsWithPagination(pageIndex, pageSize);
    }
  }, [token, pageIndex]);

  const handleEdit = (id: string) => {
    setEditingSpecializationId(id);
  };

  return (
    <div className="my-auto">
      {fetchSpecializationsLoading && <Loading label="Fetching Specializations: Loading..." />}
      {fetchSpecializationsError && <ErrorBox value={`Fetching Error: ${fetchSpecializationsError}`} />}

      {editSpecializationLoading && <Loading label="Editing Specialization: Editing..." />}
      {editSpecializationError && <ErrorBox value={`Editing Error: ${editSpecializationError}`} />}

      {deleteSpecializationLoading && <Loading label="Deleting Specializations: Deleting..." />}
      {deleteSpecializationError && <ErrorBox value={`Deleting Error: ${deleteSpecializationError}`} />}

      {createSpecializationLoading && <Loading label="Creating Specializations: Creating..." />}
      {createSpecializationError && <ErrorBox value={`Creating Error: ${createSpecializationError}`} />}

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
      
      {fetchSpecializationsData && (
        <ul className="w-full flex flex-row justify-center items-center space-x-4">
          {(fetchSpecializationsData as PaginatedList<Specialization>).items.map((specialization) => (
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
                      <Button onClick={async () => {
                        await deleteSpecialization(specialization.id);
                        setEditingSpecializationId(null);
                        fetchSpecializationsWithPagination(pageIndex, pageSize);
                      }} className="bg-red-600 hover:bg-red-700">
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
      {fetchSpecializationsData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={(fetchSpecializationsData as PaginatedList<Specialization>).totalPages}
          hasPreviousPage={(fetchSpecializationsData as PaginatedList<Specialization>).hasPreviousPage}
          hasNextPage={(fetchSpecializationsData as PaginatedList<Specialization>).hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}