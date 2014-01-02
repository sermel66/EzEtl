using Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Configuration_Test
{
    
    
    /// <summary>
    ///This is a test class for ConfigurationTest and is intended
    ///to contain all ConfigurationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConfigurationTest
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
        ///A test for Configuration Constructor
        ///</summary>
        [TestMethod()]
        public void ConfigurationConstructorTest()
        {
            string configFilePath = @"C:\Users\sergey\Source\Repos\EzEtl\Test\AdHoc_take2.xml";
            List<string> processedConfigFilePathList = null;
            Configuration.Configuration target = new Configuration.Configuration(configFilePath, processedConfigFilePathList);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Configuration Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Configuration.dll")]
        public void ConfigurationConstructorTest1()
        {
            Configuration_Accessor target = new Configuration_Accessor();
            Assert.IsNotNull(target);
        }
    }
}
