using Escher_Test.Classes;
using Escher_Test.Enumerations;
using Escher_Test.Interfaces;
using Escher_Test.Models;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace Escher_Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Setup and create if needed the main data file
            string MainFileName = ValidateCreateMainDataFile("c:\\people\\mainfile.txt", args.Length > 0 ? args[0] : "");

            do
            {
                try
                {
                    var person = InputData();
                    if (person.MaritalStatus == MaritalStatus.Maried)
                    {
                        Console.WriteLine("Please enter spouse information:");
                        var linkedPerson = InputData();
                        PeopleLink foo = new();
                        person.LinkedPersons?.Add(new PeopleLink { IsMain = false, PersonID = linkedPerson.Id });
                        linkedPerson.LinkedPersons?.Add(new PeopleLink { IsMain = true, PersonID = person.Id });
                        SaveLinkedPerson(linkedPerson, MainFileName);
                    }
                    SavePerson(person, MainFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Continue to register persons? (Y,N)");
            } while (Console.ReadLine() != null);
        }


        public static string SerializePerson(IPerson person)
        {
            var consent = person.GetType().Name == "PersonAdult" ? "null" : "true";
            string p = person.FirstName + "|" +
                person.Surname + "|" +
                person.DateOfBirth.ToShortDateString() + "|" +
                person.Surname + "|" +
                person.MaritalStatus.ToString() + "|" +
                consent;


            if (person.LinkedPersons != null && person.LinkedPersons.Count > 0)
            {
                {
                    foreach (var lp in person.LinkedPersons)
                    {
                        var ismain = lp.IsMain ? "true" : "false";
                        p = p + "|" + lp.PersonID + "|" + ismain;
                    }
                }
            }
            return p;
        }


        static void SavePerson(IPerson person, string mainFileName)
        {
            using var fs = File.AppendText(mainFileName);
            fs.WriteLine(SerializePerson(person));
        }

        static void SaveLinkedPerson(IPerson linkedPerson, string mainFileName)
        {
            var LinkedPersonFileName = Path.GetDirectoryName(mainFileName);
            using var fs = File.Create(LinkedPersonFileName + "\\" + linkedPerson.Id + ".txt");
            byte[] info = new UTF8Encoding(true).GetBytes(SerializePerson(linkedPerson));
            fs.Write(info, 0, info.Length);
        }

        static IPerson InputData()
        {
            string? Firstname = Get<string>(Prompt("First Name: "));
            string? Surname = Get<string>(Prompt("Last Name: "));
            DateOnly DOB = Get<DateOnly>(Prompt("Date of Birth: "));
            MaritalStatus Married = Get<MaritalStatus>(Prompt("Marital Status (Maried, Single, Declined): "));

            // Implement Business Rules
            var age = Math.Truncate((DateTime.Now - DOB.ToDateTime(new TimeOnly())).Days/365.24);
            if (age < 16)
            {
                throw new ArgumentException("The Date of Birth does not meet the minimum requirements. Minimum age for registration is 16.");
            }
            else if(age < 18)
            {
                if ( Get<bool>(Prompt("Parental Consent (Yes,No): ")))
                {
                    return new PersonChild { DateOfBirth = DOB, FirstName= Firstname, MaritalStatus = Married, Surname = Surname, GuardianConsent = true };
                }
                else
                {
                    throw new ArgumentException("The Date of Birth does not meet the minimum requirements. Minors require parental consent.");
                }
            }
            else
            {
                return new PersonAdult { DateOfBirth = DOB, FirstName = Firstname, MaritalStatus = Married, Surname = Surname };

            }
        }

        static string? Prompt(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        public static T? Get<T>(string? input)
        {
            object ret;

            if (input != null)
            {
                if (typeof(T) == typeof(DateOnly))
                {
                    object parsedDate = DateOnly.Parse((string)input);
                    ret = (T)parsedDate;
                }
                else if (typeof(T) == typeof(MaritalStatus))
                {
                    if (Enum.TryParse<MaritalStatus>((string)input, true, out MaritalStatus res))
                    {
                        ret = (T)(object)res;
                    }
                    else 
                    { 
                        ret = (T)(object)MaritalStatus.Declined; 
                    }

                }
                else if (typeof(T) == typeof(bool))
                {

                    try
                    {
                        if (input != null)
                        {

                            ret = ((string)input).ToUpper() switch
                            {
                                "YES" => (T)(object)true,
                                "NO" => (T)(object)false,
                                _ => (object)(T)(object)false,
                            };
                        }
                        else
                        {
                            ret = (T)(object)false;
                        }
                    }
                    catch
                    {
                        ret = (T)(object)false;
                    }
                }
                else
                {
                    ret = (T)(object)input;
                }

            }
            else { ret = (T)(object)""; }

            return (T)ret;
        }

        public static string ValidateCreateMainDataFile(string defaultFileName, string commandlineName)
        {
            string fileName;
            var FN = Path.GetFileName(commandlineName);
            if (!string.IsNullOrEmpty(commandlineName) && FN.IndexOfAny(Path.GetInvalidFileNameChars()) < 0) 
            {
                if (!File.Exists(commandlineName))
                {
                    var dir = Path.GetDirectoryName(commandlineName);
                    if (dir != null)
                    {
                        Directory.CreateDirectory(dir);
                        using var fs = File.Create(commandlineName);
                    }
                }
                fileName = commandlineName;
            }
            else
            {
                if (!File.Exists(defaultFileName))
                {
                    var dir = Path.GetDirectoryName(defaultFileName);
                    if (dir != null)
                    {
                        Directory.CreateDirectory(dir);
                        using var fs = File.Create(defaultFileName);
                    }
                }
                fileName = defaultFileName;
            }
            return fileName;
        }
    }
}
