using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadoutUI : MonoBehaviour
{

    [SerializeField]
    public struct LoadoutUIField
    {
        public GameObject gameObject;
        public TextMeshProUGUI textUI;
    }

    public List<List<LoadoutUIField>> teamsUI = new List<List<LoadoutUIField>>();
    
    public void Activate(int[] teamCounts)
    {

        if (teamCounts.Length > teamsUI.Count)
        {
            Debug.LogError("Failed to activate UI, team count array does not match amount of teams");
            return;
        }

        var curPlayerCount = 0;
        
        for (int i = 0; i < teamCounts.Length; i++)
        {
            var teamLoadoutsUI = teamsUI[i];

            if (teamLoadoutsUI.Count > teamCounts[i])
            {
                Debug.LogError("Tried to activate too many members for a team. " + $"{i} - {teamsUI}");
                return;
            }


            for (int x = 0; x < teamLoadoutsUI.Count; x++)
            {
                var playerUI = teamLoadoutsUI[x];
                playerUI.gameObject.SetActive(true);
                playerUI.textUI.text = "Player " + (++curPlayerCount);
            }
        }
    }

    public void Display(int teamNumber, int teamPlayerNumber, int tempLoadoutSelection)
    {
        if (teamNumber > teamsUI.Count)
        {
            Debug.LogError("Failed to display invalid teamNumber, greater than team count "+ $" {teamNumber} {teamsUI.Count}");
            return;
        }

        var teamUI = teamsUI[teamNumber];
        

    }


    public void Reset()
    {
        foreach (var set in teamsUI)
        {
            foreach (var playerUI in set)
            {
                playerUI.gameObject.SetActive(false);
                playerUI.textUI.text = "";
            }
        }
    }
    
}
