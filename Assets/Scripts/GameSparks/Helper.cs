using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Helper : MonoBehaviour
{
    public static string ValidateUsernameSignUp(Text username)
    {
        if (string.IsNullOrEmpty(username.text))
        {
            return "Username field can't be empty";
        }

        if (!Regex.Match(username.text, "^(?=[a-zA-Z0-9]{8,20}$)").Success)
        {
            return "You can use letters and numbers only";
        }

        return string.Empty;
    }

    public static string ValidateUsernameLogin(Text username)
    {
        if (string.IsNullOrEmpty(username.text))
        {
            return "Username field can't be empty";
        }

        return string.Empty;
    }

    public static string ValidatePasswordLogin(Text password)
    {
        if (string.IsNullOrEmpty(password.text))
        {
            return "Password field can't be empty";
        }

        return string.Empty;
    }

    public static string ValidatePasswordSignUp(Text password)
    {
        if (string.IsNullOrEmpty(password.text))
        {
            return "Password field can't be empty";
        }

        if (!Regex.Match(password.text, "((?=.*[0-9])(?=.*[A-Z]).{8,50})").Success)
        {
            return "Minumum 8 characters, at least 1 uppercase letter and 1 number";
        }

        return string.Empty;
    }

    public static string ValidateDisplayNameSignUp(Text displayName)
    {
        if (string.IsNullOrEmpty(displayName.text))
        {
            return "Display Name field can't be empty";
        }

        if (!Regex.Match(displayName.text, "^(?=[a-zA-Z0-9]{1,20}$)").Success)
        {
            return "You can use letters and numbers only";
        }

        return string.Empty;
    }

    public static void DisplayMessage(Text infoText, Color color, string text)
    {
        infoText.color = color;
        infoText.text = text;
    }
}
