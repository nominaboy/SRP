using System;
using UnityEngine;

[Serializable]
public struct CameraBufferSettings {
    public bool allowHDR;
    public bool copyColor, copyColorReflection, copyDepth, copyDepthReflection;
    public enum BicubicRescalingMode { Off, UpOnly, UpAndDown }
    public BicubicRescalingMode bicubicRescaling;

    [Range(0.1f, 2f)]
    public float renderScale;
}