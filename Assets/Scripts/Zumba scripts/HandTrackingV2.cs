using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingV2 : MonoBehaviour
{
  public ZumbaPointList scriptObject;
  public List<Vector4> inOrderAction;
  public List<Vector3> currentTrackedPoints;
  public List<float> closestTrackedPoints;
  public List<float> currentTrackedPointsTimer;
  public List<float> accuracyList;
  public float timer;
  public int index;
  public bool started = false;
  private void Start() {
    inOrderAction = new List<Vector4>(scriptObject.actionList);
  }
  private void Update() {
    if (started) {
      timer += Time.deltaTime;
      if (inOrderAction.Count > index) {
        if (timer > inOrderAction[index].w) {
          currentTrackedPoints.Add(inOrderAction[index]);
          closestTrackedPoints.Add(Vector3.Distance(transform.localPosition, inOrderAction[index]));
          currentTrackedPointsTimer.Add(timer + scriptObject.duration);
          index++;
        }
      }
      for (int i = 0; i < currentTrackedPoints.Count; i++) {
        if (Vector3.Distance(transform.localPosition, currentTrackedPoints[i]) < closestTrackedPoints[i]) {
          closestTrackedPoints[i] = Vector3.Distance(transform.localPosition, currentTrackedPoints[i]);
        }
        if (timer > currentTrackedPointsTimer[i]) {
          float temp;
          if (closestTrackedPoints[i] > 0.4) {
            temp = 0;
          } else {
            temp = (closestTrackedPoints[i] / 0.4f) * 100;
          }
          accuracyList.Add(temp);
          currentTrackedPoints.RemoveAt(i);
          closestTrackedPoints.RemoveAt(i);
          currentTrackedPointsTimer.RemoveAt(i);
          i--;
        }
      }
    }
  }
  public void UpdateList(ZumbaPointList zpl) {
    inOrderAction = new List<Vector4>(zpl.actionList);
    index = 0;
    timer = 0;
  }

}
