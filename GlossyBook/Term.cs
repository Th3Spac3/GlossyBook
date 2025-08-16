using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GlossyBook
{
    public class Term : INotifyPropertyChanged
    {
        private string name;
        private string? text;
        private string? tag;
        private string? image;
        private int? currentCounter;

        public int Id { get; set; }
        public int TranslationThemeId { get; set; }
        public TranslationTheme? Theme { get; set; }

        public string? Tag
        {
            get { return tag; }
            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string? Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public string? Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        public int? CurrentCounter
        {
            get { return currentCounter; }
            set
            {
                currentCounter = value;
                OnPropertyChanged("CurrentCounter");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
