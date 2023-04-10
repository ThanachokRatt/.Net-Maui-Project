using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using Project.Model;
using static System.Net.Mime.MediaTypeNames;

namespace Project.ViewModel
{
	public partial class ItemVM : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<Item> items;

        public ItemVM()
        {
            Items = new ObservableCollection<Item>();

        }
        public void AddItem(string pItemFilename ,DateTime pItemDate,string pItemTotalGb, string pItemText , List<Byte[]> pImagePaths, string pItemEditor, ObservableCollection<ImageSource> pimageList)
        {
            Items.Add(new Item
            {
                Filename = pItemFilename,
                Date = pItemDate,
                TotalGb = pItemTotalGb,
                Text = pItemText,
                ImagePaths = pImagePaths,
                editorText = pItemEditor,
               imageList = pimageList

            });
        }
    }
}

