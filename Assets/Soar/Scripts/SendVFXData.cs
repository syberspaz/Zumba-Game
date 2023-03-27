using System.Collections;
using System.Collections.Generic;
using SoarSDK;
using UnityEngine;
using UnityEngine.VFX;

public class SendVFXData : MonoBehaviour
{

    [SerializeField]
    public VisualEffect visualEffect;
    public GameObject modelMesh;

    private MeshFilter meshFilter;
    private RenderTexture rgbArray;
    private RenderTexture depthArray;
    private Vector4[] camIntrinsics;
    private Vector4[] cameraPositions;
    private Vector4[] depthSizeArray;
    private float[] cameraNearArray;
    private float[] cameraFarArray;
    private int cameraCount;
    private Matrix4x4[] matrixArray;

    // Update is called once per frame
    void Update()
    {
        SetVFX();
    }


    public void SetVFX()
    {
        meshFilter = modelMesh.GetComponent<MeshFilter>();

        if (modelMesh.GetComponent<PlaybackInstance>() != null && meshFilter.mesh != null)
        {
            meshFilter.mesh.RecalculateNormals();
            visualEffect.GetComponent<Transform>().localPosition = modelMesh.GetComponent<Transform>().localPosition;
            visualEffect.SetMesh("Volumetric Mesh", meshFilter.mesh);
            MaterialPropertyBlock props = modelMesh.GetComponent<PlaybackInstance>().MaterialProps;
            cameraCount = modelMesh.GetComponent<PlaybackInstance>().CameraCount;
            visualEffect.SetInt("cameraCount", cameraCount);
            rgbArray = props.GetTexture("_CameraRGB") as RenderTexture;
            depthArray = props.GetTexture("_CameraDepth") as RenderTexture;
            cameraPositions = props.GetVectorArray("cameraExtrinsics");
            cameraNearArray = props.GetFloatArray("cameraDepthNear");
            cameraFarArray = props.GetFloatArray("cameraDepthFar");
            depthSizeArray = props.GetVectorArray("cameraDepthSize");
            camIntrinsics = props.GetVectorArray("cameraIntrinsics");
            if (rgbArray != null)
            {
                visualEffect.SetTexture("Color Texture Array", rgbArray);
                visualEffect.SetTexture("Depth Array", depthArray);
                matrixArray = props.GetMatrixArray("_CameraMatrices");
                Shader.SetGlobalMatrixArray("_CameraMatrices", matrixArray);
                Shader.SetGlobalVectorArray("cameraExtrinsics", cameraPositions);
                Shader.SetGlobalVectorArray("cameraIntrinsics", camIntrinsics);
                Shader.SetGlobalVectorArray("cameraDepthSize", depthSizeArray);
                Shader.SetGlobalFloatArray("cameraDepthNearVFX", cameraNearArray);
                Shader.SetGlobalFloatArray("cameraDepthFarVFX", cameraFarArray);
            }
        }
    }
}
