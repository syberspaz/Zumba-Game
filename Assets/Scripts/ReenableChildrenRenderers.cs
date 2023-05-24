using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReenableChildrenRenderers : MonoBehaviour
{
    // This is needed for the FIND ME GAMES SPECIFICALLY, as the positions of the kinect tracker are used a raycast for the custom cursor, so it's needed to actually see what you are hovering over. 
    // Start is called before the first frame update

    public GameObject kinectMeshesEnable;

    void Start()
    {
        kinectMeshesEnable = GameObject.Find("pointBody");

        if (kinectMeshesEnable = null)
        {
            EnableMeshRenderersRecursive(kinectMeshesEnable.transform);
        }
        else
        {
            Debug.LogError("Parent object is not assigned!");
        }

    }

    void EnableMeshRenderersRecursive(Transform parent)
    {
        // Disable MeshRenderers in the current parent object
        MeshRenderer[] renderers = parent.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        // Disable MeshRenderers in the child objects recursively, this is needed to disable to disable the meshrenderer's of the child objects children as well 
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            EnableMeshRenderersRecursive(child);
        }
    }
}