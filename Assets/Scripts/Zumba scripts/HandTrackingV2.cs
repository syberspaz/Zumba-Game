using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandTrackingV2 : MonoBehaviour {
  public ZumbaPointList scriptObject;
  public List<Vector4> inOrderAction;
  public List<Vector3> currentTrackedPoints;
  public List<float> closestTrackedPoints;
  public List<float> currentTrackedPointsTimer;
  public List<float> accuracyList;
  public float timer;
  public int index;
  public bool started = false;
  public float subTimer;
  public int recentAccuracyCount;
  public float averageRecentAccuracy;
  public float accuracyTimeInterval = 5;
  public int moveTracker = 0;
  public float increaseRate = 5.0f; // Amount to increase per second
  private float elapsedTime = 0.0f;
  private AverageRecentRating ARR;

  private void Start()
    {
        inOrderAction = new List<Vector4>(GetComponent<PointChanger>().pointList[moveTracker].actionList);
        ARR = FindObjectOfType<AverageRecentRating>();

    }
    private void Update() {
    if (started && SceneManager.GetActiveScene() == SceneManager.GetSceneAt(Menu.Zumba)) {
      elapsedTime += Time.deltaTime;
      averageRecentAccuracy += increaseRate * Time.deltaTime;
      timer += Time.deltaTime;
      if (inOrderAction.Count > index) {
        if (timer > inOrderAction[index].w - 1) {
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
          averageRecentAccuracy += temp;
          recentAccuracyCount += 1;
          if (subTimer >= accuracyTimeInterval) {
            subTimer = 0;
            averageRecentAccuracy = averageRecentAccuracy / recentAccuracyCount;
            if (ARR == null) {
              ARR = GameObject.Find("Manager").GetComponent<AverageRecentRating>();
            }
            ARR.average += averageRecentAccuracy;
            ARR.jointCount += 1;
            // RECENT ACCURACY OUTPUT IN A DECIMAL FORMAT (IE 0.0 - 1.0 is 0 to  100%)
            //INSERT STAR CODE HERE
            if (averageRecentAccuracy >= 10)
            {
               ARR.score = 1;
            }
            if (averageRecentAccuracy >= 20 && averageRecentAccuracy <= 30)
            {
                ARR.score = 2;
            }
            if (averageRecentAccuracy >= 31 && averageRecentAccuracy <= 40)
            {
                ARR.score = 3;
            }
            if (averageRecentAccuracy >= 41 && averageRecentAccuracy <= 50)
            {
                ARR.score = 4;
            }
            if (averageRecentAccuracy >= 51)
            {
                ARR.score = 5;
            }
            averageRecentAccuracy = 0;

          }
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
