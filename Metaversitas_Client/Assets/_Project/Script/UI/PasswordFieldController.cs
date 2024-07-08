using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PasswordFieldController : MonoBehaviour
{
    // Referensi ke TMP_InputField dan Button
    public TMP_InputField passwordField;
    public Button toggleVisibilityButton;
    public TextMeshProUGUI buttonText;

    private bool isPasswordVisible = false;

    private void Start()
    {
        // Set default mode ke Password
        passwordField.contentType = TMP_InputField.ContentType.Password;
        passwordField.ForceLabelUpdate();

        // Tambahkan listener untuk tombol
        toggleVisibilityButton.onClick.AddListener(TogglePasswordVisibility);
    }

    private void TogglePasswordVisibility()
    {
        // Ubah mode visibilitas
        isPasswordVisible = !isPasswordVisible;

        if (isPasswordVisible)
        {
            // Jika password terlihat, ubah ke mode Standard
            passwordField.contentType = TMP_InputField.ContentType.Standard;
            buttonText.text = "Hide";
        }
        else
        {
            // Jika password tersembunyi, ubah ke mode Password
            passwordField.contentType = TMP_InputField.ContentType.Password;
            buttonText.text = "Show";
        }

        // Paksa update pada TMP_InputField
        passwordField.ForceLabelUpdate();
    }
}
