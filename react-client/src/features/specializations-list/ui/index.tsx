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
      editStateSelector={(state: RootState) => state.editSpecialization}
      createStateSelector={(state: RootState) => state.createSpecialization}
      fetchAction={fetchSpecializationsWithPaginationRequest}
      deleteAction={deleteSpecializationRequest}
      CardComponent={SpecializationCard}
      entityName="specialization"
    />
  );
}
