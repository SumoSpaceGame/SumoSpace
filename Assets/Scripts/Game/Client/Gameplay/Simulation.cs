using System.Collections.Generic;
using Game.Common.Instances;
using UnityEngine;

public class Simulation : MonoBehaviour, IGameInstance {
    private readonly List<SimulationObjectPair> transforms = new List<SimulationObjectPair>();

    private void Awake()
    {
        MainInstances.Add(this);
        Debug.Log("Added main instance for simulation");
    }

    private void OnDestroy()
    {
        
        MainInstances.Remove<Simulation>();
        Debug.Log("Removed main instance for simulation");
    }

    public void Add(SimulationObject simObject)
    {
        /*simObject.representative.transform.localPosition =
            new Vector3(simObject.transform.position.x, 0, simObject.transform.position.y);*/
        transforms.Add(new SimulationObjectPair(simObject.transform, simObject.representative.transform));
    }

    public void Update() {
        for (int i = transforms.Count - 1; i >= 0; i--) {
            var oldLocalPosition = transforms[i].repObjTransform.localPosition;
            var newLocalPosition = transforms[i].simObjTransform.localPosition;

            var oldAngle = transforms[i].repObjTransform.eulerAngles;
            var newAngle = transforms[i].simObjTransform.eulerAngles;
        
            transforms[i].repObjTransform.localPosition = new Vector3(newLocalPosition.x, oldLocalPosition.y, newLocalPosition.y);
            transforms[i].repObjTransform.eulerAngles = new Vector3(oldAngle.x, -newAngle.z, oldAngle.z);
        }
    }

    public void Remove(Transform simTransform) {
        transforms.RemoveAll(x => x.simObjTransform == simTransform);
    }

    private readonly struct SimulationObjectPair {
        public readonly Transform simObjTransform;
        public readonly Transform repObjTransform;

        public SimulationObjectPair(Transform simObjTransform, Transform repObjTransform) {
            this.simObjTransform = simObjTransform;
            this.repObjTransform = repObjTransform;
        }
    }
}
