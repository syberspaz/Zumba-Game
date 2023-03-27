using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if SOAR_HAS_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif
public class SendLightData : MonoBehaviour
{

    private MeshRenderer meshRender;
    private GameObject lightGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #if SOAR_HAS_HDRP
        if (gameObject.GetComponent<MeshRenderer>() != null)
        {
            meshRender = gameObject.GetComponent<MeshRenderer>();
            lightGameObject = GameObject.FindGameObjectWithTag("Light");
            var rotatedVector = lightGameObject.transform.rotation * Vector3.back;
            var lightData = lightGameObject.GetComponent<HDAdditionalLightData>();
            var colorTemp = Mathf.CorrelatedColorTemperatureToRGB(lightGameObject.GetComponent<Light>().colorTemperature);
            meshRender.material.SetColor("_LightColor", lightData.color);
            meshRender.material.SetFloat("_Diffuse", (lightData.intensity / 130000.0f));
            meshRender.material.SetVector("_LightPos", rotatedVector);
            meshRender.material.SetColor("_TemperatureColor", colorTemp);
        }
        #endif
    }
}
