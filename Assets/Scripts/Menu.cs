using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    private string UserName;
    private string Password;


    public InputField UserNameField;
    public InputField PasswordField;

    public Button SignInButton;
    public Button SignUpButton;
    private read_user singletonInstance;
    void Start() {
        singletonInstance = GameObject.FindObjectOfType<read_user>();

    }
    
    void update() { }

  
    public void StartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Signup()
    {
        UserName = UserNameField.text;
        Password = PasswordField.text;
         if(singletonInstance.signUp(UserName, Password)=="signup_ok"){
         StartGame();   
        }
        else {Debug.Log("Invalid Sign up"); }
    }

    public void SignIn()
    {
        UserName=UserNameField.text;
        Password=PasswordField.text;
        if(singletonInstance.signIn(UserName, Password)=="login_ok"){
         StartGame();   

        }
        else {Debug.Log("Invalid Sign in");}

    }
}
