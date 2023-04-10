using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DataSet", order = 1)]

public class ZumbaPointList : ScriptableObject
{
  public float duration;
  public List<Vector4> actionList;
}
