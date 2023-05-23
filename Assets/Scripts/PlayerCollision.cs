using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	public PlayerMovement movement;     // A reference to our PlayerMovement script
	public Player2Movement movement2;
	
	
	// This function runs when we hit another object.
	// We get information about the collision and call it "collisionInfo".
	void OnCollisionEnter (Collision collisionInfo)
	{
		// We check if the object we collided with has a tag called "Obstacle".
		if (collisionInfo.collider.tag == "Obstacle")
		{
			movement.enabled = false;
			movement2.enabled = false;
		                                // Disable the players movement.
			
			FindObjectOfType<GameManager>().EndGame();
		}
	}

}
