using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MoveDirection
{
    Left, Right, Up, Down
}

public class GameManager : MonoBehaviour
{
    public GridGenerator GridGen;
    public GameObject MainChar;
    public GameObject PlayerChar;
    public GameObject EggPrefab;
    public GameObject CurrEgg;
    public Text ScoreBoard;
    public Image Background;
    public int CurrEggLocation;
    public player PlayerSCR;
    public GameObject OverLay;
    public GameObject OverLayInstant;
    public Image OverLayImg;
    public RectTransform OverlayRT;
    public AutoGridLayout AGL;
    public int columns;
    public float baseCD = 3.0f;
    float tempCD;
    public int score = 0;
    public GameObject PauseScreen;
    public Text HighscorePause;
    int currentStyle;
    public Image thumbnail;
    faceDir FD;
    public bool gameOver = false;
    public float gameOverTimer = -1f;
    public AdMobManager AMM;
    public int totalScore;
    public Text totalScoreBoard;
    bool endGame = false;
    public GameObject EndScreen;

    public GameObject purchaseButton;
    public Text purchasePrice;

    InputManager IM;


    void Start()
    {
        Application.targetFrameRate = 60;

        loadAds();
        platformTouch();

        GridGen = GetComponent<GridGenerator>();
        GridGen.DrawSquare(columns);
        
        findComponents();

        Spawn();
        unlockChars();
    }

    // Update is called once per frame
    void Update()
    {
        //If player standing on hot he dies
        if(PlayerChar != null)
        {
            /*
            if (GridGen.tiles[PlayerSCR.convArrToInt(PlayerSCR.location)].TileState == gridState.Hot)
            {
                Destroy(PlayerChar);
                Destroy(OverLayInstant);
                GameOver();
            }
            */
            OverlayRT.position = PlayerSCR.RT.position;
        }

        if (gameOver)
        {
            gameOverTimer += Time.deltaTime;
        }

        if (gameOverTimer > 0 && !endGame){
            endGame = true;
            GameOver();
        }
        

    }

    void findComponents()
    {
        //ScoreBoard = transform.Find("MainPanel").Find("ScoreBoard").Find("Points").GetComponent<Text>();
        //Background = transform.Find("MainPanel").GetComponent<Image>();
        Background.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style")].IMG_Background;
        //PauseScreen = transform.Find("PausePanel").gameObject;
        PauseScreen.SetActive(false);
        //EndScreen = transform.Find("EndPanel").gameObject;
        EndScreen.SetActive(false);
        totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        //totalScoreBoard = transform.Find("MainPanel").Find("TotalCoins").GetComponent<Text>();
        totalScoreBoard.text = "" + PlayerPrefs.GetInt("TotalScore");
    }

    void loadAds()
    {
        AMM = Camera.main.GetComponent<AdMobManager>();
        AMM.OnClickShowBanner();
    }

    void platformTouch()
    {
        #if UNITY_ADROID
        IM = GetComponent<InputManager>();
        IM.enabled = false;
        #elif UNITY_IPHONE
        IM = GetComponent<InputManager>();
        IM.enabled = false;
        #endif       
    }

    void unlockChars()
    {
        for (int i = 0; i < StyleContainer.Instance.Styles.Length; i++)
        {
            if (PlayerPrefs.GetInt("isLocked" + i, 1) == 0)
            {
                StyleContainer.Instance.Styles[i].isLocked = false;
            }
            else
            {
                StyleContainer.Instance.Styles[i].isLocked = true;
            }
        }
        StyleContainer.Instance.Styles[0].isLocked = false;
    }

    public void Move(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Down:
                PlayerSCR.Move(new int[] { -1, 0 });
                break;
            case MoveDirection.Up:
                PlayerSCR.Move(new int[] { 1, 0 });
                break;
            case MoveDirection.Left:
                FD.faceLeft();
                PlayerSCR.Move(new int[] { 0, -1 });
                break;
            case MoveDirection.Right:
                FD.faceRight();
                PlayerSCR.Move(new int[] { 0, 1 });
                break;
            default:
                Debug.LogError("wrong input for direction");
                break;
        }
    }

    public void Spawn()
    {
        PlayerChar = Instantiate(MainChar, GridGen.tilesGO[0].transform);
        PlayerSCR = PlayerChar.GetComponent<player>();
        OverLayInstant = Instantiate(OverLay, transform.Find("MainPanel").transform);
        Image temp = OverLayInstant.GetComponentInChildren<Image>();
        FD = OverLayInstant.GetComponentInChildren<faceDir>();
        temp.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style")].SPR_Player;
        OverlayRT = OverLayInstant.GetComponent<RectTransform>();
        OverLayImg = OverLayInstant.GetComponentInChildren<Image>();
        SpawnEgg(PlayerSCR.location);
    }

    public void SpawnEgg(int[] desiredLocation)
    {
        int rand = Random.Range(0, GridGen.tiles.Count - 1);
        while (rand == PlayerSCR.convArrToInt(desiredLocation))
        {
            rand = Random.Range(0, GridGen.tiles.Count - 1);
        }
        CurrEggLocation = rand;
        CurrEgg = Instantiate(EggPrefab, GridGen.tilesGO[rand].transform);
        Image temp = CurrEgg.GetComponentInChildren<Image>();
        temp.sprite = StyleContainer.Instance.Styles[PlayerPrefs.GetInt("Style")].SPR_Coin;
    }

    public void Restart()
    {
        AMM.DeleteBanner();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GridGen.paused = false;
    }

    public void Pause()
    {
        PauseScreen.SetActive(true);
        GridGen.paused = true;
    }

    public void Resume()
    {
        PauseScreen.SetActive(false);
        GridGen.paused = false;
    }

    public void NextStyle()
    {
        int length = StyleContainer.Instance.Styles.Length;
        if(currentStyle + 2 <= length)
        {
            currentStyle++;
            if (!StyleContainer.Instance.Styles[currentStyle].isLocked)
            {
                PlayerPrefs.SetInt("Style", currentStyle);
            }
        }
        DisplayStyle();
    }

    public void PrevStyle()
    {
        if (currentStyle >= 1)
        {
            currentStyle--;
            if (!StyleContainer.Instance.Styles[currentStyle].isLocked)
            {
                PlayerPrefs.SetInt("Style", currentStyle);
            }
        }
        DisplayStyle();
    }

    public void DisplayStyle()
    {
        if (!StyleContainer.Instance.Styles[currentStyle].isLocked)
        {
            purchaseButton.SetActive(false);
            thumbnail.sprite = StyleContainer.Instance.Styles[currentStyle].thumbnail;
        }
        else
        {
            purchasePrice.text = "" + StyleContainer.Instance.Styles[currentStyle].price;
            purchaseButton.SetActive(true);
            thumbnail.sprite = StyleContainer.Instance.Styles[currentStyle].dead;
        }
        thumbnail.preserveAspect = true;
    }

    public void Purchase()
    {
        if (StyleContainer.Instance.Styles[currentStyle].isLocked)
        {
            if(PlayerPrefs.GetInt("TotalScore") > StyleContainer.Instance.Styles[currentStyle].price)
            {
                StyleContainer.Instance.Styles[currentStyle].isLocked = false;
                PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") - StyleContainer.Instance.Styles[currentStyle].price);
            }
        }
        PlayerPrefs.SetInt("isLocked" + currentStyle, 0);
        DisplayStyle();
    }

    public void GameOver()
    {
        currentStyle = PlayerPrefs.GetInt("Style", 0);
        EndScreen.SetActive(true);
        Text tempText = EndScreen.transform.Find("Highscore").GetComponent<Text>();
        tempText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("Highscore");
        Text tempText2 = EndScreen.transform.Find("Points").GetComponent<Text>();
        tempText2.text = "" + score;

        thumbnail = transform.Find("EndPanel").Find("StyleSelection").Find("Thumbnail").GetComponent<Image>();
        DisplayStyle();
    }
}
