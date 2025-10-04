export const ApiMethod = {
  GET: "GET",
  POST: "POST",
  PUT: "PUT",
  DELETE: "DELETE",
  PATCH: "PATCH",
} as const;

export type ApiMethodType = (typeof ApiMethod)[keyof typeof ApiMethod];
