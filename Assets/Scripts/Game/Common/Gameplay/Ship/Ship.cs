using UnityEngine;

public class Ship : MonoBehaviour {
    public float Rotation => transform.eulerAngles.y;
    public Vector3 Position => transform.position;
    public bool invulnerable;
    public Animator animator;
    
    public bool isPlayer;
    public ushort playerMatchID;
}
