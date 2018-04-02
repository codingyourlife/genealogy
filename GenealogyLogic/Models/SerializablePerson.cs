namespace GenealogyLogic.Models
{
    using GenealogyLogic.Interfaces;
    using System;
    using System.Collections.Generic;

    public class SerializablePerson : IPersonDetails, IPersonUIFeatures
    {
        public SerializablePerson(string firstname, string lastname, int id, List<int> parentGuids, bool isFemale, string biography, bool isHidden, DateTime? dateOfBirth, DateTime? dateOfDeath)
        {
            this.Id = id;
            this.FirstName = firstname;
            this.LastName = lastname;
            this.ParentGuids = parentGuids;
            this.IsFemale = isFemale;
            this.Biography = biography;
            this.IsHidden = isHidden;
            this.DateOfBirth = dateOfBirth;
            this.DateOfDeath = dateOfDeath;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<int> ParentGuids { get; set; }

        public bool IsFemale { get; set; }
        public string Biography { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }

        public UIPerson ToUIPerson()
        {
            var person = new UIPerson(this.FirstName, this.LastName);
            person.FirstName = this.FirstName;
            person.LastName = this.LastName;
            person.Id = this.Id;
            person.IsFemale = this.IsFemale;
            person.Biography = this.Biography;
            person.IsHidden = this.IsHidden;
            person.DateOfBirth = this.DateOfBirth;
            person.DateOfDeath = this.DateOfDeath;

            return person;
        }
    }
}
