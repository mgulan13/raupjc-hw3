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

        public bool IsCompleted
        {
            get
            {
                return DateCompleted.HasValue;
            }
            private set
            {
                
            }
        }

        

        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                return true;
            }
            return false;
        }

        protected bool Equals(TodoItem other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TodoItem)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}