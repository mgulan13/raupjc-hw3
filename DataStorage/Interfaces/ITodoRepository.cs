using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataStorage.Models;

namespace DataStorage.Interfaces
{
    public interface ITodoRepository
    {
        TodoItem Get(Guid todoId, Guid userId);
        void Add(TodoItem todoItem);
        bool Remove(Guid todoId, Guid userId);
        void Update(TodoItem todoItem, Guid userId);
        bool MarkAsCompleted(Guid todoId, Guid userId);
        List<TodoItem> GetAll(Guid userId);
        List<TodoItem> GetActive(Guid userId);
        List<TodoItem> GetCompleted(Guid userId);
        List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId);
        Task<IList<TodoItemLabel>> GetAllLabelsAsync();
        Task<bool> AddLabelAsync(TodoItemLabel label);
        Task<bool> RemoveLabelAsync(Guid id);
        Task<bool> UpdateLabelAsync(TodoItemLabel label);
        Task<TodoItemLabel> GetLabelAsync(Guid id);
        Task<TodoItemLabel> GetLabelAsync(string value);
    }
}