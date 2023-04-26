using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointChanger : MonoBehaviour
{
  public List<ZumbaPointList> pointList;
  public ZumbaPointList GetSongPointList(int index) {
    return pointList[index];
  }
}
