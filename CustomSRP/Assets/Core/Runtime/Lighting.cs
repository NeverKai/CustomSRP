using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    /// <summary>
    /// 照明类，主要处理灯光和物体表面的交互（包括阴影）
    /// </summary>
    public class Lighting
    {
        private const string BUFFER_NAME = "lighting";

        private static int DirLightDirectionID = Shader.PropertyToID("_DirectionalLightDirection");
        private static int DirLightColorID = Shader.PropertyToID("_DirectionalLightColor");
        
        private Shadows _shadow = new Shadows();
    
        private CommandBuffer _commandBuffer = new CommandBuffer()
        {
            name = BUFFER_NAME
        };

        public void Setup(ScriptableRenderContext context,
            CullingResults cullingResults,
            ShadowSettings shadowSettings)
        {
            _commandBuffer.BeginSample(BUFFER_NAME);
            SetupDirectionalLight();
            _commandBuffer.EndSample(BUFFER_NAME);
        
            context.ExecuteCommandBuffer(_commandBuffer);
            _commandBuffer.Clear();
            
            _shadow.Setup(context, cullingResults, shadowSettings);
            _shadow.Render();
        }

        void SetupDirectionalLight()
        {
            Light light = RenderSettings.sun;
        
            _commandBuffer.SetGlobalVector(DirLightColorID, light.color.linear * light.intensity);
            _commandBuffer.SetGlobalVector(DirLightDirectionID, light.transform.forward);
        }

        public void Cleanup()
        {
            _shadow.Cleanup();
        }
    }
}
