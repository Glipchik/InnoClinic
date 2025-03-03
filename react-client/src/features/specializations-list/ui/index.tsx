import { fetchSpecializationsWithPaginationRequest } from "../../../shared/store/fetch-specializations-with-pagination";
import { deleteSpecializationRequest } from "../store/delete-specialization";
import SpecializationCard from "./specialization-card";
import { RootState } from "@app/store";
import { List } from "@widgets/list";

export const SpecializationsList = () => {
  return (
    <List
      fetchStateSelector={(state: RootState) => state.fetchSpecializationsWithPagination}
      deleteStateSelector={(state: RootState) => state.deleteSpecialization}
      fetchAction={fetchSpecializationsWithPaginationRequest}
      deleteAction={deleteSpecializationRequest}
      CardComponent={SpecializationCard}
      entityName="specialization"
    />
  );
}
