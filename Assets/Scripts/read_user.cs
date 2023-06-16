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
    private Socket socket;
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
void Update(){
   // Debug.Log(userName);
}


    void Start()
    {
        // create a TCP/IP socket
     socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //connect to the server
       socket.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.43"), 1234));        
        }
    


//sign IN 
public string signIn(string name,string pass){
             //client log in
            // send login credentials to the server
                byte[] buffer = new byte[1024];
                buffer = Encoding.ASCII.GetBytes("login:" + name+":"+pass);
                socket.Send(buffer);

            // wait for a response from the server
            buffer = new byte[1024];
            int bytesRead = socket.Receive(buffer);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // check the response from the server
            if (response == "login_ok")
            { userName=name;
                auth=true;
            }
          
     return(response);        
            }


//sign up 
 public string signUp(string name,string pass){    
                byte[] buffer = new byte[1024];       
                buffer = Encoding.ASCII.GetBytes("signup:" + name+":"+pass);
                socket.Send(buffer);
            // wait for a response from the server
            buffer = new byte[1024];
            int bytesRead = socket.Receive(buffer);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // check the response from the server
            if (response == "signup_ok")
            {   userName=name;
                auth = true;
            }
           
            return response;
            
            
            }

 
 
 //send message->recipient:message
 public void SendMessages(string message)
    {
    byte[] buffer = Encoding.ASCII.GetBytes(userName+":"+message);
    socket.Send(buffer);
    }

//recieve message
public string ReceiveMessages()
    {
    byte[] buffer = new byte[1024];
    int bytesRead = socket.Receive(buffer);
    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
    return message;
    }

//getters and setters
public string getName(){
return userName;
}
public Socket getSocket(){
return socket;
}
public bool getAuth(){
return auth;
}
public void setLogout(){
    logout=true;  
    socket.Close(); 
}
}





