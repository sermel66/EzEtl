using Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Utilities_Test
{
    
    
    /// <summary>
    ///This is a test class for SimpleLogTest and is intended
    ///to contain all SimpleLogTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SimpleLogTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for SetUpLog
        ///</summary>
        [TestMethod()]
        public void SetUpLogTest()
        {
            string fqLogFilePath = @"C:\Temp\SetUpLogTest.log";
            SimpleLogEventType maxLoggingVerbosity = SimpleLogEventType.Error; 
            SimpleLog.SetUpLog(fqLogFilePath, maxLoggingVerbosity, true);
            Assert.IsTrue(SimpleLog.IsInitialized);
        }

        /// <summary>
        ///A test for ToLog
        ///</summary>
        [TestMethod()]
        public void ToLogTest()
        {
            string message = "Test message";
            SimpleLogEventType entryType = SimpleLogEventType.Error;
            SimpleLog.ToLog(message, entryType);
            Assert.Inconclusive("Verify the actual log file and .bak file");
        }

    }
}
