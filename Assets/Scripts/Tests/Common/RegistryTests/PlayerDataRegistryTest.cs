using System.Collections;
using Game.Common.Registry;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.RegistryTests
{
    public class PlayerDataRegistryTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void PlayerDataRegistryTestSimplePasses()
        {
            //Initialization
            var instance = ScriptableObject.CreateInstance<PlayerDataRegistry>();
            var idRegistry = ScriptableObject.CreateInstance<PlayerIDRegistry>();

            idRegistry.RegisterPlayer(0);
            idRegistry.RegisterPlayer(1);
            idRegistry.RegisterPlayer(2);

            var playerOne = idRegistry.Get(0);
            var playerTwo = idRegistry.Get(1);
            var playerThree = idRegistry.Get(2);
            
            //Testing
            
            PlayerData data;
            bool success;
            
            Assert.IsFalse(instance.TryGet(playerOne, out data));
            Assert.IsFalse(instance.TryGet(playerTwo, out data));
            Assert.IsFalse(instance.TryGet(playerThree, out data));
            
            success = instance.Add(playerOne, new PlayerData()
            {
                PlayerMatchID = 100
            });
            
            Assert.IsTrue(success);
            
            instance.TryGet(playerOne, out data);
            Assert.IsTrue(data.PlayerMatchID == 100);
            Assert.IsFalse(instance.TryGet(playerTwo, out data));
            Assert.IsFalse(instance.TryGet(playerThree, out data));

            
            success = instance.Add(playerTwo, new PlayerData()
            {
                PlayerMatchID = 200
            });
            
            Assert.IsTrue(success);
            
            Assert.IsTrue(instance.TryGet(playerOne, out data));
            instance.TryGet(playerTwo, out data);
            Assert.IsTrue(data.PlayerMatchID == 200);
            Assert.IsFalse(instance.TryGet(playerThree, out data));
            
            
            success = instance.Add(playerThree, new PlayerData()
            {
                PlayerMatchID = 300
            });
            
            Assert.IsTrue(success);
            
            Assert.IsTrue(instance.TryGet(playerOne, out data));
            Assert.IsTrue(instance.TryGet(playerTwo, out data));
            instance.TryGet(playerThree, out data);
            Assert.IsTrue(data.PlayerMatchID == 300);
            
            
            Assert.IsFalse(instance.Add(playerOne, new PlayerData()));
            Assert.IsFalse(instance.Add(playerTwo, new PlayerData()));
            Assert.IsFalse(instance.Add(playerThree, new PlayerData()));
        }

    }
}
