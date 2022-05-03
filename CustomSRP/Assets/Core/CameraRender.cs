using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    public partial class CameraRender : MonoBehaviour
    {
        private ScriptableRenderContext _context;
        private Camera _camera;

        private const string BUFFER_NAME = "Render Camera";

        private CommandBuffer _buffer = new CommandBuffer()
        {
            name = BUFFER_NAME
        };

        private CullingResults _cullingResults;

        private static ShaderTagId UnlintShaderTagId = new ShaderTagId("SRPDefaultUnlit");
        private static ShaderTagId LitlintShaderTagId = new ShaderTagId("CustomLit");

        public void Render(ScriptableRenderContext context, 
            Camera camera,
            ShadowSettings shadowSettings)
        {
            _context = context;
            _camera = camera;
            
            // 把game view渲染的所有物体同步到scene view
            PrepareForSceneWindow();
            PrepareBuffer();
            if(!Cull(shadowSettings.maxDistance)) return;

            Setup();
            DrawVisibleGeometry(true, true);
            DrawGizmos();
            Submit();
        }

        void Setup()
        {
            var clearFlags = _camera.clearFlags;
            _buffer.ClearRenderTarget(clearFlags <= CameraClearFlags.Depth, 
                clearFlags == CameraClearFlags.Color,
                clearFlags == CameraClearFlags.Color ? _camera.backgroundColor.linear : Color.clear);
            _buffer.ClearRenderTarget(true, true, Color.clear);
            _context.SetupCameraProperties(_camera);

            _buffer.BeginSample(SampleName);
            ExecuteBuffer();
        }
        
        void DrawVisibleGeometry(bool enableDynamicBatching, bool enableInstancing)
        {

            var sortingSettings = new SortingSettings(_camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };
            
            var drawingSettings = new DrawingSettings(UnlintShaderTagId, sortingSettings)
            {
                enableDynamicBatching = enableDynamicBatching,
                enableInstancing = enableInstancing
            };
            
            drawingSettings.SetShaderPassName(1, LitlintShaderTagId);
            
            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            
            // 渲染不透明
            _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
            
            _context.DrawSkybox(_camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;
            _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        }

        void Submit()
        {
            _buffer.EndSample(SampleName);
            _context.Submit();
        }

        void ExecuteBuffer()
        {
            _context.ExecuteCommandBuffer(_buffer);
            _buffer.Clear();
        }

        bool Cull(float maxShadowDistance)
        {
            ScriptableCullingParameters scriptableCullingParameters;
            if (_camera.TryGetCullingParameters(out scriptableCullingParameters))
            {
                scriptableCullingParameters.shadowDistance = Mathf.Min(maxShadowDistance, _camera.farClipPlane);
                _cullingResults = _context.Cull(ref scriptableCullingParameters);
                return true;
            }

            return false;
        }
    }
}
