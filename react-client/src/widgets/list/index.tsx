import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@app/store";
import { Pagination } from "@widgets/pagination";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";
import ConfirmationModal from "@widgets/confirmation-modal";
import PaginatedList from "@models/paginatedList";

interface PaginatedListProps<T> {
  fetchStateSelector: (state: RootState) => { loading: boolean; error?: string; data?: PaginatedList<T> };
  deleteStateSelector: (state: RootState) => { loading: boolean; error?: string; success?: boolean };
  fetchAction: (params: { pageIndex: number; pageSize: number }) => void;
  deleteAction: (params: { id: string }) => void;
  CardComponent: React.ComponentType<{ item: T; onDelete: () => void }>;
  entityName: string;
}

export const List = <T,>({ fetchStateSelector, deleteStateSelector, fetchAction, deleteAction, CardComponent, entityName }: PaginatedListProps<T>) => {
  const [isModalOpen, setModalOpen] = useState(false);
  const [itemIdToDelete, setItemIdToDelete] = useState<string | null>(null);
  const [pageIndex, setPageIndex] = useState<number>(1);

  const dispatch = useDispatch();
  const { loading, error, data } = useSelector(fetchStateSelector);
  const { loading: deleteLoading, error: deleteError, success } = useSelector(deleteStateSelector);

  useEffect(() => {
    dispatch(fetchAction({ pageIndex, pageSize: import.meta.env.VITE_PAGE_SIZE }));
  }, [pageIndex, dispatch, fetchAction]);

  useEffect(() => {
    if (success) {
      dispatch(fetchAction({ pageIndex, pageSize: import.meta.env.VITE_PAGE_SIZE }));
    }
  }, [success, dispatch, fetchAction, pageIndex]);

  const handleConfirm = () => {
    if (itemIdToDelete) {
      dispatch(deleteAction({ id: itemIdToDelete }));
    }
    setModalOpen(false);
  };

  return (
    <div className="flex flex-col my-auto">
      {deleteLoading && <Loading label={`Deleting ${entityName}...`} />}
      {deleteError && <Label value={`Deleting: ${deleteError}`} type="error" />}
      {success && <Label value={`Deleting: Successfully deleted`} type="success" />}

      {loading && <Loading label={`Fetching ${entityName}...`} />}
      {error && <Label value={`Fetching: ${error}`} type="error" />}
      {data && (
        <ul className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {data.items.map((item) => (
            <CardComponent key={(item as { id: string }).id} item={item} onDelete={() => {
              setModalOpen(true);
              setItemIdToDelete((item as { id: string }).id);
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
          setModalOpen(false);
          setItemIdToDelete(null);
        }}
        onConfirm={handleConfirm}
        text={`Do you really want to delete this ${entityName}?`}
      />
    </div>
  );
}