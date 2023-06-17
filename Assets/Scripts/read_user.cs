using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
public class read_user : MonoBehaviour
{
    public static read_user Instance { get; private set; } // Singleton instance

    private string userName;
    private bool auth = false;
    private bool logout = false;
    private TcpClient client;
    private NetworkStream stream;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances of the singleton
        }
        else
        {
            Instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Don't destroy the singleton when loading a new scene
        }
    }
    //for test
    void Update()
    {
        // Debug.Log(userName);
    }


    void Start()
    {
        IPAddress ipAddress;

        ipAddress = Dns.GetHostAddresses("ec2-13-51-92-187.eu-north-1.compute.amazonaws.com")[0];
        client = new TcpClient();
        client.Connect(ipAddress, 55000);
        Console.WriteLine("Connected to server");
        stream = client.GetStream();

    }
    /*try
    {


    }
    catch { 
        Debug.Log("CAN'T");
        ipAddress = IPAddress.Parse("13.51.92.187");

    }
    Debug.Log(ipAddress.ToString());
    // create a TCP/IP socket
    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //connect to the server
   socket.Connect(new IPEndPoint(ipAddress, 55000));
    Debug.Log("Connected");
    }*/



    //sign IN 
    public string signIn(string name, string pass)
    {
        //client log in
        // send login credentials to the server
        byte[] buffer = new byte[1024];
        buffer = Encoding.ASCII.GetBytes("login:" + name + ":" + pass);
        stream.Write(buffer);

        // wait for a response from the server
        buffer = new byte[1024];
        int bytesRead = stream.Read(buffer);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        // check the response from the server
        if (response == "login_ok")
        {
            userName = name;
            auth = true;
        }

        return (response);
    }


    //sign up 
    public string signUp(string name, string pass)
    {
        byte[] buffer = new byte[1024];
        buffer = Encoding.ASCII.GetBytes("signup:" + name + ":" + pass);
        stream.Write(buffer);
        // wait for a response from the server
        buffer = new byte[1024];
        int bytesRead = stream.Read(buffer);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        // check the response from the server
        if (response == "signup_ok")
        {
            userName = name;
            auth = true;
        }

        return response;


    }



    //send message->recipient:message
    public void SendMessages(string message)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(userName + ":" + message);
        stream.Write(buffer);
    }

    //recieve message
    public string ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer);
        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        return message;
    }

    //getters and setters
    public string getName()
    {
        return userName;
    }

    public bool getAuth()
    {
        return auth;
    }
    public void setLogout()
    {
        logout = true;

    }

}