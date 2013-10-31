using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CelestroneDriverTestProject
{
    
    using CelestroneDriver.HardwareWorker;
    using CelestroneDriver.TelescopeWorker;

    using Moq;
    using NUnit.Framework;

    [TestFixture]    
    public class CelestroneInteraction12_Test
    {
        private ITelescopeInteraction _ti;
        private IDeviceWorker _dw;

        [TestFixtureSetUp]
        public void Setup()
        {
            byte[] buff;
            var dx = new Moq.Mock<IDeviceWorker>();
            dx.Setup(x => x.Connect(It.IsAny<object>())).Returns(true);
            dx.Setup(x => x.Transfer(It.IsAny<byte[]>())).Returns((byte[] b) => b);// ((byte[] buff) => buff);
            dx.Setup(x => x.Transfer(It.IsAny<string>())).Returns("#");
            dx.Setup(x => x.CheckConnected(It.IsAny<string>()));
            dx.Setup(x => x.Disconnect());
            dx.Setup(x => x.IsConnected).Returns(true);
            _dw = dx.Object;
            var ti = ATelescopeInteraction.GeTelescopeInteraction(_dw);
        }
        

        [Test]
        public void AltAzm_Get()
        {
            var ti = 1;
        }

    }

    [TestFixture]
    public class MockTelescope_Test
    {
        [TestCase(new object[]{ 1.6, 1, 6 })]
        [TestCase(new object[] { 1.10, 1, 1 })]
        [TestCase(new object[] { 3.16, 3, 16 })]
        private static void TestMakeVersion(double vers, byte v1, byte v2)
        {
            var mt = new MockTelescope(vers, TelescopeType.NextStar);
            var ver = mt.makeVersion();
            Assert.AreEqual(2, ver.Length);
            Assert.AreEqual(v1, ver[0]);
            Assert.AreEqual(v2, ver[1]);
        }

    }


}
