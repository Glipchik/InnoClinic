import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@app/store";
import { Pagination } from "@widgets/pagination";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";
import ConfirmationModal from "@widgets/confirmation-modal";
import PaginatedList from "@models/paginatedList";

interface PaginatedListProps<T> {
  fetchStateSelector: (state: RootState) => {
    loading: boolean;
    error?: string;
    data?: PaginatedList<T>;
  };
  deleteStateSelector: (state: RootState) => {
    loading: boolean;
    error?: string;
    success?: boolean;
  };
  resetDeleteState: () => void;
  editStateSelector: (state: RootState) => {
    loading: boolean;
    error?: string;
    success?: boolean;
  };
  createStateSelector: (state: RootState) => {
    loading: boolean;
    error?: string;
    success?: boolean;
  };
  fetchAction: (params: { pageIndex: number; pageSize: number }) => void;
  deleteAction: (params: { id: string }) => void;
  CardComponent: React.ComponentType<{ item: T; onDelete: () => void }>;
  entityName: string;
}

export const List = <T,>({
  fetchStateSelector,
  deleteStateSelector,
  resetDeleteState,
  fetchAction,
  deleteAction,
  editStateSelector,
  createStateSelector,
  CardComponent,
  entityName,
}: PaginatedListProps<T>) => {
  const [isModalOpen, setModalOpen] = useState(false);
  const [itemIdToDelete, setItemIdToDelete] = useState<string | null>(null);
  const [pageIndex, setPageIndex] = useState<number>(1);

  const dispatch = useDispatch();
  const { loading, error, data } = useSelector(fetchStateSelector);
  const {
    loading: deleteLoading,
    error: deleteError,
    success: deleteSuccess,
  } = useSelector(deleteStateSelector);
  const {
    loading: editLoading,
    error: editError,
    success: editSuccess,
  } = useSelector(editStateSelector);
  const {
    loading: createLoading,
    error: createError,
    success: createSuccess,
  } = useSelector(createStateSelector);

  useEffect(() => {
    dispatch(
      fetchAction({ pageIndex, pageSize: import.meta.env.VITE_PAGE_SIZE })
    );
  }, [pageIndex, dispatch, fetchAction]);

  useEffect(() => {
    if (deleteSuccess) {
      dispatch(
        fetchAction({ pageIndex, pageSize: import.meta.env.VITE_PAGE_SIZE })
      );
    }
  }, [deleteSuccess, dispatch, fetchAction, pageIndex]);

  const handleConfirm = () => {
    if (itemIdToDelete) {
      dispatch(resetDeleteState());
      dispatch(deleteAction({ id: itemIdToDelete }));
    }
    setModalOpen(false);
  };

  return (
    <div className="flex flex-col my-auto">
      {deleteLoading && <Loading label={`Deactivating ${entityName}...`} />}
      {deleteError && (
        <Label value={`Deactivating: ${deleteError}`} type="error" />
      )}
      {deleteSuccess && deleteSuccess === true && (
        <Label value={`Deactivating: Successfully deactivated`} type="success" />
      )}

      {editLoading && <Loading label={`Editing ${entityName}...`} />}
      {editError && <Label value={`Editing: ${editError}`} type="error" />}
      {editSuccess && editSuccess === true && (
        <Label value={`Editing: Successfully edited`} type="success" />
      )}

      {createLoading && <Loading label={`Creating ${entityName}...`} />}
      {createError && <Label value={`Creating: ${createError}`} type="error" />}
      {createSuccess && createSuccess === true && (
        <Label value={`Creating: Successfully created`} type="success" />
      )}

      {loading && <Loading label={`Fetching ${entityName}...`} />}
      {error && <Label value={`Fetching: ${error}`} type="error" />}
      {data && (
        <ul className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {data.items.map((item) => (
            <CardComponent
              key={(item as { id: string }).id}
              item={item}
              onDelete={() => {
                setModalOpen(true);
                setItemIdToDelete((item as { id: string }).id);
              }}
            />
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
};
