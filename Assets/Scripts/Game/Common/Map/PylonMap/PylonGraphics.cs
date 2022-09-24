using UnityEngine;

namespace Game.Common.Map.PylonMap
{
    public class PylonGraphics : MonoBehaviour
    {
        public Pylon pylonA, pylonB;
        public LineRenderer lineRenderer;
        
        public void Init()
        {
        
        }
        public void Reset()
        {
            
        }
        
        public void UpdateGraphics()
        {
            lineRenderer.SetPositions(new Vector3[] { pylonA.transform.position, pylonB.transform.position });
        }

        public void SetPylons(Pylon a, Pylon b)
        {
            pylonA = a;
            pylonB = b;
            lineRenderer.SetPositions(new Vector3[] { pylonA.transform.position, pylonB.transform.position });
        }
    }
}