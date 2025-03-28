import { render, waitFor } from "@testing-library/react";
import { Pagination } from "@widgets/pagination";
// s
const renderPagination = (
  hasNextPage: boolean,
  hasPreviousPage: boolean,
  pageIndex: number,
  totalPages: number
) =>
  render(
    <Pagination
      hasNextPage={hasNextPage}
      hasPreviousPage={hasPreviousPage}
      onPageChange={jest.fn()}
      pageIndex={pageIndex}
      totalPages={totalPages}
    />
  );

describe("Pagination", () => {
  it("renders", async () => {
    const { queryByTestId } = renderPagination(false, false, 1, 1);

    await waitFor(() => {
      expect(queryByTestId("pagination-text")).toBeInTheDocument();
      expect(queryByTestId("has-next-page-button")).toBeInTheDocument();
      expect(queryByTestId("has-previous-page-button")).toBeInTheDocument();
    });
  });

  it("pageIndex is correct", () => {
    const { getByTestId } = renderPagination(true, true, 2, 3);

    expect(getByTestId("pagination-text")).toHaveTextContent("2 / 3");
  });

  it("buttons are enabled when there are next and previous pages", async () => {
    const { getByTestId } = renderPagination(true, true, 2, 3);

    await waitFor(() => {
      expect(getByTestId("has-next-page-button")).toBeEnabled();
      expect(getByTestId("has-previous-page-button")).toBeEnabled();
    });
  });

  it("buttons are disabled when no next and previous pages", async () => {
    const { getByTestId } = renderPagination(false, false, 2, 3);

    await waitFor(() => {
      expect(getByTestId("has-next-page-button")).toBeDisabled();
      expect(getByTestId("has-previous-page-button")).toBeDisabled();
    });
  });

  it("matches snapshot", () => {
    const { asFragment } = renderPagination(true, true, 2, 3);
    expect(asFragment()).toMatchSnapshot();
  });
});
