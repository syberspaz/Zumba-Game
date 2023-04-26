using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriting : MonoBehaviour {
  //[System.Serializable]


  public string fileName = "/test.csv";
  public bool instantiate = true;
  TextWriter tw;

  private void Start() {
    fileName = Application.dataPath + fileName;
    if (File.Exists(fileName)) instantiate = false;
    else instantiate = true;
  }
  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) WriteCSV();
  }
  public void WriteCSV() {
    if (instantiate) {
      tw = new StreamWriter(fileName, false);
      tw.WriteLine("score 1,score 2,score 3,score 4");
      tw.Close();
      instantiate = false;
    }
    tw = new StreamWriter(fileName, true);

    tw.WriteLine(Score.a + "," +
      Score.a + "," +
      Score.a + "," +
      Score.a + ",");

    tw.Close();
  }
}