using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataStorage.Interfaces;
using DataStorage.Models;

namespace DataStorage
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            var todo = _context.TodoItems
                .Include(t => t.Labels)
                .AsNoTracking()
                .SingleOrDefault(t => t.Id == todoId);

            if (todo == null)
            {
                return null;
            }

            if (todo.UserId != userId)
            {
                throw new TodoAccessDeniedException(userId: userId, todoId: todoId);
            }

            return todo;
        }

        public void Add(TodoItem todoItem)
        {
            foreach (var label in todoItem.Labels)
            {
                _context.TodoLabels.Attach(label);
            }

            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var todo = Get(todoId, userId);
            if (todo == null)
            {
                return false;
            }

            _context.Entry(todo).State = EntityState.Unchanged;
            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            if (todoItem.UserId != userId)
            {
                throw new TodoAccessDeniedException(userId: userId, todoId: todoItem.Id);
            }

            _context.Entry(todoItem).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            var todo = Get(todoId, userId);
            if (todo == null)
            {
                return false;
            }

            _context.Entry(todo).State = EntityState.Unchanged;
            todo.MarkAsCompleted();
            _context.SaveChanges();
            return true;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return GetFiltered(t => true, userId);
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return GetFiltered(t => !t.IsCompleted, userId);
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return GetFiltered(t => t.IsCompleted, userId);
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems
                .Include(t => t.Labels)
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .Where(filterFunction)
                .ToList();
        }

        public async Task<List<TodoItemLabel>> GetAllLabelsAsync()
        {
            return await _context.TodoLabels
                .Include(l => l.LabelTodoItems)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AddLabelAsync(TodoItemLabel label)
        {
            if (await GetLabelAsync(label.Value) != null)
            {
                return false;
            }

            _context.TodoLabels.Add(label);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> RemoveLabelAsync(Guid id)
        {
            var label = await GetLabelAsync(id);
            if (label == null)
            {
                return false;
            }

            _context.TodoLabels.Attach(label);
            _context.TodoLabels.Remove(label);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TodoItemLabel> GetLabelAsync(Guid id)
        {
            return await _context.TodoLabels
                .Include(l => l.LabelTodoItems)
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TodoItemLabel> GetLabelAsync(string value)
        {
            return await _context.TodoLabels
                .Include(l => l.LabelTodoItems)
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Value == value);
        }

        public async Task<bool> UpdateLabelAsync(TodoItemLabel label)
        {
            if (await GetLabelAsync(label.Value) != null)
            {
                return false;
            }

            Update(label);
            await _context.SaveChangesAsync();
            return true;
        }

        private void Update(object entry)
        {
            _context.Entry(entry).State = EntityState.Modified;
        }
    }
}
