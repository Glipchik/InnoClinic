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
  const { loading, error, data } = useSelector(fetchStateSelector);
  const [pageIndex, setPageIndex] = useState<number>(data ? data.pageIndex : 1);

  const dispatch = useDispatch();
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
    <div className="flex flex-col my-auto" data-testid="list">
      {deleteLoading && (
        <Loading
          data_testid="deactivateLoading"
          label={`Deactivating ${entityName}...`}
        />
      )}
      {deleteError && (
        <Label
          value={`Deactivating: ${deleteError}`}
          data_testid="deleteError"
          type="error"
        />
      )}
      {deleteSuccess && deleteSuccess === true && (
        <Label
          data_testid="deleteSuccess"
          value={`Deactivating: Successfully deactivated`}
          type="success"
        />
      )}

      {editLoading && (
        <Loading data_testid="editLoading" label={`Editing ${entityName}...`} />
      )}
      {editError && (
        <Label
          data_testid="editError"
          value={`Editing: ${editError}`}
          type="error"
        />
      )}
      {editSuccess && editSuccess === true && (
        <Label
          data_testid="editSuccess"
          value={`Editing: Successfully edited`}
          type="success"
        />
      )}

      {createLoading && (
        <Loading
          data_testid="createLoading"
          label={`Creating ${entityName}...`}
        />
      )}
      {createError && (
        <Label
          data_testid="createError"
          value={`Creating: ${createError}`}
          type="error"
        />
      )}
      {createSuccess && createSuccess === true && (
        <Label
          data_testid="createSuccess"
          value={`Creating: Successfully created`}
          type="success"
        />
      )}

      {loading && (
        <Loading
          data_testid="fetch-loading"
          label={`Fetching ${entityName}...`}
        />
      )}
      {error && (
        <Label
          data_testid="fetch-error"
          value={`Fetching: ${error}`}
          type="error"
        />
      )}
      {data && (
        <ul
          className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"
          data-testid="fetchData"
        >
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
