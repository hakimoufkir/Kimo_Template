using System.ComponentModel.DataAnnotations;

namespace Kimo.ZLApp.Application.Common.Filtering.Requests;

/// <summary>
///     Definiert eine einzelne Filteroption.
/// </summary>
public sealed record FilterOption
{
    /// <summary>
    ///     Das Feld, auf welches sich der Filter bezieht.
    /// </summary>
    [Required]
    [RegularExpression("^[A-Za-z0-9_]+$")]
    public required string Field { get; init; }

    /// <summary>
    ///     Der Wert, anhand welchen gefiltert werden soll.
    /// </summary>
    public string? Value { get; init; }
}