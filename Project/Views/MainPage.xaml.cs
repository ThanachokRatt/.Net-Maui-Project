using System.Collections.ObjectModel;


namespace Project;

public partial class MainPage : ContentPage
{
    ViewModel.ItemVM vm;

    double currentBill = 0.0;
    double tipPerCent = 0.1;
    string tip = "";
    double countStepper = 1;
    double countStepperGlobal = 1;
    public MainPage()
    {
        InitializeComponent();
        btn10Percent.BackgroundColor = Color.FromArgb("#4FAD71");
        btn10Percent.TextColor = Color.FromArgb("#FFFFFF");
        selectedButton = btn10Percent;


        tip = "10%";
        
        QuantityLabel.Text = "1";

        vm = new ViewModel.ItemVM();
        this.BindingContext = vm;

    }


    void Entry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        billEntry.TextColor = Color.FromArgb("#4DA86E");
        billEntry.FontAttributes = FontAttributes.Bold;


        if (double.TryParse(billEntry.Text, out double bill))
        {
            currentBill = bill;

            // ]print the currentBill value here if needed
        }
        else if (!string.IsNullOrEmpty(billEntry.Text))
        {
            // If the entered text is not a valid number, remove the invalid characters
            billEntry.Text = new string(billEntry.Text.Where(c => char.IsDigit(c) || c == '.' && billEntry.Text.IndexOf(c) == billEntry.Text.LastIndexOf(c)).ToArray());
        }
    }


    Button selectedButton = null;
   

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        var button = (Button)sender;

        if (selectedButton == button) // if the clicked button is already selected
        {
            button.BackgroundColor = Color.FromArgb("#D7F9EB"); // set background color to default
            button.TextColor = Color.FromArgb("#4FAD71"); // set text color to default
            selectedButton = null; // reset currently selected button
            tipPerCent = 0.0; // reset tip percentage to 0

        }
        else // if the clicked button is not already selected
        {
            if (selectedButton != null)// if there is already a selected button
            {
                selectedButton.BackgroundColor = Color.FromArgb("#D7F9EB");
                selectedButton.TextColor = Color.FromArgb("#4FAD71"); // set text color to default
            }

            button.BackgroundColor = Color.FromArgb("#4FAD71"); // set background color to selected color
            button.TextColor = Color.FromArgb("#FFFFFF"); // set text color to selected color

            // Update tipPerCent based on which button is clicked
            if (button == btn0Percent)
            {
                tipPerCent = 0.0;
                tip = "0%";

            }
            else if (button == btn10Percent)
            {
                tipPerCent = 0.1;
                tip = "10%";

            }
            else if (button == btn20Percent)
            {
                tipPerCent = 0.2;
                tip = "20%";

            }

            selectedButton = button; // set currently selected button

        }

    }


    void Stepper_ValueChanged(System.Object sender, Microsoft.Maui.Controls.ValueChangedEventArgs e)
    {
        countStepper = e.NewValue;
        countStepperGlobal = countStepper;
        QuantityLabel.Text = $"{countStepper}";
    }
   async void Button_Clicked_1(System.Object sender, System.EventArgs e)
    {
        double total = (currentBill + (currentBill * tipPerCent)) / countStepperGlobal;



        string TotalGb = Convert.ToString($"{total:N2}");

          string Text = ($"Split between {countStepperGlobal} people.\nwith {tip}tip");

         
        var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes.txt");
        
        var filedetail = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes2.txt");

        var content2 = $"Total Per Person :{TotalGb}\n{Text}";
        

        

        DateTime Date = DateTime.Now;
        var content1 = $"Total Per 9Person32 :{Text}\n{TotalGb}";
        File.WriteAllText(filename, content2);

        File.WriteAllText(filedetail, content1);
      

        var content3 = File.ReadAllText(filename);

        List<Byte[]> imageByteArray= new List<byte[]>();
        vm.AddItem(filename, Date, TotalGb,Text,imageByteArray,content3,null);

        // item add;
       
        var secondPage = new SecondPage();
        secondPage.BindingContext = this.BindingContext;
        await Navigation.PushAsync(secondPage);
        



    }

    async private void Button_Clicked_2(System.Object sender, System.EventArgs e)
    {        // Navigate to ThirdPage
        await Navigation.PushAsync(new ThridPage());
    }
}
    



