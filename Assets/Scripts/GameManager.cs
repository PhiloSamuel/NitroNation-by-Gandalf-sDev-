using UnityEngine;
using UnityEngine.Networking;
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
            gameInstance.client.Close();
            SceneManager.LoadScene("Credits");
           // Application.LoadLevel(Credits);

            for(int i=0;i<1000;i++);//dummy loop
            gameInstance.Recordee=false;

        }
        else
        {   gameInstance.client.Close();
            gameInstance.Recordee=false;
            Debug.Log("GAME OVER YOU lost");
           // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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

