using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavBar : MonoBehaviour
{
    public Button[] sceneButtons; // Array to hold buttons

    void Start()
    {
        sceneButtons[0].onClick.AddListener(() => LoadScene(4));

    }

    void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
