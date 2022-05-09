using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    [CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {
        [SerializeField]
        private ShadowSettings _shadows = default;

        [SerializeField] private bool useDynamicBatching = true;
        [SerializeField] private bool useGPUInstancing = true;
        [SerializeField] private bool useSRPBatcher = true;
        
        protected override RenderPipeline CreatePipeline()
        {
            return new CustomRenderPipeline(useDynamicBatching,
                useGPUInstancing,
                useSRPBatcher, _shadows);
        }
    }
}
