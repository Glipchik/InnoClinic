import { AxiosError } from "axios";

export type ApiError = AxiosError<{
  detail?: string;
  title?: string;
}>;

const handleError = (error: ApiError) => {
  const defaultError = "An unknown error occurred"
  if ((error as ApiError).response && (error as ApiError).response?.data) {
    const problemDetails = (error as ApiError).response!.data;
    return problemDetails.detail || problemDetails.title || defaultError;
  }
  return defaultError
}

export default handleError