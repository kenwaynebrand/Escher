using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escher_Test.Classes;
using Escher_Test.Enumerations;

namespace Escher_Test.Interfaces
{
    public interface IPerson
    {
        Guid Id { get; }
        string? FirstName { get; set; }
        string? Surname { get; set; }
        DateOnly DateOfBirth { get; set; }
        MaritalStatus MaritalStatus { get; set; }   
        List<PeopleLink>? LinkedPersons { get; set; }

    }
}
