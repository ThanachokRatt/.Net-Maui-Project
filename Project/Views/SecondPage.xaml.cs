
using Project.ViewModel;
namespace Project;

public partial class SecondPage : ContentPage
{
   
    public SecondPage()
    {
        InitializeComponent();
        

    }

    private async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        var mainPage = new MainPage();
        await Navigation.PushAsync(mainPage);
    }

    private async void Button_Clicked_1(System.Object sender, System.EventArgs e)
    {
 
        var thirdPage = new ThridPage();
        thirdPage.BindingContext = this.BindingContext;
        await Navigation.PushAsync(thirdPage);
    }
}
