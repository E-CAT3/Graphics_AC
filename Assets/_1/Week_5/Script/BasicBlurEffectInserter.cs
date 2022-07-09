using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways, ImageEffectAllowedInSceneView]
public class BasicBlurEffectInserter : MonoBehaviour
{
    [SerializeField] private Material _material;

    private int resolution;
    private Vector2 displaySize;

    private void Awake()
    {
        resolution = Shader.PropertyToID("_Resolution");
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        displaySize.x = Screen.currentResolution.width;
        displaySize.y = Screen.currentResolution.height;
        
        // 横幅を半分にしたレンダーテクスチャを作成
        var rth = RenderTexture.GetTemporary(src.width / 2, src.height);
        Graphics.Blit(src, rth, _material);

        // さらに縦幅を半分にしたレンダーテクスチャを作成
        var rtv = RenderTexture.GetTemporary(rth.width , rth.height / 2);
        Graphics.Blit(rth, rtv, _material);
        
        // レンダーテクスチャのサイズを元に戻す（？）
        Graphics.Blit(rtv, dest, _material);
        
        // 作成したレンダーテクスチャの解放
        RenderTexture.ReleaseTemporary(rth);
        RenderTexture.ReleaseTemporary(rtv);
        Debug.Log("test");
    }
}
