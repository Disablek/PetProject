namespace TaskFlow.Business.Exceptions;

/// <summary>
/// Исключение для случаев, когда ресурс не найден
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Исключение для случаев нарушения прав доступа
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
    public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Исключение для случаев некорректных данных запроса
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Исключение для бизнес-логики
/// </summary>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
    public BusinessException(string message, Exception innerException) : base(message, innerException) { }
}