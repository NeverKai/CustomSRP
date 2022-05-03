using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Rendering;

public class Shadows
{
    private const string BUFFER_NAME = "Shadows";
    private const int MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT = 1;

    private readonly int DIR_SHADOW_ATLAS_ID = Shader.PropertyToID("_DirectionalShadowAtlas");

    struct ShadowedDirectionalLight
    {
        public int VisibleLightIndex;
    }
    
    ShadowedDirectionalLight[] ShadowedDirectionalLights = new ShadowedDirectionalLight[MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT];

    private CommandBuffer _buffer = new CommandBuffer()
    {
        name = BUFFER_NAME
    };

    private ScriptableRenderContext _context;
    private CullingResults _cullingResults;
    private ShadowSettings _shadowSettings;

    private int _shadowedDirectionalLightCount;

    public void Setup(ScriptableRenderContext context, 
        CullingResults cullingResults,
        ShadowSettings shadowSettings)
    {
        _context = context;
        _cullingResults = cullingResults;
        _shadowSettings = shadowSettings;

        _shadowedDirectionalLightCount = 0;
    }

    public void ReserveDirectionalShadows(Light light, int visibleLightIndex)
    {
        if (_shadowedDirectionalLightCount >= MAX_SHADOWED_DIRECTIONAL_LIGHT_COUNT) return;
        if (light.shadows == LightShadows.None) return;
        if(light.shadowStrength <= 0) return;

        if (_cullingResults.GetShadowCasterBounds(visibleLightIndex, out Bounds bounds))
        {
            ShadowedDirectionalLights[_shadowedDirectionalLightCount++] = new ShadowedDirectionalLight()
            {
                VisibleLightIndex = visibleLightIndex
            };
        }
    }

    public void Render()
    {
        if (_shadowedDirectionalLightCount > 0)
        {
            RenderDirectionalShadows();
        }
    }

    void RenderDirectionalShadows()
    {
        // int atlasSize = (int)_shadowSettings.TextureSize;
        int atlasSize = 1024;
        _buffer.GetTemporaryRT(DIR_SHADOW_ATLAS_ID, atlasSize, atlasSize, 32,
            FilterMode.Bilinear, RenderTextureFormat.Shadowmap);
        
        _buffer.SetRenderTarget(DIR_SHADOW_ATLAS_ID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
        
        _buffer.ClearRenderTarget(true, false, Color.clear);
        
        ExecuteBuffer();
    }

    void ExecuteBuffer()
    {
        _context.ExecuteCommandBuffer(_buffer);
        _buffer.Clear();
    }

    public void Cleanup()
    {
        _buffer.ReleaseTemporaryRT(DIR_SHADOW_ATLAS_ID);
        ExecuteBuffer();
    }
}
