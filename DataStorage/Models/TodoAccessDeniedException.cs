using System;

namespace DataStorage.Models
{
    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(Guid userId, Guid todoId, Exception innerException = null) : base($"User with ID: {userId} tried to access {todoId} without permission.", innerException)
        {

        }
    }
}
