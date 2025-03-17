using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class FindMeManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public List<GameObject> findMeObjects;
    public List<GameObject> findMeObjectsEditable;
    public List<float> timeList;

    public int errorCount;
    public GameObject target;
    public int count = 0;

    public AudioSource audioSource;
    public AudioClip correctSound;

    private string filePath;

    float timer = 0;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        findMeObjectsEditable = new List<GameObject>(findMeObjects);
        target = findMeObjectsEditable[Random.Range(0, findMeObjectsEditable.Count)];
        findMeObjectsEditable.Remove(target);

        filePath = Application.dataPath + "/MetricsTest.csv";
    }
    void Update()
    {
        timer += Time.deltaTime;
        text.text = target.GetComponent<FindMeObject>().hint;
        if (count < 5 && target.GetComponent<FindMeObject>().clicked)
        {
            count += 1;
            target.GetComponent<FindMeObject>().clicked = false;
            target = findMeObjectsEditable[Random.Range(0, findMeObjectsEditable.Count)];
            findMeObjectsEditable.Remove(target);
            timeList.Add(timer);
            timer = 0;

            if (correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }


        }
        else if (target.GetComponent<FindMeObject>().clicked)
        {
            //RETURN ERROR COUNT FOR THIS ONE OBJECT AND THE TIME IT TOOK  
            ShakeText();
            float temp = 0;
            for (int i = 0; i < timeList.Count; i++)
            {
                temp += timeList[i];
            }
            audioSource.PlayOneShot(correctSound);
            Score.findMeTimeAverage = temp / timeList.Count;
            Score.findMeErrorCount = errorCount;

            SaveMetricsToCSV();

            SceneManager.LoadScene(Menu.Zumba);
        }
        for (int i = 0; i < findMeObjects.Count; i++)
        {
            if (findMeObjects[i].GetComponent<FindMeObject>().clicked)
            {
                errorCount++;
                findMeObjects[i].GetComponent<FindMeObject>().clicked = false;
                ShakeText();
            }
        }

    }
    void ShakeText()
    {
        StartCoroutine(ShakeTextCoroutine());
    }
    IEnumerator ShakeTextCoroutine()
    {
        Vector3 originalPos = text.transform.position;

        float shakeAmount = 10f;
        float shakeDuration = 0.5f;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float xOffset = Random.Range(-shakeAmount, shakeAmount);
            float yOffset = Random.Range(-shakeAmount, shakeAmount);
            text.transform.position = new Vector3(originalPos.x + xOffset, originalPos.y + yOffset, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        text.transform.position = originalPos; // Reset to original position after shake
    }
    void SaveMetricsToCSV()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        // Open or create the CSV file
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            // Check if the file is empty and write the headers
            if (new FileInfo(filePath).Length == 0)
            {
                writer.WriteLine("Scene Name,Correct Answers,Error Count,Average Time");
            }

            // Write the metrics to the CSV file
            writer.WriteLine($"{sceneName},{count},{errorCount},{Score.findMeTimeAverage}");


            // Debug log to confirm the data is being written
            Debug.Log("Metrics Saved to CSV: Scene: " + sceneName + ", Correct Answers: " + count + ", Error Count: " + errorCount + ", Average Time: " + Score.findMeTimeAverage);
        }
    }
}