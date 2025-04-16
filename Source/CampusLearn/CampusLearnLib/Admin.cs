using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearnLib
{
    internal class Admin : User
    {
        public Admin() : base() // Calls the User constructor
        {
        }
        public Module createModule(string moduleName, string moduleCode, string description)
        {
            // Create a new module
            Module newModule = new Module
            {
                id = GenerateModuleId(),
                name = moduleName,
                code = moduleCode,
                description = description,
                createdBy = this,
                creationDate = DateTime.Now,
                topics = new List<Topic>()
            };
            Console.WriteLine($"Module '{moduleName}' (Code: {moduleCode}) created successfully.");
            return newModule;
        }

        public Tutor assignTutorRole(Student student)
        {

            Tutor newTutor = new Tutor
            {

                id = student.Id,
                name = student.Name,
                surname = student.Surname,
                email = student.Email,
                password = student.Password,
                contactNumber = student.ContactNumber,
                messages = new List<Message>(),


                ownedTopics = new List<Topic>()
            };
            Console.WriteLine($"User {student.Name} {student.Surname} has been assigned Tutor role.");

            return newTutor;

        }
        public class Module
        {
            public int Id { get; set; }
            public string name { get; set; }
            public string code { get; set; }
            public string description { get; set; }
            public Admin createdBy { get; set; }
            public DateTime creationDate { get; set; }
            public List<Topic> topics { get; set; }

        }
    }
}
