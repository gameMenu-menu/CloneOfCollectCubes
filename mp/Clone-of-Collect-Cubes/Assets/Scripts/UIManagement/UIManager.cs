using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject PlayScreen, LooseScreen, WinScreen;

    public TMP_Text TimerText, CollectionText, EnemyCollectionText, WinCollectionText;

    public GameObject TimerParent, CollectionParent, EnemyCollectionParent, WinCollectionParent, HardnessPanel, SettingsPanel, StartText;

    IEnumerator WLastR;

    public float writeTimer, punchScaler, punchTime;

    public bool WithEnemy;

    public GameObject[] ElementsWillAppearAfterChoosingHardness;

    void OnEnable()
    {
        SceneManager.Instance.controller.OnLevelFail += OnLevelFail;
        SceneManager.Instance.controller.OnLevelVictory += OnLevelVictory;
        SceneManager.Instance.controller.OnClickContinue += OnClickContinue;
        SceneManager.Instance.controller.OnStartTimeStage += OnStartTimeStage;
        SceneManager.Instance.controller.OnWriteCollection += OnWriteCollection;
        SceneManager.Instance.controller.OnWriteEnemyCollection += OnWriteEnemyCollection;
        SceneManager.Instance.controller.OnWriteTimer += OnWriteTimer;
        SceneManager.Instance.controller.OnPressPause += OnPressPause;
        SceneManager.Instance.controller.OnStartGame += OnStartGame;
    }

    void OnDisable()
    {
        SceneManager.Instance.controller.OnLevelFail -= OnLevelFail;
        SceneManager.Instance.controller.OnLevelVictory -= OnLevelVictory;
        SceneManager.Instance.controller.OnClickContinue -= OnClickContinue;
        SceneManager.Instance.controller.OnStartTimeStage -= OnStartTimeStage;
        SceneManager.Instance.controller.OnWriteCollection -= OnWriteCollection;
        SceneManager.Instance.controller.OnWriteEnemyCollection -= OnWriteEnemyCollection;
        SceneManager.Instance.controller.OnWriteTimer -= OnWriteTimer;
        SceneManager.Instance.controller.OnPressPause -= OnPressPause;
        SceneManager.Instance.controller.OnStartGame -= OnStartGame;
    }

    void OnClickContinue()
    {
        LooseScreen.SetActive(false);
        WinScreen.SetActive(false);
        PlayScreen.SetActive(true);
    }

    void OnLevelFail()
    {
        LooseScreen.SetActive(true);
        PlayScreen.SetActive(false);
    }

    void OnLevelVictory()
    {
        WinScreen.SetActive(true);
        PlayScreen.SetActive(false);
        
        if(WLastR != null)
        {
            StopCoroutine(WLastR);

        }

        WLastR = WriteLastRoutine();

        StartCoroutine(WLastR);
    }

    IEnumerator WriteLastRoutine()
    {
        int count = SceneManager.Instance.stageController.GetTotalCollectionCount();
        int num = 0;
        WinCollectionParent.SetActive(true);

        WaitForSeconds wait = new WaitForSeconds(writeTimer);
        while(true)
        {
            WinCollectionText.text = num.ToString();

            if(num >= count)
            {
                WinCollectionParent.transform.DOPunchScale(Vector3.one * punchScaler, punchTime, 1);
                WLastR = null;
                yield break;
            }

            num++;
            yield return wait;
        }
    }

    public void ClickContinue()
    {
        TimerParent.SetActive(false);
        CollectionParent.SetActive(false);
        EnemyCollectionParent.SetActive(false);

        if(WLastR != null)
        {
            StopCoroutine(WLastR);

        }

        WinCollectionParent.SetActive(false);

        SceneManager.Instance.controller.ClickContinue();
    }

    void OnStartTimeStage(int timeCount)
    {
        //TimerParent.SetActive(true);
        //CollectionParent.SetActive(true);
        //if(WithEnemy) EnemyCollectionParent.SetActive(true);

        TimerText.text = "TIME " + timeCount.ToString();
        CollectionText.text = 0.ToString();
        EnemyCollectionText.text = 0.ToString();
    }

    void OnWriteCollection(int count)
    {
        CollectionText.text = count.ToString();
    }

    void OnWriteEnemyCollection(int count)
    {
        EnemyCollectionText.text = count.ToString();
    }

    void OnWriteTimer(int remaining)
    {
        TimerText.text = "TIME " + remaining.ToString();
    }

    public void HardButton()
    {
        SceneManager.Instance.controller.ChooseHardness(Hardness.Hard);

        StartText.SetActive(true);

        //ActivatePanels();

        HardnessPanel.SetActive(false);
    }

    void ActivatePanels()
    {
        for(int i=0; i<ElementsWillAppearAfterChoosingHardness.Length; i++)
        {
            ElementsWillAppearAfterChoosingHardness[i].SetActive(true);
        }
    }

    public void EasyButton()
    {
        SceneManager.Instance.controller.ChooseHardness(Hardness.Easy);

        StartText.SetActive(true);

        //ActivatePanels();

        HardnessPanel.SetActive(false);
    }

    void OnPressPause()
    {
        SettingsPanel.SetActive(true);
    }

    public void PauseButton()
    {
        SceneManager.Instance.controller.PressPause();
    }

    void OnStartGame()
    {
        SettingsPanel.SetActive(false);
        StartText.SetActive(false);

        ActivatePanels();
    }

    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
