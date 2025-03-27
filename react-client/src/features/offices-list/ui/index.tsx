import { fetchOfficesRequest } from "@shared/store/fetch-offices";
import { RootState } from "@app/store";
import OfficeCard from "./office-card";
import { deleteOfficeRequest, resetState } from "../store/delete-office";
import { List } from "@widgets/list";

export const OfficesList = () => {
  return (
    <List
      fetchStateSelector={(state: RootState) => state.fetchOffices}
      deleteStateSelector={(state: RootState) => state.deleteOffice}
      resetDeleteState={resetState}
      editStateSelector={(state: RootState) => state.editOffice}
      createStateSelector={(state: RootState) => state.createOffice}
      fetchAction={fetchOfficesRequest}
      deleteAction={deleteOfficeRequest}
      CardComponent={OfficeCard}
      entityName="Office"
    />
  );
};
