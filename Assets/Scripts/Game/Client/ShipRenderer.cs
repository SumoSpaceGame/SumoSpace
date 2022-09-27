using UnityEngine;

public class ShipRenderer : MonoBehaviour {

    [SerializeField] private TrailRenderer tracerEffect;
    public Transform gunMuzzle;
    [SerializeField] private float tracerRange;
    
    private TrailRenderer tracer;

    public void Shoot() {
        tracer = Instantiate(tracerEffect, gunMuzzle);
        Destroy(tracer, 0.08f);
        tracer.transform.position = gunMuzzle.position;
        tracer.AddPosition(gunMuzzle.position + gunMuzzle.forward * tracerRange);
    }

    public void StartBeam() {
        tracer = Instantiate(tracerEffect, gunMuzzle);
        tracer.AddPosition(gunMuzzle.position + gunMuzzle.forward * 100);
        //tracer.transform.position = gunMuzzle.position;
    }

    public void EndBeam() {
        Destroy(tracer);
    }

    public void Beam() {
        Physics.Raycast(gunMuzzle.position + gunMuzzle.forward * 2, gunMuzzle.forward, out var hit, 100f, 127);

        if (tracer == null) return;
        
        if (tracer.positionCount != 0)
            tracer.SetPosition(tracer.positionCount - 1, hit.collider ? hit.point : gunMuzzle.position + gunMuzzle.forward * 100);
        else
            tracer.AddPosition(gunMuzzle.position + gunMuzzle.forward * 100);
    }
}
