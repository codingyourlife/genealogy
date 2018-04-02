namespace GenealogyLogic.Components
{
    using GenealogyLogic.EventArguments;
    using GenealogyLogic.Interfaces;
    using GenealogyLogic.Models.RelationManagers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GenealogyMaster
    {
        private List<IUIPerson> persons = new List<IUIPerson>();

        public GenealogyMaster()
        {
            this.ChildRelationmanager = new ChildRelationmanager(this.persons);
            this.ChildRelationmanager.GenealogyChanged += (obj, e) => { this.OnGenealogyChanged((GenealogyMasterEventArgs)e); };

            this.ParentRelationManager = new ParentRelationManager(this.persons);
            this.ParentRelationManager.GenealogyChanged += (obj, e) => { this.OnGenealogyChanged((GenealogyMasterEventArgs)e); };
        }

        public event EventHandler GenealogyChanged;

        protected virtual void OnGenealogyChanged(GenealogyMasterEventArgs e)
        {
            EventHandler handler = GenealogyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void Load(List<IUIPerson> persons)
        {
            this.persons = persons;
        }

        public ChildRelationmanager ChildRelationmanager { get; private set; }
        public ParentRelationManager ParentRelationManager { get; private set; }

        public List<IUIPerson> GetAll()
        {
            return this.persons;
        }

        public void AddPersonAsync(IUIPerson person, bool autoDefineGender = false)
        {
            Task.Run(() => AddPerson(person, autoDefineGender));
        }

        public void AddPerson(IUIPerson person, bool autoDefineGender = false)
        {
            person.FirstName = person.FirstName.Trim();
            person.LastName = person.LastName.Trim();

            if(!this.persons.Contains(person))
            {
                if(autoDefineGender)
                {
                    person.AutoDefineGender();
                }

                this.persons.Add(person);

                this.OnGenealogyChanged(new GenealogyMasterEventArgs(this.persons));
            }
        }

        private void UnLinkImplicitPartners(IUIPerson person)
        {
            IUIPerson lastParent = null;
            foreach (var parentOfPersonToRemoveParent in person.Parents)
            {
                if (lastParent != null)
                {
                    if (lastParent.Partners.Contains(parentOfPersonToRemoveParent))
                    {
                        if (!lastParent.Children.Intersect(parentOfPersonToRemoveParent.Children).Any())
                        {
                            lastParent.Partners.Remove(parentOfPersonToRemoveParent);
                            parentOfPersonToRemoveParent.Partners.Remove(lastParent);
                        }
                    }
                }

                lastParent = parentOfPersonToRemoveParent;
            }

            foreach (var partnerOfPersonToRemove in person.Partners)
            {
                partnerOfPersonToRemove.Partners.Remove(person);
            }
        }

        public void RemovePerson(IUIPerson person)
        {
            this.persons.Remove(person);
            this.RemovePersonFromAllChildren(person);
            this.RemovePersonFromAllParents(person);

            this.UnLinkImplicitPartners(person);

            person.Children.Clear();
            person.Parents.Clear();
            person.Partners.Clear();

            this.OnGenealogyChanged(new GenealogyMasterEventArgs(this.persons));
        }

        public void Swap(int listPositionA, int listPositionB)
        {
            this.persons = this.persons.Swap(listPositionA, listPositionB).ToList();
        }

        private void RemovePersonFromAllChildren(IUIPerson person)
        {
            foreach (var child in person.Children)
            {
                child.Parents.Remove(person);
            }
        }

        private void RemovePersonFromAllParents(IUIPerson person)
        {
            foreach (var parent in person.Parents)
            {
                parent.Children.Remove(person);
            }
        }

        internal IUIPerson GetPersonById(int parentId)
        {
            return this.persons.Single(x=>x.Id == parentId);
        }
    }
}
