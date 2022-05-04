using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const string BUFFER_NAME = "lighting";

    private static int DirLightDirectionID = Shader.PropertyToID("_DirectionalLightDirection");
    private static int DirLightColorID = Shader.PropertyToID("_DirectionalLightColor");
    
    private CommandBuffer _commandBuffer = new CommandBuffer()
    {
        name = BUFFER_NAME
    };

    public void Setup(ScriptableRenderContext context)
    {
        _commandBuffer.BeginSample(BUFFER_NAME);
        SetupDirectionalLight();
        _commandBuffer.EndSample(BUFFER_NAME);
        
        context.ExecuteCommandBuffer(_commandBuffer);
        _commandBuffer.Clear();

    }

    void SetupDirectionalLight()
    {
        Light light = RenderSettings.sun;
        
        _commandBuffer.SetGlobalVector(DirLightColorID, light.color.linear * light.intensity);
        _commandBuffer.SetGlobalVector(DirLightDirectionID, light.transform.forward);
    }
}
