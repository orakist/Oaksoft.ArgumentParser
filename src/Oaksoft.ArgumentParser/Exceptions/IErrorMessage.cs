﻿using System;

namespace Oaksoft.ArgumentParser.Exceptions;

public record ErrorInfo(string Code, string Format);

public interface IErrorMessage
{
    ErrorInfo Error { get; }

    string Message { get; }

    object[]? Values { get; }

    string? OptionName { get; }
}