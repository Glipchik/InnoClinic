import { fetchOfficesRequest } from "@shared/store/fetch-offices";
import { RootState } from "@app/store";
import OfficeCard from "./office-card";
import { deleteOfficeRequest } from "../store/delete-office";
import { List } from "@widgets/list";

export function OfficesList() {
  return (
    <List
      fetchStateSelector={(state: RootState) => state.fetchOffices}
      deleteStateSelector={(state: RootState) => state.deleteOffice}
      fetchAction={fetchOfficesRequest}
      deleteAction={deleteOfficeRequest}
      CardComponent={OfficeCard}
      entityName="Office"
    />
  );
}