using System;

namespace TodoApplication.Models.TodoViewModels
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public bool IsCompleted { get; set; }

        public String DaysLeft
        {
            get
            {
                if (Date is null)
                {
                    return null;
                }

                int daysLeft = (Date.Value - DateTime.UtcNow).Days;
                if (daysLeft < 0)
                {
                    return "Prošao krajnji rok!";
                }

                String returnMessage = $"za {daysLeft} dan";
                if (daysLeft % 10 != 1 || daysLeft % 100 == 11)
                {
                    returnMessage += "a";
                }
                returnMessage += "!";

                return returnMessage;
            }
        }
    }
}
