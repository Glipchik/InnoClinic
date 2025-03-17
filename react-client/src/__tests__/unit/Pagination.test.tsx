import { render, screen } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";
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
      onPageChange={vi.fn()}
      pageIndex={pageIndex}
      totalPages={totalPages}
    />
  );

describe("Pagination", () => {
  it("renders", () => {
    renderPagination(false, false, 1, 1);

    expect(screen.getByTestId("pagination")).toBeInTheDocument();
  });

  it("pageIndex is correct", () => {
    renderPagination(true, true, 2, 3);

    expect(screen.getByTestId("paginationText")).toHaveTextContent("2 / 3");
  });

  it("buttons are enabled", async () => {
    renderPagination(true, true, 2, 3);
    expect(screen.getByTestId("hasNextPageButton")).toBeEnabled();
    expect(screen.getByTestId("hasPreviousPageButton")).toBeEnabled();
  });

  it("buttons are disabled", async () => {
    renderPagination(false, false, 2, 3);
    expect(screen.getByTestId("hasNextPageButton")).toBeDisabled();
    expect(screen.getByTestId("hasPreviousPageButton")).toBeDisabled();
  });
});
