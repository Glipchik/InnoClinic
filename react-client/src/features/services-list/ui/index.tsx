import { fetchServicesWithPaginationRequest } from "@shared/store/fetch-services-with-pagination";
import { deleteServiceRequest } from "../store/delete-service";
import ServiceCard from "./service-card";
import { RootState } from "@app/store";
import { List } from "@widgets/list";

export const ServicesList = () => {
  return (
    <List
      fetchStateSelector={(state: RootState) => state.fetchServicesWithPagination}
      deleteStateSelector={(state: RootState) => state.deleteService}
      fetchAction={fetchServicesWithPaginationRequest}
      deleteAction={deleteServiceRequest}
      CardComponent={ServiceCard}
      entityName="service"
    />
  );
}
