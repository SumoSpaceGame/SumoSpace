using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UtilTest
{
    public class Vec3UtilTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Vec3UtilTestSimplePasses()
        {
            Assert.IsTrue(Vec3Util.Vec2ToVec3Z(new Vector2(1,2), 3) == new Vector3(1, 2, 3));
            Assert.IsTrue(Vec3Util.Vec2ToXZ(new Vector2(1,3)) == new Vector3(1, 0, 3));
        }
    }
}
