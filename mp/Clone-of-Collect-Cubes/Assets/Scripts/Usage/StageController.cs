using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    const string StageNumberKey = "StageNumber";
    public Transform Ground;

    public int StageNumber;

    public GameVariables Variables;

    int CubeCount;

    int CollectionCount, EnemyCollectionCount;

    public bool TimedStage;

    int Timer;

    public int TimerTime;

    public int CubeCountOnTimedLevel;

    IEnumerator TimerRoutine;

    public List<Transform> Cubes;

    void OnEnable()
    {
        SceneManager.Instance.controller.OnPrepareScene += OnPrepareScene;
        SceneManager.Instance.controller.OnLevelVictory += OnLevelVictory;
        SceneManager.Instance.controller.OnCollectCube += OnCollectCube;
        SceneManager.Instance.controller.OnStartTimer += OnStartTimer;
        SceneManager.Instance.controller.OnEnemyCollectCube += OnEnemyCollectCube;
    }

    void OnDisable()
    {
        SceneManager.Instance.controller.OnPrepareScene -= OnPrepareScene;
        SceneManager.Instance.controller.OnLevelVictory -= OnLevelVictory;
        SceneManager.Instance.controller.OnCollectCube -= OnCollectCube;
        SceneManager.Instance.controller.OnStartTimer -= OnStartTimer;
        SceneManager.Instance.controller.OnEnemyCollectCube -= OnEnemyCollectCube;
    }

    void OnPrepareScene()
    {
        SceneManager.Instance.Pool.CleanScene();

        Cubes = new List<Transform>();

        CubeCount = 0;
        CollectionCount = 0;
        EnemyCollectionCount = 0;

        if(PlayerPrefs.HasKey(StageNumberKey))
        {
            StageNumber = PlayerPrefs.GetInt(StageNumberKey);
        }
        else
        {
            StageNumber = 0;
        }

        SpawnLevelBasedOnImage();
        if(TimedStage)
        {
            Timer = TimerTime;

            SceneManager.Instance.controller.StartTimeStage(Timer);
        }
    }

    void SpawnLevelBasedOnImage()
    {

        SceneManager.Instance.controller.RefreshAgents();

        int pixelSizeX = Variables.PixelSizeX;
        int pixelSizeY = Variables.PixelSizeY;

        int xLimit = Variables.XLimit;
        int yLimit = Variables.YLimit;


        Texture2D tex = GetTex();

        Vector3 center = new Vector3((float) ( tex.width / pixelSizeX ) / 2f, 0, (float) ( tex.height / pixelSizeY ) / 2f);

        for(int i=0; i<tex.width; i++)
        {
            for(int k=0; k<tex.height; k++)
            {
                if( i % pixelSizeX == 0 && k % pixelSizeY == 0 )
                {
                    if(i>xLimit && k>yLimit && i<tex.width-xLimit && k<tex.height-yLimit)
                    {
                        SpawnPixel(tex, i, k, pixelSizeX, pixelSizeY, center);
                    }
                }
            }
        }
    }

    Texture2D GetTex()
    {
        Texture2D[] temp = Variables.PixelMaps;

        int num = StageNumber;

        if(num >= temp.Length)
        {
            num = Random.Range(0, temp.Length);
        }

        return temp[num];
    }

    void OnLevelVictory()
    {
        StageNumber++;
        PlayerPrefs.SetInt(StageNumberKey, StageNumber);
    }

    void SpawnPixel(Texture2D tex, int width, int height, int pixelSizeX, int pixelSizeY, Vector3 center)
    {
        Color color = tex.GetPixel(width, height);

        if( (color.r + color.g + color.b) > 2.9f ) return;

        CubeCount++;

        GameObject cube = SceneManager.Instance.Pool.ObtainFromPool(ObjectType.CollectibleCube);

        Cubes.Add(cube.transform);

        cube.transform.position = Vector3.zero;

        cube.SetActive(true);

        

        Vector3 pos = new Vector3( (float) width / pixelSizeX , 0,  (float) height / pixelSizeY );

        pos -= center;

        pos *= 1.4f;

        cube.GetComponent<CollectibleController>().Initialize(pos, color);

    }

    void OnCollectCube(Transform cube)
    {
        Cubes.Remove(cube);
        CubeCount--;
        CollectionCount++;

        SceneManager.Instance.controller.WriteCollection(CollectionCount);

        if(CubeCount <= 0)
        {
            if(!TimedStage)
            {
                CheckResult();
            }
            else
            {
                SpawnLevelBasedOnImage();
            }
        }
    }

    void OnEnemyCollectCube(Transform cube)
    {
        Cubes.Remove(cube);
        CubeCount--;
        EnemyCollectionCount++;

        SceneManager.Instance.controller.WriteEnemyCollection(EnemyCollectionCount);

        if(CubeCount <= 0)
        {
            if(!TimedStage)
            {
                CheckResult();
            }
            else
            {
                SpawnLevelBasedOnImage();
            }
        }
    }

    void CheckResult()
    {
        if(EnemyCollectionCount >= CollectionCount)
        {
            SceneManager.Instance.controller.LevelFail();

            if(TimerRoutine != null)
            {
                StopCoroutine(TimerRoutine);
            }

        }
        else
        {
            SceneManager.Instance.controller.LevelVictory();

            if(TimerRoutine != null)
            {
                StopCoroutine(TimerRoutine);
            }

        }
    }

    void OnStartTimer()
    {
        if(TimerRoutine != null)
        {
            StopCoroutine(TimerRoutine);
        }

        TimerRoutine = TimerR();

        StartCoroutine(TimerRoutine);
    }

    IEnumerator TimerR()
    {
        WaitForSeconds sec = new WaitForSeconds(1f);
        while(true)
        {
            yield return sec;
            Timer--;
            SceneManager.Instance.controller.WriteTimer(Timer);

            if(Timer == 0)
            {
                yield return sec;
                CheckResult();
            }
        }
    }

    public Transform GetCube()
    {
        return Cubes[Random.Range(0, Cubes.Count)];
    }

    public Transform GetGround()
    {
        return Ground;
    }

    public int GetTotalCollectionCount()
    {
        return CollectionCount;
    }
}
