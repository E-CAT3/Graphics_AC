using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class BasicBlurRenderFeature : ScriptableRendererFeature
{
    
    public int blurlevel = 10;
    private BasicBlurRenderPass _renderPass;
    
    class BasicBlurRenderPass : ScriptableRenderPass
    {
        // private readonly string profilerTag = "BasicBlur Pass";
        private const string NAME = nameof(BasicBlurRenderPass);
        
        private Material _material = null;

        private RenderTargetIdentifier _currentTarget;

        private int _BlurLevel = 10;
        
        public BasicBlurRenderPass(Shader _shader)
        {
            _material = new Material(_shader);
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // コマンドバッファ
            var buf = CommandBufferPool.Get(NAME);
            CameraData camData = renderingData.cameraData;

            if (camData.isSceneViewCamera)
            {
                return;
            }
            
            // int texId = Shader.PropertyToID("_TempTexture");
            int w = camData.camera.scaledPixelWidth;
            int h = camData.camera.scaledPixelHeight;
            
            // RenderTextureDescriptor descriptor = camData.cameraTargetDescriptor;
            // buf.GetTemporaryRT(texId, descriptor, FilterMode.Bilinear);

            Vector2 reso = new Vector2(w, h);
            _material.SetVector("_Resolution", reso);
            _material.SetFloat("_BlurLevel", _BlurLevel);
            
            var rth = RenderTexture.GetTemporary(w / 2, h); 
            Blit(buf,_currentTarget, rth, _material);

            var rtv = RenderTexture.GetTemporary(rth.width, rth.height / 2); 
            Blit(buf, rth, rtv, _material); //シェーダー処理を加えて、縦半分のレンダーテクスチャにコピー
            
            Blit(buf,rtv, _currentTarget, _material); //元サイズから1/4になったレンダーテクスチャを、元のサイズに戻す
            
            context.ExecuteCommandBuffer(buf);
            
            RenderTexture.ReleaseTemporary(rth);
            RenderTexture.ReleaseTemporary(rtv);
            CommandBufferPool.Release(buf);
        }

        public void SetParam(RenderTargetIdentifier renderTarget, int downSample)
        {
            _currentTarget = renderTarget;
            _BlurLevel = downSample;
            if (_BlurLevel <= 0) _BlurLevel = 1;
        }
    }

    private BasicBlurRenderPass scriptablePass;
    

    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        var _shader = Shader.Find("Unlit/BasicBlur");
        if (_shader)
        {
            scriptablePass = new BasicBlurRenderPass(_shader);
            scriptablePass.renderPassEvent = _renderPassEvent;
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // パスにカメラのカラーを渡す
        scriptablePass.SetParam(renderer.cameraColorTarget, blurlevel);
        renderer.EnqueuePass(scriptablePass);
    }
}
