using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CelestroneDriverTestProject
{
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.HardwareWorker;
    using ASCOM.CelestronAdvancedBlueTooth.CelestroneDriver.TelescopeWorker;

    using Moq;
    using NUnit.Framework;

    [TestFixture]    
    public class CelestroneInteraction12_Test
    {
        private ITelescopeInteraction _ti;
        private IDeviceWorker _dw;

        private Mock<IDeviceWorker> dMock;
        private MockTelescope _telescope;

        [TestFixtureSetUp]
        public void Setup()
        {
            _telescope = new MockTelescope(1.6, TelescopeType.GT);

            //byte[] buff;
            dMock = new Mock<IDeviceWorker>();
            dMock.Setup(x => x.Connect(It.IsAny<object>())).Returns(true);
            dMock.Setup(x => x.Transfer(It.IsAny<byte[]>())).Returns((byte[] buff) => _telescope.exchange(buff));// ((byte[] buff) => buff);
            dMock.Setup(x => x.Transfer(It.IsAny<string>())).Returns((string buff) => _telescope.exchange(buff));
            dMock.Setup(x => x.CheckConnected(It.IsAny<string>()));
            dMock.Setup(x => x.Disconnect());
            dMock.Setup(x => x.IsConnected).Returns(true);
            _dw = dMock.Object;
        }
        
        private void SetTelescopeInteraction(double version = 1.6)
        {
            _telescope.FirmwareVersion = version;
            _ti = ATelescopeInteraction.GeTelescopeInteraction(_dw);
        }

        [TestCase(new object[] { 1.2, "Z" })]
        [TestCase(new object[] { 2.3, "z" })]
        public void AltAzm_Get(double ver, string val)
        {
            this.SetTelescopeInteraction(ver);
            var altAzm = _ti.AltAzm;
            dMock.Verify((x) =>x.Transfer(val));
        }

        [TestCase(new object[] { 1.2, "E" })]
        [TestCase(new object[] { 1.6, "e" })]
        public void RaDec_Get(double ver, string val)
        {
            this.SetTelescopeInteraction(ver);
            var RaDec = _ti.RaDec;
            dMock.Verify((x) => x.Transfer(val));
        }

    }

    //[TestFixture]
    //public class MockTelescope_Test
    //{
    //    [TestCase(new object[]{ 1.6, 1, 6 })]
    //    [TestCase(new object[] { 1.10, 1, 1 })]
    //    [TestCase(new object[] { 3.16, 3, 16 })]
    //    private static void TestMakeVersion(double vers, byte v1, byte v2)
    //    {
    //        var mt = new MockTelescope(vers, TelescopeType.NextStar);
    //        var ver = mt.makeVersion();
    //        Assert.AreEqual(2, ver.Length);
    //        Assert.AreEqual(v1, ver[0]);
    //        Assert.AreEqual(v2, ver[1]);
    //    }

    //}


}
