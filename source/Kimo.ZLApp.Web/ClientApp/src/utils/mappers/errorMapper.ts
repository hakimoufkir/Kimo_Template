import axios from "axios";

export function mapToError(e: unknown): Error {
  if (axios.isAxiosError(e)) return e;
  if (e instanceof Error) return e;
  return new Error("Unknown error");
}

export const ErrorMapper = {
  mapToError
};
