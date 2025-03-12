using UnityEngine;
using UnityEngine.Rendering;

public class MeshBall : MonoBehaviour
{
    private static int baseColorID = Shader.PropertyToID("_BaseColor");
    private static int metallicID = Shader.PropertyToID("_Metallic");
    private static int smoothnessID = Shader.PropertyToID("_Smoothness");

    [SerializeField]
    private Mesh m_Mesh = default;

    [SerializeField]
    private Material m_Material = default;

    [SerializeField]
    private LightProbeProxyVolume m_LightProbeVolume = null;

    private Matrix4x4[] m_Matrices = new Matrix4x4[1023];
    private Vector4[] m_BaseColors = new Vector4[1023];
    private float[] m_Metallic = new float[1023], m_Smoothness = new float[1023];


    private MaterialPropertyBlock m_Block;

    private void Awake() {
        for (int i = 0; i < m_Matrices.Length; i++) {
            m_Matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10f, 
                Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f),
                Vector3.one * Random.Range(0.5f, 1f));
            m_BaseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1f));
            m_Metallic[i] = Random.value < 0.25f ? 1f : 0f;
            m_Smoothness[i] = Random.Range(0.05f, 0.95f);
        }
    }

    private void Update() {
        if (m_Block == null) {
            m_Block = new MaterialPropertyBlock();
            m_Block.SetVectorArray(baseColorID, m_BaseColors);
            m_Block.SetFloatArray(metallicID, m_Metallic);
            m_Block.SetFloatArray(smoothnessID, m_Smoothness);

            if (!m_LightProbeVolume) {
                var positions = new Vector3[1023];
                for (int i = 0; i < m_Matrices.Length; i++) {
                    positions[i] = m_Matrices[i].GetColumn(3);
                }
                var lightProbes = new SphericalHarmonicsL2[1023];
                var occlusionProbes = new Vector4[1023];
                LightProbes.CalculateInterpolatedLightAndOcclusionProbes(positions, lightProbes, occlusionProbes);
                m_Block.CopySHCoefficientArraysFrom(lightProbes);
                m_Block.CopyProbeOcclusionArrayFrom(occlusionProbes);
            }

            
        }
        Graphics.DrawMeshInstanced(m_Mesh, 0, m_Material, m_Matrices, 1023, m_Block,
                ShadowCastingMode.On, true, 0, null, 
                m_LightProbeVolume ? LightProbeUsage.UseProxyVolume : LightProbeUsage.CustomProvided, m_LightProbeVolume);
    }
}
