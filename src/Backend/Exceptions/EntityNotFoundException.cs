using System;

namespace Backend.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, int id)
            : base($"No {entityName} with id: {id} was found.")
        {
        }
    }
}