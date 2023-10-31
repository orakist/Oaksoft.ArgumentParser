using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glowy.CLxParser.Options;

namespace Glowy.CLxParser.Parser;

public abstract class BaseApplicationOptions : IApplicationOptions
{
    public bool Help { get; set; }

    public List<IBaseOption> Options
        => _options.Values.ToList();

    private PropertyInfo[]? _propertyInfos;
    private List<string>? _registeredPropertyNames;
    private readonly Dictionary<string, IBaseOption> _options;

    protected BaseApplicationOptions()
    {
        _options = new Dictionary<string, IBaseOption>();
    }

    internal void ClearOptions()
    {
        _registeredPropertyNames ??= GetRegisteredPropertyNames();

        _propertyInfos ??= GetType().GetProperties()
            .Where(p => _registeredPropertyNames.Contains(p.Name))
            .ToArray();

        foreach (var option in _options.Values.Cast<BaseOption>())
        {
            option.Clear();
        }

        foreach (var property in _propertyInfos)
        {
            var type = property.PropertyType;
            if (_options.TryGetValue(property.Name, out var option) && 
                option is IHaveValueOption valOption && !string.IsNullOrWhiteSpace(valOption.DefaultValue))
            {
                // all scalar-command and non-command options may have a default option.
                // so apply default option to registered property if it exists.
                if (type.IsAssignableFrom(typeof(string)))
                {
                    property.SetValue(this, valOption.DefaultValue);
                    continue;
                }

                if (option is ScalarCommandOption scalarOption)
                {
                    scalarOption.ApplyDefaultValue(this, property);
                    continue;
                }
            }

            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            property.SetValue(this, defaultValue);
        }
    }

    internal void UpdateProperties(IBaseOption option)
        {
            if (option.ValidatedTokenCount < 1)
                return;

            var baseOption = (option as BaseOption)!;
            if (!string.IsNullOrWhiteSpace(baseOption.CountProperty))
            {
                var countProp = _propertyInfos!.First(p => p.Name == baseOption.CountProperty);
                if (countProp.PropertyType == typeof(bool))
                {
                    countProp.SetValue(this, true);
                }
                else if (countProp.PropertyType == typeof(int))
                {
                    countProp.SetValue(this, option.ValidatedTokenCount);
                }
            }

            var keyProp = _propertyInfos!.First(p => p.Name == baseOption.KeyProperty);
            var type = keyProp.PropertyType;
            if (option is SwitchOption)
            {
                if (type == typeof(bool))
                {
                    keyProp.SetValue(this, true);
                }
                else if (type == typeof(int))
                {
                    keyProp.SetValue(this, option.ValidatedTokenCount);
                }
            }
            else if (option is IHaveValueOption valOption)
            {
                if (type.IsAssignableFrom(typeof(List<string>)))
                {
                    keyProp.SetValue(this, valOption.ParsedValues);
                }
                else if (type.IsAssignableFrom(typeof(string[])))
                {
                    keyProp.SetValue(this, valOption.ParsedValues.ToArray());
                }
                else if (type.IsAssignableFrom(typeof(string)))
                {
                    keyProp.SetValue(this, valOption.ParsedValues.First());
                }
                else if (option is ScalarCommandOption scalarOption)
                {
                    scalarOption.UpdatePropertyValue(this, keyProp);
                }
            }
        }

        private List<string> GetRegisteredPropertyNames()
        {
            var propertyNames = _options.Values.OfType<BaseOption>()
                .Where(o => !string.IsNullOrWhiteSpace(o.CountProperty))
                .Select(a => a.CountProperty!)
                .ToList();

            propertyNames.AddRange(_options.Keys);

            return propertyNames;
        }
    }
