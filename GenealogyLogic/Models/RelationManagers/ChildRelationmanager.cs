namespace GenealogyLogic.Models.RelationManagers
{
    using GenealogyLogic.EventArguments;
    using GenealogyLogic.Exceptions;
    using GenealogyLogic.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ChildRelationmanager
    {
        private List<IUIPerson> persons;

        public event EventHandler GenealogyChanged;

        public ChildRelationmanager(List<IUIPerson> persons)
        {
            this.persons = persons;
        }

        protected virtual void OnGenealogyChanged(GenealogyMasterEventArgs e)
        {
            EventHandler handler = GenealogyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void AddChild(IUIPerson parent, IUIPerson child)
        {
            if (parent == child)
            {
                return;
            }

            if (parent.Children.Contains(child))
            {
                return;
            }

            if (child.Parents.Count == 2 && !child.Parents.Contains(parent))
            {
                throw new InvalidStateException(string.Format("Mehr als 2 Eltern sind nicht erlaubt und \"{0}\" hat bereits 2", child.FullName));
            }

            if (child.Children.Contains(parent) || parent.Parents.Contains(child))
            {
                throw new InvalidStateException(string.Format("Person \"{0}\" kann nicht Kind und Eltern von \"{1}\" zugergleich sein", child.FullName, parent.FullName));
            }

            if (parent.GetAllDescendants().Contains(child))
            {
                throw new InvalidStateException(string.Format("Person \"{0}\" ist bereits ein Nachfahre von \"{1}\"", child.FullName, parent.FullName));
            }

            if (!persons.Contains(child))
            {
                persons.Add(child);
            }

            if (!persons.Contains(parent))
            {
                persons.Add(parent);
            }

            parent.Children.Add(child);
            child.Parents.Add(parent);

            LinkImplicitPartners(child);

            this.OnGenealogyChanged(new GenealogyMasterEventArgs(this.persons));
        }

        private static void LinkImplicitPartners(IUIPerson child)
        {
            IUIPerson lastParent = null;
            foreach (var parentOfPersonToAddParent in child.Parents)
            {
                if (lastParent != null)
                {
                    if (!lastParent.Partners.Contains(parentOfPersonToAddParent))
                    {
                        lastParent.Partners.Add(parentOfPersonToAddParent);
                        parentOfPersonToAddParent.Partners.Add(lastParent);
                    }
                }

                lastParent = parentOfPersonToAddParent;
            }
        }

        private void UnLinkImplicitPartners(IUIPerson child)
        {
            IUIPerson lastParent = null;
            foreach (var parentOfPersonToRemoveParent in child.Parents)
            {
                if (lastParent != null)
                {
                    if (lastParent.Partners.Contains(parentOfPersonToRemoveParent))
                    {
                        if (!lastParent.GetChildrenConceivedWithPerson(parentOfPersonToRemoveParent).Any(x => x.Id != child.Id))
                        {
                            lastParent.Partners.Remove(parentOfPersonToRemoveParent);
                            parentOfPersonToRemoveParent.Partners.Remove(lastParent);
                        }
                    }
                }

                lastParent = parentOfPersonToRemoveParent;
            }
        }

        public void RemoveChild(IUIPerson personToRemoveChild, IUIPerson child)
        {
            if (personToRemoveChild.Children.FirstOrDefault(x => x.Id == child.Id) != null && child.Parents.FirstOrDefault(x => x.Id == personToRemoveChild.Id) != null)
            {
                this.UnLinkImplicitPartners(child);

                personToRemoveChild.Children.Remove(child);
                child.Parents.Remove(personToRemoveChild);

                this.OnGenealogyChanged(new GenealogyMasterEventArgs(this.persons));
            }
        }
    }
}
