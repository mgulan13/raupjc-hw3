using System;
using System.Collections.Generic;

namespace DataStorage.Models
{
    public class TodoItemLabel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public List<TodoItem> LabelTodoItems { get; set; }

        public TodoItemLabel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }

        protected bool Equals(TodoItemLabel other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TodoItemLabel) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }

}
