using System.Collections.ObjectModel;
using System.IO;
using Project.ViewModel;
using Project.Model;
namespace Project;


public partial class FourthPage : ContentPage
{
    ItemVM vm;
    List<Byte[]> imageByteArray = new List<byte[]>();
    Item item;


    bool IsPicker = false;
    public FourthPage()
    {
        InitializeComponent();
        vm = new ItemVM();
        this.BindingContext = vm;

        item = new Item // create the item object in the constructor
        {
            ImagePaths = imageByteArray,
            imageList = new ObservableCollection<ImageSource>()
        };
        vm.Items.Add(item); // add the item to the Items collection
    }
    protected override void OnAppearing()
    {
       
        base.OnAppearing();
       
        

        if (IsPicker == false)
        {
            item.ImagePaths = imageByteArray;
            item.imageList = new ObservableCollection<ImageSource>();
            
            foreach (var byteArray in item.ImagePaths)
        {
            item.imageList.Add(ImageSource.FromStream(() => new MemoryStream(byteArray)));
        }
            

            cvImages.ItemsSource = item.imageList;
            LoadImageFromDevice();
            vm.Items.Add(item);

            }

        }
    

    protected override void OnDisappearing()
    {
        IsPicker = false;
        base.OnDisappearing();
       
       
    }

    async void Delete_Images(System.Object sender, System.EventArgs e)
    {
        bool answer = await DisplayAlert("Question?", "Do you want to delete all Images", "Yes", "No");
        if (answer)
        {
            var imgPath = Path.Combine(FileSystem.Current.AppDataDirectory, "images");
            bool isFile = false;
            foreach (var file in System.IO.Directory.GetFiles(imgPath))
            {
                File.Delete(file);
                isFile = true;
            }
            if (isFile)
            {
               

                Directory.Delete(imgPath);
                imageByteArray.Clear();
                item.imageList.Clear();
            }
            await DisplayAlert("Information", "Delete Completed", "OK");
        }
    }



    async void Delete_Clicked(System.Object sender, System.EventArgs e)
    {
        var vm = (Item)BindingContext;
        /* bool answer = await DisplayAlert("Question?", "Do you want to delete all Note", "Yes", "No");
        if (answer)
        {
            var imgPath = Path.Combine(FileSystem.Current.AppDataDirectory, "images");
            bool isFile = false;
            foreach(var file in System.IO.Directory.GetFiles(imgPath))
            {
                File.Delete(file);
                isFile = true;
            }
            if (isFile)
            {
                Directory.Delete(imgPath);
                imageByteArray.Clear();
                imageList.Clear();
            }
        }*/
       
        if (File.Exists(vm.Filename))
        {
            File.Delete(vm.Filename);
         
        }
        await Navigation.PopAsync();
    }
    




    async void Save_Clicked(System.Object sender, System.EventArgs e)
    {
        var vm = (Item)BindingContext;
       
       

        if (string.IsNullOrWhiteSpace(vm.Filename))
        {
            // Save
            var filename = Path.Combine(App.FolderPath, $"{Path.GetRandomFileName()}.notes.txt");
            
            File.WriteAllText(filename,vm.editorText );

            vm.Filename = filename;

        }
        else
        {
            // Update
            File.WriteAllText(vm.Filename,vm.editorText);
            
        }
        if (imageByteArray != null && imageByteArray.Count > 0)
        {
            HasImageFolder();
            string imageDir = Path.Combine(FileSystem.AppDataDirectory, "images");
            foreach (var imageByte in imageByteArray)
            {
                var photoname = Guid.NewGuid().ToString();
                string imgPath = Path.Combine(imageDir, $"{photoname}.png");
                await SaveImage(imgPath,imageByte);
            }
            await DisplayAlert("Save Note", $"Information Save Completed \nImages Count = {imageByteArray.Count}", "Save");
        }
        

        await Navigation.PopAsync();
    }

    //Part of Image
    private void HasImageFolder()
    {
        var imageFolderPath = Path.Combine(FileSystem.Current.AppDataDirectory, "images");
        if (!Directory.Exists(imageFolderPath))
        {
            Directory.CreateDirectory(imageFolderPath);
        }
    }
    public async Task SaveImage(string filePath, byte[] imageBytes)
    {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
        }
    }
    private void LoadImageFromDevice()
    {
        var imgPath = Path.Combine(FileSystem.Current.AppDataDirectory, "images");
        // image = Folder Name
        try
        {
            var Files = System.IO.Directory.GetFiles(imgPath);
            foreach(var file in Files)
            {
                
                item.imageList.Add(ImageSource.FromFile(file));
            }
            if (Files != null && Files.Count() > 0)
            {
                foreach (var image in Files)
                {
                    FileStream fs = new FileStream(image, FileMode.Open, FileAccess.Read);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    async void Images_Clicked(object sender, EventArgs e)
    {
        var pickResult = await FilePicker.Default.PickMultipleAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Pick image(s)"
        });
        if (pickResult != null)
        {
            imageByteArray.Clear();
            

            foreach (var result in pickResult)
            {
                var stream = await result.OpenReadAsync();
                imageByteArray.Add(StreamToByteArray(stream));
            }

            cvImages.ItemsSource = item.imageList;
            try
            {
                foreach (var image in imageByteArray)
                {
                    item.imageList.Add(ImageSource.FromStream(() => new MemoryStream(image)));
                }
            }
            catch (Exception ex)
            {
            }
            IsPicker = true;
            vm.Items.Add(item);
        }
    }
    public byte[] StreamToByteArray(System.IO.Stream input)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            input.CopyTo(ms); return ms.ToArray();
        }
    }


    /*  async void Images_Clicked(object sender, EventArgs e)
      {

          var pickResult = await FilePicker.Default.PickAsync(new PickOptions
          {
              FileTypes = FilePickerFileType.Images,
              PickerTitle = "Pick an image"
          });
          if (pickResult != null)
          {
              var stream = await pickResult.OpenReadAsync();

             resultImage.Source = ImageSource.FromStream(() => stream);



          }
      }*/

    async void TakePhoto_Clicked(System.Object sender, System.EventArgs e)
    {
        await TakePhotoAsync();
    }

    async Task TakePhotoAsync()
    {
        
        try
        {
            var photo = await MediaPicker.Default.CapturePhotoAsync();
            string PhotoPath = await LoadPhotoAsync(photo);
            await DisplayAlert("System Message", $"CapturePhotoAsync COMPLETED: {PhotoPath}", "OK");
           // if (PhotoPath != "")
               resultTakePhotoImage.Source = ImageSource.FromFile(PhotoPath);
        }
        catch (FeatureNotSupportedException fnsEx)
        {// Feature is not supported on the device
        }
        catch (PermissionException pEx)
        {
            // Permissions not granted
        }
        catch (Exception ex){
            // Error Unexpected}}
        }

    async Task<String> LoadPhotoAsync(FileResult photo)
        {// canceled
         if (photo == null)
            {
                return "";
            }
         // save the file into local storage
         string newFilePath= Path.Combine(FileSystem.CacheDirectory, photo.FileName);

         using Stream sourceStream= await photo.OpenReadAsync();
         using FileStream fileStream= File.OpenWrite(newFilePath);
         await sourceStream.CopyToAsync(fileStream);
            // Copy Source Stream to new Stream
         return newFilePath;}
        }

    void Detete_takePhoto(System.Object sender, System.EventArgs e)
    {
       
  
    }
}


