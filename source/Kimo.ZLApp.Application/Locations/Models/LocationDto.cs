namespace Kimo.ZLApp.Application.Locations.Models;

/// <summary>
///     Das Datenobjekt für einen Standort.
/// </summary>
/// <param name="Id">Die ID des Standorts.</param>
/// <param name="Name">Der Name des Standorts.</param>
public sealed record LocationDto(int Id, string Name);