namespace GenealogyLogic.Models
{
    using System.Collections.Generic;
    using GenealogyLogic.Interfaces;
    using System.Linq;
    using System;

    public class UIPerson : Person, IUIPerson
    {
        public UIPerson(string firstName, string lastName, bool isHidden = false) : base(firstName, lastName)
        {
            this.IsHidden = isHidden;

            this.Parents = new List<IUIPerson>();
            this.Children = new List<IUIPerson>();

            this.Partners = new List<IUIPerson>();
        }

        public bool IsHidden { get; set; }

        public List<IUIPerson> Partners { get; set; }

        public List<IUIPerson> Parents { get; set; }

        public List<IUIPerson> Children { get; set; }

        public SerializablePerson GetSerializeable()
        {
            var serializeable = new SerializablePerson(this.FirstName, this.LastName, this.Id, this.Parents.Select(x => x.Id).ToList(), this.IsFemale, this.Biography, this.IsHidden, (this.DateOfBirth.HasValue ? this.DateOfBirth.Value : default(DateTime?)), (this.DateOfDeath.HasValue ? this.DateOfDeath.Value : default(DateTime?)));
            return serializeable;
        }

        public List<IUIPerson> GetAllDescendants()
        {
            List<IUIPerson> descendants = new List<IUIPerson>();

            var pointer = this;

            this.DescendantRetriever(pointer, ref descendants);

            return descendants;
        }

        private void DescendantRetriever(IUIPerson pointer, ref List<IUIPerson> descendants)
        {
            if(pointer.Children == null || !pointer.Children.Any())
            {
                return;
            }

            foreach (var descendant in pointer.Children)
            {
                descendants.Add(descendant);

                this.DescendantRetriever(descendant, ref descendants);
            }
        }

        public void HidePerson()
        {
            this.IsHidden = true;
        }

        public void HidePersonAndDescendants()
        {
            this.IsHidden = true;

            var descendants = this.GetAllDescendants();

            foreach (var descendant in descendants)
            {
                descendant.IsHidden = true;
            }
        }

        public void ShowPerson()
        {
            this.IsHidden = false;
        }

        public void ShowPersonAndDescendants()
        {
            this.IsHidden = false;

            var descendants = this.GetAllDescendants();

            foreach (var descendant in descendants)
            {
                descendant.IsHidden = false;
            }
        }

        public List<IUIPerson> GetChildrenConceivedWithPerson(IUIPerson possiblePartner)
        {
            var commonChildren = new List<IUIPerson>();

            if (!this.Partners.Contains(possiblePartner))
            {
                return commonChildren;
            }

            var ownChildren = this.Children;
            var childrenOfPartner = possiblePartner.Children;

            commonChildren = ownChildren.Intersect(childrenOfPartner).ToList();

            return commonChildren;
        }
    }
}
