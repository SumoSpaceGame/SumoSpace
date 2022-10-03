using Game.Common.Instances;
using UnityEngine;

public class SimulationObject : MonoBehaviour {

    public GameObject representative;

    [SerializeField] private bool create;
    
    //private Transform cachedTransform;

    private void Start()
    {
        if (create)
        {
            Create();
        }
    }

    public void Create()
    {
        var sim = MainInstances.Get<Simulation>();
        var go = Instantiate(representative, GameObject.Find("GameView").transform, false);
        representative = go;
        sim.Add(this);
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
