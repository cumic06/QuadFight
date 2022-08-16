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
                    StartCoroutine(ReadyDelay());
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
    public GameObject gameOption;
    public GameObject ClearPanel;

    [SerializeField] GameObject stage2Button;
    [SerializeField] GameObject stage3Button;

    public GameObject stageplayer;

    public Text ScoreTxt;

    public static GameManager gamemanager;


    public int Score;

    public bool isClear;

    public bool isMain;

    #endregion
    private void Awake()
    {
        gamemanager = GetComponent<GameManager>();

        isClear = false;

        isMain = false;
    }
    private void FixedUpdate()
    {
        ScoreUI();
        ClearUI();
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
        isMain = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region 스코어
    public void ScoreUI()
    {
        ScoreTxt.text = string.Format("Score : " + "{0:n0}", Score);
    }
    #endregion


    #region 스테이지 버튼
    public void Stage1Button()
    {
        StartCoroutine(WaitchangeStage());
        stageplayer.transform.Translate(new Vector2(425, 0) * Time.deltaTime);
        SceneManager.LoadScene(1);
        StageManager.stageManager.wstage = WStage.stage1;
    }

    public void Stage2Button()
    {
        StartCoroutine(WaitchangeStage());
        SceneManager.LoadScene(2);
        StageManager.stageManager.wstage = WStage.stage2;
    }

    public void Stage3Button()
    {
        StartCoroutine(WaitchangeStage());
        SceneManager.LoadScene(3);
        StageManager.stageManager.wstage = WStage.stage3;
    }

    IEnumerator WaitchangeStage()
    {
        yield return new WaitForSeconds(1f);
    }
    #endregion

    #endregion

    #region 클리어 확인창
    public void ClearUI()
    {
        if (isClear)
        {
            ClearPanel.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    #endregion

    IEnumerator ReadyDelay()
    {
        yield return new WaitForSeconds(3f);
        gameState = GameState.Run;
    }
}
