using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour {

	public GameManager gameManager;

	void OnTriggerEnter ()
	{
		gameManager.EndGame();

		FindObjectOfType<Client>().Close();
	}

}
