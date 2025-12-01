using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField EnterUsername;
    public TMP_InputField EnterPassword;
    public GameObject errorMessage; // Make a small text object and disable it first

    [SerializeField]
    private string correctPassword = "1234";

    public void Login()
    {
        if (EnterPassword == null)
        {
            Debug.LogError("EnterPassword is not assigned in the Inspector on " + gameObject.name);
            return;
        }

        string entered = EnterPassword.text != null ? EnterPassword.text.Trim() : "";
        Debug.Log($"Login attempt with password='{entered}'");

        if (entered != correctPassword)
        {
            if (errorMessage != null)
                errorMessage.SetActive(true);
            else
                Debug.LogWarning("errorMessage GameObject not assigned; cannot show error UI.");

            Debug.Log("Login failed: incorrect password.");
            return;
        }

        string sceneName = "SampleScene";
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Cannot load scene '{sceneName}'. Make sure it's added to Build Settings and spelled exactly.");
            return;
        }

        Debug.Log("Login successful. Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}