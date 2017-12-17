using System;

namespace TodoApplication.Models.TodoViewModels
{
    public class TodoViewModel
    {
        public Guid Id;
        public string Text { get; set; }
        public DateTime? DateDue { get; set; }
        public bool IsCompleted { get; }
        public String DaysLeft
        {
            get
            {
                if (DateDue is null)
                {
                    return null;
                }

                int daysLeft = (DateDue.Value - DateTime.UtcNow).Days;
                if (daysLeft < 0)
                {
                    return "Prošao krajnji rok!";
                }

                String returnMessage = $"za {daysLeft} dan";
                if (daysLeft % 10 != 1 || daysLeft % 100 == 11)
                {
                    returnMessage += "a";
                }

                return returnMessage;
            }
        }

        public TodoViewModel(Guid id, string text, DateTime? dateDue, bool isCompleted)
        {
            Id = id;
            Text = text;
            DateDue = dateDue;
            IsCompleted = isCompleted;
        }
    }
}
