import { useOffices } from "../../shared/hooks/useOffices";
import { useEffect, useState } from "react";
import Button from "../../shared/ui/controls/Button";
import { OfficeForm } from "./OfficeForm";
import { Pagination } from "../../shared/ui/controls/Pagination";
import CreateOfficeModel from "../../models/offices/CreateOfficeModel";
import { EditOfficeModel } from "../../models/offices/EditOfficeModel";

interface OfficesListProps {
  token: string
}

export function OfficesList({ token }: OfficesListProps) {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const pageSize = 2;

  const [editingOfficeId, setEditingOfficeId] = useState<string | null>(null);

  const { fetchOfficesData, fetchOffices, editOffice, deleteOffice } = useOffices(token);

  useEffect(() => {
    if (token) {
      fetchOffices(pageIndex, pageSize);
    }
  }, [token, pageIndex]);

  const handleEdit = (id: string) => {
    setEditingOfficeId(id);
  };

  return (
    <div className="my-auto">
      {fetchOfficesData && (
        <ul className="w-full flex flex-row justify-center items-center space-x-4">
          {fetchOfficesData.items.map((office) => (
            <li key={office.id} className="p-4 w-[40%] flex flex-col rounded-xl space-y-3 bg-gray-200 justify-between items-center">
              {editingOfficeId === office.id ? (
                <OfficeForm
                  createOfficeModel={office as CreateOfficeModel}
                  onCancel={() => setEditingOfficeId(null)}
                  onSubmit={async (createOfficeModel: CreateOfficeModel) => {
                    await editOffice({ ...createOfficeModel, id: office.id } as EditOfficeModel);
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
                      <Button onClick={async () => {
                        await deleteOffice(office.id);
                        fetchOffices(pageIndex, pageSize);
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
      {fetchOfficesData && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={fetchOfficesData.totalPages}
          hasPreviousPage={fetchOfficesData.hasPreviousPage}
          hasNextPage={fetchOfficesData.hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
    </div>
  );
}