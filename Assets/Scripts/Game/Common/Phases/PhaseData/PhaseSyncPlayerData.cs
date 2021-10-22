using System.Text;

namespace Game.Common.Phases.PhaseData
{
    public class PhaseSyncPlayerData
    {
        public static byte[] Serialized(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        public static string Deserialized(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}