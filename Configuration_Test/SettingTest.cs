using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configuration;

namespace Configuration_Test
{
    [TestClass]
    public class SettingTest
    {
        [TestMethod]
        public void StringSetting()
        {
            Setting setting = new Setting("SomeKey", typeof(String));
            Assert.IsNotNull(setting);

            Assert.IsFalse(setting.IsPresent);
            Assert.IsFalse(setting.IsValid);

            string value = "Some Value";
            setting.RawValue = value;

            Assert.AreEqual(value, setting.StringValue);

            Assert.IsTrue(setting.IsPresent);
            Assert.IsTrue(setting.IsValid);
            Assert.AreEqual(setting.Message, string.Empty);

        }
    }
}
