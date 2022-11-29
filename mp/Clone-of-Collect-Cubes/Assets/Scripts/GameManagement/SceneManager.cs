using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public SceneController controller;
    
    public PoolingManager Pool;
    
    public UIManager uIManager;

    public StageController stageController;

    public Transform Player;

    public CheckPointerUI UIChecker;

    bool Started;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        controller.OnPrepareScene += OnPrepareScene;
        controller.OnStartGame += OnStartGame;
        controller.OnLevelFail += OnLevelFail;
        controller.OnLevelVictory += OnLevelVictory;
        controller.OnPressPause += OnPressPause;
    }

    void OnDisable()
    {
        controller.OnPrepareScene -= OnPrepareScene;
        controller.OnStartGame -= OnStartGame;
        controller.OnLevelFail -= OnLevelFail;
        controller.OnLevelVictory -= OnLevelVictory;
        controller.OnPressPause -= OnPressPause;
    }

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        controller.PrepareScene();
        controller.StartScene();
    }

    void OnPrepareScene()
    {
        StopAllCoroutines();
        Started = false;
    }

    void OnStartGame()
    {
        Started = true;
        Time.timeScale = 1;
        SceneManager.Instance.controller.StartTimer();
    }

    void OnLevelFail()
    {
        Started = false;

    }

    void OnLevelVictory()
    {
        Started = false;

    }

    public bool IsGameStarted()
    {
        return Started;
    }

    void OnPressPause()
    {
        Started = false;
        Time.timeScale = 0;
    }

    
}
