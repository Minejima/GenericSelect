using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using System.ComponentModel;

namespace GenericSelect.Components;

public partial class GenericSelect<TKey>
{
    private IEnumerable<Option<string?>> _items = null!;

    private IEnumerable<GenericOption<TKey>> _options = null!;

    [Parameter]
    public IEnumerable<GenericOption<TKey>> Options
    {
        get => _options;
        set
        {
            _options = value;
            if (_options is not null)
                _items = _options.Select(x => x.ToOption()).ToList();
        }
    }

    private string? _value
    {
        get => Value?.ToString();
        set
        {
            TKey? v = default(TKey?);
            if (TryParse(value, ref v))
            {
                ValueChanged.InvokeAsync(v);
                Value = v;
            }
            else
            {
                ValueChanged.InvokeAsync(default(TKey?));
                Value = default(TKey?);
            }
        }
    }

    [Parameter]
    public TKey? Value { get; set; }

    [Parameter]
    public EventCallback<TKey?> ValueChanged { get; set; }


    private bool TryParse<T>(string? input, ref T? result)
    {
        try
        {
            if (input == null) return false;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter is not null)
            {
                var co = converter.ConvertFromString(input);
                if (co is not null)
                {
                    result = (T)co;
                    return true;
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}

public class GenericOption<TKey>
{
    public TKey? Key { get; set; }
    public string? Value { get; set; }
    public bool Disabled { get; set; } = false;
    public bool Selected { get; set; } = false;

    public Option<string?> ToOption()
    {
        var option = new Option<string?> { Key = Key?.ToString(), Value = Value, Disabled = Disabled };
        return option;
    }
}

