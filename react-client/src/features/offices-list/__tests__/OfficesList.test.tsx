import {
  Matcher,
  MatcherOptions,
  render,
  waitFor,
} from "@testing-library/react";
import { Provider } from "react-redux";
import { MemoryRouter } from "react-router-dom";
import { Store } from "@reduxjs/toolkit";
import { OfficesList } from "@features/offices-list";
import { createMockStore, paginatedListWithOffice } from "./__mocks__";

const renderOfficesListWithProviders = (store: Store) =>
  render(
    <Provider store={store}>
      <MemoryRouter>
        <OfficesList />
      </MemoryRouter>
    </Provider>
  );

describe("OfficesList", () => {
  it("renders", async () => {
    const { queryByTestId } = renderOfficesListWithProviders(
      createMockStore({
        loading: false,
        data: paginatedListWithOffice,
      })
    );

    await waitFor(() => {
      expect(queryByTestId("office-card")).toBeInTheDocument();
      expect(queryByTestId("edit-button")).toBeInTheDocument();
      expect(queryByTestId("deactivate-button")).toBeInTheDocument();
      expect(queryByTestId("pagination")).toBeInTheDocument();
    });
  });

  it("renders loading", async () => {
    const { queryByTestId } = renderOfficesListWithProviders(
      createMockStore({
        loading: true,
      })
    );

    await waitFor(() => {
      expect(queryByTestId("fetch-loading")).toBeInTheDocument();
      expectNoOfficeCard(queryByTestId);
    });
  });

  it("renders error", async () => {
    const { queryByTestId } = renderOfficesListWithProviders(
      createMockStore({
        loading: false,
        error: "Error",
      })
    );

    await waitFor(() => {
      expect(queryByTestId("fetch-error")).toBeInTheDocument();
      expectNoOfficeCard(queryByTestId);
    });
  });
});

function expectNoOfficeCard(
  queryByTestId: (
    id: Matcher,
    options?: MatcherOptions | undefined
  ) => HTMLElement | null
) {
  expect(queryByTestId("office-card")).not.toBeInTheDocument();
  expect(queryByTestId("deactivate-button")).not.toBeInTheDocument();
  expect(queryByTestId("pagination")).not.toBeInTheDocument();
}
