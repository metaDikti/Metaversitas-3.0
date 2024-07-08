using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMateri : MonoBehaviour
{
    public OfflineLobbyButton[] offlineLobbyButtons;
    [SerializeField] private Button _borobudurButton;
    [SerializeField] private Button _pengenalanKomputerButton;
    // Start is called before the first frame update

    private void Start()
    {
        _borobudurButton.onClick.AddListener(() =>
         {
             ChangeToBorobudur();
         });
        _pengenalanKomputerButton.onClick.AddListener(() =>
        {
            ChangeToPengenalanKomputer();
        });
    }
    void ChangeToBorobudur()
    {
        foreach (OfflineLobbyButton offlineLobbyButton in offlineLobbyButtons)
        {
            if (offlineLobbyButton != null)
            {
                // Ubah nilai _materi
                offlineLobbyButton._materi = Materi.Borobudur; // atau Materi.PengenalanKomputer
            }
        }
    }

    void ChangeToPengenalanKomputer()
    {
        foreach (OfflineLobbyButton offlineLobbyButton in offlineLobbyButtons)
        {
            if (offlineLobbyButton != null)
            {
                // Ubah nilai _materi
                offlineLobbyButton._materi = Materi.PengenalanKomputer; // atau Materi.PengenalanKomputer
            }
        }
    }
}
