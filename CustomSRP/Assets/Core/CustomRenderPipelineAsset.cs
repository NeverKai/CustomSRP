using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    [CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {
        [SerializeField]
        private ShadowSettings _shadows = default;
        protected override RenderPipeline CreatePipeline()
        {
            return new CustomRenderPipeline(_shadows);
        }
    }
}
