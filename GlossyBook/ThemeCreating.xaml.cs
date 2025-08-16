using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GlossyBook
{
    public partial class ThemeCreating : Window
    {
        public TranslationTheme Theme { get; private set; }
        public ThemeCreating(TranslationTheme theme)
        {
            InitializeComponent();
            Theme = theme;
            DataContext = Theme;
        }

        private void OnCreateClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
