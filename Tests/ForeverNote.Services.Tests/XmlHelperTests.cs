using ForeverNote.Services.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ForeverNote.Services.Tests
{
    [TestClass()]
    public class XmlHelperTests
    {

        [TestMethod()]
        public void XmlEncodeAsIsTest()
        {
            Assert.IsNull(XmlHelper.XmlEncode(null));

            string actual = "anything";
            Assert.AreEqual(actual, XmlHelper.XmlEncodeAsIs(actual));
        }
    }
}
