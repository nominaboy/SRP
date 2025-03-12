using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CustomRenderPipeline : RenderPipeline {
    CameraRenderer renderer;

    private bool allowHDR, useDynamicBatching, useGPUInstancing, useLightsPerObject;
    private ShadowSettings shadowSettings;
    private PostFXSettings postFXSettings;
    private int colorLUTResolution;
    private CameraBufferSettings CameraBufferSettings;

    public CustomRenderPipeline(CameraBufferSettings cameraBufferSettings, bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher, 
            bool useLightsPerObject, ShadowSettings shadowSettings, PostFXSettings postFXSettings, int colorLUTResolution, 
            Shader cameraRendererShader) {
        this.CameraBufferSettings = cameraBufferSettings;
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        this.useLightsPerObject = useLightsPerObject;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
        GraphicsSettings.lightsUseLinearIntensity = true;
        this.shadowSettings = shadowSettings;
        this.postFXSettings = postFXSettings;
        this.colorLUTResolution = colorLUTResolution;
        InitializeForEditor();
        renderer = new CameraRenderer(cameraRendererShader);
    }
    protected override void Render(ScriptableRenderContext context, Camera[] cameras) {
    }

    protected override void Render(ScriptableRenderContext context, List<Camera> cameras) {
        for (int i = 0; i < cameras.Count; i++) {
            renderer.Render(context, cameras[i], CameraBufferSettings, useDynamicBatching, useGPUInstancing, useLightsPerObject, 
                shadowSettings, postFXSettings, colorLUTResolution);
        }
    }
}
