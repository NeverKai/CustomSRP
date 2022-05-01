using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    public class CameraRender : MonoBehaviour
    {
        private ScriptableRenderContext _context;
        private Camera _camera;

        private const string BUFFER_NAME = "Render Camera";

        private CommandBuffer _buffer = new CommandBuffer()
        {
            name = BUFFER_NAME
        };

        private CullingResults _cullingResults;

        private static ShaderTagId unlintShaderTagId = new ShaderTagId("SRPDefaultUnlit");

        public void Render(ScriptableRenderContext context, Camera camera)
        {
            _context = context;
            _camera = camera;
            
            if(!Cull()) return;

            Setup();
            DrawVisibleGeometry();
            Submit();
        }

        void Setup()
        {
            _buffer.ClearRenderTarget(true, true, Color.clear);
            _buffer.BeginSample(BUFFER_NAME);
            _context.SetupCameraProperties(_camera);
        }
        
        void DrawVisibleGeometry()
        {

            var sortingSettings = new SortingSettings(_camera)
            {
                criteria = SortingCriteria.CommonOpaque
            };
            
            var drawingSettings = new DrawingSettings(unlintShaderTagId, sortingSettings);
            var filteringSettings = new FilteringSettings(RenderQueueRange.all);
            
            _context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
            
            _context.DrawSkybox(_camera);
        }

        void Submit()
        {
            _buffer.EndSample(BUFFER_NAME);
            // ExecuteBuffer();
            _context.Submit();
        }

        void ExecuteBuffer()
        {
            _context.ExecuteCommandBuffer(_buffer);
            _buffer.Clear();
        }

        bool Cull()
        {
            ScriptableCullingParameters scriptableCullingParameters;
            if (_camera.TryGetCullingParameters(out scriptableCullingParameters))
            {
                _cullingResults = _context.Cull(ref scriptableCullingParameters);
                return true;
            }

            return false;
        }
    }
}
