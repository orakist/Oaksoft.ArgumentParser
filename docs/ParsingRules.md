## Parsing Rules

### 1. Option Alias Prefix Rules

Prefix of an alias can be two hyphens (--), one hyphen (-) or forward slash (/). 
These prefixes are configurable. There are 5 types of alias prefix configuration.

This is the Option Alias Prefix Rules enumaration:

```cs
[Flags]
public enum OptionPrefixRules
{
    AllowSingleDash = 0x01,
    AllowSingleDashShortAlias = 0x02,
    AllowDoubleDash = 0x04,
    AllowDoubleDashLongAlias = 0x08,
    AllowForwardSlash = 0x10,
    Default = AllowSingleDashShortAlias | AllowDoubleDashLongAlias | AllowForwardSlash,
    All = Default | AllowDoubleDash | AllowSingleDash
}
```

1. Allow single dash (-) for all aliases</br>
   Valid alias: -o, -start
2. Allow single dash (-) for only short aliases</br>
   Valid alias: -o
3. Allow double dash (--) for all aliases</br>
   Valid alias: --o, --start
4. Allow double dash (--) for only long aliases</br>
   Valid alias: --start
5. Allow forvard slash (/)  for all aliases</br>
   Valid alias: /o /start

If you want to allow only short aliases with single dash. You can configure your parser to allow only short aliases with the code below. Then parser allows only -v and -c aliases.

```cs
var parser = CommandLine.CreateParser<MyOptions>(optionPrefix: OptionPrefixRules.AllowSingleDashShortAlias)
    .AddNamedOption(s => s.Value)
    .AddNamedOption(s => s.Count)
    .Build();
```

Default alias prefix rules of the Oaksoft.ArgumentParser are 2, 4 and 5. The following examples shows valid commands for the default rule:

```
./> myapp --open file.txt --read 100 --verbosity quiet
./> myapp /open file.txt /read 100 /verbosity quiet
./> myapp -o file.txt -r 100 -v quiet
./> myapp /o file.txt /r 100 /v quiet
./> myapp --open file.txt -r 100 /verbosity quiet
```

First command is parsed by the library into these named options: (--open file.txt), (--read 10), (--verbosity quiet). 
