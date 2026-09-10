namespace Metrica.Application.Common.Exceptions;

public sealed class NotFoundException(string resource, object key)
    : Exception($"No se encontró {resource} con identificador '{key}'.");
