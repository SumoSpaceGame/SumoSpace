using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Common.Map.PylonMap
{
    /// <summary>
    /// Temporary name/place for this.
    /// Builds the pylon maps graphics
    /// </summary>
    public class PylonBuilder : MonoBehaviour
    {
        public GameObject lineRendererPrefab;
        public List<PylonGraphics> pylonGraphicsList = new List<PylonGraphics>();
        
        public void Build(PylonMap map)
        {
            if (!map.initialized)
            {
                Debug.LogError("Failed to build map. Pylon map not initialized. ");
                return;
            }

            for (int i = pylonGraphicsList.Count - 1; i >= 0; i--)
            {
                if (pylonGraphicsList[i] == null)
                {
                    pylonGraphicsList.RemoveAt(i);
                }
            }
            

            List<PylonGraphics> currentLineRenderers = new List<PylonGraphics>(pylonGraphicsList);
            pylonGraphicsList.Clear();


            for (int i = 0; i < map.pointList.connections.Length; i += 2)
            {
                Pylon pylon1 = map.pylons[map.pointList.connections[i]];
                Pylon pylon2 = map.pylons[map.pointList.connections[i + 1]];

                PylonGraphics pylonGraphics;
                if (currentLineRenderers.Count > 0)
                {
                    pylonGraphics = currentLineRenderers.Last();
                    currentLineRenderers.RemoveAt(currentLineRenderers.Count - 1);
                    pylonGraphics.Reset();
                }
                else
                {
                    pylonGraphics = Instantiate(lineRendererPrefab).GetComponent<PylonGraphics>();
                    pylonGraphics.Init();
                }
                
                pylonGraphicsList.Add(pylonGraphics);
                pylonGraphics.SetPylons(pylon1, pylon2);
            }

            foreach (var go in currentLineRenderers)
            {
                Destroy(go);
            }
        }


        public void UpdateGraphics()
        {
            for (int i = pylonGraphicsList.Count - 1; i >= 0; i--)
            {
                if (pylonGraphicsList[i] == null)
                {
                    pylonGraphicsList.RemoveAt(i);
                }
                pylonGraphicsList[i].UpdateGraphics();
            }
        }

        public void DestroyGraphics()
        {
            foreach (var go in pylonGraphicsList)
            {
                Destroy(go);
            }
            
            pylonGraphicsList.Clear();
            
            return;
        }
    }
}