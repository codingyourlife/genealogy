namespace GenealogyLogicTests.Helpers
{
    using GenealogyLogic.Exceptions;
    using GenealogyLogic.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class MiniGuidCoreTests
    {
        public MiniGuidCore MiniGuidCore { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            this.MiniGuidCore = new MiniGuidCore(0, 999);
        }

        [TestMethod]
        public void GenerateTwoGuids_Distinct()
        {
            var guid1 = this.MiniGuidCore.NewGuid();
            var guid2 = this.MiniGuidCore.NewGuid();

            Assert.AreNotEqual(guid1, guid2);
        }

        [TestMethod]
        public void Generate999Guids_Distinct()
        {
            //arrange
            List<int> usedGuids = new List<int>();

            //act
            for (int i = 0; i < 999; i++)
            {
                usedGuids.Add(this.MiniGuidCore.NewGuid());
            }

            //assert
            var distinct = usedGuids.GroupBy(id => id).Select(g => g.First()).ToList();
            var distinctCount = distinct.Count;
            var duplicates = usedGuids.Except(distinct);
            var duplicatesCount = duplicates.Count();

            Assert.AreEqual(999, usedGuids.Count);
            Assert.AreEqual(999, distinctCount);
            Assert.AreEqual(0, duplicatesCount);
        }

        [TestMethod]
        [ExpectedException(typeof(OutOfMiniGuidsException))]
        public void GenerateMoreThan999Guids_ThrowsOutOfMiniGuidsException()
        {
            //arrange

            //act
            for (int i = 0; i < 1000; i++)
            {
                this.MiniGuidCore.NewGuid();
            }

            //assert
        }
    }
}
