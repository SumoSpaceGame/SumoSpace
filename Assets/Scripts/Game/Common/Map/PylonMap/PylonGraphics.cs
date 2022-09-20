using UnityEngine;

namespace Game.Common.Map.PylonMap
{
    public class PylonGraphics : MonoBehaviour
    {
        public Pylon pylonA, pylonB;
        public LineRenderer lineRenderer;

        public void Update()
        {
            lineRenderer.SetPositions(new Vector3[] { pylonA.transform.position, pylonB.transform.position });
        }
    }
}