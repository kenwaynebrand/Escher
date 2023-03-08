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
    internal class Program
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
                        person.LinkedPersons.Add(new PeopleLink { IsMain = false, PersonID = linkedPerson.Id });
                        linkedPerson.LinkedPersons.Add(new PeopleLink { IsMain = true, PersonID = person.Id });
                        SaveLinkedPerson(linkedPerson, MainFileName);
                    }
                    SavePerson(person, MainFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Continue to register persons? (Y,N)");
            } while (Console.ReadLine().ToUpper() == "Y");
        }


        private static string SerializePerson(IPerson person)
        {
            var consent = person.GetType().Name == "PersonAdult" ? "null" : "true";
            string p = person.FirstName + "|" +
                person.Surname + "|" +
                person.DateOfBirth.ToShortDateString() + "|" +
                person.Surname + "|" +
                person.MaritalStatus.ToString() + "|" +
                consent;


            if (person.LinkedPersons.Count > 0)
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


        private static void SavePerson(IPerson person, string mainFileName)
        {
            using (var fs = File.AppendText(mainFileName))
            {
                fs.WriteLine(SerializePerson(person));
            }
        }

        private static void SaveLinkedPerson(IPerson linkedPerson, string mainFileName)
        {
            var LinkedPersonFileName = Path.GetDirectoryName(mainFileName);
            using (var fs = File.Create(LinkedPersonFileName + "\\" + linkedPerson.Id + ".txt"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(SerializePerson(linkedPerson));
                fs.Write(info, 0, info.Length);
            }
        }

        static IPerson InputData()
        {
            string? Firstname = Get<string>("First Name: ");
            string? Surname = Get<string>("Last Name: ");
            DateOnly DOB = Get<DateOnly>("Date of Birth: ");
            MaritalStatus Married = Get<MaritalStatus>("Marital Status (Maried, Single, Declined): ");

            // Implement Business Rules
            var age = Math.Truncate((DateTime.Now - DOB.ToDateTime(new TimeOnly())).Days/365.24);
            if (age < 16)
            {
                throw new ArgumentException("The Date of Birth does not meet the minimum requirements. Minimum age for registration is 16.");
            }
            else if(age < 18)
            {
                if ( Get<bool>("Parental Consent (Yes,No): "))
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

        static T? Get<T>(string prompt)
        {
            Console.WriteLine(prompt);
            object? input = Console.ReadLine();
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
                        //var foo = (object)res;
                        //ret = (T)foo;
                        ret = (T)(object)res;
                    }
                    else { ret = (T)default; }

                }
                else if (typeof(T) == typeof(bool))
                {

                    try
                    {
                        if (input != null)
                        {
                            ret = (input as string).ToUpper() switch
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
                    ret = (T)input;
                }

            }
            else { ret = (T)default; }

            return (T)ret;
        }

        static string ValidateCreateMainDataFile(string defaultFileName, string commandlineName)
        {
            string fileName;
            if (commandlineName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0) // test if supplied path/name is valid
            {
                try
                {
                    if (!File.Exists(commandlineName))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(commandlineName));
                        File.Create(commandlineName);
                    }
                    fileName = commandlineName;
                }
                catch
                {
                    if (!File.Exists(defaultFileName))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(defaultFileName));
                        File.Create(defaultFileName);
                    }
                    fileName = defaultFileName;
                }
            }
            else // either the commandline doesn't exist or had issues being created. use the default
            {
                if (!File.Exists(defaultFileName))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(defaultFileName));
                    File.Create(defaultFileName);
                }
                fileName = defaultFileName;
            }
            return fileName;
        }
    }
}