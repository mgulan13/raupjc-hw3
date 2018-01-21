using System;
using System.ComponentModel.DataAnnotations;
using DataStorage.Models;

namespace TodoApplication.Models.TodoViewModels
{
    public class EditLabelViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Value { get; set; }

        public static EditLabelViewModel FromEntity(TodoItemLabel label)
        {
            return new EditLabelViewModel()
            {
                Id = label.Id,
                Value = label.Value
            };
        }
    }
}
