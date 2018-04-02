namespace GenealogyLogicTests.Components
{
    using GenealogyLogic.Components;
    using GenealogyLogic.Interfaces;
    using GenealogyLogic.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class PersistorCoreTests
    {
        public PersistorCore PersistorCoreTestsModel { get; private set; }

        [TestInitialize]
        public void Init()
        {
            this.PersistorCoreTestsModel = new PersistorCore();
        }

        [TestMethod]
        public void SimplePerson_Serialization_Deserialization_PropertyEqualityCheck()
        {
            //arrange
            var genealogyMaster = new GenealogyMaster();
            var person = new UIPerson("Firstname", "Lastname");
            person.IsFemale = true;
            person.Biography = "My super biography";
            person.IsHidden = true;
            person.DateOfBirth = new DateTime(1990, 12, 10);
            person.DateOfDeath = new DateTime(1990, 12, 12);

            //act
            genealogyMaster.AddPerson(person);
            var restoredPersons = this.SerializeThenDeserializePersons(genealogyMaster.GetAll());

            //assert
            var restoredPerson = restoredPersons.SingleOrDefault();

            Assert.AreEqual(1, restoredPersons.Count);
            Assert.AreEqual(person.FirstName, restoredPerson.FirstName);
            Assert.AreEqual(person.LastName, restoredPerson.LastName);
            Assert.AreEqual(person.IsFemale, restoredPerson.IsFemale);
            Assert.AreEqual(person.Biography, restoredPerson.Biography);
            Assert.AreEqual(person.IsHidden, restoredPerson.IsHidden);
            Assert.AreEqual(person.DateOfBirth, restoredPerson.DateOfBirth);
            Assert.AreEqual(person.DateOfDeath, restoredPerson.DateOfDeath);
        }

        [TestMethod]
        public void PersonWithParent_Serialization_Deserialization_PropertyEqualityCheck()
        {
            //arrange
            var genealogyMaster = new GenealogyMaster();
            var child = new UIPerson("Bart", "Simpson");
            var parent = new UIPerson("Marge", "Simpson");
            genealogyMaster.AddPerson(parent);
            genealogyMaster.AddPerson(child);

            //act
            genealogyMaster.ParentRelationManager.AddParent(child, parent);
            var restoredPersons = this.SerializeThenDeserializePersons(genealogyMaster.GetAll());

            //assert
            var restoredChild = restoredPersons.FirstOrDefault(x=> x.FirstName == "Bart");

            Assert.AreEqual(2, restoredPersons.Count);
            Assert.AreEqual(1, restoredChild.Parents.Count);
            Assert.AreEqual(0, restoredChild.Children.Count);
            Assert.AreEqual(1, restoredChild.Parents[0].Children.Count);
            Assert.AreEqual(0, restoredChild.Parents[0].Parents.Count);
        }

        private List<IUIPerson> SerializeThenDeserializePersons(List<IUIPerson> persons)
        {
            this.PersistorCoreTestsModel.Persist("unitTestPersons.json", "unitTestPersons.bak.json", persons);
            var restoredPersons = this.PersistorCoreTestsModel.Restore("unitTestPersons.json");

            return restoredPersons;
        }
    }
}
