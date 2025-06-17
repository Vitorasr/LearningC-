using Learning.Views;
namespace Learning;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnTeste1Clicked(Object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Teste1());
    }
}