using System.Collections;
using Game.Ships.Heavy.Common.Abilities;
using TMPro;
using UnityEngine;

namespace Game.Ships.Heavy.Client.Behaviours
{
    public class HeavyPrimaryFireClientBehaviour : RenderableAbilityBehaviour<HeavyPrimaryFireAbility> {
        private Coroutine coroutine;
        private bool _shouldBeamStart = false;
        [SerializeField]
        private GameObject _shootVFXPrefab;
        private GameObject _shootVFX;
        private GameObject _representative;
        private bool _hasRunLocally;
        
        private Animator _animator;

        private void Start()
        {
            _representative = shipManager.simulationObject.representative;
            _animator = _representative.transform.GetChild(0).GetComponent<Animator>(); // TODO: Move animator to _representative or some other more convenient location.
        }

        public override void Execute()
        {
            if (_hasRunLocally)
            {
                _hasRunLocally = false;
                return;
            }
            if (!Ability.IsDisabled)
            {
                //ShipRenderer.StartBeam();
                //_fireEffect.Reinit();
                //_shootVFX ??= Instantiate(_shootVFXPrefab, _representative.transform.GetChild(0).GetChild(0));
                _animator.SetBool("IsFiring", true);
                //coroutine = shipManager.StartCoroutine(ClientSide());
            }
            else
                _shouldBeamStart = true;
            if (++oooCounter == 1) {
                executing = true;
            }
        }

        public override void QuickExecute()
        {
            Execute();
            _hasRunLocally = true;
        }

        public override void Stop() {
            if (coroutine != null) shipManager.StopCoroutine(coroutine);
            //ShipRenderer.EndBeam();
            //_fireEffect.SendEvent("OnStop");
            //_fireEffect.Stop();
            _animator.SetBool("IsFiring", false);
            // Destroy(_shootVFX);
            _shootVFX = null;
            _shouldBeamStart = false;
            if (--oooCounter == 0) {
                executing = false;
            }
        }

        private IEnumerator ClientSide() {
            while (true) {
                if (Ability.IsDisabled)
                {
                    executing = false;
                    //ShipRenderer.EndBeam();
                    //_fireEffect.SendEvent("OnStop");
                    //_fireEffect.Stop();
                    Destroy(_shootVFX);
                    _shootVFX = null;
                    _shouldBeamStart = true;
                    shipManager.StopCoroutine(coroutine);
                }
                else
                {
                    //ShipRenderer.Beam();
                }
                yield return null;
            }
        }

        // If the fire ability is reenabled while executing, start the beam.
        private void Update()
        {
            if (!Ability.IsDisabled && _shouldBeamStart)
            {
                _shouldBeamStart = false;
                QuickExecute();
            }
        }
    }
}