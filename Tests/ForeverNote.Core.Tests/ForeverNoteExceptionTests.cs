using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ForeverNote.Core.Tests
{
    [TestClass()]
    public class ForeverNoteExceptionTests {
        [TestMethod()]
        public void pass_individual_message_to_exception() {
            try {
                throw new ForeverNoteException("lorem ipsum 123");
            }
            catch(Exception ex) {
                Assert.AreEqual("lorem ipsum 123", ex.Message);
            }            
        }
    }
}