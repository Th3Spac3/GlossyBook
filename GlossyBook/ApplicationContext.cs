using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlossyBook;
using Microsoft.EntityFrameworkCore;

namespace Gloss
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TranslationTheme> Themes { get; set; } = null!;
        public DbSet<Term> Terms { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=glossary.db");
        }
    }
}
