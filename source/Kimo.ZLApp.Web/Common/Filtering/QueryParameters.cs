namespace Kimo.ZLApp.Web.Common.Filtering;

/// <summary>
///     Definiert die Sammlung aller Query-Parameter.
/// </summary>
public sealed record QueryParameters
{
    /// <summary>
    ///     Die Anzahl an Elementen die für die Paginierung übersprungen werden sollen.
    /// </summary>
    public required int Skip { get; init; }

    /// <summary>
    ///     Die Anzahl an Elementen die für die Paginierung genommen werden sollen.
    /// </summary>
    public required int Take { get; init; }

    /// <summary>
    ///     Das Feld anhand welchem sortiert werden soll.
    /// </summary>
    public required string Sort { get; init; }

    /// <summary>
    ///     Die Richtung in welche sortiert werden soll.
    /// </summary>
    public required string Order { get; init; }

    /// <summary>
    ///     Key-Value-Paare zum Filtern von Werten.
    ///     Beispiel: ["name"] = "Toni" : Filtert nach dem Namen mit dem Wert "Toni".
    /// </summary>
    public IDictionary<string, string?>? Filters { get; init; }
}