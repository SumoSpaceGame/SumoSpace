using Game.Common.Instances;
using Game.Common.UI;
using Game.Common.Networking;
using System.Collections;
using System.Net.Sockets;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;

public class HeavyBurstClientBehaviour : RenderableAbilityBehaviour<HeavyBurstAbility>
{
    private RectTransform _innerBar;
    private AgentMovementNetworkManager _networkMovement;
    private GameObject _representative;
    [SerializeField]
    private GameObject _burstVFXPrefab;

    // Set up progress bar for charge.
    private void Start()
    {
        _representative = shipManager.simulationObject.representative;
        Transform chargeBar = MainPersistantInstances.Get<MasterUIController>().transform.GetChild(0).Find("TempChargeBar");
        if (!shipManager.isPlayer)
            return;
        chargeBar.gameObject.SetActive(true);
        chargeBar.GetChild(1).GetComponent<RectTransform>().anchoredPosition 
            = new Vector2(chargeBar.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x,
            (float)(400 * Ability.MinCharge) / ushort.MaxValue);
        _innerBar = chargeBar.GetChild(0).GetComponent<RectTransform>();
        _innerBar.anchoredPosition = new Vector2(_innerBar.anchoredPosition.x, 0);
        _innerBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        _networkMovement = shipManager.networkMovement;
    }

    // Maintain progress bar for charge.
    private void Update()
    {
        ushort charge = _networkMovement.TempPassiveCharge;
        _burstVFXPrefab.GetComponent<VisualEffect>().SetFloat("Size", Ability.MaxRadius((ushort)(charge/2)));
        if (!shipManager.isPlayer)
            return;
        _innerBar.anchoredPosition = new Vector2(_innerBar.anchoredPosition.x,  (float)(400 * charge) / (2 * ushort.MaxValue));
        _innerBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)(400 * charge) / ushort.MaxValue);
    }

    public override void Execute()
    {
        ushort charge = _networkMovement.TempPassiveCharge;
        if (charge < Ability.MinCharge)
            return;
        GameObject go = Instantiate(_burstVFXPrefab, _representative.transform.GetChild(0));
        StartCoroutine(AnimateAndDestroy(go));
    }

    // Destroy the effect after the animation is complete.
    private IEnumerator AnimateAndDestroy(GameObject go)
    {
        yield return new WaitForSeconds(3);
        Destroy(go);
    }

    /*public override void QuickExecute()
    {
        if (++oooCounter == 1)
        {
            executing = true;
        }
    }

    public override void Stop()
    {
        if (--oooCounter == 0)
        {
            executing = false;
        }
    }*/
}