using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ChangeScene(int num)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(num);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
