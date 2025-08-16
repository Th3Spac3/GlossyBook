using Gloss;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GlossyBook
{
    public partial class MainWindow : Window
    {
        private ApplicationViewModel viewModel;

        private TextParser parser;

        private BitmapSource placeholder;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ApplicationViewModel();
            DataContext = viewModel;
            ChildInit();
            AsyncInit();
        }

        private async Task AsyncInit()
        {
            var o = await Options.Get();

            parser = await TextParser.TextParserFactory();
        }
        
        private void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Resize(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void Collapse(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ChangePosition(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (e.ClickCount >= 2)
            {
                Resize(sender, e);
            }
        }

        private void ChildInit()
        {
            ThemeSelection.ItemsSource = viewModel.Themes;
            App.Current.Exit += OnAppExit;
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            viewModel.DeleteCounters.Execute(this);
        }

        private void OnThemeSelectionOpened(object sender, EventArgs e)
        {
            if (viewModel.Themes.Count == 0)
            {
                AddTheme();
            } else
            {
                ThemeSelection.ItemsSource = viewModel.Themes;
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ThemeSelection.SelectedValue is { })
            {
                viewModel.SelectedTheme = ThemeSelection.SelectedIndex;
                viewModel.SelectThemeContext.Execute(this);
            }
        }

        private void OnAddThemeClicked(object sender, RoutedEventArgs e)
        {
            AddTheme();
        }

        private void AddTheme()
        {
            viewModel.AddTheme.Execute(this);
        }

        private async void OnReadFileClicked(object sender, RoutedEventArgs e)
        {
            if(ThemeSelection.SelectedValue != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Select file to read";
                openFileDialog.Filter = "Text Files (*.txt)|*.txt";

                bool? result = openFileDialog.ShowDialog();
                if (result != null && result.Value)
                {
                    viewModel.DeleteCounters.Execute(this);
                    string text = await FileWorker.Read(openFileDialog.FileName);
                    string parsedText = await parser.ParseText(text);
                    List<string> words = new List<string>();
                    words.AddRange(parsedText.Split(' ').Where(s => s.Any(c => c >= '\u4E00' && c <= '\u9FFF') || s.Any(c => c >= '\u3400' && c <= '\u4DBF') || s.Any(c => c >= '\uF900' && c <= '\uFAFF')).ToArray());
                    foreach (string word in words)
                    {
                        viewModel.InputString = word;
                        viewModel.AddTerm.Execute(this);
                    }
                    viewModel.SelectThemeContext.Execute(this);
                }
            }
        }

        private void OnDescriptionChanged(object sender, RoutedEventArgs e)
        {
            if(viewModel.SelectedTerm != null)
            {
                var tr = new TextRange(TermDescription.Document.ContentStart, TermDescription.Document.ContentEnd);
                Trace.WriteLine(tr.Text);
                viewModel.InputString = tr.Text;
                viewModel.EditTermText.Execute(this);
            }
            
        }

        private void OnTermSelected(object sender, SelectionChangedEventArgs e)
        {
            if(viewModel.SelectedTerm is { })
            {
                RefreshImage();
            }
        }

        private void RefreshImage()
        {
            if (viewModel.SelectedTerm.Image is { })
            {
                BitmapImage image = new BitmapImage();
                using(var fs = new FileStream(viewModel.SelectedTerm.Image, FileMode.Open))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = fs;
                    image.EndInit();
                }
                TermImage.Source = image;
            }
            else
            {
                TermImage.Source = new BitmapImage(new Uri("Resources/noImage.png", UriKind.Relative));
            }
            TermDescription.Document.Blocks.Clear();
            TermDescription.Document.Blocks.Add(new Paragraph(new Run(viewModel.SelectedTerm.Text)));
        }

        private void OnTagChanged(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTerm != null)
            {
                viewModel.InputString = TagBox.Text;
                viewModel.EditTermTag.Execute(this);
            }
        }

        private void OnImageChanged(object sender, MouseButtonEventArgs e)
        {
            if(viewModel.SelectedTerm is { })
            {
                if(viewModel.SelectedTerm.Image is { })
                {
                    TermImage.Source = null;
                    try
                    {
                        File.Delete(viewModel.SelectedTerm.Image);
                    } catch(IOException ex)
                    {
                        Trace.WriteLine(ex);
                    }
                }
                var bmp = Clipboard.GetImage();
                if (bmp != null)
                {
                    Guid guid = Guid.NewGuid();
                    var path = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent;
                    string dir = $@"{(path != null ? path : @"C:\tmp")}\UserImages";
                    
                    
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    using (var fs = new FileStream($@"{dir}\{guid.ToString()}.png", FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bmp));
                        encoder.Save(fs);
                    }
                    viewModel.InputString = $@"{dir}\{guid.ToString()}.png";
                    viewModel.EditTermImage.Execute(this);
                    RefreshImage();
                }
            }
        }
    }
}