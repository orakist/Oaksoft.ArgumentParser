using System;
using System.Collections.Generic;

namespace Oaksoft.ArgumentParser.Callbacks;

public interface IParsingCallbacks<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    List<TValue> TryParseValues(List<string> values);

    bool TryParseValue(string value, out TValue result);
}

/// <summary>Encapsulates a method to parse string values that has two parameters and returns boolean result.</summary>
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
/// <typeparam name="TValue">The type of the parsed value.</typeparam>
public delegate bool TryParse<TValue>(string arg1, out TValue arg2);
