namespace Kimo.ZLApp.Application.Common.Filtering.Responses;

/// <summary>
///     Das Datenobjekt für eine paginierte Datensammlung.
/// </summary>
/// <param name="Data">Die Datensätze.</param>
/// <param name="Page">Die aktuelle Seite.</param>
/// <param name="PageSize">Die Anzahl an Elementen der Seite.</param>
/// <param name="TotalRecords">Die Gesamtanzahl aller Elemente.</param>
/// <typeparam name="TData">Der Datentyp der einzelnen Datensätze.</typeparam>
public sealed record PaginatedData<TData>(ICollection<TData> Data, int Page, int PageSize, int TotalRecords);