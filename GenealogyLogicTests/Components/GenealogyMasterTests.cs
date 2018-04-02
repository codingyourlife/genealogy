namespace GenealogyLogicTests.Components
{
    using GenealogyLogic.Components;
    using GenealogyLogic.Exceptions;
    using GenealogyLogic.Helpers;
    using GenealogyLogic.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass]
    public class GenealogyMasterTests
    {
        public GenealogyMaster GenealogyMasterModel { get; private set; }

        [TestInitialize]
        public void Init()
        {
            MiniGuid.Reset();

            this.GenealogyMasterModel = new GenealogyMaster();
        }

        [TestMethod]
        public void AddPerson_NoParentOrChild()
        {
            //arrange

            //act
            this.GenealogyMasterModel.AddPerson(new UIPerson("Lonely", "Guy"));

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(1, persons.Count);
            Assert.AreEqual(0, persons[0].Parents.Count);
            Assert.AreEqual(0, persons[0].Children.Count);
        }

        [TestMethod]
        public void AddPersonTwice_AddedJustOnce()
        {
            //arrange

            //act
            var personInstance = new UIPerson("Lonely", "Guy");
            this.GenealogyMasterModel.AddPerson(personInstance);
            this.GenealogyMasterModel.AddPerson(personInstance);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(1, persons.Count);
        }

        [TestMethod]
        public void AddParent_LinksParentAndChildAccordingly()
        {
            //arrange
            var parent = new UIPerson("Homer", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parent);

            //act
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, parent);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(2, persons.Count);
            Assert.AreEqual(1, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(1, parent.Children.Count);
            Assert.AreEqual(0, parent.Parents.Count);
        }

        [TestMethod]
        public void AddParentTwice_AddedJustOnce()
        {
            //arrange
            var parent = new UIPerson("Homer", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parent);

            //act
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, parent);
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, parent);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(2, persons.Count);
            Assert.AreEqual(1, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(1, parent.Children.Count);
            Assert.AreEqual(0, parent.Parents.Count);
        }

        [TestMethod]
        public void AddChild_LinksParentAndChildAccordingly()
        {
            //arrange
            var parent = new UIPerson("Homer", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parent);

            //act
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parent, child);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(2, persons.Count);
            Assert.AreEqual(1, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(1, parent.Children.Count);
            Assert.AreEqual(0, parent.Parents.Count);
        }

        [TestMethod]
        public void AddChildTwice_AddedJustOnce()
        {
            //arrange
            var parent = new UIPerson("Homer", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parent);

            //act
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parent, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parent, child);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(2, persons.Count);
            Assert.AreEqual(1, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(1, parent.Children.Count);
            Assert.AreEqual(0, parent.Parents.Count);
        }

        [TestMethod]
        public void AddParents_LinksImplicitPartner()
        {
            //arrange
            var parentHusband = new UIPerson("Homer", "Simpson");
            var parentWife = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parentHusband);
            this.GenealogyMasterModel.AddPerson(parentWife);

            //act
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, parentHusband);
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, parentWife);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(3, persons.Count);
            Assert.AreEqual(2, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(1, parentHusband.Children.Count);
            Assert.AreEqual(1, parentWife.Children.Count);
            Assert.AreEqual(0, parentHusband.Parents.Count);
            Assert.AreEqual(0, parentWife.Parents.Count);

            Assert.AreEqual(1, parentHusband.Partners.Count);
            Assert.AreEqual(1, parentWife.Partners.Count);
            Assert.AreEqual(parentHusband.Partners[0], parentWife);
            Assert.AreEqual(parentWife.Partners[0], parentHusband);
        }

        [TestMethod]
        public void AddChildren_LinksImplicitPartner()
        {
            //arrange
            var parentHusband = new UIPerson("Homer", "Simpson");
            var parentWife = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parentHusband);
            this.GenealogyMasterModel.AddPerson(parentWife);

            //act
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parentHusband, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parentWife, child);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(3, persons.Count);
            Assert.AreEqual(2, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(1, parentHusband.Children.Count);
            Assert.AreEqual(1, parentWife.Children.Count);
            Assert.AreEqual(0, parentHusband.Parents.Count);
            Assert.AreEqual(0, parentWife.Parents.Count);

            Assert.AreEqual(1, parentHusband.Partners.Count);
            Assert.AreEqual(1, parentWife.Partners.Count);
            Assert.AreEqual(parentHusband.Partners[0], parentWife);
            Assert.AreEqual(parentWife.Partners[0], parentHusband);
        }

        [TestMethod]
        public void RemoveOneParent_UnLinksImplicitPartner()
        {
            //arrange
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, father);
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, mother);

            //act
            this.GenealogyMasterModel.RemovePerson(mother);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(2, persons.Count);
            Assert.AreEqual(1, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(1, father.Children.Count);
            Assert.AreEqual(0, mother.Children.Count);
            Assert.AreEqual(0, father.Parents.Count);
            Assert.AreEqual(0, mother.Parents.Count);

            Assert.AreEqual(0, father.Partners.Count);
            Assert.AreEqual(0, mother.Partners.Count);
        }

        [TestMethod]
        public void RemoveBothParents_UnLinksImplicitPartner()
        {
            //arrange
            var parentHusband = new UIPerson("Homer", "Simpson");
            var parentWife = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parentHusband);
            this.GenealogyMasterModel.AddPerson(parentWife);
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, parentHusband);
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, parentWife);

            //act
            this.GenealogyMasterModel.ParentRelationManager.RemoveParent(child, parentHusband);
            this.GenealogyMasterModel.ParentRelationManager.RemoveParent(child, parentWife);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(3, persons.Count);
            Assert.AreEqual(0, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(0, parentHusband.Children.Count);
            Assert.AreEqual(0, parentWife.Children.Count);
            Assert.AreEqual(0, parentHusband.Parents.Count);
            Assert.AreEqual(0, parentWife.Parents.Count);

            Assert.AreEqual(0, parentHusband.Partners.Count);
            Assert.AreEqual(0, parentWife.Partners.Count);
        }

        [TestMethod]
        public void RemoveChildren_UnLinksImplicitPartner()
        {
            //arrange
            var parentHusband = new UIPerson("Homer", "Simpson");
            var parentWife = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parentHusband);
            this.GenealogyMasterModel.AddPerson(parentWife);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parentHusband, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parentWife, child);

            //act
            this.GenealogyMasterModel.ChildRelationmanager.RemoveChild(parentHusband, child);
            this.GenealogyMasterModel.ChildRelationmanager.RemoveChild(parentWife, child);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(3, persons.Count);
            Assert.AreEqual(0, child.Parents.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual(0, parentHusband.Children.Count);
            Assert.AreEqual(0, parentWife.Children.Count);
            Assert.AreEqual(0, parentHusband.Parents.Count);
            Assert.AreEqual(0, parentWife.Parents.Count);

            Assert.AreEqual(0, parentHusband.Partners.Count);
            Assert.AreEqual(0, parentWife.Partners.Count);
        }

        [TestMethod]
        public void RemovePersonLinkedAsChild_UnLinksImplicitPartner()
        {
            //arrange
            var parentHusband = new UIPerson("Homer", "Simpson");
            var parentWife = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(parentHusband);
            this.GenealogyMasterModel.AddPerson(parentWife);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parentHusband, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(parentWife, child);

            //act
            this.GenealogyMasterModel.RemovePerson(child);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(2, persons.Count);
            Assert.AreEqual(0, parentHusband.Children.Count);
            Assert.AreEqual(0, parentWife.Children.Count);
            Assert.AreEqual(0, parentHusband.Parents.Count);
            Assert.AreEqual(0, parentWife.Parents.Count);

            Assert.AreEqual(0, parentHusband.Partners.Count);
            Assert.AreEqual(0, parentWife.Partners.Count);
        }

        [TestMethod]
        public void SwapPositions()
        {
            //arrange
            var personPos1 = new UIPerson("ShouldBeSecond", "Guy");
            var personPos2 = new UIPerson("ShouldBeFirst", "Guy");
            this.GenealogyMasterModel.AddPerson(personPos1);
            this.GenealogyMasterModel.AddPerson(personPos2);

            var personsBefore = this.GenealogyMasterModel.GetAll();
            Assert.AreNotEqual(personsBefore[0].FirstName, "ShouldBeFirst");
            Assert.AreNotEqual(personsBefore[1].FirstName, "ShouldBeSecond");

            //act
            this.GenealogyMasterModel.Swap(1, 0);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(persons[0].FirstName, "ShouldBeFirst");
            Assert.AreEqual(persons[1].FirstName, "ShouldBeSecond");

        }

        [TestMethod]
        public void FamilyWithBastard_AddParent_LinksChildrenToParentsAccordingly()
        {
            //arrange
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");

            var bitch = new UIPerson("Bitch", "Samson");
            var bastard = new UIPerson("Bastard", "Samson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(bitch);
            this.GenealogyMasterModel.AddPerson(bastard);

            //act
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, father);
            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, mother);

            this.GenealogyMasterModel.ParentRelationManager.AddParent(bastard, father);
            this.GenealogyMasterModel.ParentRelationManager.AddParent(bastard, bitch);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(5, persons.Count);
            Assert.AreEqual(2, father.Children.Count);
            Assert.AreEqual(1, mother.Children.Count);
            Assert.AreEqual(1, bitch.Children.Count);
            Assert.AreEqual(2, child.Parents.Count);
            Assert.AreEqual(2, bastard.Parents.Count);
        }

        [TestMethod]
        public void FamilyWithBastard_AddChild_LinksChildrenToParentsAccordingly()
        {
            //arrange
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");

            var bitch = new UIPerson("Bitch", "Samson");
            var bastard = new UIPerson("Bastard", "Samson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(bitch);
            this.GenealogyMasterModel.AddPerson(bastard);

            //act
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, bastard);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(bitch, bastard);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(5, persons.Count);
            Assert.AreEqual(2, father.Children.Count);
            Assert.AreEqual(1, mother.Children.Count);
            Assert.AreEqual(1, bitch.Children.Count);
            Assert.AreEqual(2, child.Parents.Count);
            Assert.AreEqual(2, bastard.Parents.Count);
        }

        [TestMethod]
        public void FamilyWithTwoChildren_OneChildPersonRemoved_LinksChildrenToParentsAccordingly()
        {
            //arrange
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child1 = new UIPerson("Bart", "Simpson");
            var child2 = new UIPerson("Lisa", "Simpson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child1);
            this.GenealogyMasterModel.AddPerson(child2);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child1);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child1);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child2);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child2);

            //act
            this.GenealogyMasterModel.RemovePerson(child2);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(3, persons.Count);
            Assert.AreEqual(1, father.Children.Count);
            Assert.AreEqual(1, mother.Children.Count);
            Assert.AreEqual(2, child1.Parents.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStateException))]
        public void InvalidState_AddThirdParent_ViaAddChild()
        {
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child1 = new UIPerson("Bart", "Simpson");
            var child2 = new UIPerson("Lisa", "Simpson");

            var bitch = new UIPerson("Bitch", "Samson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child1);
            this.GenealogyMasterModel.AddPerson(child2);
            this.GenealogyMasterModel.AddPerson(bitch);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child1);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child1);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child2);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child2);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(bitch, child1); //the invalid one

            var persons = this.GenealogyMasterModel.GetAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStateException))]
        public void InvalidState_AddThirdParent_ViaAddParent()
        {
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child1 = new UIPerson("Bart", "Simpson");
            var child2 = new UIPerson("Lisa", "Simpson");

            var bitch = new UIPerson("Bitch", "Samson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child1);
            this.GenealogyMasterModel.AddPerson(child2);
            this.GenealogyMasterModel.AddPerson(bitch);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child1);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child1);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child2);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child2);

            this.GenealogyMasterModel.ParentRelationManager.AddParent(child1, bitch); //the invalid one

            var persons = this.GenealogyMasterModel.GetAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStateException))]
        public void InvalidState_AddChildAsParent_ViaAddChild()
        {
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(child, father); //child with parent as child invalid

            var persons = this.GenealogyMasterModel.GetAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStateException))]
        public void InvalidState_AddChildAsParent_ViaAddParent()
        {
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("Marge", "Simpson");
            var child = new UIPerson("Bart", "Simpson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child);

            this.GenealogyMasterModel.ParentRelationManager.AddParent(father, child); //child with parent as child invalid

            var persons = this.GenealogyMasterModel.GetAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStateException))]
        public void InvalidState_GrandsonAlsoSon_ViaAddChild()
        {
            var father = new UIPerson("Homer", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            var granpa = new UIPerson("Abraham", "Simpson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(granpa);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(granpa, father);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(granpa, child); //invalid

            var persons = this.GenealogyMasterModel.GetAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidStateException))]
        public void InvalidState_GrandsonAlsoSon_ViaAddParent()
        {
            var father = new UIPerson("Homer", "Simpson");
            var child = new UIPerson("Bart", "Simpson");
            var granpa = new UIPerson("Abraham", "Simpson");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(child);
            this.GenealogyMasterModel.AddPerson(granpa);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(granpa, father);

            this.GenealogyMasterModel.ParentRelationManager.AddParent(child, granpa); //invalid

            var persons = this.GenealogyMasterModel.GetAll();
        }

        [TestMethod]
        public void TwoChildren_OneAllParentsRemoved_ViaRemoveParent_ValidatesAsExpected()
        {
            //arrange
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("marge", "Simpson");
            var child1 = new UIPerson("Lisa", "Simpson");
            var child2 = new UIPerson("Rod", "Flanders");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child1);
            this.GenealogyMasterModel.AddPerson(child2);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child1);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child1);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child2);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child2);

            //act
            this.GenealogyMasterModel.ParentRelationManager.RemoveParent(child2, father);
            this.GenealogyMasterModel.ParentRelationManager.RemoveParent(child2, mother);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(father.Children.Count, 1);
            Assert.AreEqual(mother.Children.Count, 1);
            Assert.AreEqual(child1.Parents.Count, 2);
            Assert.AreEqual(father.Partners.Count, 1);
            Assert.AreEqual(mother.Partners.Count, 1);
        }

        [TestMethod]
        public void TwoChildren_OneAllParentsRemoved_ViaRemoveChild_ValidatesAsExpected()
        {
            //arrange
            var father = new UIPerson("Homer", "Simpson");
            var mother = new UIPerson("marge", "Simpson");
            var child1 = new UIPerson("Lisa", "Simpson");
            var child2 = new UIPerson("Rod", "Flanders");

            this.GenealogyMasterModel.AddPerson(father);
            this.GenealogyMasterModel.AddPerson(mother);
            this.GenealogyMasterModel.AddPerson(child1);
            this.GenealogyMasterModel.AddPerson(child2);

            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child1);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child1);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(father, child2);
            this.GenealogyMasterModel.ChildRelationmanager.AddChild(mother, child2);

            //act
            this.GenealogyMasterModel.ChildRelationmanager.RemoveChild(father, child2);
            this.GenealogyMasterModel.ChildRelationmanager.RemoveChild(mother, child2);

            //assert
            var persons = this.GenealogyMasterModel.GetAll();
            Assert.AreEqual(father.Children.Count, 1);
            Assert.AreEqual(mother.Children.Count, 1);
            Assert.AreEqual(child1.Parents.Count, 2);
            Assert.AreEqual(father.Partners.Count, 1);
            Assert.AreEqual(mother.Partners.Count, 1);
        }

        [TestMethod]
        public void IdGenerator_1000People_NoDuplicates()
        {
            //arrange

            //act
            for (int i = 0; i < 999; i++)
            {
                var person = new UIPerson("FirstName", "LastName");
                this.GenealogyMasterModel.AddPerson(person, false);
            }

            //assert
            var people = this.GenealogyMasterModel.GetAll();
            var peopleIds = people.Select(x => x.Id);

            var distinct = people.GroupBy(person => person.Id).Select(g => g.First()).ToList();
            var distinctCount = distinct.Count;
            var duplicates = people.Except(distinct);
            var duplicatesCount = duplicates.Count();

            Assert.AreEqual(999, peopleIds.Count());
            Assert.AreEqual(999, distinctCount);
            Assert.AreEqual(0, duplicatesCount);
        }
    }
}
