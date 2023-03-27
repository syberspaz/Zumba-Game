using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class CollisionZoneInfo : MonoBehaviour
{
    public string collidedWithName;
    
};



public class CollisionZone : MonoBehaviour
{
    public CollisionZoneInfo zoneInfo;
    public Vector3Int gridIndex;
    public List<GameObject> collisionObject;
    Renderer rend;
    Material mat;
    float alphaDelta=0.8f;
    float alphaStart = 0.3f;
    Color col;

    public AnimationClip clip;
    private GameObjectRecorder m_Recorder;
    public bool isActivated = false;




    // Start is called before the first frame update
    void Start()
    {
        zoneInfo = gameObject.AddComponent(typeof(CollisionZoneInfo)) as CollisionZoneInfo;
        rend = GetComponent<Renderer>();
        mat = rend.material;
        col = mat.color;
        col.a = alphaStart;
        mat.color = new Color(1,1,1,alphaStart);// SetColor("_Color", col);


        m_Recorder = new GameObjectRecorder(gameObject);
        //m_Recorder.BindComponentsOfType(gameObject, typeof(CollisionZoneInfo), false);
        m_Recorder.BindAll(gameObject, false);


    }
    void LateUpdate()
    {
        if (clip == null) return;
        m_Recorder.TakeSnapshot(Time.deltaTime);
    }
    void OnDisable()
    {
        if (clip == null) return;
        if (m_Recorder.isRecording)
        {
            m_Recorder.SaveToClip(clip);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        GameObject collidedWith = other.gameObject;
        Debug.Log("triggerEnter: " + collidedWith.name);
        zoneInfo.collidedWithName = collidedWith.name;
        isActivated = true;
        // if list is empty, any will do
        // otherwise check if the object was in the list
        col.a += alphaDelta;
       // mat.SetColor("_Color", col);
        mat.color = new Color(1, 0, 0, col.a);
    }

    void OnTriggerExit()
    {
        col.a -= alphaDelta;
        mat.SetColor("_Color", col);
        mat.color = new Color(1, 0, 1, col.a);
        zoneInfo.collidedWithName = "";
        isActivated = false;
        //print("No longer in contact with " + collisionInfo.transform.name);
    }
}
