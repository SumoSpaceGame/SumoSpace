using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRenderer : MonoBehaviour {

    [SerializeField] private TrailRenderer tracerEffect;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private float tracerRange;


    public void Shoot() {
        var tracer = Instantiate(tracerEffect, gunMuzzle);
        Destroy(tracer, 0.08f);
        tracer.transform.position = gunMuzzle.position;
        tracer.AddPosition(gunMuzzle.position + gunMuzzle.forward * tracerRange);
    }
}
