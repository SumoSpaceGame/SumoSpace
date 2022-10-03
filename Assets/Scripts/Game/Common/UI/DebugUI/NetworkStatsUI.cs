using System;
using FishNet;
using FishNet.Managing.Statistic;
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


        private bool initated = false;

        // Update is called once per frame
        public void Update()
        {

            //Reusable in case network manager was reset
            if (InstanceFinder.StatisticsManager == null || !InstanceFinder.StatisticsManager )
            {
                initated = false;
                return;
            }else if (!initated)
            {
                if (InstanceFinder.IsServer)
                {   
                    InstanceFinder.StatisticsManager.NetworkTraffic.OnServerNetworkTraffic += OnNetworkServerStatistics;
                }
                else
                {
                    InstanceFinder.StatisticsManager.NetworkTraffic.OnClientNetworkTraffic += OnNetworkServerStatistics;
                }
                
                initated = true;
            }

        }

        private void OnDestroy()
        {
            if (InstanceFinder.IsServer)
            {   
                InstanceFinder.StatisticsManager.NetworkTraffic.OnServerNetworkTraffic -= OnNetworkServerStatistics;
            }
            else
            {
                InstanceFinder.StatisticsManager.NetworkTraffic.OnClientNetworkTraffic -= OnNetworkServerStatistics;
            }
        }

        /// <summary>
        /// Grabs network status and saves it class variables.
        /// </summary>
        private void UpdateNetworkStats()
        {
        
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

        public void OnNetworkServerStatistics(NetworkTrafficArgs args)
        {
            BandwidthIn = args.FromServerBytes;
            BandwidthOut = args.ToServerBytes;
            UpdateNetworkStats();
            UpdateUI();
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
