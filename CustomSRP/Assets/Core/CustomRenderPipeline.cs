using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    public class CustomRenderPipeline : RenderPipeline
    {
        private readonly CameraRender _render = new CameraRender();

        private ShadowSettings _shadows = default;
        public CustomRenderPipeline(ShadowSettings shadowSettings)
        {
            _shadows = shadowSettings;
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


