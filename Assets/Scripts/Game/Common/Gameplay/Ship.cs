using UnityEngine;

public class Ship : MonoBehaviour {
    public Vector2 velocity;
    public float rotation;

    public Rigidbody2D rb;
    public bool invulnerable;

    void Update() {
        rb.rotation = rotation;
        rb.velocity = velocity;
    }
}