
using System.IO;
using System.Collections.ObjectModel;
using Project.ViewModel;
using Project.Model;

namespace Project
{
    public partial class ThridPage : ContentPage
    {
        ItemVM vm;
        public ThridPage()
        {
            InitializeComponent();
            vm = new ItemVM();
            this.BindingContext = vm;
            listView.BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.Items.Clear();
           

            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");
            

            files = files.OrderByDescending(x => File.GetCreationTime(x));
            
                foreach (var filename in files)
                {
                    var item = new Item
                    
                    {
                        Filename = filename,
                        Date = File.GetCreationTime(filename),
                        editorText = File.ReadAllText(filename),
                        ImagePaths = new List<byte[]>(),
                        imageList = null
                       
                    };
                    vm.Items.Add(item);
                }
        }
        

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var mainPage = new MainPage();

            await Navigation.PushAsync(mainPage);
        }

        async void listView_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
        {


            {
                if (e.SelectedItem == null)
                    return;

                var selectedNote = e.SelectedItem as Item;

                var fourthPage = new FourthPage();
                fourthPage.BindingContext = selectedNote; // Pass the selected Item instance to the fourth page

                await Navigation.PushAsync(fourthPage);

                listView.SelectedItem = null;
            }

        }
    }
}
