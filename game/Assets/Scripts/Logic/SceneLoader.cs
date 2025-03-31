using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("LogicPart", LoadSceneMode.Additive);
        SceneManager.LoadScene("UIPart", LoadSceneMode.Additive);
    }
}
