namespace Oaksoft.ArgumentParser.Callbacks;

/// <summary>Encapsulates a method to parse string values that has two parameters and returns boolean result.</summary>
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
/// <typeparam name="TValue">The type of the parsed value.</typeparam>
public delegate bool TryParse<TValue>(string arg1, out TValue arg2);
