using System;
using System.Diagnostics;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using TMPro;
using UnityEngine;

namespace Game.Common.UI.DebugUI
{
    public class NetworkStatsUI : MonoBehaviour
    {

        public float BandwidthIn = 0.0f;
        public float BandwidthOut = 0.0f;

        public bool useRaw = false;
    
        public TMP_Text bandiwdthInText;
        public TMP_Text bandiwdthOutText;
        public TMP_Text pingText;


        private Stopwatch pingTimer = new Stopwatch();
        private const long PING_TIMER = 2500;
    

        private bool initated = false;

        // Update is called once per frame
        public void Update()
        {

            //Reusable in case network manager was reset
            if (NetworkManager.Instance != null || !NetworkManager.Instance.isActiveAndEnabled)
            {
                initated = false;
                return;
            }else if (!initated)
            {
                NetworkManager.Instance.Networker.onPingPong += onPong;
                NetworkManager.Instance.Networker.Ping();
            }
            
            
            if (!pingTimer.IsRunning)
            {
                pingTimer.Start();
            }

            if (pingTimer.ElapsedMilliseconds > PING_TIMER)
            {
                pingTimer.Restart();
                NetworkManager.Instance.Networker.Ping();
            }
        
            UpdateNetworkStats();
            UpdateUI();
        }

        /// <summary>
        /// Grabs network status and saves it class variables.
        /// </summary>
        private void UpdateNetworkStats()
        {
        
            BandwidthIn = NetworkManager.Instance.Networker.BandwidthIn;
            BandwidthOut = NetworkManager.Instance.Networker.BandwidthOut;
        }

    
        // TODO: Rely on the game network manager for ping data.
        /// <summary>
        /// Custom handler for ping UI
        /// </summary>
        /// <param name="ping"></param>
        /// <param name="sender"></param>
        private void onPong(double ping, NetWorker sender)
        {
            MainThreadManager.Run(() =>
            {
                pingText.text = "Ping - " + ping + "ms";
            });
        }

        private void UpdateUI()
        {

            if (useRaw)
            {
                bandiwdthInText.text = "IN - " + BandwidthIn;
                bandiwdthOutText.text = "OUT - " + BandwidthOut;
            }
            else
            {
                bandiwdthInText.text = "IN - " + ConvertBytesCountToText(BandwidthIn);
                bandiwdthOutText.text = "OUT - " + ConvertBytesCountToText(BandwidthOut);
            }
        }

        private void OnDestroy()
        {
            NetworkManager.Instance.Networker.onPingPong -= onPong;
        }


        /// <summary>
        /// Converts Bytes into proper Bytes,Kilo,Mega and assigns a suffix depending on data size.
        /// </summary>
        /// <returns></returns>
        private String ConvertBytesCountToText(double byteCount)
        {
            String suffix = "B/S";

            if (byteCount / 1000 > 0.01)
            {
                byteCount /= 1000;
                suffix = "KB/s";

                if (byteCount / 1000 > 0.01)
                {
                    byteCount /= 1000;
                    suffix = "MB/s";
                }
            }
        
            return byteCount + suffix;
        }
    }
}
