import { fetchOfficesRequest } from "../store/fetch-offices";
import { deleteOfficeRequest } from "../store/delete-office";
import OfficeCard from "./office-card";
import { RootState } from "@app/store";
import { List } from "@widgets/list";

export function OfficesList() {
  return (
    <List
      fetchStateSelector={(state: RootState) => state.fetchOffices}
      deleteStateSelector={(state: RootState) => state.deleteOffice}
      fetchAction={fetchOfficesRequest}
      deleteAction={deleteOfficeRequest}
      CardComponent={OfficeCard}
      entityName="office"
    />
  );
}
