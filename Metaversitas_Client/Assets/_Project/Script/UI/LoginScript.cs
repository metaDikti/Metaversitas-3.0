using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using TMPro;
using SpacetimeDB;
using SpacetimeDB.Types;

public class LoginScript : MonoBehaviour
{
    public InputField userInputField;
    public InputField passwordInputField;
    public Text loginStatusText;

    private string backendURL = "http://your-backend-url.com/login";

    public void OnLoginButtonClick()
    {
        StartCoroutine(LoginUser(userInputField.text, passwordInputField.text));
    }

    private IEnumerator LoginUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(backendURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                loginStatusText.text = "Network error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                // Parse the response JSON or handle it based on your backend's response format
                if (responseText.Contains("success"))
                {
                    loginStatusText.text = "Login successful!";
                    // Here you can fetch additional data from the backend if needed
                }
                else
                {
                    loginStatusText.text = "Login failed. Invalid username/password.";
                }
            }
        }
    }
}