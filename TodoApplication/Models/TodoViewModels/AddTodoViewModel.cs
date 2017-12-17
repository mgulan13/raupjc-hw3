using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApplication.Models.TodoViewModels
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateDue { get; set; }

        public string Labels { get; set; }
    }
}
