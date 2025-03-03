import { useEffect, useState } from "react";
import { fetchOfficesRequest } from "@features/offices-list/store/fetch-offices";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@app/store";
import { Pagination } from "@widgets/pagination";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";
import OfficeCard from "./office-card";
import ConfirmationModal from "@widgets/confirmation-modal";
import { deleteOfficeRequest } from "../store/delete-office";

export function OfficesList() {
  const [isModalOpen, setModalOpen] = useState(false);
  const [officeIdToDelete, setOfficeIdToDelete] = useState<string | null>(null);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const pageSize = 2;
  const dispatch = useDispatch();
  const { loading, error, data } = useSelector((state: RootState) => state.fetchOffices);
  const { loading: deleteLoading, error: deleteError, success } = useSelector((state: RootState) => state.deleteOffice);

  const handleConfirm = () => {
    if (officeIdToDelete) {
      dispatch(deleteOfficeRequest({officeId: officeIdToDelete}))
    }
    setModalOpen(false);
  };

  useEffect(() => {
    dispatch(fetchOfficesRequest({ pageIndex, pageSize }));
  }, [pageIndex, dispatch]);

  return (
    <div className="flex flex-col my-auto">
      {deleteLoading && <Loading label="Deleting office..." />}
      {deleteError && <Label value={`Deleting: ${deleteError}`} type="error"></Label>}
      {success && <Label value={`Successfully deleted office`} type="error"></Label>}

      {loading && <Loading label="Fetching offices..." />}
      {error && <Label value={error} type="error"></Label>}
      {data && (
        <ul className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {data.items.map((officeModel) => (
            <OfficeCard officeModel={officeModel} onDelete={() => {
              setModalOpen(true)
              setOfficeIdToDelete(officeModel.id)
            }} />
          ))}
        </ul>
      )}

      {data && (
        <Pagination
          pageIndex={pageIndex}
          totalPages={data.totalPages}
          hasPreviousPage={data.hasPreviousPage}
          hasNextPage={data.hasNextPage}
          onPageChange={setPageIndex}
        />
      )}
      <ConfirmationModal
        isOpen={isModalOpen}
        onClose={() => {
          setModalOpen(false)
          setOfficeIdToDelete(null)
        }}
        onConfirm={handleConfirm}
        text="Do you really want to delete this office?"
      />
    </div>
  );
}