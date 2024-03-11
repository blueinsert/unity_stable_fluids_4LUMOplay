using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulationLocalConfigData : LocalAccountConfigDataBase
{
    public FluidConfig fluidConfig = null;
    public bool isHideDebugInfo = true;
}

public class FluidSimulationLocalConfig : LocalAccountConfig<FluidSimulationLocalConfigData>
{
    protected override void ResetLocalAccountConfigData()
    {
        m_data = new FluidSimulationLocalConfigData();
        m_data.fluidConfig = new FluidConfig();
        m_data.isHideDebugInfo = true;
    }

}
