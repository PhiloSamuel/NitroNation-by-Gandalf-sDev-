using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    bool gameHasEnded = false;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;
    
    // Declare singletonInstance here
    public Client gameInstance;

    public void CompleteLevel ()
    {
        completeLevelUI.SetActive(true);
    }
     
    public void EndGame ()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER YOU WON");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            gameInstance.Recordee=false;

        }
        else
        {   gameInstance.Recordee=false;
            Debug.Log("GAME OVER YOU lost");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void Awake()
    {
        gameInstance = GameObject.FindObjectOfType<Client>();
   }
}

