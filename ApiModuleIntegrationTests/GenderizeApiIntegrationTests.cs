namespace ApiModuleIntegrationTests
{
    using ApiModule;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GenderizeApiIntegrationTests
    {
        [TestClass]
        public class FirstNameIsFemaleIntegrationTests : GenderizeApiIntegrationTests
        {
            [TestMethod]
            public void MariaIsAGirlsName()
            {
                Assert.IsTrue(GenderizeApi.FirstNameIsFemale("Maria"));
            }

            [TestMethod]
            public void JohannesIsNotAGirlsName()
            {
                Assert.IsFalse(GenderizeApi.FirstNameIsFemale("Johannes"));
            }
        }

        [TestClass]
        public class FirstNameIsMaleIntegrationTests : GenderizeApiIntegrationTests
        {
            [TestMethod]
            public void MariaIsNotABoysName()
            {
                Assert.IsFalse(GenderizeApi.FirstNameIsMale("Maria"));
            }

            [TestMethod]
            public void JohannesIsABoysName()
            {
                Assert.IsTrue(GenderizeApi.FirstNameIsMale("Johannes"));
            }
        }
    }
}
