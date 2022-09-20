using UnityEngine;

namespace Game.Common.Map
{
    public class TestWithin : MonoBehaviour
    {
        public float radius;
        public PylonMap.PylonMap map;
        private void OnDrawGizmos()
        {
            map.Init();
            map.UpdateMap(0);
            if (map.WithinMap(new Vector2(this.transform.position.x, this.transform.position.z)))
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawWireCube(this.transform.position , Vector3.one * radius);

            if (map.WithinMap(new Vector2(this.transform.position.x, this.transform.position.z), radius))
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }   
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }
}
