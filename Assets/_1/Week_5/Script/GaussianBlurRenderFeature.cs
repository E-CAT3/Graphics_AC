using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class GaussianBlurRenderFeature : ScriptableRendererFeature
{
    
    public int blurlevel = 10;
    private GaussianBlurRenderPass _renderPass;
    
    class GaussianBlurRenderPass : ScriptableRenderPass
    {
        // private readonly string profilerTag = "BasicBlur Pass";
        private const string NAME = nameof(GaussianBlurRenderPass);
        
        private Material _material = null;

        private RenderTargetIdentifier _currentTarget;

        private int _BlurLevel = 10;

        private int _directionID = 0;
        
        public GaussianBlurRenderPass(Shader _shader)
        {
            _material = new Material(_shader);
            _directionID = Shader.PropertyToID("_Direction");
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
            _material.SetFloat("_TexelInterval", _BlurLevel);
            
            
            
            var rth = RenderTexture.GetTemporary(w / 2, h); 
            
            var v_h = new Vector2(1, 0); //ブラー方向のベクトル(U方向)
            buf.SetGlobalVector(_directionID, v_h);
//            _material.SetVector("_Direction", v_h); //シェーダー内の変数にブラー方向を設定
            
            Blit(buf,_currentTarget, rth, _material);

            var rtv = RenderTexture.GetTemporary(rth.width, rth.height / 2); 
            var v_v = new Vector2(0, 1);　//ブラー方向のベクトル(V方向) 
            // _material.SetVector("_Direction", v_v); //シェーダー内の変数にブラー方向を設定
            buf.SetGlobalVector(_directionID, v_v);
            Blit(buf, _currentTarget, rtv, _material); //シェーダー処理を加えて、縦半分のレンダーテクスチャにコピー

            // ▼ 以下の書き方だと最後に書いた方の設定で毎回処理される
            // _material.SetVector("_Direction", v_v);

            Blit(buf,rth, _currentTarget, _material); //元サイズから1/4になったレンダーテクスチャを、元のサイズに戻す

            
            
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

    private GaussianBlurRenderPass scriptablePass;
    

    [SerializeField] private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    
    public override void Create()
    {
        var _shader = Shader.Find("Unlit/GaussianBlur");
        if (_shader)
        {
            scriptablePass = new GaussianBlurRenderPass(_shader);
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
