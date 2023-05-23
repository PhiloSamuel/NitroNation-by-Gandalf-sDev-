using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class Server_philo : MonoBehaviour
{
    private TcpListener listener;
    private TcpClient client1;
    private TcpClient client2;
    private NetworkStream stream1;
    private NetworkStream stream2;
    private byte[] buffer;
    private Vector3 car1Position;
    private Vector3 car2Position;

    // Start listening for incoming connections whenthe game starts
    void Start()
    {
        listener = new TcpListener(IPAddress.Any, 8888);
        listener.Start();
        client1 = listener.AcceptTcpClient();
        stream1 = client1.GetStream();
        client2 = listener.AcceptTcpClient();
        stream2 = client2.GetStream();
        buffer = new byte[1024];
    }

    // Read the car coordinates sent by the clients and update their positions
    void Update()
    {
        // Read the position of car 1
        int bytesRead1 = stream1.Read(buffer, 0, buffer.Length);
        string message1 = Encoding.ASCII.GetString(buffer, 0, bytesRead1);
        string[] parts1 = message1.Split(',');
        float x1 = float.Parse(parts1[0]);
        float y1 = float.Parse(parts1[1]);
        float z1 = float.Parse(parts1[2]);
        car1Position = new Vector3(x1, y1, z1);

        // Read the position of car 2
        int bytesRead2 = stream2.Read(buffer, 0, buffer.Length);
        string message2 = Encoding.ASCII.GetString(buffer, 0, bytesRead2);
        string[] parts2 = message2.Split(',');
        float x2 = float.Parse(parts2[0]);
        float y2 = float.Parse(parts2[1]);
        float z2 = float.Parse(parts2[2]);
        car2Position = new Vector3(x2,y2, z2);

        // Update the positions of both cars
        GameObject car_2 = GameObject.Find("Car_2"); // Replace "Car1" with the name of your first car object
        car_2.transform.position = car1Position;
        GameObject car_3 = GameObject.Find("Car_3"); // Replace "Car2" with the name of your second car object
        car_3.transform.position = car2Position;
    }
}