using GlossyBook;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Gloss
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private ApplicationContext db = new ApplicationContext();
        private RelayCommand? addTerm;
        private RelayCommand? addTheme;
        private RelayCommand? deleteCounters;
        private RelayCommand? selectThemeContext;
        private RelayCommand? editTermText;
        private RelayCommand? editTermTag;
        private RelayCommand? editTermImage;

        private Term selectedTerm;

        public int SelectedTheme;
        public string InputString;

        public ObservableCollection<TranslationTheme> Themes {  get; set; }
        public ObservableCollection<Term> Terms { get; set; }
        public ObservableCollection<Term> ThemeContext { get; set; }

        public UITheme UI {  get; set; }

        public ApplicationViewModel()
        {
            db.Database.EnsureCreated();
            db.Themes.Load();
            db.Terms.Load();
            ThemeContext = new ObservableCollection<Term>();
            Themes = db.Themes.Local.ToObservableCollection();
            Terms = db.Terms.Local.ToObservableCollection();
            UI = UITheme.Default();
        }

        public Term SelectedTerm
        {
            get { return selectedTerm; }
            set
            {
                selectedTerm = value;
                OnPropertyChanged("SelectedTerm");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
        public RelayCommand AddTerm
        {
            get
            {
                return addTerm ??
                    (addTerm = new RelayCommand(o =>
                    {
                        if (!Terms.Any(t => t.TranslationThemeId - 1 == SelectedTheme && t.Name == InputString))
                        {
                            Term term = new Term();
                            term.Name = InputString;
                            term.TranslationThemeId = SelectedTheme;
                            term.Theme = Themes[SelectedTheme];
                            term.CurrentCounter = 1;
                            db.Terms.Add(term);
                            db.SaveChanges();
                        }
                        else
                        { 
                            var term = db.Terms.Where(t => t.TranslationThemeId - 1 == SelectedTheme && t.Name == InputString).First();
                            if (term != null)
                            {
                                if (term.CurrentCounter == null) term.CurrentCounter = 1;
                                else term.CurrentCounter++;
                                db.Entry(term).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }));
            }
        }

        public RelayCommand AddTheme
        {
            get
            {
                return addTheme ??
                    (addTheme = new RelayCommand(o => 
                    {
                        ThemeCreating window = new ThemeCreating(new TranslationTheme());
                        if(window.ShowDialog() == true)
                        {
                            TranslationTheme theme = window.Theme;
                            db.Themes.Add(theme);
                            db.SaveChanges();
                        }
                }));
            }
        }

        public RelayCommand DeleteCounters
        {
            get
            {
                return deleteCounters ??
                    (deleteCounters = new RelayCommand(o =>
                    {
                        foreach(var term in Terms)
                        {
                            term.CurrentCounter = null;
                            db.Entry(term).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }));
            }
        }

        public RelayCommand SelectThemeContext
        {
            get
            {
                return selectThemeContext ??
                    (selectThemeContext = new RelayCommand(o =>
                    {
                        ThemeContext.Clear();
                        foreach (var term in Terms.Where(t => t.TranslationThemeId - 1 == SelectedTheme && t.CurrentCounter > 0))
                            ThemeContext.Add(term);
                    }));
            }
        }

        public RelayCommand EditTermText
        {
            get
            {
                return editTermText ??
                    (editTermText = new RelayCommand(o =>
                    {
                        SelectedTerm.Text = InputString;
                        db.Entry(SelectedTerm).State = EntityState.Modified;
                        db.SaveChanges();
                    }));
            }
        }

        public RelayCommand EditTermTag
        {
            get
            {
                return editTermTag ??
                    (editTermTag = new RelayCommand(o =>
                    {
                        SelectedTerm.Tag = InputString;
                        db.Entry(SelectedTerm).State = EntityState.Modified;
                        db.SaveChanges();
                    }));
            }
        }

        public RelayCommand EditTermImage
        {
            get
            {
                return editTermImage ??
                    (editTermImage = new RelayCommand(o =>
                    {
                        SelectedTerm.Image = InputString;
                        db.Entry(SelectedTerm).State = EntityState.Modified;
                        db.SaveChanges();
                    }));
            }
        }
    }
}
