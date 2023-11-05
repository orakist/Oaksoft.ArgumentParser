using System;
using System.Reflection;

namespace Oaksoft.ArgumentParser.Callbacks;

internal sealed class DefaultParsingCallbacks<TValue> : BaseParsingCallbacks<TValue>
    where TValue : IComparable, IEquatable<TValue>
{
    public static readonly DefaultParsingCallbacks<TValue> Instance = new();

    public bool IsValidParser { get; }

    private readonly bool _isStringType;
    private readonly MethodInfo? _convertValueMethod;
    private readonly MethodInfo? _validateValueMethod;
    
    private DefaultParsingCallbacks()
    {
        var type = typeof(TValue);

        if (type == typeof(string))
            _isStringType = true;

        if (!_isStringType)
        {
            _convertValueMethod = type.GetMethod("Parse",
                BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { typeof(string) }, default);
            if (_convertValueMethod == null)
                return;

            _validateValueMethod = type.GetMethod("TryParse",
                BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { typeof(string), type.MakeByRefType() }, default);
            if (_validateValueMethod == null)
                return;
        }
        
        IsValidParser = true;
    }

    public override bool ValidateValue(string value)
    {
        if (_isStringType)
            return true;

        var parameters = new object[] { value, default! };
        var result = _validateValueMethod?.Invoke(null, parameters);
        return result is not null && (bool)result;
    }

    public override TValue ConvertValue(string value)
    {
        if (_isStringType)
            return (TValue)(object)value;

        var parameter = new object[] { value };
        var result = _convertValueMethod?.Invoke(null, parameter);
        return result is null ? default! : (TValue)result;
    }
}
