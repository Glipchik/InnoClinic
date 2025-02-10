import { useDispatch, useSelector } from "react-redux";
import { useOffices } from "./hooks/useOffices";
import { useContext, useEffect, useState } from "react";
import { RootState } from "../../store/store";
import { UserManagerContext } from "../../shared/contexts/UserManagerContext"
import Button from "../../shared/ui/controls/Button";
import { fetchOfficesDataFailure, fetchOfficesDataRequest, fetchOfficesDataSuccess } from "../../store/slices/officesSlice"

import { DELETE } from '../../shared/api/officeApi'

export function OfficesList() {
  const [token, setToken] = useState<string | null>(null)

  const [pageIndex, setPageIndex] = useState<number>(1)
  //const [pageSize, setPageSize] = useState<number | null>(null)
  const pageSize = 2
    
  const dispatch = useDispatch()

  const userManager = useContext(UserManagerContext)
  const { isUserAuthorized } = useSelector(
    (state: RootState) => state.auth
  );

  const { loading: officesLoading, error: officesError, officesData, fetchOffices } = useOffices(token)

  useEffect(() => {
    if (userManager) {
      async function fetchUser() {
        const user = await userManager!.getUser()
        setToken(user?.access_token ?? null)
      }
      fetchUser()
    }
  }, [userManager, isUserAuthorized])

  useEffect(() => {
    if (token) {
      fetchOffices(pageIndex, pageSize)
    }
  }, [token, pageIndex])

  const handleEdit = (id: string) => {
    console.log(`Изменить офис с ID: ${id}`);
  };

  const handleDelete = async (id: string) => {
    if (token) {
      try {
        dispatch(fetchOfficesDataRequest())
        const response = await DELETE(id, token)
        dispatch(fetchOfficesDataSuccess(response.data))
        fetchOffices(pageIndex, pageSize)
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
    <div>
      <h1>Offices</h1>
      {officesLoading && <p>Loading...</p>}
      {officesError && <p>{officesError}</p>}
      {officesData && (
        <ul className="w-full flex flex-col justify-center items-center space-y-4">
          {officesData.items.map((office) => (
            <li key={office.id} className="p-4 w-[40%] flex flex-col rounded-2xl space-y-3 bg-gray-600 justify-between items-center">
              <p className="text-4xl font-semibold">Address: {office.address}</p>
              <p className="text-3xl">Phone number: {office.registryPhoneNumber}</p>
              <p className="text-3xl">Is active: {office.isActive ? 'Yes' : 'No'}</p>
              <div className="flex space-x-4">
                <Button 
                  onClick={() => handleEdit(office.id)}
                >
                  Edit
                </Button>
                <Button 
                  onClick={() => handleDelete(office.id)} 
                  className="bg-red-800 hover:bg-red-900"
                >
                  Delete
                </Button>
              </div>
            </li>
          ))}
        </ul>
      )}
      {/* Pagination */}
      <div className="mt-4 flex justify-center space-x-4">
        <Button
          onClick={() => setPageIndex((prev) => Math.max(prev - 1, 1))}
          disabled={!officesData?.hasPreviousPage}
        >
          Back
        </Button>

        <span className="px-4 py-2">{`${pageIndex} / ${officesData?.totalPages}`}</span>

        <Button
          onClick={() => setPageIndex((prev) => prev + 1)}
          disabled={!officesData?.hasNextPage}
        >
          Next
        </Button>
      </div>
    </div>
  );
}