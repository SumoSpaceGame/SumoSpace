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

        
        public ulong BandwidthIn = 0;
        public ulong BandwidthOut = 0;
        private ulong BandwidthInLast = 0;
        private ulong BandwidthOutLast = 0;
        public float bandwidthTime = 0;
        
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
            if (NetworkManager.Instance == null || !NetworkManager.Instance.isActiveAndEnabled)
            {
                initated = false;
                return;
            }else if (!initated)
            {
                NetworkManager.Instance.Networker.onPingPong += onPong;
                NetworkManager.Instance.Networker.Ping();
                initated = true;
            }

            bandwidthTime += Time.deltaTime;
            
            if (!pingTimer.IsRunning)
            {
                pingTimer.Start();
            }

            if (pingTimer.ElapsedMilliseconds > PING_TIMER)
            {
                pingTimer.Restart();
                NetworkManager.Instance.Networker.Ping();
                
                BandwidthIn = NetworkManager.Instance.Networker.BandwidthIn - BandwidthInLast;
                BandwidthIn = Convert.ToUInt64(BandwidthIn / bandwidthTime);

                BandwidthOut = NetworkManager.Instance.Networker.BandwidthOut - BandwidthOutLast;
                BandwidthOut = Convert.ToUInt64(BandwidthIn / bandwidthTime);
                
                BandwidthInLast = NetworkManager.Instance.Networker.BandwidthIn;
                BandwidthOutLast = NetworkManager.Instance.Networker.BandwidthOut;
                bandwidthTime = 0;
            }
        
            UpdateNetworkStats();
            UpdateUI();
        }

        /// <summary>
        /// Grabs network status and saves it class variables.
        /// </summary>
        private void UpdateNetworkStats()
        {
        
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
                bandiwdthInText.text = "IN/s - " + ConvertBytesCountToText(BandwidthIn);
                bandiwdthOutText.text = "OUT/s - " + ConvertBytesCountToText(BandwidthOut);
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
        private String ConvertBytesCountToText(ulong byteCount)
        {
            String suffix = "B/S";
            double count = Convert.ToDouble(byteCount);
            
            if (count / 1000 > 0.01)
            {
                count /= 1000;
                suffix = "KB/s";

                if (count / 1000 > 0.01)
                {
                    count /= 1000;
                    suffix = "MB/s";
                }
            }

            count *= 100;
            count = Math.Truncate(count);
            count /= 100;
            
            return count + suffix;
        }
    }
}
