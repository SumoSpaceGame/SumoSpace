using UnityEngine;
using System.Collections.Generic;

public class Simulation : MonoBehaviour {
    private List<SimulationObjectPair> transforms = new List<SimulationObjectPair>();

    public void Add(SimulationObject simObject) {
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

    struct SimulationObjectPair {
        public Transform simObjTransform;
        public Transform repObjTransform;

        public SimulationObjectPair(Transform simObjTransform, Transform repObjTransform) {
            this.simObjTransform = simObjTransform;
            this.repObjTransform = repObjTransform;
        }
    }
}
