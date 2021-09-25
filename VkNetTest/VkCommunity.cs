using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkNetTest
{
    class VkCommunity
    {
        // Domain name. Example: barevape => from vk.com/barevape
        public string Domain; 
        // Community ID. Unique id of community.
        public int CID;

        public static List<VkCommunity> Communities{ get; private set; } = new List<VkCommunity>();

        /// TODO: impletent AddCommunity
        public static void AddCommunity(VkCommunity com)
            => throw new NotImplementedException();
    }
}
