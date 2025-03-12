using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEngine.Profiling;

public partial class CameraRenderer {

#if UNITY_EDITOR
    private static Material errorMaterial;
    private static ShaderTagId[] legacyShaderTagIds = new ShaderTagId[] {
            new ShaderTagId("Always"), new ShaderTagId("ForwardBase"), new ShaderTagId("PrepassBase"), new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"), new ShaderTagId("VertexLM")};
    private string SampleName { get; set; }


    partial void DrawUnsupportedShaders() {
        if (errorMaterial == null) {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }

        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera)) { overrideMaterial = errorMaterial };
        for (int i = 1; i < legacyShaderTagIds.Length; i++) {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        var filteringSettings = FilteringSettings.defaultValue;
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }
    partial void DrawGizmosBeforeFX() {
        if (Handles.ShouldRenderGizmos()) {
            if (useIntermediateBuffer) {
                Draw(depthAttachmentId, BuiltinRenderTextureType.CameraTarget, true);
                ExecuteBuffer();
            }

            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
        }
    }


    partial void DrawGizmosAfterFX() {
        if (Handles.ShouldRenderGizmos()) {
            if (postFXStack.IsActive) {
                Draw(depthAttachmentId, BuiltinRenderTextureType.CameraTarget, true);
                ExecuteBuffer();
            }

            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }





    partial void PrepareForSceneWindow() {
        if (camera.cameraType == CameraType.SceneView) {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            useScaledRendering = false;
        }
    }
    partial void PrepareBuffer() {
        Profiler.BeginSample("Editor Only");
        buffer.name = SampleName = camera.name;
        Profiler.EndSample();
    }



#else
    private string SampleName => bufferName;
#endif
    partial void DrawUnsupportedShaders();
    partial void DrawGizmosBeforeFX();
    partial void DrawGizmosAfterFX();
    partial void PrepareForSceneWindow();
    partial void PrepareBuffer();
}
