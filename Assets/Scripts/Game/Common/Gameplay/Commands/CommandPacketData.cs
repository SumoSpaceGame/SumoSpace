using System;
using System.Text;
using UnityEngine;

namespace Game.Common.Gameplay.Commands
{
    public struct CommandPacketData
    {

        private byte[] data;


        /// <summary>
        /// Set a string set of data to be set across the network. Overrides the current data
        /// </summary>
        /// <param name="stringData"></param>
        public void Set(string stringData)
        {
            Set(Encoding.UTF8.GetBytes(stringData));
        }

        /// <summary>
        /// Set a byte array to be sent across the network. Overrides the current data
        /// </summary>
        /// <param name="byteData"></param>
        public void Set(byte[] byteData)
        {
            if (byteData == null)
            {
                Debug.LogError("Failed to set CommandPacketData, data is null");
                return;
            }
            
            data = byteData;
        }
        
        /// <summary>
        /// Set a string set of data to be set across the network. Creates a new commandPacket
        /// </summary>
        /// <param name="stringData"></param>
        public static CommandPacketData Create(string stringData)
        {
            CommandPacketData data = new CommandPacketData();
            data.Set(stringData);
            return data;
        }

        /// <summary>
        /// Set a byte array to be sent across the network. Creates a new commandPacket
        /// </summary>
        /// <param name="byteData"></param>
        public static CommandPacketData Create(byte[] byteData)
        {
            CommandPacketData data = new CommandPacketData();
            data.Set(byteData);
            return data;
        }
        
        /// <summary>
        /// Creates an empty packet (used for pinging)
        /// </summary>
        /// <param name="byteData"></param>
        public static CommandPacketData Empty()
        {
            CommandPacketData data = new CommandPacketData();
            data.Set(new byte[]{});
            return data;
        }
        
        
        /// <summary>
        /// Get the raw byte array back.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return data;
        }

        /// <summary>
        /// Tries to encode the byte array into string via UTF8
        /// </summary>
        /// <param name="stringData">Out Data</param>
        /// <returns>Success</returns>
        public bool TryGetString(out string stringData)
        {
            stringData = "";
            if (data == null)
            {
                return false;
            }
            
            try
            {
                var someString = Encoding.UTF8.GetString(data);

                stringData = someString;
                return true;
            }
            catch (ArgumentException e)
            {
                Debug.LogException(e);
                return false;
            }
        }

    }
}