using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;


public class Client_philo : MonoBehaviour{
    private Vector3 car2Position;
    private Vector3 car3Position;
    private string Name;
    public Socket sock;
    private int framecounter;

    private int framestowait=100;

    private bool canplay=false;

    private string message;



     void recv()
        {

            while (true)
            {
                            Thread.Sleep(800);

                try
                { 
                    Byte[] buffer = new Byte[255];
                int rec = sock.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rec);
                string recievedmessage=Encoding.Default.GetString(buffer);
                string[] parts1 = recievedmessage.Split(',');
                if (rec <=0) {
                    Debug.Log(Name+"has disconnected"+"second fail");
                        throw new SocketException();
                        }


                if(recievedmessage=="Now you can play"){
                    canplay=true;
                }
                else if(recievedmessage=="Waiting for 1 Users to connect"){
                    Debug.Log(recievedmessage);

                    continue;
                }
                else if(parts1.Length>=3){
                
                Debug.Log(recievedmessage);
                float x1 = float.Parse(parts1[0]);
                float y1 = float.Parse(parts1[1]);
                float z1 = float.Parse(parts1[2]);
                car2Position = new Vector3(x1, y1, z1);

                }

                }
                catch {
                    Debug.Log("The server was closed you are disconnected");
                    sock.Close();
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
                  Thread.Sleep(800);
                    if(canplay){   
                    Debug.Log(message);
                    byte[] data = Encoding.ASCII.GetBytes(message); // Convert the string to bytes
                    sock.Send(data, 0, data.Length, 0);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }
        }




void Start()
    {
         int port= 5423;
         IPAddress ip;
        Name="Nagy";
       /*
            //   Console.WriteLine("Please Enter Your Name :");
            //     Name = Console.ReadLine();
            //     Console.WriteLine("Please Enter the Ip of the Server You want to connect To ");
            //     ip =  IPAddress.Parse(Console.ReadLine());
            //     Console.WriteLine("Please Enter the Port of the Server You want to connect To ");
            //     string inputport = Console.ReadLine();

                // try
                // {
                //     port= int.Parse(inputport);

                // }
                // catch (Exception ex)
                // {
                //     port = 5423;
                // }  

            */    
        sock = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //sock.Connect(new IPEndPoint(ip, port));
                sock.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.38"), 5423));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting");
            }
            byte[] name = Encoding.Default.GetBytes( Name );
             Console.WriteLine(Name);
            sock.Send(name, 0, name.Length, 0);

                Byte[] buffer = new Byte[255];
                int rec1 = sock.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rec1);
                string recievedmessage=Encoding.Default.GetString(buffer);
                string[] parts1 = recievedmessage.Split(',');
                float x1 = float.Parse(parts1[0]);
                float y1 = float.Parse(parts1[1]);
                float z1 = float.Parse(parts1[2]);
        car3Position = new Vector3(x1, y1,z1);
        GameObject car_3 = GameObject.Find("Car_3"); // Replace "Car1" with the name of your first car object
         car_3.transform.position = car3Position; 

         Thread  rec = new Thread(() => recv());
        rec.Start();
        Thread  sen = new Thread(() => send());
        sen.Start();

      
    }

    // Send the car coordinates to the server every frame
    void Update()
    {

        framecounter++;
        if(framecounter>=framestowait){

        car3Position = transform.position; // Get the car position
        message = car3Position.x + "," + car3Position.y + "," + car3Position.z; // Convert the position to a string
        byte[] data = Encoding.ASCII.GetBytes(message); // Convert the string to bytes
        framecounter=0;

         GameObject car_2 = GameObject.Find("Car_2"); // Replace "Car1" with the name of your first car object
         car_2.transform.position = car2Position;       
        }
    }


    public void Close(){
        sock.Close();
    }
}
        
        