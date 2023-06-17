using System.Net.Sockets;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine.SceneManagement;


public class Client : MonoBehaviour
{
    private Vector3 car2Position;
    private Vector3 car3Position;
    private string Name;
    private int framecounter;
    public TcpClient client;
    private NetworkStream stream;

    private int framestowait = 10;

    private bool canplay = false;
    public bool Recordee = true;
    private string message;
    private int matchNum;
    private read_user singletonInstance;


    private void OnApplicationQuit()
    {
        try
        {
            singletonInstance.setLogout();
            client.Close();
            Recordee = false;
        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }
    }

    void recv()
    {

        while (true)
        {
            Thread.Sleep(150);
            try
            {
                Byte[] buffer = new Byte[255];
                int rec = stream.Read(buffer);
                Array.Resize(ref buffer, rec);
                string recievedmessage = Encoding.Default.GetString(buffer);
                string[] parts1 = recievedmessage.Split(',');
                if (rec <= 0)
                {
                    Debug.Log(Name + "has disconnected");
                    throw new SocketException();
                }

                canplay = true;

                if (recievedmessage == "Now you can play")
                {
                    canplay = true;
                }
                else if (recievedmessage == "Waiting for 1 Users to connect")
                {
                    Debug.Log(recievedmessage);

                    continue;
                }
                else if (recievedmessage == "STOP")
                {
                    canplay = false;
                }
                else if (parts1.Length >= 3)
                {

                    Debug.Log(recievedmessage);
                    float x1 = float.Parse(parts1[0]);
                    float y1 = float.Parse(parts1[1]);
                    float z1 = float.Parse(parts1[2]);
                    car2Position = new Vector3(x1, y1, z1);

                }

            }
            catch
            {
                Debug.Log("The server was closed you are disconnected");
                client.Close();
                break;
            }
        }
    }

    void send()
    {

        try
        {
            while (true)
            {
                Thread.Sleep(150);
                if (canplay)
                {
                    Debug.Log(message);
                    byte[] data = Encoding.ASCII.GetBytes(message); // Convert the string to bytes
                    stream.Write(data);
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    void Recordings()
    {
        // create a MongoDB client and database
        MongoClient client = new MongoClient("mongodb+srv://19p3041:admin123@cluster0.lzbu4ip.mongodb.net/");
        IMongoDatabase database = client.GetDatabase("GameDB");

        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("replay");

        try
        {
            while (Recordee)
            {
                Thread.Sleep(1000);
                if (canplay)
                {
                    var document = new BsonDocument
    {
        { "x", new BsonDouble(car3Position.x) },
        { "y", new BsonDouble(car3Position.y) },
        { "z",new BsonDouble(car3Position.z) },
          { "Game#",BsonInt32.Create(matchNum)},
        { "UserName", Name }

    };
                    Debug.Log("THIS IS THE DOC" + document);
                    collection.InsertOne(document);


                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }


    void Awake()
    {
        singletonInstance = GameObject.FindObjectOfType<read_user>();

        IPAddress ipAddress;

        ipAddress = Dns.GetHostAddresses("ec2-13-51-92-187.eu-north-1.compute.amazonaws.com")[0];
        client = new TcpClient();
        client.Connect("192.168.1.115", 5423);
        Console.WriteLine("Connected to server");
        stream = client.GetStream();
        Name = singletonInstance.getName();

        byte[] name = Encoding.Default.GetBytes(Name);
        Console.WriteLine(Name);
        stream.Write(name);



        Byte[] buffer2 = new Byte[255];
        int rec2 = stream.Read(buffer2);
        Array.Resize(ref buffer2, rec2);
        matchNum = BitConverter.ToInt32(buffer2, 0);
        Debug.Log(matchNum + " <-------This is the server Num");

        Byte[] buffer = new Byte[255];
        int rec1 = stream.Read(buffer);
        Array.Resize(ref buffer, rec1);
        string recievedmessage = Encoding.Default.GetString(buffer);
        Debug.Log(recievedmessage + " <-------This is the ur start loc");

        string[] parts1 = recievedmessage.Split(',');
        float x1 = float.Parse(parts1[0]);
        float y1 = float.Parse(parts1[1]);
        float z1 = float.Parse(parts1[2]);
        car3Position = new Vector3(x1, y1, z1);
        GameObject car_3 = GameObject.Find("Car_3"); // Replace "Car1" with the name of your first car object
        car_3.transform.position = car3Position;

        Thread rec = new Thread(() => recv());
        rec.Start();
        Thread sen = new Thread(() => send());
        sen.Start();
        Thread Recording = new Thread(() => Recordings());
        Recording.Start();

    }

    // Send the car coordinates to the server every frame
    void Update()
    {

        framecounter++;
        if (framecounter >= framestowait && canplay)
        {

            car3Position = transform.position; // Get the car position
            message = car3Position.x + "," + car3Position.y + "," + car3Position.z + ","; // Convert the position to a string
            byte[] data = Encoding.ASCII.GetBytes(message); // Convert the string to bytes
            framecounter = 0;

            GameObject car_2 = GameObject.Find("Car_2"); // Replace "Car1" with the name of your first car object
            car_2.transform.position = car2Position;
        }
    }


    public void Close()
    {
        client.Close();
    }
}