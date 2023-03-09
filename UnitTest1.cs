using Escher_Test;
using Escher_Test.Enumerations;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.ComponentModel;

namespace EscherTests
{
    public class EscherTests
    {
        [Fact]
        public void SerializeAdultPerson()
        {
            var person = new Escher_Test.Models.PersonAdult() { FirstName="A", Surname="B", MaritalStatus=MaritalStatus.Declined, DateOfBirth = new DateOnly(1970, 01, 01) };
            Assert.Equal("A|B|1/1/1970|B|Declined|null", Escher_Test.Program.SerializePerson(person));
        }

        [Fact]
        public void SerializeChildPerson()
        {
            var person = new Escher_Test.Models.PersonChild() { FirstName = "A", Surname = "B", MaritalStatus = MaritalStatus.Single, DateOfBirth=new DateOnly(2006,01,01) };
            var foo = Escher_Test.Program.SerializePerson(person);
            Assert.Equal("A|B|1/1/2006|B|Single|true", Escher_Test.Program.SerializePerson(person));
        }

        [Fact]
        public void GetTestDateOnlyType()
        {
            Assert.IsType<DateOnly>(Escher_Test.Program.Get<DateOnly>("01/01/1970"));
        }

        [Fact]
        public void GetTestDateOnly()
        {
            Assert.Equal(new DateOnly(1970, 1, 1),  Escher_Test.Program.Get<DateOnly>("01/01/1970"));
        }

        [Fact]
        public void GetTestMaritalStatus()
        {
            Assert.Equal(MaritalStatus.Single, Escher_Test.Program.Get<MaritalStatus>("single"));
        }


        [Fact]
        public void GetTestBoolFalse()
        {
            Assert.False(Escher_Test.Program.Get<bool>("NO"));
        }

        [Fact]
        public void GetTestBoolTrue()
        {
            Assert.True(Escher_Test.Program.Get<bool>("YES"));
        }


        [Fact]
        public void CreateFileBTest()
        {
            if (File.Exists("C:\\XUnitTestDirectory\\XUnitTestFileB.txt"))
            {
                File.Delete("C:\\XUnitTestDirectory\\XUnitTestFileB.txt");
            }
            Escher_Test.Program.ValidateCreateMainDataFile("C:\\XUnitTestDirectory\\XUnitTestFileA.txt", "C:\\XUnitTestDirectory\\XUnitTestFileB.txt");
            Assert.True(File.Exists("C:\\XUnitTestDirectory\\XUnitTestFileB.txt"));
        }

        [Fact]
        public void CreateFileATest()
        {
            if (File.Exists("C:\\XUnitTestDirectory\\XUnitTestFileA.txt"))
            {
                File.Delete("C:\\XUnitTestDirectory\\XUnitTestFileA.txt");
            }
            Escher_Test.Program.ValidateCreateMainDataFile("C:\\XUnitTestDirectory\\XUnitTestFileA.txt", "");
            Assert.True(File.Exists("C:\\XUnitTestDirectory\\XUnitTestFileA.txt"));
        }
    }
}