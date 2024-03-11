using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulationLocalConfigData : LocalAccountConfigDataBase
{
    public FluidConfig fluidConfig = null;
    public PostEffectConfig postEffectConfig = null;
    public bool isHideDebugInfo = true;
}

public class FluidSimulationLocalConfig : LocalAccountConfig<FluidSimulationLocalConfigData>
{
    protected override void ResetLocalAccountConfigData()
    {
        m_data = new FluidSimulationLocalConfigData();
        m_data.fluidConfig = new FluidConfig();
        m_data.isHideDebugInfo = true;
        m_data.fluidConfig.ejecters.Add(new Ejecter()
        {
            x = 0.99f,
            y = 0.38f,
            dx = -1000,
            dy = 0,
            color = FluidSimulation.GenerateColor(),
            radius = 0.001f,
            peroid = 0.1f,
        });
        m_data.postEffectConfig = new PostEffectConfig();
    }

}
