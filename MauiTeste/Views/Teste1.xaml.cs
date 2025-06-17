using System.ComponentModel;

namespace Learning.Views;

public partial class Teste1 : ContentPage
{
    public Teste1()
    {
        InitializeComponent();
        BindingContext = new BindingClass();
    }
}

public class BindingClass : INotifyPropertyChanged
{
    private double _volume;
    public double Volume
    {
        get => _volume;
        set
        {
            if (_volume != value)
            {
                _volume = value;
                OnPropertyChanged(nameof(Volume));
                OnPropertyChanged(nameof(CombinedText));
                OnPropertyChanged(nameof(Multiply));
            }
        }
    }

    private string _entryText;
    public string EntryText
    {
        get => _entryText;
        set
        {
            if (_entryText != value)
            {
                _entryText = value;
                OnPropertyChanged(nameof(EntryText));
                OnPropertyChanged(nameof(CombinedText));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public string CombinedText => $"{EntryText}:{Volume:F0}";
    public string Multiply => $"O Dobro do Digito é:{Volume * 2:0.00}";
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public BindingClass()
    {
        _entryText = string.Empty;
        _volume = 0.0;
    }
}