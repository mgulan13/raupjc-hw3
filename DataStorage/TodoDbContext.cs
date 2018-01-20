using DataStorage.Models;
using System.Data.Entity;
namespace DataStorage
{
    public class TodoDbContext : DbContext
    {
        public IDbSet<TodoItem> TodoItems { get; set; }
        public IDbSet<TodoItemLabel> TodoLabels { get; set; }

        public TodoDbContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasKey(t => t.Id);
            modelBuilder.Entity<TodoItem>().Property(t => t.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(t => t.DateCompleted);
            modelBuilder.Entity<TodoItem>().Property(t => t.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().HasMany(t => t.Labels).WithMany(t => t.LabelTodoItems);
            modelBuilder.Entity<TodoItem>().Property(t => t.DateDue);
            modelBuilder.Entity<TodoItem>().Ignore(t => t.IsCompleted);

            modelBuilder.Entity<TodoItemLabel>().HasKey(t => t.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(t => t.Value).IsRequired();
        }
    }
}
