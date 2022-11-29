using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneState {None, Prepared, Started, Ended}
public class SceneController : MonoBehaviour
{
    public delegate void OnPrepareSceneDelegate();
    public event OnPrepareSceneDelegate OnPrepareScene;
    
    public delegate void OnStartSceneDelegate();
    public event OnStartSceneDelegate OnStartScene;

    public delegate void OnLevelVictoryDelegate();
    public event OnLevelVictoryDelegate OnLevelVictory;
    
    public delegate void OnLevelFailDelegate();
    public event OnLevelFailDelegate OnLevelFail;

    public delegate void OnClickContinueDelegate();
    public event OnClickContinueDelegate OnClickContinue;

    public delegate void OnCollectCubeDelegate(Transform cube);
    public event OnCollectCubeDelegate OnCollectCube;

    public delegate void OnEnemyCollectCubeDelegate(Transform cube);
    public event OnEnemyCollectCubeDelegate OnEnemyCollectCube;

    public delegate void OnStartTimeStageDelegate(int timeCount);
    public event OnStartTimeStageDelegate OnStartTimeStage;

    public delegate void OnStartTimerDelegate();
    public event OnStartTimerDelegate OnStartTimer;

    public delegate void OnCountDelegate();
    public event OnCountDelegate OnCount;

    public delegate void OnWriteCollectionDelegate(int count);
    public event OnWriteCollectionDelegate OnWriteCollection;

    public delegate void OnWriteEnemyCollectionDelegate(int count);
    public event OnWriteEnemyCollectionDelegate OnWriteEnemyCollection;

    public delegate void OnWriteTimerDelegate(int remaining);
    public event OnWriteTimerDelegate OnWriteTimer;

    public delegate void OnStartGameDelegate();
    public event OnStartGameDelegate OnStartGame;

    public delegate void OnRefreshAgentsDelegate();
    public event OnRefreshAgentsDelegate OnRefreshAgents;

    public delegate void OnChooseHardnessDelegate(Hardness hs);
    public event OnChooseHardnessDelegate OnChooseHardness;

    public delegate void OnPressPauseDelegate();
    public event OnPressPauseDelegate OnPressPause;


    SceneState CurrentState;
    


    public void PrepareScene()
    {
        if(GetCurrentState() != SceneState.None)
        {
            Debug.LogError("STATE ERROR");
            return;
        }
        OnPrepareScene?.Invoke();
        ChangeState(SceneState.Prepared);
    }

    public void StartScene()
    {
        if(GetCurrentState() != SceneState.Prepared)
        {
            Debug.LogError("STATE ERROR");
            return;
        }
        OnStartScene?.Invoke();
        ChangeState(SceneState.Started);
    }

    public void LevelVictory()
    {
        OnLevelVictory?.Invoke();
        ChangeState(SceneState.Ended);
    }

    public void LevelFail()
    {
        OnLevelFail?.Invoke();
        ChangeState(SceneState.Ended);
    }

    public void ClickContinue()
    {
        
        if(GetCurrentState() != SceneState.Ended)
        {
            Debug.LogError("STATE ERROR");
            return;
        }

        OnClickContinue?.Invoke();

        ChangeState(SceneState.None);

        PrepareScene();

        StartScene();
        
    }

    public void ChangeState(SceneState state)
    {
        CurrentState = state;
    }

    public SceneState GetCurrentState()
    {
        return CurrentState;
    }

    public void CollectCube(Transform cube)
    {
        OnCollectCube?.Invoke(cube);
    }

    public void EnemyCollectCube(Transform cube)
    {
        OnEnemyCollectCube?.Invoke(cube);
    }

    public void StartTimer()
    {
        OnStartTimer?.Invoke();
    }

    public void Count()
    {
        OnCount?.Invoke();
    }

    public void WriteCollection(int count)
    {
        OnWriteCollection?.Invoke(count);
    }

    public void WriteEnemyCollection(int count)
    {
        OnWriteEnemyCollection?.Invoke(count);
    }

    public void WriteTimer(int remaining)
    {
        OnWriteTimer?.Invoke(remaining);
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void StartTimeStage(int timeCount)
    {
        OnStartTimeStage?.Invoke(timeCount);
    }

    public void RefreshAgents()
    {
        OnRefreshAgents?.Invoke();
    }

    public void ChooseHardness(Hardness hs)
    {
        OnChooseHardness?.Invoke(hs);
    }

    public void PressPause()
    {
        OnPressPause?.Invoke();
    }
    
}
