using System.ComponentModel.DataAnnotations;

namespace Kimo.ZLApp.Application.Common.Filtering.Requests;

/// <summary>
///     Definiert die Optionen für die Filterung.
/// </summary>
public sealed record FilterOptions
{
    /// <summary>
    ///     Die Liste aller einzelnen Filter.
    /// </summary>
    [Required]
    public required ICollection<FilterOption> Filters { get; init; }
}