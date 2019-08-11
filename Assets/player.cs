using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    enum Movement
    {
        NotMoving, IsMoving
    }

    public RectTransform RT;
    public GameManager GM;
    Movement currentState = Movement.NotMoving;
    public int[] location = { 0, 0 };
    public int[] destination = { 0, 0 };
    int columns;
    public float speed = 5.0f;
    public float minDist = 0.2f;
    float startTime;
    float journeyLength;
    float distCovered;
    float fracJourney;
    Vector3 startMarker;
    AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        GM = FindObjectOfType<Canvas>().GetComponent<GameManager>();
        RT = GetComponent<RectTransform>();
    }

    public void Move(int[] direction)
    {
        if (!GM.PauseScreen.activeSelf)
        {
            if (currentState == Movement.NotMoving)
            {
                int[] desiredLocation = { location[0] + direction[0], location[1] + direction[1] };
                if (withinRange(desiredLocation, GM.columns))
                {
                    transform.SetParent(GM.GridGen.tilesGO[convArrToInt(desiredLocation)].transform);
                    if (convArrToInt(desiredLocation) == GM.CurrEggLocation)
                    {
                        Destroy(GM.CurrEgg);
                        audioManager.OnCoinCollect();
                        GM.SpawnEgg(desiredLocation);
                        GM.score++;
                        GM.ScoreBoard.text = "" + GM.score;
                        PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore", 0) + 1);
                        if (GM.score > PlayerPrefs.GetInt("Highscore"))
                        {
                            PlayerPrefs.SetInt("Highscore", GM.score);
                        }
                        GM.totalScoreBoard.text = "" + PlayerPrefs.GetInt("TotalScore");
                    }

                    currentState = Movement.IsMoving;
                    startMarker = RT.localPosition;
                    destination = desiredLocation;
                    startTime = Time.time;
                    journeyLength = Vector3.Magnitude(RT.localPosition);
                }
            }
        }
    }

    private void Update()
    {
        if (currentState == Movement.IsMoving)
        {
            if (Vector3.Magnitude(RT.localPosition) < minDist)
            {
                RT.localPosition = Vector3.zero;
                currentState = Movement.NotMoving;
                location = destination;
            }

            if (location != destination)
            {
                distCovered = (Time.time - startTime) * speed;
                fracJourney = distCovered / journeyLength;
                RT.localPosition = Vector3.Lerp(startMarker, Vector3.zero, fracJourney);
            }
        }

    }

    bool withinRange(int[] array, int size)
    {
        if (array[0] > -size && array[0] < 1 && array[1] < size && array[1] > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int convArrToInt(int[] intArr)
    {
        return intArr[0] * -GM.columns + intArr[1];
    }

    
}
