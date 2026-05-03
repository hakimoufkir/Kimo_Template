export function formatDate(date: string | Date): string {
  return new Date(date).toLocaleDateString("de-DE", {
    day: "2-digit",
    month: "2-digit",
    year: "numeric"
  });
}

export function normalizeDateOnly(date: string | Date): string {
  const parsedDate = typeof date === "string" ? new Date(date) : date;

  const yyyy = parsedDate.getFullYear();
  const mm = String(parsedDate.getMonth() + 1).padStart(2, "0");
  const dd = String(parsedDate.getDate()).padStart(2, "0");

  return `${yyyy}-${mm}-${dd}`;
}
