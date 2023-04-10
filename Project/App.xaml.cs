namespace Project;
using System.IO;
public partial class App : Application
{
    public static string FolderPath { get; set; }
	public App()
	{
		InitializeComponent();
        FolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
       
        MainPage = new AppShell();
        Shell.SetNavBarIsVisible(MainPage, false);

    }
}

