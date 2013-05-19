using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulate.Samples.Clocks
{
    public class CustomSimulationEnvironment : SimulationEnvironment
    {
        #region Protected Methods

        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        #endregion
    }
}
