using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour {
    static int baseColorID = Shader.PropertyToID("_BaseColor");
    static int cutoffID = Shader.PropertyToID("_Cutoff");
    static int metallicID = Shader.PropertyToID("_Metallic");
    static int smoothnessID = Shader.PropertyToID("_Smoothness");
    static int emissionColorID = Shader.PropertyToID("_EmissionColor");

    static MaterialPropertyBlock block;

    [SerializeField]
    Color baseColor = Color.white;

    [SerializeField, ColorUsage(false, true)]
    Color emissionColor = Color.black;

    [SerializeField, Range(0f, 1f)]
    float cutoff = 0.5f, metallic = 0f, smoothness = 0.5f;

    private void OnValidate() {
        if (block == null) {
            block = new MaterialPropertyBlock();
        }
        block.SetColor(baseColorID, baseColor);
        block.SetColor(emissionColorID, emissionColor);
        block.SetFloat(cutoffID, cutoff);
        block.SetFloat(metallicID, metallic);
        block.SetFloat(smoothnessID, smoothness);
        GetComponent<Renderer>().SetPropertyBlock(block);
    }

    private void Awake() {
        OnValidate();
    }

}
