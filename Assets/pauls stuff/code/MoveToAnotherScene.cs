using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToAnotherScene : MonoBehaviour
{
    public void moveToSecondScene()
    {
        SceneManager.LoadScene(1);
    }
}
