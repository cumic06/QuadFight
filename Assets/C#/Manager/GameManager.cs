using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,
    Pause,
    Run,
    GameOver
}
public class GameManager : MonoBehaviour
{
    #region 변수
    private GameState gameState;
    public GameState Gamestate
    {
        get => gameState;
        set
        {
            switch (value)
            {
                case GameState.Ready:
                    break;
                case GameState.Pause:
                    break;
                case GameState.Run:
                    break;
                case GameState.GameOver:
                    break;
            }
        }
    }

    public static int clearStage = 0;

    public int Score;


    public GameObject gameOption;
    public Image ClearPanel;


    public GameObject stageplayer;
    public GameObject playerdead;

    public Text ScoreTxt;

    public bool isClear = false;

    public bool isGameOver = false;


    [SerializeField] GameObject[] stageButtons;
    [SerializeField] Image[] stageImage;
    [SerializeField] Text isClearTxt;
    [SerializeField] Image ScoreImage;

    [SerializeField] Text hpText;
    #endregion

    #region Component
    public static GameManager Instance;
    #endregion

    private void Awake()
    {
        Instance = GetComponent<GameManager>();

        if (0 == SceneManager.GetActiveScene().buildIndex)
        {
            for (int i = 0; i <= clearStage; i++)
            {
                Debug.Log(i);
                stageButtons[i].SetActive(true);
            }
            for (int i = 0; i <= clearStage - 1; i++)
            {
                stageImage[i].color = Color.white;
            }
        }
    }
    private void FixedUpdate()
    {
        ScoreUI();
        ClearUI();
        GameOverUI();
    }
    #region UI

    #region 옵션창
    public void OpenOption()
    {
        gameOption.SetActive(true);

        Time.timeScale = 0f;

        gameState = GameState.Pause;
    }
    public void CloseOption()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1f;

        gameState = GameState.Run;
    }
    public void ReStartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClearUI()
    {
        if (isClear == true)
        {
            ClearPanel.gameObject.SetActive(true);
            ClearPanel.color = new Color(0, 0, 0, 0.4f);
            isClearTxt.text = "Clear!";
            Time.timeScale = 0f;

            clearStage = clearStage < SceneManager.GetActiveScene().buildIndex ? clearStage = SceneManager.GetActiveScene().buildIndex : clearStage;
        }
    }

    public void GameOverUI()
    {
        if (isGameOver == true)
        {
            ClearPanel.gameObject.SetActive(true);
            playerdead.gameObject.SetActive(true);
            isClearTxt.text = "Game Over";
            isClearTxt.color = Color.red;
            Time.timeScale = 0f;
        }
    }
    #endregion

    #region 스코어
    public void ScoreUI()
    {
        if (isClear == false && isGameOver == false)
        {
            ScoreImage.gameObject.SetActive(true);
            ScoreTxt.text = string.Format("Score : " + "{0:n0}", Score);
        }

        else
        {
            ScoreImage.gameObject.SetActive(false);
        }
    }
    #endregion

    public void SetHpText(string value)
    {
        hpText.text = $"Hp : {value}";
    }

    #region 스테이지 버튼
    public void Stage1Button()
    {
        StartCoroutine(WaitchangeStage());
        stageplayer.transform.Translate(new Vector2(425, 0) * Time.deltaTime);
        SceneManager.LoadScene(1);
        StageManager.Instance.wstage = WStage.stage1;
    }

    public void Stage2Button()
    {
        StartCoroutine(WaitchangeStage());
        SceneManager.LoadScene(2);
        StageManager.Instance.wstage = WStage.stage2;
    }

    public void Stage3Button()
    {
        StartCoroutine(WaitchangeStage());
        SceneManager.LoadScene(3);
        StageManager.Instance.wstage = WStage.stage3;
    }

    IEnumerator WaitchangeStage()
    {
        yield return new WaitForSeconds(1f);
    }
    #endregion

    #endregion
}
