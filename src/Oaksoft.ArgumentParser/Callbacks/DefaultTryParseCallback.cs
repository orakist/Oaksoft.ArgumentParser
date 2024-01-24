using System;
using System.Reflection;

namespace Oaksoft.ArgumentParser.Callbacks;

internal sealed class DefaultTryParseCallback<TValue>
{
    public static readonly DefaultTryParseCallback<TValue> Instance = new();

    public bool IsValidParser { get; }

    private readonly bool _isEnumType;
    private readonly bool _isStringType;
    private readonly MethodInfo? _tryParseValueMethod;

    private DefaultTryParseCallback()
    {
        var type = typeof(TValue);

        if (type == typeof(string))
        {
            _isStringType = true;
        }
        else if (type.IsEnum)
        {
            _isEnumType = true;
        }

        if (!_isStringType && !_isEnumType)
        {
            _tryParseValueMethod = type.GetMethod("TryParse",
                BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new[] { typeof(string), type.MakeByRefType() }, default);
            if (_tryParseValueMethod == null)
            {
                return;
            }
        }
        
        IsValidParser = true;
    }

    public bool TryParse(string value, out TValue result)
    {
        if (_isStringType)
        {
            result = (TValue)(object)value;
            return true;
        }

        if (_isEnumType)
        {
            var validEnum = Enum.TryParse(typeof(TValue), value, true, out var enumValue);
            result = validEnum ? (TValue)enumValue! : default!;
            return validEnum;
        }

        var parameter = new object[] { value, default! };
        var success = _tryParseValueMethod?.Invoke(null, parameter);

        result = (TValue)parameter[1];
        return success is true;
    }
}
