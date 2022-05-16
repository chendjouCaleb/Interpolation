using System;

namespace TextBinding
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public int Age = 10;

        public Phone Phone { get; set; } = new() {Number = "69999999"};

        public string GetFullName() => FirstName + " " + LastName;

        public bool HasName { get; set; } = true;

    }

    public class Phone
    {
        public string Number { get; set; }
    }
}