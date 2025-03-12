using System;
using UnityEngine;

[Serializable]
public struct CameraBufferSettings {
    public bool allowHDR;
    public bool copyColor, copyColorReflection, copyDepth, copyDepthReflection;

    [Range(0.1f, 2f)]
    public float renderScale;
}