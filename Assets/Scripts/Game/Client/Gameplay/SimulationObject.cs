using UnityEngine;

public class SimulationObject : MonoBehaviour {

    public GameObject representative;

    [SerializeField] private bool create;
    
    //private Transform cachedTransform;

    private void Start() {
        var sim = GetComponentInParent<Simulation>();
        sim.Add(this);
        if (create) {
            var go = Instantiate(representative, GameObject.Find("GameView").transform, false);
            representative = go;
        }
    }

    /*private void Update() {
        //var oldLocalPosition = representative.localPosition;
        //var newLocalPosition = cachedTransform.localPosition;

        //var oldAngle = representative.eulerAngles;
        //var newAngle = cachedTransform.eulerAngles;
        
        //representative.localPosition = new Vector3(newLocalPosition.x, oldLocalPosition.y, newLocalPosition.y);
        //representative.eulerAngles = new Vector3(oldAngle.x, -newAngle.z, oldAngle.z);
    }*/
}
