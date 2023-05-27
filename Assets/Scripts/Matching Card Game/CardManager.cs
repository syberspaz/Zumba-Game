using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
  public List<Sprite> animalListMaster;
  public List<Sprite> animalList;
  public List<GameObject> cards;

  public GameObject card1;
  public GameObject card2;

  int errorCounter = 0;
  float timer;
  int pairs = 0;
  private void Start()
    {
    animalList = new List<Sprite>(animalListMaster);
        int desiredAnimalCount = Mathf.FloorToInt(cards.Count / 2f);
        List<int> cardIndices = new List<int>();

        for (int i = 0; i < cards.Count; i++)
        {
            cardIndices.Add(i);
        }

        while (animalList.Count > desiredAnimalCount)
        {
            int randIndex = Random.Range(0, cardIndices.Count);
            int cardIndex = cardIndices[randIndex];
            cardIndices.RemoveAt(randIndex);

            Sprite sprite = animalList[Random.Range(0, animalList.Count)];
            cards[cardIndex].GetComponent<Image>().sprite = sprite;
            animalList.Remove(sprite);
        }

        if (animalList.Count * 2 == cards.Count)
        {
            foreach (Sprite sprite in animalList)
            {
                int randIndex = Random.Range(0, cardIndices.Count);
                int cardIndex = cardIndices[randIndex];
                cardIndices.RemoveAt(randIndex);

                cards[cardIndex].GetComponent<Image>().sprite = sprite;
            }
        }
    }
  private void Update() {
    timer += Time.deltaTime;
    if (card1 != null && card2 != null) {
      if (card1.GetComponent<Image>().sprite.name == card2.GetComponent<Image>().sprite.name) {
        //DISABLE THE FLIP AND GET CORRECT
        card1.GetComponent<FlipCard>().locked = true;
        card2.GetComponent<FlipCard>().locked = true;
        card1 = null;
        card2 = null;
        pairs += 1;
        if (pairs >= animalList.Count) {
          //WRITE CSV AND SEND TO NEXT SCENE
          Score.matchingCardErrorCount = errorCounter;
          Score.matchingCardTimer = timer;
          //SEND TO NEXT SCENE
          SceneManager.LoadScene(Menu.Zumba);
        }
      }
    }

  }
  public void FlipCard(GameObject g) {
    if (card1 == null) {
      card1 = g;
    } else if (card2 == null) {
      card2 = g;
    } else {
      //NEWCARD FLIPPED (ALSO THERE FORE OLD PAIR WRONG
      errorCounter += 1;
      card1.GetComponent<FlipCard>().isFront = false;
      card2.GetComponent<FlipCard>().isFront = false;
      card1 = g;
      card2 = null;
    }
  }
}
