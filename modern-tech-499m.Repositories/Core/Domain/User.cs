using System;
using modern_tech_499m.Repositories.Core.Repositories;

namespace modern_tech_499m.Repositories.Core.Domain
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        //Real - OLE Automation Date
        public DateTime BirthDate { get; set; }
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
    }
}
