export default interface PaginatedList<T> {
  items: T[],
  pageIndex: number,
  totalPages: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean
}