using System;
using System.ComponentModel.DataAnnotations;
using modern_tech_499m.Repositories.Core.Repositories;
using modern_tech_499m.Repositories.Persistence;

namespace modern_tech_499m.Repositories.Core.Domain
{
    public class User : ValidatorBase, IEntity
    {
        [Required]
        public int Id { get; set; }

        [Required] 
        public string Username { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string LastName { get; set; }

        public string Patronymic { get; set; }

        //Real - OLE Automation Date
        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        public string FullName => $"{LastName} {FirstName} {Patronymic}";
    }
}
