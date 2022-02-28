using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform followTarget;
    public Vector3 offset;
    
    void Start() {
        if(followTarget != null) offset = transform.position - followTarget.position;
    }

    void LateUpdate() {
        if (followTarget != null) {
            transform.position = followTarget.position + offset;
            transform.LookAt(followTarget.position);
        }
    }
}
