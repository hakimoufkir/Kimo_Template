using System.ComponentModel.DataAnnotations;

namespace Kimo.ZLApp.Application.Common.Filtering.Requests;

/// <summary>
///     Definiert die Optionen für die Sortierung.
/// </summary>
public sealed record SortingOptions
{
    /// <summary>
    ///     Das Feld, worauf sich die Sortierung bezieht.
    /// </summary>
    [Required]
    [RegularExpression("^[A-Za-z0-9_]+$")]
    public required string Field { get; init; }

    /// <summary>
    ///     Die Sortierungsrichtung.
    /// </summary>
    [Required]
    [EnumDataType(typeof(SortingDirection))]
    public required SortingDirection Direction { get; init; }
}