import { render, waitFor } from "@testing-library/react";
import { Pagination } from "@widgets/pagination";

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
      expect(queryByTestId("paginationText")).toBeInTheDocument();
      expect(queryByTestId("hasNextPageButton")).toBeInTheDocument();
      expect(queryByTestId("hasPreviousPageButton")).toBeInTheDocument();
    });
  });

  it("pageIndex is correct", () => {
    const { getByTestId } = renderPagination(true, true, 2, 3);

    expect(getByTestId("paginationText")).toHaveTextContent("2 / 3");
  });

  it("buttons are enabled when there are next and previous pages", async () => {
    const { getByTestId } = renderPagination(true, true, 2, 3);

    await waitFor(() => {
      expect(getByTestId("hasNextPageButton")).toBeEnabled();
      expect(getByTestId("hasPreviousPageButton")).toBeEnabled();
    });
  });

  it("buttons are disabled when no next and previous pages", async () => {
    const { getByTestId } = renderPagination(false, false, 2, 3);

    await waitFor(() => {
      expect(getByTestId("hasNextPageButton")).toBeDisabled();
      expect(getByTestId("hasPreviousPageButton")).toBeDisabled();
    });
  });
});
