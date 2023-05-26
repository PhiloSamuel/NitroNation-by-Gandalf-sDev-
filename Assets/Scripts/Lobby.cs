using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

public class Lobby : MonoBehaviour
{
    private string messagePriv;
    private string messagePub;
    private string X1;
    private string X2;
    private string rcvmessage;
    private string Name;
    public InputField sendPrivText;
    public InputField sendPublicText;
    public Text Public;
    public Text Private;
    public Button sendPriv;
    public Button sendPublic;
    public Button Play;

    private read_user singletonInstance;
    void Start() {
        singletonInstance = GameObject.FindObjectOfType<read_user>();
        Name=singletonInstance.getName();
        X1=Name;
        X2=X1;
        Thread rcvThread = new Thread(() => rcv());
        rcvThread.Start();

    }
    
    void Update() {
        Public.text = X1;
        Private.text =X2; }

    public void rcv() {
        while (true)
        {
            rcvmessage = singletonInstance.ReceiveMessages();
            string[] parts = rcvmessage.Split(':');
            if(parts.Length>1){
            if (parts[1] == "bc")
            {
               X1 += "\n"+parts[0]+": " + parts[2];
                    Debug.Log(X1);
            }
            else if(parts[1] == Name)
            {
                    X2 += "\n" + parts[0] + ": " + parts[2];
            }
            }
         
            else { X1 += "\n"+rcvmessage;
                Debug.Log(X1);
            }
        }
    }


   
    public void StartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SENDPUB()
    {
        messagePub= sendPublicText.text;
        singletonInstance.SendMessages("bc:" + messagePub);
        X1 += "\n" + Name + ": " + messagePub;

    }

    public void SENDPRIV()
    {
        messagePriv = sendPrivText.text;
        singletonInstance.SendMessages(messagePriv);
        X2 += "\n from: " + messagePub;


    }

    private void OnApplicationQuit()
    {
        try
        {
            singletonInstance.setLogout();
        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }
    }





}
