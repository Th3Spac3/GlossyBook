using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GlossyBook
{
    public class TranslationTheme : INotifyPropertyChanged
    {
        private string name;
        private ObservableCollection<Term>? terms;

        public int Id { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public ObservableCollection<Term>? Terms
        {
            get { return terms; }
            set
            {
                terms = value;
                OnPropertyChanged("Terms");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return name;
        }
    }
}
