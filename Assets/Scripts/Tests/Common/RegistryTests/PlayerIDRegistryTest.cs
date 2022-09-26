using System.Collections;
using Game.Common.Registry;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.RegistryTests
{
    public class PlayerIDRegistryTest
    {
    
    
        [Test]
        public void PlayerIDRegistryTestSimplePasses()
        {
            var instance = ScriptableObject.CreateInstance<PlayerIDRegistry>();

            PlayerID tempData;
            Assert.IsFalse(instance.TryGetByNetworkID(0, out tempData));
            Assert.IsFalse(instance.TryGetByNetworkID(uint.MaxValue/2, out tempData));
            Assert.IsFalse(instance.TryGetByNetworkID(uint.MaxValue, out tempData));
        
        
            Assert.IsTrue(instance.RegisterPlayer(0, 1));
            Assert.IsTrue(instance.RegisterPlayer(uint.MaxValue/2, 2));
            Assert.IsTrue(instance.RegisterPlayer(uint.MaxValue, 3));
            
            Assert.IsFalse(instance.RegisterPlayer(0, 1));
            Assert.IsFalse(instance.RegisterPlayer(uint.MaxValue/2, 2));
            Assert.IsFalse(instance.RegisterPlayer(uint.MaxValue, 3));
        
        
            Assert.IsTrue(instance.TryGetByNetworkID(0, out tempData));
            Assert.IsTrue(instance.TryGetByNetworkID(uint.MaxValue/2, out tempData));
            Assert.IsTrue(instance.TryGetByNetworkID(uint.MaxValue, out tempData));
        
            instance.Reset();

            Assert.IsFalse(instance.TryGetByNetworkID(0, out tempData));
            Assert.IsFalse(instance.TryGetByNetworkID(uint.MaxValue/2, out tempData));
            Assert.IsFalse(instance.TryGetByNetworkID(uint.MaxValue, out tempData));
        }

    }
}
