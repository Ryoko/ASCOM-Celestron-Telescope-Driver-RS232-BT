using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CelestroneDriverTestProject
{
    using ASCOM.CelestronAdvancedBlueTooth.TelescopeWorker;

    using ASCOM.CelestronAdvancedBlueTooth;
    using NUnit.Framework;

    [TestFixture]    
    public class CelestroneInteraction12_Test
    {
        private ITelescopeInteraction _ti;
        private IDeviceWorker _dw;

        [TestFixtureSetUp]
        public void Setup()
        {
            
            //            _ti = new
        }
        
        [Test]
        public void AltAzm_Get()
        {
        }

    }
}
