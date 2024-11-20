﻿namespace DesignPatterns.Strategy.Shared.Results;

public record Error
{
    public static Error None => new Error(string.Empty, string.Empty, ErrorType.Failure);
    public static Error NotNull => new Error("Error.NullValue", "Null value was provided", ErrorType.Failure);

    private Error(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        Type = errorType;
    }

    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }

    public static Error Failure(string code, string description) => new Error(code, description, ErrorType.Failure);
    public static Error Validation(string code, string description) => new Error(code, description, ErrorType.Validation);
    public static Error NotFound(string code, string description) => new Error(code, description, ErrorType.NotFound);
    public static Error Conflict(string code, string description) => new Error(code, description, ErrorType.Conflict);
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3
}