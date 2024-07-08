using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MapIndex
{
    HomePage,
    Borobudur,
    Koin,
    Pengenalan_Komputer,
};

public class MapLoading : MonoBehaviour
{
    [SerializeField] private GameObject _loadScreen;

    [Header("Scenes")]
    [SerializeField] private SceneReference _homePage;
    [SerializeField] private SceneReference _koin;
    [SerializeField] private SceneReference _borobudur;
    [SerializeField] private SceneReference _pengenalanKomputer;
    [SerializeField] private SceneReference[] _maps;
    // Start is called before the first frame update
    private void Awake()
    {
        _loadScreen.SetActive(false);
    }

    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void LoadSceneByMapIndex(MapIndex mapIndex)
    {
        StartCoroutine(LoadSceneAsync(mapIndex));
    }

    private IEnumerator LoadSceneAsync(MapIndex mapIndex)
    {
        _loadScreen.SetActive(true); // Activate the loading screen

        // Load the scene asynchronously based on the map index
        switch (mapIndex)
        {
            case MapIndex.HomePage:
                yield return SceneManager.LoadSceneAsync(_homePage.ScenePath);
                break;
            case MapIndex.Borobudur:
                yield return SceneManager.LoadSceneAsync(_borobudur.ScenePath);
                break;
            case MapIndex.Koin:
                yield return SceneManager.LoadSceneAsync(_koin.ScenePath);
                break;
            case MapIndex.Pengenalan_Komputer:
                yield return SceneManager.LoadSceneAsync(_pengenalanKomputer.ScenePath);
                break;
            default:
                Debug.LogWarning($"MapIndex {mapIndex} not recognized.");
                break;
        }

        _loadScreen.SetActive(false); // Deactivate the loading screen after loading
        gameManager.CallUpdateInGameStatus();
    }

    public MapIndex GetMapIndexByMateri(string materi)
    {
        // Customize this method to map materi values to MapIndex enum values
        switch (materi)
        {
            case "Borobudur":
                return MapIndex.Borobudur;
            case "Koin":
                return MapIndex.Koin;
            case "PengenalanKomputer":
                return MapIndex.Pengenalan_Komputer;
            default:
                return MapIndex.HomePage; // Default to HomePage if no match found
        }
    }
}
