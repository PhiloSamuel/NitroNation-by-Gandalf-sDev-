using UnityEngine;

public class EndTrigger : MonoBehaviour {

	public GameManager gameManager;

	void OnTriggerEnter ()
	{
		gameManager.CompleteLevel();
		FindObjectOfType<Client_philo>().Close();
	}

}
