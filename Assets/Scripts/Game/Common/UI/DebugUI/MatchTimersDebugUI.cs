using System.Collections.Generic;
using Game.Common.Instances;
using Game.Common.Networking.Misc;
using TMPro;
using UnityEngine;

public class MatchTimersDebugUI : MonoBehaviour
{
    public GameObject MatchTimerTextFieldPrefab;
    public GameObject MatchTimerTextLocations;

    public List<GameObject> MatchTimeUIS = new List<GameObject>();
    

    // Update is called once per frame
    void Update()
    {
        var manager = MainPersistantInstances.Get<MatchNetworkTimerManager>();
        if (manager == null) return;

        var timers = manager.GetTimers();

        if (timers.Length > MatchTimeUIS.Count)
        {
            int amount = timers.Length - MatchTimeUIS.Count;

            for (int i = 0; i < amount; i++)
            {
                var go = Instantiate(MatchTimerTextFieldPrefab, MatchTimerTextLocations.transform);
                MatchTimeUIS.Add(go);
                
            }
            
        }
        else
        {
            for (int i = 0; i < MatchTimeUIS.Count; i++)
            {
                if (i > timers.Length - 1)
                {
                    for (int j = MatchTimeUIS.Count; j >= i; j--)
                    {
                        Destroy(MatchTimeUIS[j]);
                        MatchTimeUIS.RemoveAt(j);
                    }

                    break;
                }

                var text = MatchTimeUIS[i].GetComponent<TextMeshProUGUI>();
                text.text = GetTimerText(timers[i]);
            }
        }
    }


    public string GetTimerText(MatchNetworkTimer timer)
    {
        return $"{timer.ID} - {timer.ToString()}";
    }
}
