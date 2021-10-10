using System;
using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.SimpleJSON;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "GameMatchSettings", menuName = "GameSO/Game Match Settings")]
public class GameMatchSettings : ScriptableObject
{
    
    public bool IsDataSynced { get; private set; }
    
    
    [SerializeField]public int TeamCount;
    [SerializeField]public int TeamSize;

    /// <summary>
    /// Match id of the player. On server side this is a temp variable that is not used.
    /// </summary>
    [SerializeField]public int ClientMatchID;
    [SerializeField] public int ClientTeam;
    [SerializeField] public int ClientTeamPosition;
    public int PlayerCount
    {
        get
        {
            return TeamSize * TeamCount;
        }
    }


    private void Awake()
    {
        IsDataSynced = false;
    }

    public void Reset()
    {
        IsDataSynced = false;
    }

    public void Sync(string data)
    {
        
        JsonUtility.FromJsonOverwrite(data, this);

        IsDataSynced = true;
    }

    
    public string GetSerialized()
    {
        return JsonUtility.ToJson(this);
    }
}
