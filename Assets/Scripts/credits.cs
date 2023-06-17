 using UnityEngine;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("THE GAME HAS QUIT"); 
        SceneManager.LoadScene("Lobby");
    }
}
