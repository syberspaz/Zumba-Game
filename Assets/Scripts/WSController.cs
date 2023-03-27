using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WSController : MonoBehaviour
{
    public GameObject four;
    public GameObject five;
    public GameObject six;
    public GameObject seven;
    public int selectedLength;
    public int selectedWord;
    public string[] fourBank;
    public string[] fiveBank;
    public string[] sixBank;
    public string[] sevenBank;
    WordScramble script;
    public TextMeshProUGUI[] fourLetters;
    public TextMeshProUGUI[] fiveLetters;
    public TextMeshProUGUI[] sixLetters;
    public TextMeshProUGUI[] sevenLetters;
    public GameObject[] fourSlots;
    public GameObject[] fiveSlots;
    public GameObject[] sixSlots;
    public GameObject[] sevenSlots;
    public RectTransform[] fourSlotsPos = new RectTransform[4];
    public RectTransform[] fiveSlotsPos = new RectTransform[5];
    public RectTransform[] sixSlotsPos = new RectTransform[6];
    public RectTransform[] sevenSlotsPos = new RectTransform[7];
    public string temp;
    public System.Random r = new System.Random();
    public Canvas[] canvas;
    public Vector2[] fourPos;
    public Vector2[] fivePos;
    public Vector2[] sixPos;
    public Vector2[] sevenPos;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < fourSlotsPos.Length; i++)
        {
            //fourSlotsPos[i] = fourSlots[i].GetComponent<RectTransform>();
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //    (RectTransform)canvas[0].transform,
            //    fourSlotsPos[i].anchoredPosition,
            //    four.GetComponent<Canvas>().worldCamera,
            //    out fourPos[i]);
            fourPos[i] = fourSlots[i].GetComponent<RectTransform>().anchoredPosition;
        }
        for (int i = 0; i < fiveSlotsPos.Length; i++)
        {
            //fiveSlotsPos[i] = fiveSlots[i].GetComponent<RectTransform>();
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //    (RectTransform)canvas[1].transform,
            //    fiveSlotsPos[i].anchoredPosition,
            //    five.GetComponent<Canvas>().worldCamera,
            //    out fivePos[i]);
            fivePos[i] = fiveSlots[i].GetComponent<RectTransform>().anchoredPosition;
        }
        for (int i = 0; i < sixSlotsPos.Length; i++)
        {
            //sixSlotsPos[i] = sixSlots[i].GetComponent<RectTransform>();
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //    (RectTransform)canvas[2].transform,
            //    sixSlotsPos[i].anchoredPosition,
            //    six.GetComponent<Canvas>().worldCamera,
            //    out sixPos[i]);
            sixPos[i] = sixSlots[i].GetComponent<RectTransform>().anchoredPosition;
        }
        for (int i = 0; i < sevenSlotsPos.Length; i++)
        {
            //sevenSlotsPos[i] = sevenSlots[i].GetComponent<RectTransform>();
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //    (RectTransform)canvas[3].transform,
            //    sevenSlotsPos[i].anchoredPosition,
            //    seven.GetComponent<Canvas>().worldCamera,
            //    out sevenPos[i]);
            sevenPos[i] = sevenSlots[i].GetComponent<RectTransform>().anchoredPosition;
        }
        GenerateWord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateWord()
    {
        selectedLength = Random.Range(4, 8);
        switch (selectedLength)
        {
            case 4:
                four.gameObject.SetActive(true);
                five.gameObject.SetActive(false);
                six.gameObject.SetActive(false);
                seven.gameObject.SetActive(false);
                selectedWord = Random.Range(0, fourBank.Length - 1);
                script = four.gameObject.GetComponent<WordScramble>();
                script.answer = fourBank[selectedWord];
                temp = fourBank[selectedWord];
                temp = new string(temp.ToCharArray().OrderBy(s => r.Next()).ToArray());
                for (int i = 0; i < selectedLength; i++)
                {
                    fourLetters[i].text = temp[i].ToString();
                    //fourSlots[i].transform.localPosition = fourSlotsPos[i].localPosition;
                    //fourSlots[i].transform.position = canvas[0].transform.TransformPoint(fourPos[i]);
                    fourSlots[i].GetComponent<RectTransform>().anchoredPosition = fourPos[i];
                }
                break;
            case 5:
                four.gameObject.SetActive(false);
                five.gameObject.SetActive(true);
                six.gameObject.SetActive(false);
                seven.gameObject.SetActive(false);
                selectedWord = Random.Range(0, fiveBank.Length - 1);
                script = five.gameObject.GetComponent<WordScramble>();
                script.answer = fiveBank[selectedWord];
                temp = fiveBank[selectedWord];
                temp = new string(temp.ToCharArray().OrderBy(s => r.Next()).ToArray());
                for (int i = 0; i < selectedLength; i++)
                {
                    fiveLetters[i].text = temp[i].ToString();
                    //fiveSlots[i].transform.localPosition = fiveSlotsPos[i].localPosition;
                    //fiveSlots[i].transform.position = canvas[1].transform.TransformPoint(fivePos[i]);
                    fiveSlots[i].GetComponent<RectTransform>().anchoredPosition = fivePos[i];
                }
                break;
            case 6:
                four.gameObject.SetActive(false);
                five.gameObject.SetActive(false);
                six.gameObject.SetActive(true);
                seven.gameObject.SetActive(false);
                selectedWord = Random.Range(0, sixBank.Length - 1);
                script = six.gameObject.GetComponent<WordScramble>();
                script.answer = sixBank[selectedWord];
                temp = sixBank[selectedWord];
                temp = new string(temp.ToCharArray().OrderBy(s => r.Next()).ToArray());
                for (int i = 0; i < selectedLength; i++)
                {
                    sixLetters[i].text = temp[i].ToString();
                    //sixSlots[i].transform.localPosition = sixSlotsPos[i].localPosition;
                    //sixSlots[i].transform.position = canvas[2].transform.TransformPoint(sixPos[i]);
                    sixSlots[i].GetComponent<RectTransform>().anchoredPosition = sixPos[i];
                }
                break;
            case 7:
                four.gameObject.SetActive(false);
                five.gameObject.SetActive(false);
                six.gameObject.SetActive(false);
                seven.gameObject.SetActive(true);
                selectedWord = Random.Range(0, sevenBank.Length - 1);
                script = seven.gameObject.GetComponent<WordScramble>();
                script.answer = sevenBank[selectedWord];
                temp = sevenBank[selectedWord];
                temp = new string(temp.ToCharArray().OrderBy(s => r.Next()).ToArray());
                for (int i = 0; i < selectedLength; i++)
                {
                    sevenLetters[i].text = temp[i].ToString();
                    //sevenSlots[i].transform.localPosition = sevenSlotsPos[i].localPosition;
                    //sevenSlots[i].transform.position = canvas[3].transform.TransformPoint(sevenPos[i]);
                    sevenSlots[i].GetComponent<RectTransform>().anchoredPosition = sevenPos[i];
                }
                break;
        }
    }
}
