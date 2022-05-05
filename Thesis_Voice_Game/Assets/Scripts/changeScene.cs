using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("meganVer"))
        {
            int r = Random.Range(0, 3);
            PlayerPrefs.SetInt("meganVer", r);
            Debug.Log(PlayerPrefs.GetInt("meganVer"));
        }
        //PlayerPrefs.SetInt("meganVer", 2);
    }

    public void playGame()
    {
        // load next scene in queue
        if (SceneManager.GetActiveScene().buildIndex == 2 && !PlayerPrefs.HasKey("name"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("meganVer") + 3);
        } else if (SceneManager.GetActiveScene().buildIndex == 1 && PlayerPrefs.HasKey("name"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("meganVer") + 3);
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void finish()
    {
        SceneManager.LoadScene("Finish");
    }

    public void goBack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void exitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
    