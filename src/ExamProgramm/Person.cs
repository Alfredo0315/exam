using System;
using System.Collections.Generic;
using System.Text;

namespace ExamProgramm
{
    public class Person
    {
        public string LastName { get; }
        public string FirstName { get; }
        public int BirthYear { get; }

        public Person(string lastName, string firstName, int birthYear)
        {
            LastName = lastName;
            FirstName = firstName;
            BirthYear = birthYear;
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName}, {BirthYear}";
        }
    }
}
