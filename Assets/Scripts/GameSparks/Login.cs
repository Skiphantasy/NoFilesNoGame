using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Text usernameInput, passwordInput, displayNameInput;
    public Text usernameInfo, passwordInfo, displayNameInfo, info;

    public void AuthenticatePlayerButton()
    {
        usernameInfo.text = Helper.ValidateUsernameLogin(usernameInput);
        passwordInfo.text = Helper.ValidatePasswordLogin(passwordInput);

        if (!string.IsNullOrEmpty(displayNameInput.text)) {
            UpdateDisplayName();
        }


        if (!string.IsNullOrEmpty(usernameInfo.text) || !string.IsNullOrEmpty(passwordInfo.text))
            return;

        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetPassword(passwordInput.text)
            .SetUserName(usernameInput.text)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Authenticated");
                    //RetrievePlayerData.GetUserDisplayName();
                    StartGame.LoadGame();
                }
                else
                {
                    Debug.Log("Error Authenticating Player");
                    Helper.DisplayMessage(info, Color.red, "Login failed.\n Invalid username or password.");
                }
            });
    }

    void UpdateDisplayName()
    {
        new GameSparks.Api.Requests.ChangeUserDetailsRequest()
            .SetDisplayName(displayNameInput.text)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Display name was successfully updated");
                }
                else
                {
                    Debug.Log("Error to set new display name");
                }
            });
    } 
}

