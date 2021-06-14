using UnityEngine;

public class RetrievePlayerData : MonoBehaviour
{
    public static string PlayerName;

    public static void GetUserDisplayName()
    {
        new GameSparks.Api.Requests.AccountDetailsRequest()
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    Debug.Log("Error Retrieving playername");
                }
                else
                {
                    Debug.Log(response.DisplayName);
                    PlayerName = response.DisplayName;
                }
            });
    }
}
