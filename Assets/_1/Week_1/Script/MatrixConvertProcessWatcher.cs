using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixConvertProcessWatcher : MonoBehaviour
{
    private void Start()
    {
        ConvertMatrix();
    }

    [ContextMenu("Convert!")]
    public void ConvertMatrix()
    {
        var camera = FindObjectOfType<Camera>();

        var modelMatrix = transform.localToWorldMatrix; //オブジェクトのモデリング座標マトリックス
        Debug.Log("modelMatrix: \n" + modelMatrix);

        var vertex = new Vector4(1, 2, 3, 1);　//適当な頂点座標を用意
        Debug.Log(modelMatrix * vertex);　//モデリング座標と用意した頂点で行列変換し、頂点をワールド座標に変換

        var viewMatrix = camera.worldToCameraMatrix; //カメラの座標マトリックス
        Debug.Log("viewMatrix: \n" + viewMatrix); 

        var viewport = viewMatrix * modelMatrix; //カメラとモデリングの座標マトリックスから投影変換
        Debug.Log("viewport: \n" + viewport);

        var targetPos = viewport * vertex; //最終的なディスプレイ座標
        targetPos.z *= -1; //z値を反転
        Debug.Log(targetPos);
    }
}
