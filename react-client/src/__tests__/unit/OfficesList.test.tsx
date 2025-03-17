import { render, screen } from "@testing-library/react";
import { Provider } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import { describe, it, expect } from "vitest";
import PaginatedList from "@shared/models/paginatedList";
import OfficeModel from "@shared/models/offices/officeModel";
import { configureStore, Store } from "@reduxjs/toolkit";
import { fetchOfficesSliceReducer } from "@shared/store/fetch-offices";
import { deleteOfficeSliceReducer } from "@features/offices-list/store/delete-office";
import { editOfficeSliceReducer } from "@features/edit-office-form/store/edit-office";
import { createOfficeSliceReducer } from "@features/create-office-form/store/create-office";
import { OfficesList } from "@features/offices-list";
import store from "@app/store";

const paginatedListWithOneOffice: PaginatedList<OfficeModel> = {
  hasNextPage: false,
  hasPreviousPage: false,
  items: [
    {
      id: "1",
      address: "Office 1 address",
      registryPhoneNumber: "1234567890",
      isActive: true,
    },
  ],
  pageIndex: 1,
  totalPages: 1,
};

const createMockStore = (fetchOfficesState: {
  loading: boolean;
  data?: PaginatedList<OfficeModel>;
  error?: string;
}) => {
  return configureStore({
    reducer: {
      fetchOffices: fetchOfficesSliceReducer,
      deleteOffice: deleteOfficeSliceReducer,
      editOffice: editOfficeSliceReducer,
      createOffice: createOfficeSliceReducer,
    },
    preloadedState: {
      fetchOffices: fetchOfficesState,
      createOffice: { loading: false },
      deleteOffice: { loading: false },
      editOffice: { loading: false },
    },
  });
};

const renderOfficesWithProviders = (store: Store) =>
  render(
    <Provider store={store}>
      <MemoryRouter>
        <OfficesList />
      </MemoryRouter>
    </Provider>
  );

describe("OfficesList", () => {
  it("renders", async () => {
    renderOfficesWithProviders(store);
    expect(screen.getByTestId("list")).toBeInTheDocument();
  });

  it("renders loading", async () => {
    const mockStore = createMockStore({
      loading: true,
      data: undefined,
      error: undefined,
    });

    renderOfficesWithProviders(mockStore);
    expect(screen.getByTestId("fetchLoading")).toBeInTheDocument();
  });

  it("renders error", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: undefined,
      error: "Error",
    });

    renderOfficesWithProviders(mockStore);
    expect(screen.getByTestId("fetchError")).toBeInTheDocument();
  });

  it("renders 1 active office and deactivate button is enabled", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithOneOffice,
      error: undefined,
    });

    renderOfficesWithProviders(mockStore);

    const listItems = await screen.findAllByTestId("office-card");
    expect(listItems.length).toBe(1);
    const disableButton = await screen.findByTestId("deactivate-button");
    expect(disableButton).not.toBeDisabled();
  });

  it("renders 1 office and edit button is enabled", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithOneOffice,
      error: undefined,
    });

    renderOfficesWithProviders(mockStore);

    const listItems = await screen.findAllByTestId("office-card");
    expect(listItems.length).toBe(1);
    const editButton = await screen.findByTestId("edit-button");
    expect(editButton).not.toBeDisabled();
  });

  it("renders 1 inactive office and deactivate button is disabled", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: {
        ...paginatedListWithOneOffice,
        items: [{ ...paginatedListWithOneOffice.items[0], isActive: false }],
      },
      error: undefined,
    });

    renderOfficesWithProviders(mockStore);

    const listItems = await screen.findAllByTestId("office-card");
    expect(listItems.length).toBe(1);
    const disableButton = await screen.findByTestId("deactivate-button");
    expect(disableButton).toBeDisabled();
  });
});
