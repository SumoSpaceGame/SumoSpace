using UnityEngine;
using UnityEngine.VFX;

namespace Game.Ships.Agility.Client
{
    public class AgilityRenderer: ShipRenderer
    {
        public GameObject muzzleFlash;
        public string MuzzlePrefabLocation = "AgilePrefab/Sphere/BulletSpawn/MuzzlePrefab";
        
        public void PrimaryMuzzleFlash()
        {
            transform.Find("../Sphere/BulletSpawn/MuzzlePrefab").GetComponent<VisualEffect>().Play();
        }
    }
}