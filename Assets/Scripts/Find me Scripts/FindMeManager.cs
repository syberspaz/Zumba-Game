using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    float timer = 0;
  void Start() {
        audioSource = GetComponent<AudioSource>();
        findMeObjectsEditable = new List<GameObject>(findMeObjects);
    target = findMeObjectsEditable[Random.Range(0, findMeObjectsEditable.Count)];
    findMeObjectsEditable.Remove(target);
  }
  void Update() {
    timer += Time.deltaTime;
    text.text = target.GetComponent<FindMeObject>().hint;
    if (count < 5 && target.GetComponent<FindMeObject>().clicked) {
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


        } else if (target.GetComponent<FindMeObject>().clicked) {
            //RETURN ERROR COUNT FOR THIS ONE OBJECT AND THE TIME IT TOOK  
            ShakeText();
      float temp = 0;
      for (int i = 0; i < timeList.Count; i++) {
        temp += timeList[i];
      }
            audioSource.PlayOneShot(correctSound);
            Score.findMeTimeAverage = temp/timeList.Count;
      Score.findMeErrorCount = errorCount;
      SceneManager.LoadScene(Menu.Zumba);
    }
    for (int i = 0; i < findMeObjects.Count; i++) {
      if (findMeObjects[i].GetComponent<FindMeObject>().clicked) {
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
}
