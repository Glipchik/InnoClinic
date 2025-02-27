import { useEffect, useState } from "react";
import { fetchOfficesRequest } from "@features/offices-list/store/slices/fetch-offices";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@app/store";
import Button from "@shared/ui/controls/Button";
import { Pagination } from "@widgets/pagination";
import Loading from "@shared/ui/controls/Loading";
import Label from "@shared/ui/containers/Label";

export function OfficesList() {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const pageSize = 2;  
  const dispatch = useDispatch();
  const { loading, error, data } = useSelector((state: RootState) => state.fetchOffices);

  useEffect(() => {
    dispatch(fetchOfficesRequest({pageIndex, pageSize}));
  }, [pageIndex, dispatch]);

  return (
    <div className="my-auto">
      {loading && <Loading label="Fetching offices..." />}
      {error && <Label value={error} type="error"></Label>}
      {data && (
        <ul className="w-full flex flex-row justify-center items-center space-x-4">
          {data.items.map((office) => (
            <li key={office.id} className="p-4 w-[40%] flex flex-col rounded-xl space-y-3 bg-gray-200 justify-between items-center">
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
            </li>
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
    </div>
  );
}