using System;
using System.Collections.Generic;

namespace DataStorage.Models
{
    public class TodoItemLabel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public List<TodoItem> LabelTodoItems { get; set; }

        public TodoItemLabel()
        {
            
        }

        public TodoItemLabel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }

    }

}
