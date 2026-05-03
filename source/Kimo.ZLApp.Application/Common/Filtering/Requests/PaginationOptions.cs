using System.ComponentModel.DataAnnotations;

namespace Kimo.ZLApp.Application.Common.Filtering.Requests;

/// <summary>
///     Definiert die Optionen für die Paginierung.
/// </summary>
public sealed record PaginationOptions
{
    /// <summary>
    ///     Die Anzahl an Elementen, die übersprungen werden soll.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int Skip { get; init; }

    /// <summary>
    ///     Die Anzahl an Elementen, die maximal retourniert werden darf.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int Take { get; init; }
}