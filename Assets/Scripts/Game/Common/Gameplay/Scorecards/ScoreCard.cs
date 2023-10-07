using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ScoreCards keep track of scoring data per player
/// </summary>
public class ScoreCard
{
    public int Kills;
    public int Deaths;
    public int Assists; // If you hit a player within X time of them dying, then you get the assist. Could be more percise later on
    
    public float FriendlyDamageTaken;
    public float FriendlyDamageGiven;
    public float EnemyDamageTaken;
    public float EnemyDamageGiven;
    public float NeutralDamageTaken; // Environmental damage
    public float NeutralDamageGiven; // Environmental damage

    public int shotsHit;
    public int shotsMiss;

    public float DistanceTraveled;
    public float TimeSpentOutOfBounds;
    
}

