import {
  Matcher,
  MatcherOptions,
  render,
  waitFor,
} from "@testing-library/react";
import { Provider, useDispatch } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import { Store } from "redux";
import { OfficesPage } from "..";
import { createMockStore, paginatedListWithOneOffice } from "./__mocks__";
import userEvent from "@testing-library/user-event";

jest.mock("react-redux", () => ({
  ...jest.requireActual("react-redux"),
  useDispatch: jest.fn(),
}));

const renderOfficesPageWithProviders = (store: Store) =>
  render(
    <Provider store={store}>
      <MemoryRouter>
        <OfficesPage />
      </MemoryRouter>
    </Provider>
  );

describe("OfficesPage", () => {
  let mockDispatch: jest.Mock;

  beforeEach(() => {
    mockDispatch = jest.fn();
    (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  });

  it("renders", async () => {
    const { getByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOneOffice,
      })
    );

    await waitFor(() => {
      expect(getByTestId("offices-page-header")).toBeInTheDocument();
      expect(getByTestId("create-button")).toBeInTheDocument();
      expect(getByTestId("office-card")).toBeInTheDocument();
    });
  });

  it("renders loading", async () => {
    const { getByTestId, queryByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: true,
      })
    );

    await waitFor(() => {
      expect(getByTestId("offices-page-header")).toBeInTheDocument();
      expect(getByTestId("create-button")).toBeInTheDocument();
      expect(queryByTestId("office-card")).not.toBeInTheDocument();
      expect(getByTestId("fetch-loading")).toBeInTheDocument();
    });
  });

  it("renders error", async () => {
    const { getByTestId, queryByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        error: "Error",
      })
    );

    await waitFor(() => {
      expect(getByTestId("offices-page-header")).toBeInTheDocument();
      expect(getByTestId("create-button")).toBeInTheDocument();
      expect(queryByTestId("office-card")).not.toBeInTheDocument();
      expect(getByTestId("fetch-error")).toBeInTheDocument();
    });
  });

  it("renders confirmation modal when deactivating office", async () => {
    const { getByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOneOffice,
      })
    );

    await clickDeactivateButton(getByTestId);
    expect(getByTestId("modal")).toBeInTheDocument();
  });

  it("calls delete handler when confirm deactivating is clicked", async () => {
    const { getByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOneOffice,
      })
    );

    await clickDeactivateButton(getByTestId);
    const confirmButton = getByTestId("confirm-button");
    await userEvent.click(confirmButton);

    await waitFor(() => {
      expect(mockDispatch).toHaveBeenCalledWith({
        type: "DeleteOfficeSlice/deleteOfficeRequest",
        payload: { id: "1" },
      });
    });
  });

  it("renders create form when clicked", async () => {
    const { getByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOneOffice,
      })
    );

    await openCreateOfficeForm(getByTestId);
    expect(getByTestId("create-office-form")).toBeInTheDocument();
  });

  it("closes create form when clicked cancel", async () => {
    const { getByTestId, queryByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOneOffice,
      })
    );

    await openCreateOfficeForm(getByTestId);
    expect(getByTestId("create-office-form")).toBeInTheDocument();
    const cancelButton = getByTestId("cancel-button");
    await userEvent.click(cancelButton);
    await waitFor(() => {
      expect(queryByTestId("create-office-form")).not.toBeInTheDocument();
    });
  });

  it("pagination is right", async () => {
    const { getByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOneOffice,
      })
    );

    expect(getByTestId("pagination")).toBeInTheDocument();
    expect(getByTestId("has-previous-page-button")).toBeEnabled();
    expect(getByTestId("has-next-page-button")).toBeEnabled();
    expect(getByTestId("pagination-text")).toHaveTextContent("2 / 3");
  });

  it("next page in pagination works right", async () => {
    const { getByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOneOffice,
      })
    );

    const nextPageButton = getByTestId("has-next-page-button");
    await userEvent.click(nextPageButton);
    await waitFor(() => {
      expect(mockDispatch).toHaveBeenCalledWith({
        type: "FetchOfficesSlice/fetchOfficesRequest",
        payload: { pageIndex: 3 },
      });
    });
  });

  it("previous page in pagination works right", async () => {
    const { getByTestId } = renderOfficesPageWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOneOffice,
      })
    );

    const previousPageButton = getByTestId("has-previous-page-button");
    await userEvent.click(previousPageButton);
    await waitFor(() => {
      expect(mockDispatch).toHaveBeenCalledWith({
        type: "FetchOfficesSlice/fetchOfficesRequest",
        payload: { pageIndex: 1 },
      });
    });
  });
});

async function openCreateOfficeForm(
  getByTestId: (
    id: Matcher,
    options?: MatcherOptions | undefined
  ) => HTMLElement
) {
  const officeCard = getByTestId("office-card");
  expect(officeCard).toBeInTheDocument();
  const createButton = getByTestId("create-button");
  await userEvent.click(createButton);
}

async function clickDeactivateButton(
  getByTestId: (
    id: Matcher,
    options?: MatcherOptions | undefined
  ) => HTMLElement
) {
  const officeCard = getByTestId("office-card");
  expect(officeCard).toBeInTheDocument();
  const deactivateButton = getByTestId("deactivate-button");
  await userEvent.click(deactivateButton);
}
