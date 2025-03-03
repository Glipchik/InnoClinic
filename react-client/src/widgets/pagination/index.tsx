import Button from "@shared/ui/controls/Button";

interface PaginationProps {
  pageIndex: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  onPageChange: (newPage: number) => void;
}

export function Pagination({ pageIndex, totalPages, hasPreviousPage, hasNextPage, onPageChange }: PaginationProps) {
  return (
    <div className="mt-4 flex justify-center space-x-4">
      <Button onClick={() => onPageChange(pageIndex - 1)} disabled={!hasPreviousPage}>
        Back
      </Button>
      <span className="px-4 py-2">{`${pageIndex} / ${totalPages}`}</span>
      <Button onClick={() => onPageChange(pageIndex + 1)} disabled={!hasNextPage}>
        Next
      </Button>
    </div>
  );
};