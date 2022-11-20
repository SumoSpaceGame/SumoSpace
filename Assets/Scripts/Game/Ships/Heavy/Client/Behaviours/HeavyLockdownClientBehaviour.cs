using System.Collections;
using UnityEngine;

public class HeavyLockdownClientBehaviour : RenderableAbilityBehaviour<HeavyLockdownAbility>
{
    private HeavyPrimaryFireAbility heavyFire = null;
    [SerializeField]
    private GameObject _lockdownVFXPrefab;
    private GameObject _lockdownVFX;
    private GameObject _representative;
    private Coroutine _coroutine;
    private bool _hasRunLocally;

    private void Start() => _representative = shipManager.simulationObject.representative;

    /// <summary>
    /// Enable or disable the lockdown.
    /// </summary>
    public override void Execute()
    {
        if (_hasRunLocally)
        {
            _hasRunLocally = false;
            return;
        }
        if (heavyFire is null)
        {
            heavyFire = shipManager.shipLoadout.PrimaryFire as HeavyPrimaryFireAbility;
            if (heavyFire is null)
            {
                Debug.LogError("No Heavy Primary Fire Ability found.");
                return;
            }
        }
        if (heavyFire.IsDisabled)
            return;
        if (!executing)
        {
            /*_coroutine = */shipManager.StartCoroutine(ClientSideStart());
            executing = true;
            _lockdownVFX ??= Instantiate(_lockdownVFXPrefab, _representative.transform.GetChild(0));
        }
        else
        {
            /*_coroutine = */shipManager.StartCoroutine(ClientSideStop());
        }
    }

    /// <summary>
    /// Enables/disables the lockdown without talking to the server.
    /// </summary>
    public override void QuickExecute()
    {
        Execute();
        _hasRunLocally = true;
    }

    // Winds up and activates the lockdown.
    private IEnumerator ClientSideStart()
    {
        print("Coroutine start starting.");
        heavyFire.IsDisabled = true;
        yield return new WaitForSeconds(Ability.WindUpTime);
        heavyFire.IsDisabled = false;
        _coroutine = null;
    }

    // Winds down and deactivates the lockdown.
    private IEnumerator ClientSideStop()
    {
        print("Coroutine stop starting.");
        heavyFire.IsDisabled = true;
        yield return new WaitForSeconds(Ability.WindDownTime);
        heavyFire.IsDisabled = false;
        Destroy(_lockdownVFX.gameObject);
        _lockdownVFX = null;
        print("Lockdown animation destroyed/reset.");
        executing = false;
        _coroutine = null;
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