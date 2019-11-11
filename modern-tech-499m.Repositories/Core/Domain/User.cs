using System;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.Repositories.Core.Domain
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
