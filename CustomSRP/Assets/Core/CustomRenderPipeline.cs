using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    public class CustomRenderPipeline : RenderPipeline
    {
        private readonly CameraRender _render = new CameraRender();
        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                _render.Render(context, camera);
            }
        }
    }
}


