using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SqlServerDiagramManager;

public class NotifyPropertyChangedBase : INotifyPropertyChanged
{
    //The interface only includes this evennt
    public event PropertyChangedEventHandler? PropertyChanged;

    //Common implementations of SetProperty
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        bool propertyChanged = false;

        //If we have a different value, do stuff
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            OnPropertyChanged(name);
            propertyChanged = true;
        }

        return propertyChanged;
    }

    //The C#6 version of the common implementation
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
