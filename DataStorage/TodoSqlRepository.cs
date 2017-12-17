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
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == todoId);
            if (todo != null)
            {
                if (todo.UserId != userId)
                {
                    throw new TodoAccessDeniedException(userId: userId, todoId: todoId);
                }
                return todo;
            }

            return null;
        }

        public void Add(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var todo = Get(todoId, userId);
            if (todo != null)
            {
                _context.TodoItems.Remove(todo);
                _context.SaveChanges();
                return true;
            }

            return false;
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
            if (todo != null)
            {
                todo.MarkAsCompleted();
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId).ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId && t.IsCompleted == false).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId && t.IsCompleted).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId).Where(filterFunction).ToList();
        }

        public TodoItemLabel AddLabel(TodoItemLabel label)
        {
            TodoItemLabel existingLabel = _context.TodoLabels.FirstOrDefault(t => t.Value.Equals(label.Value));

            if (existingLabel == null)
            {
                _context.TodoLabels.Add(label);
                _context.SaveChanges();
                return label;
            }

            return existingLabel;
        }

        public List<TodoItem> GetLastTodos(int n)
        {
            return _context.TodoItems.OrderBy(t => t.DateCreated).Take(n).ToList();
        }
    }
}
