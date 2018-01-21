using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApplication.Models.TodoViewModels
{
    public class AddLabelViewModel
    {
        [Required]
        public string Value { get; set; }
    }
}
