using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB_Simulator_Reborn_Server
{
    [Serializable]
    class transmitClientListLight
    {
        public Int32 x;
        public List<CB_Simulator_clientInfoLight> transmitList;
    }
}
