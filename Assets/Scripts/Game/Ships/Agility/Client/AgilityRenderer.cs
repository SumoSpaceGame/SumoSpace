using UnityEngine;
using UnityEngine.VFX;

namespace Game.Ships.Agility.Client
{
    public class AgilityRenderer: ShipRenderer
    {
        public GameObject muzzleFlash;
        
        public void PrimaryMuzzleFlash()
        {
            muzzleFlash.transform.Find("Sphere/AgileShip/MuzzlePrefab").GetComponent<VisualEffect>().Play();
        }
    }
}