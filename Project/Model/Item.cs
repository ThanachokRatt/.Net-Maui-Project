using System;
using System.Collections.ObjectModel;

namespace Project.Model
{
    public class Item
    {
        public string Filename { get; set; }
        public DateTime? Date { get; set; }
        public string TotalGb { get; set; }
        public string Text { get; set; }
        public string editorText { get; set; }
        public List<Byte[]> ImagePaths { get; set; }
        public ObservableCollection<ImageSource> imageList {get;set;}

    }
}

