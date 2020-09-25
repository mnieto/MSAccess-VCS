using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asvn;

namespace asvnTest {
    /// <summary>
    /// Summary description for exportTest
    /// </summary>
    [TestClass]
    public class ExportTest {
        public ExportTest() {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //DeploymentItems are relative to the folder that contains the .sln file
        //http://stackoverflow.com/questions/1822612/what-is-the-relative-path-root-of-deploymentitemattribute
        [TestMethod]
        [DeploymentItem(@"asvnTest\asvnTest2003.mdb")]
        [DeploymentItem(@"asvnTest\testFolder\")]
        public void ExportAllTest() {
            TestHelper helper = new TestHelper(TestContext);
            helper.StartProcess("asvn.exe", "e asvnTest2003.mdb asvnTest *.*");
            Assert.IsTrue(helper.ProcessOutput == asvnTest.Properties.Resources.ExportAllOutput);
        }

        [TestMethod]
        public void UsageTest() {
            TestHelper helper = new TestHelper(TestContext);
            helper.StartProcess("asvn.exe", "h");
            Assert.IsTrue(helper.ProcessOutput == asvn.Properties.Resources.Usage);
        }

        [TestMethod]
        [DeploymentItem(@"asvnTest\asvnTest2003.mdb")]
        [DeploymentItem(@"asvnTest\testFolder\")]
        public void ExportAllFormsTest() {
            TestHelper helper = new TestHelper(TestContext);
            helper.StartProcess("asvn.exe", "e asvnTest2003.mdb asvnTest frm.*");
            Assert.IsTrue(helper.ProcessOutput == asvnTest.Properties.Resources.ExportAllFormsOutput);
        }        

    }
}
