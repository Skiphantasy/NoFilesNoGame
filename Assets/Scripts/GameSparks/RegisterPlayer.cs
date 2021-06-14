using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class RegisterPlayer : MonoBehaviour
{
    public Text displayNameInput, usernameInput, passwordInput;
    public Text usernameInfo, passwordInfo, displayNameInfo, info;

    public void RegisterPlayerButton()
    {
        usernameInfo.text = Helper.ValidateUsernameSignUp(usernameInput);
        passwordInfo.text = Helper.ValidatePasswordSignUp(passwordInput);
        displayNameInfo.text = Helper.ValidateDisplayNameSignUp(displayNameInfo);

        if (!string.IsNullOrEmpty(usernameInfo.text) || !string.IsNullOrEmpty(passwordInfo.text) 
            || !string.IsNullOrEmpty(displayNameInfo.text))
            return;

        new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(displayNameInput.text)
            .SetPassword(passwordInput.text)
            .SetUserName(usernameInput.text)
            .Send((response) => {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Registered");
                    Helper.DisplayMessage(info, Color.green, "Player has been registered successfully.\nPlease try to login.");           
                }
                else
                {
                    Debug.Log("Error Registering Player");
                    Helper.DisplayMessage(info, Color.green, "Error registering player.\nPlease try again.");
                }
            });
    }
}
