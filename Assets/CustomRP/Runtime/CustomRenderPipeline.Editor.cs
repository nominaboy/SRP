using UnityEngine;
using Unity.Collections;
using UnityEngine.Experimental.GlobalIllumination;
using LightType = UnityEngine.LightType;


public partial class CustomRenderPipeline {
    partial void InitializeForEditor();
    partial void DisposeForEditor();
    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        DisposeForEditor();
        renderer.Dispose();
    }

#if UNITY_EDITOR
    partial void InitializeForEditor() {
        Lightmapping.SetDelegate(lightsDelegate);
    }
    partial void DisposeForEditor() {
        Lightmapping.ResetDelegate();
    }

    private static Lightmapping.RequestLightsDelegate lightsDelegate =
        (Light[] lights, NativeArray<LightDataGI> output) => {
            var lightData = new LightDataGI();
            for (var i = 0; i < lights.Length; i++) {
                Light light = lights[i];
                switch (light.type) {
                    case LightType.Directional:
                        var directionalLight = new DirectionalLight();
                        LightmapperUtils.Extract(light, ref directionalLight);
                        lightData.Init(ref directionalLight);
                        break;
                    case LightType.Point:
                        var pointLight = new PointLight();
                        LightmapperUtils.Extract(light, ref pointLight);
                        lightData.Init(ref pointLight);
                        break;
                    case LightType.Spot:
                        var spotLight = new SpotLight();
                        LightmapperUtils.Extract(light, ref spotLight);
                        spotLight.innerConeAngle = light.innerSpotAngle * Mathf.Deg2Rad;
                        spotLight.angularFalloff = AngularFalloffType.AnalyticAndInnerAngle;
                        lightData.Init(ref spotLight);
                        break;
                    case LightType.Area:
                        var rectangleLight = new RectangleLight();
                        LightmapperUtils.Extract(light, ref rectangleLight);
                        rectangleLight.mode = LightMode.Baked; // only support baked 
                        lightData.Init(ref rectangleLight);
                        break;
                    case LightType.Disc:
                        break;
                    default:
                        lightData.InitNoBake(light.GetInstanceID());
                        break;
                }
                lightData.falloff = FalloffType.InverseSquared;
                output[i] = lightData;
            }
        };

#endif




}
