using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a ship's kinematic properties (position, vel, etc)
 */
public class Ship3D : MonoBehaviour {
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 rotation;

    void Update() {
        transform.eulerAngles = rotation;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }
}
