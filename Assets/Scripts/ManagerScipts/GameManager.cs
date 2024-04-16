using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public EnvironmentHandler environmentHandler;
    public CoinsSpawner coinsSpawner;
    public LeaderBoard leaderBoard;

    [Space]
    [SerializeField] Animator _cameraAnimator;
    [Header("                                                                Audio Data")]
    public AudioSource bgSource;
    public AudioSource SfxSource;
    [Space]
    public AudioClip mmClip;
    public AudioClip bgClip;
    public AudioClip gameOverClip;
    public AudioClip tapClip;
    public AudioClip buttonClip;
    public AudioClip explotionClip;
    public AudioClip coinClip;

    [Header("                                                                UI Data")]

    [Header("Start Screen")]
    [SerializeField] GameObject gameStartScreen;
    [SerializeField] GameObject dummyPlanes;
    [SerializeField] bool soundStatus = true;
    [SerializeField] Image soundImage;
    [SerializeField] Sprite soundImageOn;
    [SerializeField] Sprite soundImageOff;

    [SerializeField] List<Image> planeSlectBg;
    [SerializeField] Color selectedColor = new(255, 255, 255, 1);
    [SerializeField] Color notSelectedColorr = new(255, 255, 255, 1);
    [Space]
    [SerializeField] GameObject leaderBoardUI;
    [SerializeField] Transform contentScore;
    [SerializeField] int elementCount = 10;
    [SerializeField] GameObject scoreUIElement;
    [SerializeField] List<TextMeshProUGUI> scoreTextList = new();

    [Header("GamePlay Screen")]

    [SerializeField] GameObject gamePlayScreen;

    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] GameObject getReadyText;
    [SerializeField] AnimationCurve getReadyAnimCurve;
    [SerializeField] GameObject tappingHand;

    [SerializeField] TextMeshProUGUI playerScore;
    [Header("GamePlay Screen")]
    [SerializeField] GameObject pauseScreen;
    [Header("GameOver Screen")]
    [SerializeField] GameObject gameOverScreen;
    [Space]
    [SerializeField] TextMeshProUGUI collectCoins;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] GameObject InputPlayerNamePanel;
    [SerializeField] TMP_InputField InputField;


    [Space]
    [Header("                                                                GamePlay")]
    [Header("Player Data")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] ParticleSystem playerDeathPartcle;
    [SerializeField] PlaneColor planeColor = PlaneColor.RedPlane;
    [Space]
    [SerializeField] Transform playerSpawnPos;
    [SerializeField] Transform playerGamePos;
    [SerializeField] AnimationCurve playerAnimCurve;
    [SerializeField] float playerMoveInTime;
    [Space]
    [SerializeField] int collectedCoins;
    [SerializeField] ParticleSystem playerCoinPartcle;

    PlaneController playerController;
    Rigidbody2D playerRigidbody;
    int totalScore;

    public static GameManager Instance;
    private void Awake()
    {
        Application.targetFrameRate = 30;
    }
    private void Start()
    {
        Instance = this;
        InItUIData();

    }
    private void Update()
    {
        if (gameStarted && Input.anyKey)
        {
            StartGame();
            tappingHand.SetActive(false);
            gameStarted = false;
            playerRigidbody.AddForce(30 * Vector2.up, ForceMode2D.Impulse);
        }

    }
    public void ChangeMusic(AudioClip newClip)
    {
        StartCoroutine(ChangeMusicRoutine(newClip));
    }
    IEnumerator ChangeMusicRoutine(AudioClip clipNew)
    {
        bgSource.Stop();
        bgSource.volume = 1f;
        bgSource.clip = clipNew;

        yield return null;
        bgSource.Play();

    }
    void InItUIData()
    {
        gameStartScreen.SetActive(true);
        gamePlayScreen.SetActive(false);
        gameOverScreen.SetActive(false);

        playerScore.gameObject.SetActive(false);
        playerScore.text = $"Coins : {collectedCoins}";

        countDownText.gameObject.SetActive(false);
        getReadyText.SetActive(false);
        CreateUIEmelents();
    }
    bool gameStarted = false;


    public void OnSoundButtonClick()
    {
        soundStatus = !soundStatus;
        if (!soundStatus)
        {
            soundImage.sprite = soundImageOn;
            bgSource.volume = 0;
        }
        else
        {
            soundImage.sprite = soundImageOff;
            bgSource.volume = 1;
        }

        ButtonClickSound();
    }

    public void OnLeaderBoardClick()
    {
        leaderBoardUI.SetActive(true);
        SetScoreUI();

    }
    public void OnCloseLeaderBoardUI()
    {
        leaderBoardUI.SetActive(false);
    }
    void CreateUIEmelents()
    {
        GameObject temp;
        for (int i = 0; i < elementCount; i++)
        {
            temp = Instantiate(scoreUIElement, contentScore);
            scoreTextList.Add(temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }
    void SetScoreUI()
    {
        string newText;
        for (int i = 0; i < elementCount; i++)
        {
            if (i < leaderBoard.scoreList.topScoreList.Count)
            {
                newText = $"{i + 1}: {leaderBoard.scoreList.topScoreList[i].playerName}  {leaderBoard.scoreList.topScoreList[i].playerScore}";
                scoreTextList[i].text = newText;

            }
            else
            {
                scoreTextList[i].transform.parent.gameObject.SetActive(false);

            }
        }

    }
    void ButtonClickSound() => SfxSource.PlayOneShot(buttonClip);



    public void OnPlaneSelect(int index)
    {
        planeColor = (PlaneColor)index;
        foreach (Image img in planeSlectBg)
        {
            img.color = notSelectedColorr;
        }
        planeSlectBg[index].color = selectedColor;

    }
    public void OnClickPlayGame()
    {
        gameStartScreen.SetActive(false);
        gamePlayScreen.SetActive(true);
        SpawnPlayer();
        PlayGame();
        dummyPlanes.SetActive(false);
        ButtonClickSound();


    }
    void PlayGame()
    {
        StartCoroutine(MovePlayerInScreen(playerMoveInTime));
        StartCoroutine(ShowCountDown());
        ChangeMusic(bgClip);

    }
    void SpawnPlayer()
    {


        playerPrefab.GetComponent<PlaneController>().planeColor = planeColor;

        playerPrefab = Instantiate(playerPrefab, transform.position, Quaternion.identity);


        playerController = playerPrefab.GetComponent<PlaneController>();
        playerController.enabled = false;

        playerRigidbody = playerPrefab.GetComponent<Rigidbody2D>();
        playerRigidbody.simulated = false;

        playerPrefab.transform.position = new Vector3(15, 0);




    }

    IEnumerator MovePlayerInScreen(float moveTime = 3f)
    {

        float elaspedTime = 0;
        while (elaspedTime < moveTime)
        {
            playerPrefab.transform.position = Vector3.Lerp(playerSpawnPos.position, playerGamePos.position, playerAnimCurve.Evaluate((elaspedTime / moveTime)));
            elaspedTime += Time.deltaTime;
            yield return null;

        }

    }
    IEnumerator ShowCountDown(float countDownTime = 3)
    {


        countDownText.gameObject.SetActive(true);
        ScalingEffect scallingEffect = countDownText.GetComponent<ScalingEffect>();
        while (countDownTime > 0f)
        {
            scallingEffect.PlayEffect();
            countDownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime -= 1f;
            ButtonClickSound();
        }

        countDownText.gameObject.SetActive(false);
        getReadyText.SetActive(true);

        yield return new WaitForSeconds(scallingEffect.time + .5f);

        getReadyText.SetActive(false);

        gameStarted = true;
        tappingHand.SetActive(true);


    }
    #region GamePlay

    void StartGame()
    {

        playerController.enabled = true;
        playerRigidbody.simulated = true;
        playerScore.gameObject.SetActive(true);
        environmentHandler.CanSpawnObstacle = true;
        coinsSpawner.canSpawnCoin = true;

    }

    public void OnClickPause()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        ButtonClickSound();
        ChangeMusic(mmClip);

    }
    public void OnClickResume()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        ButtonClickSound();
        ChangeMusic(bgClip);
    }
    #endregion


    #region GameOver
    public void ShakeCamera()
    {
        _cameraAnimator.SetTrigger("ShakeCamera");
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }
    public void AddCoin(Transform pos)
    {
        collectedCoins++;
        playerScore.text = $"Coins : {collectedCoins}";
        playerCoinPartcle.Play();
        playerCoinPartcle.transform.position = pos.position;
        SfxSource.PlayOneShot(coinClip);

    }
    IEnumerator GameOverRoutine()
    {
        SfxSource.PlayOneShot(explotionClip);
        ShakeCamera();

        playerController.enabled = false;
        playerRigidbody.simulated = false;
        playerDeathPartcle.transform.position = playerPrefab.transform.position;
        playerPrefab.transform.position = new Vector3(15, 15, 0f);




        environmentHandler.SetEnvironment(false);
        environmentHandler.CanSpawnObstacle = false;
        coinsSpawner.canSpawnCoin = false;

        playerDeathPartcle.Play();
        yield return new WaitForSeconds(.5f);

        bgSource.volume = 0f;
        yield return null;

        SfxSource.PlayOneShot(gameOverClip);

        gamePlayScreen.SetActive(false);


        int coinsBonus = collectedCoins * 2;
        float timeBonus = environmentHandler.CurrentGameTime * 1.2f;

        totalScore = coinsBonus + Mathf.RoundToInt(timeBonus);

        if (!leaderBoard.IsNewTopScore(totalScore))
        {
            GameOverDialogue();
        }
        else
            InputPlayerNamePanel.SetActive(true);




    }

    void GameOverDialogue()
    {


        collectCoins.text = collectedCoins.ToString();
        timeText.text = (Mathf.RoundToInt(environmentHandler.CurrentGameTime)).ToString();

        totalScore = Mathf.RoundToInt(totalScore);

        totalScoreText.text = $"TOTAL SCORE = {totalScore}";

        collectedCoins = 0;
        playerScore.text = "SCORE :";


        InputPlayerNamePanel.SetActive(false);
        gameOverScreen.SetActive(true);

    }
    public void OnClickSubmitScore()
    {
        string name = InputField.text != string.Empty ? InputField.text : $"{DateTime.Now:MMM/dddd:hh:mm}";
        PlayerScoreData ps = new()
        {
            playerName = name,
            playerScore = totalScore
        };

        leaderBoard.AddNewScore(ps);

        GameOverDialogue();
        InputField.text = "";
    }
    public void OnClickRestart()
    {
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        gamePlayScreen.SetActive(true);
        environmentHandler.DisableObstacles();
        environmentHandler.SetEnvironment(true);

        PlayGame();
        ButtonClickSound();

    }

    #endregion
}
