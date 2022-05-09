using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    public class CustomRenderPipeline : RenderPipeline
    {
        private readonly CameraRender _render = new CameraRender();

        private ShadowSettings _shadows = default;
        public CustomRenderPipeline(bool useDynamicBatching, 
            bool useGPUInstancing,
            bool useSRPBatcher,
            ShadowSettings shadowSettings)
        {
            _shadows = shadowSettings;
            GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                _render.Render(context, camera, _shadows);
            }
        }
    }
}


