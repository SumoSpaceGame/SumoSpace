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
            Assert.IsFalse(instance.TryGet(0, out tempData));
            Assert.IsFalse(instance.TryGet(uint.MaxValue/2, out tempData));
            Assert.IsFalse(instance.TryGet(uint.MaxValue, out tempData));
        
        
            Assert.IsTrue(instance.RegisterPlayer(0));
            Assert.IsTrue(instance.RegisterPlayer(uint.MaxValue/2));
            Assert.IsTrue(instance.RegisterPlayer(uint.MaxValue));
            
            Assert.IsFalse(instance.RegisterPlayer(0));
            Assert.IsFalse(instance.RegisterPlayer(uint.MaxValue/2));
            Assert.IsFalse(instance.RegisterPlayer(uint.MaxValue));
        
        
            Assert.IsTrue(instance.TryGet(0, out tempData));
            Assert.IsTrue(instance.TryGet(uint.MaxValue/2, out tempData));
            Assert.IsTrue(instance.TryGet(uint.MaxValue, out tempData));
        
            instance.Reset();

            Assert.IsFalse(instance.TryGet(0, out tempData));
            Assert.IsFalse(instance.TryGet(uint.MaxValue/2, out tempData));
            Assert.IsFalse(instance.TryGet(uint.MaxValue, out tempData));
        }

    }
}
