using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

    }
}
