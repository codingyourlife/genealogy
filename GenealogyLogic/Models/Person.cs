namespace GenealogyLogic.Models
{
    using GenealogyLogic.Helpers;
    using GenealogyLogic.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class Person : IPerson
    {
        public Person(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;

            this.Id  = MiniGuid.NewGuid();

            this.Parents = new List<IPerson>();
            this.Children = new List<IPerson>();

            this.Partners = new List<IPerson>();
        }

        public int Id { get; set; }

        public virtual List<IPerson> Partners { get; }

        public virtual List<IPerson> Parents { get; }

        public virtual List<IPerson> Children { get; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public string Description { get; set; }

        public virtual bool IsFemale { get; set; }

        public virtual string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public string Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }

        public string LifeTimeText
        {
            get
            {
                if(DateOfBirth == null || DateOfBirth == default(DateTime))
                {
                    return string.Empty;
                }

                var lifeTimeText = this.DateOfBirth.Value.ToShortDateString() + (DateOfDeath == null || DateOfDeath == default(DateTime) ? string.Empty : " - " + DateOfDeath.Value.ToShortDateString());
                return lifeTimeText;
            }
        }

        public string Label
        {
            get
            {
                var label = " <b>" + this.FullName + "</b>" + (!string.IsNullOrWhiteSpace(this.LifeTimeText) ? "<br/>" + this.LifeTimeText : string.Empty);

                if(label.Contains("<") && !label.StartsWith("<"))
                {
                    return "< " + label + " >";
                }

                return label;
            }
        }
    }
}
