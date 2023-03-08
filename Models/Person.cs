using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escher_Test.Classes;
using Escher_Test.Enumerations;
using Escher_Test.Interfaces;

namespace Escher_Test.Models
{
    public class PersonBase : IPerson
    {
        public PersonBase() 
        { 
            Id = Guid.NewGuid();
            LinkedPersons = new List<PeopleLink> { };
        }
        public Guid Id { get; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public List<PeopleLink>? LinkedPersons { get; set; }
    }

    public class PersonAdult : PersonBase
    {
        public PersonAdult() { }
    }

    public class PersonChild : PersonBase
    {
        public PersonChild() { }

        public bool GuardianConsent { get; set; }
    }
}
