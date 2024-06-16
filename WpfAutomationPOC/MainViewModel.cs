using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfAutomationPOC;

public class MainViewModel : ObservableObject
{
    private int _total;
    private int? _value;

    public MainViewModel()
    {
        Add = new AsyncRelayCommand(Execute, CanExecute);
    }

    public AsyncRelayCommand Add { get; }

    public int? Value
    {
        get => _value;
        set
        {
            SetProperty(ref _value, value);
            Add.NotifyCanExecuteChanged();
        }
    }

    public int Total
    {
        get => _total;
        set => SetProperty(ref _total, value);
    }

    private bool CanExecute() => Value is not null && Total < 69;

    private Task Execute()
    {
        if (CanExecute() is false)
            throw new InvalidOperationException("Cannot execute Add");

        var val = Value!.Value;
        var newTotal = Total + val;

        if (newTotal <= 69)
        {
            Total = newTotal;
            Value = null;
        }
        else
        {
            MessageBox.Show("Total cannot be larger than 69", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        if (newTotal == 69)
        {
            MessageBox.Show("Nice!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        return Task.CompletedTask;
    }

}
