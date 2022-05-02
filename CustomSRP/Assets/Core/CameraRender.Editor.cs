using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    public partial class CameraRender : MonoBehaviour
    {
        partial void PrepareBuffer();
#if UNITY_EDITOR
        private string SampleName { get; set; }

        partial void PrepareBuffer()
        {
            _buffer.name = SampleName = _camera.name;
        }
#else
        const SampleName = BUFFER_NAMe;
#endif
        
        
        partial void DrawGizmos();
#if UNITY_EDITOR
        partial void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                _context.DrawGizmos(_camera, GizmoSubset.PreImageEffects);
                _context.DrawGizmos(_camera, GizmoSubset.PostImageEffects);
            }
        }
#endif

        partial void PrepareForSceneWindow();
#if UNITY_EDITOR
        partial void PrepareForSceneWindow()
        {
            if (_camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(_camera);
            }
        }
#endif
    }
}
