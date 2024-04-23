using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangesController : MonoBehaviour
{
    public void LoadFirst()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadSecond()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadThird()
    {
        SceneManager.LoadScene("Level3");
    }
}
