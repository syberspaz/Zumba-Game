using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriting : MonoBehaviour
{
    [System.Serializable]
    public class Scores
    {
        public float score1;
        public float score2;
        public float score3;
        public float score4;
    }
    [System.Serializable]
    public class ScoreList
    {
        public Scores[] scores;
    }

    public string fileName = "/test.csv";
    public ScoreList scoreList = new ScoreList();
    public bool instantiate = true;
    TextWriter tw;

    private void Start()
    {
        fileName = Application.dataPath + fileName;
        if (File.Exists(fileName)) instantiate = false;
        else instantiate = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) WriteCSV();
    }
    public void WriteCSV()
    {
        if (scoreList.scores.Length > 0)
        {
            if (instantiate)
            {
                tw = new StreamWriter(fileName, false);
                tw.WriteLine("score 1,score 2,score 3,score 4");
                tw.Close();
                instantiate = false;
            }
            tw = new StreamWriter(fileName, true);

            for (int i = 0; i < scoreList.scores.Length; i++)
            {
                tw.WriteLine(scoreList.scores[i].score1 + "," +
                  scoreList.scores[i].score2 + "," +
                  scoreList.scores[i].score3 + "," +
                  scoreList.scores[i].score4 + ",");
            }
            tw.Close();
        }
    }
}