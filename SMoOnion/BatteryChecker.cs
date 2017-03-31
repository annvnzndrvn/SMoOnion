using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nikon;

namespace SMoOnion
{
    class BatteryChecker
    {
        Camera camera;

        public BatteryChecker(Camera cam)
        {
            camera = cam;
        }
            
        public int ReturnBatteryLevel()
        {
            try
            {
                return camera.cam.GetInteger(eNkMAIDCapability.kNkMAIDCapability_BatteryLevel);
            }
            catch (NikonException ex)
            {
                throw new NikonException(ex.Message);
            }
        }
    }
}
