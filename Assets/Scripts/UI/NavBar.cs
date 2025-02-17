using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavBar : MonoBehaviour
{
    public Button[] sceneButtons; // Array to hold buttons

    void Start()
    {
        sceneButtons[0].onClick.AddListener(() => SceneManager.LoadScene("GeeklingScene"));

    }
}
