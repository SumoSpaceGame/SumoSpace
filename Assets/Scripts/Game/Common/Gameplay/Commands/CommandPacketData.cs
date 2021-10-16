using System;
using System.Text;
using UnityEngine;

namespace Game.Common.Gameplay.Commands
{
    public struct CommandPacketData
    {

        private byte[] data;


        public void Set(string stringData)
        {
            Set(Encoding.UTF8.GetBytes(stringData));
        }


        public void Set(byte[] byteData)
        {
            if (byteData == null)
            {
                Debug.LogError("Failed to set CommandPacketData, data is null");
            }
            
            data = byteData;
        }
        
        public byte[] GetBytes()
        {
            return data;
        }

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