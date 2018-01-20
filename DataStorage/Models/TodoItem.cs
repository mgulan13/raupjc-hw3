using System;
using System.Collections.Generic;

namespace DataStorage.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserId { get; set; }
        public List<TodoItemLabel> Labels { get; set; }
        public DateTime? DateDue { get; set; }

        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }
        public TodoItem()
        {
            // entity framework needs this one
            // not for use :)
        }

        public bool IsCompleted => DateCompleted.HasValue;

        public bool MarkAsCompleted()
        {
            if (IsCompleted)
            {
                return false;
            }
            
            DateCompleted = DateTime.Now;
            return true;
        }

    }
}