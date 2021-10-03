using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform followTarget;
    private Vector3 offset;
    
    void Start() {
        offset = transform.position - followTarget.position;
    }

    void LateUpdate() {
        transform.position = followTarget.position + offset;
    }
}
