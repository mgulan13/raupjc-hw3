using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStorage.Models;

namespace TodoApplication.Models.TodoViewModels
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateDue { get; set; }

        public Guid[] Labels { get; set; }
    }
}
