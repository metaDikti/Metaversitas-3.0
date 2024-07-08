using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject _materi;
    public Button closeButton;
    public static Interactable ActiveInteractable { get; private set; }

    public virtual void Start()
    {
        closeButton.onClick.AddListener(Close);
        _materi.SetActive(false);
    }
    public virtual void Interact()
    {
        LocalController.Instance.Toggle();
        _materi.SetActive(true);
        ActiveInteractable = this;
        Debug.Log("Interaksi dilakukan dengan " + gameObject.name);
    }

    public virtual void Close()
    {
        _materi.SetActive(false);
        if (ActiveInteractable == this)
        {
            ActiveInteractable = null;
            LocalController.Instance.Toggle();
        }
    }
}
