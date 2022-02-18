using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRenderer : MonoBehaviour {

    [SerializeField] private TrailRenderer tracerEffect;
    [SerializeField] private Transform gunMuzzle;
    private Ray ray;
    private RaycastHit hit;
    private Coroutine shootCoroutine;


    public void Shoot() {
        shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine() {
        while (true) {
            var tracer = Instantiate(tracerEffect, gunMuzzle);
            Destroy(tracer, 0.08f);
            tracer.AddPosition(gunMuzzle.position);
            ray.origin = gunMuzzle.position;
            ray.direction = gunMuzzle.forward;
            if (Physics.Raycast(ray, out hit)) {
                tracer.transform.position = hit.point;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StopShoot() {
        StopCoroutine(shootCoroutine);
    }
}
