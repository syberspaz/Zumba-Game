using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoarSDK;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZumbaController : MonoBehaviour
{
    public PlaybackInstance ins;

    public GameObject song1Standing;
    public GameObject song2Standing;
    public GameObject song3Standing;
    public GameObject song4Standing;
    public GameObject song5Standing;
    public GameObject song1Sitting;
    public GameObject song2Sitting;
    public GameObject song3Sitting;
    public GameObject song4Sitting;
    public GameObject song5Sitting;
    //public HandTrackingV2 MyScriptReference;
    public AudioSource song1StandingAudio;
    public AudioSource song2StandingAudio;
    public AudioSource song3StandingAudio;
    public AudioSource song4StandingAudio;
    public AudioSource song5StandingAudio;
    public AudioSource song1SittingAudio;
    public AudioSource song2SittingAudio;
    public AudioSource song3SittingAudio;
    public AudioSource song4SittingAudio;
    public AudioSource song5SittingAudio;
    public float timer;
    public TextMeshProUGUI text;
    public bool isFinished;
    public float songTimer = 3;
    public TextMeshProUGUI score;
    public TextMeshProUGUI results;

    // Start is called before the first frame update
    void Start()
    {
        ins = song1Standing.GetComponent<PlaybackInstance>();
        songTimer = 3;
        isFinished = false;
        song1StandingAudio = song1Standing.gameObject.GetComponent<AudioSource>();
        song2StandingAudio = song2Standing.gameObject.GetComponent<AudioSource>();
        song3StandingAudio = song3Standing.gameObject.GetComponent<AudioSource>();
        song4StandingAudio = song4Standing.gameObject.GetComponent<AudioSource>();
        song5StandingAudio = song5Standing.gameObject.GetComponent<AudioSource>();
        song1SittingAudio = song1Sitting.gameObject.GetComponent<AudioSource>();
        song2SittingAudio = song2Sitting.gameObject.GetComponent<AudioSource>();
        song3SittingAudio = song3Sitting.gameObject.GetComponent<AudioSource>();
        song4SittingAudio = song4Sitting.gameObject.GetComponent<AudioSource>();
        song5SittingAudio = song5Sitting.gameObject.GetComponent<AudioSource>();
        //HandTrackingV2 MyScriptReference = GetComponent<HandTrackingV2>();



    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            text.text = "";
            switch (Menu.song)
            {
                case 1:
                    if (Menu.isStanding)
                    {
                        song1Standing.gameObject.SetActive(true);
                        //set variable for changing moves within here
                        //MyScriptReference.moveTracker = 0;

                        if (!song1StandingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(5);
                            }
                        }
                    }
                    else if (!Menu.isStanding)
                    {
                        song1Sitting.gameObject.SetActive(true);
                        if (!song1SittingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(5);
                            }
                        }
                    }
                    break;
                case 2:
                    if (Menu.isStanding)
                    {
                        song2Standing.gameObject.SetActive(true);
                        //MyScriptReference.moveTracker = 1;
                        if (!song2StandingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(6);
                            }
                        }
                    }
                    else if (!Menu.isStanding)
                    {
                        song2Sitting.gameObject.SetActive(true);
                        if (!song2SittingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(6);
                            }
                        }
                    }
                    break;
                case 3:
                    if (Menu.isStanding)
                    {
                        song3Standing.gameObject.SetActive(true);
                        //MyScriptReference.moveTracker = 2;
                        if (!song3StandingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(7);
                            }
                        }
                    }
                    else if (!Menu.isStanding)
                    {
                        song3Sitting.gameObject.SetActive(true);
                        if (!song3SittingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(7);
                            }
                        }
                    }
                    break;
                case 4:
                    if (Menu.isStanding)
                    {
                        song4Standing.gameObject.SetActive(true);
                        //MyScriptReference.moveTracker = 3;
                        if (!song4StandingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(5);
                            }
                        }
                    }
                    else if (!Menu.isStanding)
                    {
                        song4Sitting.gameObject.SetActive(true);
                        if (!song4SittingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(5);
                            }
                        }
                    }
                    break;
                case 5:
                    if (Menu.isStanding)
                    {
                        song5Standing.gameObject.SetActive(true);
                        //MyScriptReference.moveTracker = 4;
                        if (!song5StandingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(6);
                            }
                        }
                    }
                    else if (!Menu.isStanding)
                    {
                        song5Sitting.gameObject.SetActive(true);
                        if (!song5SittingAudio.isPlaying)
                        {
                            songTimer -= Time.deltaTime;
                            if (songTimer <= 0)
                            {
                                Menu.points += 200;
                                Menu.song++;
                                SceneManager.LoadScene(6);
                            }
                        }
                    }
                    break;
                case 6:
                    //show scores
                    results.gameObject.SetActive(true);
                    score.text = "Your points: " + Menu.points.ToString();
                    break;
            }
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
            text.text = timer.ToString();
        }
    }
}
