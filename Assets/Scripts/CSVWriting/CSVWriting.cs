using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriting : MonoBehaviour {
  //[System.Serializable]


  public string fileName = "/test.csv";
  private string currentTimeString;
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
      System.DateTime currentTime = System.DateTime.Now;
      currentTimeString = currentTime.ToString();
      tw = new StreamWriter(fileName, false);
      tw.WriteLine("ID,Date,Find Me Time Average,Find Me Error Count,Matching Card Time,Matching Card Error Count,Word Scramble Time (-1 is a skip)");
      tw.Close();
      instantiate = false;
    }
    tw = new StreamWriter(fileName, true);

        tw.WriteLine(Score.ID + "," +
          DateTime.Today + "," +
          Score.findMeTimeAverage + "," +
          Score.findMeErrorCount + "," +
          Score.matchingCardTimer + "," +
          Score.matchingCardErrorCount + "," +
          Score.wordScrambleTime + "," + 
          currentTimeString);


        tw.Close();
  }
}