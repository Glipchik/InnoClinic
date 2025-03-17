import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import { Provider, useDispatch } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import { describe, it, expect, vi, Mock, beforeEach } from "vitest";
import PaginatedList from "@shared/models/paginatedList";
import OfficeModel from "@shared/models/offices/officeModel";
import { configureStore, Store } from "@reduxjs/toolkit";
import store from "@app/store";
import { OfficesPage } from "@pages/offices";
import { createOfficeSliceReducer } from "@features/create-office-form/store/create-office";
import { editOfficeSliceReducer } from "@features/edit-office-form/store/edit-office";
import { deleteOfficeSliceReducer } from "@features/offices-list/store/delete-office";
import { fetchOfficesSliceReducer } from "@shared/store/fetch-offices";

vi.mock("react-redux", async () => {
  const actual = await vi.importActual("react-redux");
  return {
    ...actual,
    useDispatch: vi.fn(),
  };
});

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

const paginatedListWithManyOffices: PaginatedList<OfficeModel> = {
  hasNextPage: true,
  hasPreviousPage: false,
  items: [
    {
      id: "1",
      address: "Office 1 address",
      registryPhoneNumber: "1234567890",
      isActive: true,
    },
    {
      id: "2",
      address: "Office 2 address",
      registryPhoneNumber: "1234567890",
      isActive: true,
    },
  ],
  pageIndex: 1,
  totalPages: 2,
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

const renderOfficesPageWithProviders = (store: Store) =>
  render(
    <Provider store={store}>
      <MemoryRouter>
        <OfficesPage />
      </MemoryRouter>
    </Provider>
  );

describe("OfficesPage", () => {
  const mockDispatch = vi.fn();

  beforeEach(() => {
    (useDispatch as Mock).mockReturnValue(mockDispatch);
  });

  it("renders", async () => {
    renderOfficesPageWithProviders(store);
    expect(screen.getByTestId("offices-page-header")).toBeInTheDocument();
  });

  it("renders office", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithOneOffice,
      error: undefined,
    });
    renderOfficesPageWithProviders(mockStore);

    await waitFor(() => {
      expect(screen.getByTestId("office-card")).toBeInTheDocument();
    });
  });

  it("renders many offices", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithManyOffices,
      error: undefined,
    });
    renderOfficesPageWithProviders(mockStore);

    const officeCards = await screen.findAllByTestId("office-card");
    expect(officeCards.length).toBe(2);
  });

  it("renders confirmation modal when deactivating office", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithOneOffice,
      error: undefined,
    });
    renderOfficesPageWithProviders(mockStore);

    const officeCard = await screen.findByTestId("office-card");
    expect(officeCard).toBeInTheDocument();
    const deactivateButton = screen.getByTestId("deactivate-button");
    fireEvent.click(deactivateButton);
    expect(await screen.findByTestId("modal")).toBeInTheDocument();
  });

  it("dispatches fetch action when confirm deactivating is clicked", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithOneOffice,
      error: undefined,
    });
    renderOfficesPageWithProviders(mockStore);

    const officeCard = await screen.findByTestId("office-card");
    expect(officeCard).toBeInTheDocument();
    const deactivateButton = screen.getByTestId("deactivate-button");
    fireEvent.click(deactivateButton);
    expect(await screen.findByTestId("modal")).toBeInTheDocument();
    const confirmButton = screen.getByTestId("confirm-button");
    fireEvent.click(confirmButton);

    await waitFor(() => {
      expect(mockDispatch).toHaveBeenCalledWith({
        payload: {
          id: "1",
        },
        type: "DeleteOfficeSlice/deleteOfficeRequest",
      });
    });
  });

  it("renders create form when clicked", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithOneOffice,
      error: undefined,
    });
    renderOfficesPageWithProviders(mockStore);

    const officeCard = await screen.findByTestId("office-card");
    expect(officeCard).toBeInTheDocument();
    const createButton = screen.getByTestId("create-button");
    fireEvent.click(createButton);
    expect(await screen.findByTestId("create-office-form")).toBeInTheDocument();
  });

  it("closes create form when clicked cancel", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithOneOffice,
      error: undefined,
    });
    renderOfficesPageWithProviders(mockStore);

    const officeCard = await screen.findByTestId("office-card");
    expect(officeCard).toBeInTheDocument();
    const createButton = screen.getByTestId("create-button");
    fireEvent.click(createButton);
    expect(await screen.findByTestId("create-office-form")).toBeInTheDocument();
    const cancelButton = screen.getByTestId("cancel-button");
    fireEvent.click(cancelButton);
    await waitFor(() => {
      expect(
        screen.queryByTestId("create-office-form")
      ).not.toBeInTheDocument();
    });
  });

  it("paginations data is right", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithManyOffices,
      error: undefined,
    });
    renderOfficesPageWithProviders(mockStore);

    const officeCard = await screen.findAllByTestId("office-card");
    expect(officeCard.length).toBe(2);
    const pagination = screen.getByTestId("pagination");
    expect(pagination).toBeInTheDocument();
    expect(screen.getByTestId("paginationText")).toHaveTextContent("1 / 2");
    expect(screen.getByTestId("hasNextPageButton")).toBeEnabled();
    expect(screen.getByTestId("hasPreviousPageButton")).toBeDisabled();
  });

  it("dispatches fetch action when next page is clicked", async () => {
    const mockStore = createMockStore({
      loading: false,
      data: paginatedListWithManyOffices,
      error: undefined,
    });

    renderOfficesPageWithProviders(mockStore);

    const nextPageButton = screen.getByTestId("hasNextPageButton");
    expect(nextPageButton).toBeInTheDocument();

    fireEvent.click(nextPageButton);

    await waitFor(() => {
      expect(mockDispatch).toHaveBeenCalledWith({
        payload: {
          pageIndex: 2,
          pageSize: import.meta.env.VITE_PAGE_SIZE,
        },
        type: "FetchOfficesSlice/fetchOfficesRequest",
      });
    });
  });
});
