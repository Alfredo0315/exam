using System;

namespace ExamProgramm
{
    public class PersonnelManagement
    {
        private Person[] _persons;

        public PersonnelManagement(Person[] persons)
        {
         
            _persons = new Person[persons.Length];
            Array.Copy(persons, _persons, persons.Length);
        }

       
        public void SortByBirthYearAndLastNameDesc()
        {
            int n = _persons.Length;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    bool shouldSwap = false;

                    if (_persons[j].BirthYear < _persons[j + 1].BirthYear)
                    {
                        shouldSwap = true;
                    }
                    
                    else if (_persons[j].BirthYear == _persons[j + 1].BirthYear)
                    {
                        
                        if (string.Compare(_persons[j].LastName, _persons[j + 1].LastName, StringComparison.Ordinal) < 0)
                        {
                            shouldSwap = true;
                        }
                    }

                    if (shouldSwap)
                    {
                        Person temp = _persons[j];
                        _persons[j] = _persons[j + 1];
                        _persons[j + 1] = temp;
                        swapped = true;
                    }
                }

                if (!swapped) break; 
            }
        }

        public Person[] GetPersons()
        {
            return _persons;
        }

        public void SaveToFile(string filePath)
        {
            using (var writer = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                foreach (var person in _persons)
                {
                    writer.WriteLine($"{person.LastName};{person.FirstName};{person.BirthYear}");
                }
            }
        }
    }
}
