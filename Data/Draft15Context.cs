using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppData;

namespace Draft15.Data
{
    public class Draft15Context : DbContext
    {
        public Draft15Context (DbContextOptions<Draft15Context> options)
            : base(options)
        {
        }

        public DbSet<AppData.Submission> Submission { get; set; } = default!;
        public DbSet<AppData.Content> Content { get; set; } = default!;
        public DbSet<AppData.Assignment> Assignment { get; set; } = default!;
        public DbSet<AppData.File> File { get; set; } = default!;
        public DbSet<AppData.Quiz> Quiz { get; set; } = default!;
        public DbSet<AppData.QuizAttempt> QuizAttempt { get; set; } = default!;
        public DbSet<AppData.QuizQuestion> QuizQuestion { get; set; } = default!;
        public DbSet<AppData.UserContent> UserContent { get; set; } = default!;
        public DbSet<AppData.QuizResponse> QuizResponse { get; set; } = default!;
        public DbSet<AppData.QuizOption> QuizOption { get; set; } = default!;
    }
}
