using System;
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
        var gameView = GameObject.Find("GameView");

        if (gameView == null)
        {
            Debug.LogError("Failed to find game view");
        }
        var go = Instantiate(representative, gameView.transform, false);
        representative = go;
        sim.Add(this);
    }

    private void OnEnable()
    {
        representative.SetActive(true);
    }

    private void OnDisable()
    {
        if(representative) representative.SetActive(false);
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
