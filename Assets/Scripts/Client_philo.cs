using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client_philo : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer;

    //my edits ->
    //public Transform player;
    //public Vector3 offset;

    // Connect to the server when the game starts
    void Start()
    {
        client = new TcpClient("127.0.0.1", 8889);
        stream = client.GetStream();
        buffer = new byte[1024];
    }

    // Send the car coordinates to the server every frame
    void Update()
    {
        Vector3 carPosition = transform.position; // Get the car position
        string message = carPosition.x + "," + carPosition.y + "," + carPosition.z; // Convert the position to a string
        byte[] data = Encoding.ASCII.GetBytes(message); // Convert the string to bytes
        stream.Write(data, 0, data.Length); // Send the bytes to the server
    }
}