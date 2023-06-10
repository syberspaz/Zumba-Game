using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriting : MonoBehaviour {
  //[System.Serializable]


   public string fileName = "/test.csv";
   public bool instantiate = true;
   public string whatTimeIsIt;
   TextWriter tw;

  private void Start() {
    fileName = Application.dataPath + fileName;
    System.DateTime currentTime = System.DateTime.Now;
    whatTimeIsIt = currentTime.ToString();
    if (File.Exists(fileName)) instantiate = false;
    else instantiate = true;
  }
  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) WriteCSV();
  }
  public void WriteCSV() {
    if (instantiate) {

      tw = new StreamWriter(fileName, false);
      tw.WriteLine("ID,Date,Find Me Time Average,Find Me Error Count,Matching Card Time,Matching Card Error Count,Word Scramble Time (-1 is a skip), Current Time");
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
          Score.JigsawTime + "," +
          whatTimeIsIt);


        tw.Close();
  }
}