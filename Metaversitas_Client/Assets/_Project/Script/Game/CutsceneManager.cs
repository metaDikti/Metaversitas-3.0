using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button skipButton;
    public GameObject controllerPrefab;
    public static CutsceneManager Cutscene { get; private set; }

    private string materi;
    private string pertemuan;
    private Dictionary<string, string[]> videoPaths;

    // Start is called before the first frame update
    void Start()
    {
        Cutscene = this;
        // Initialize video paths
        videoPaths = new Dictionary<string, string[]>
        {
            {
                "Borobudur", new string[]
                {
                    "Assets/_Project/Videos/Borobudur_P1.mp4",
                    "Assets/_Project/Videos/Borobudur_P2.mp4",
                    // Add up to 16 meetings
                }
            },
            {
                "PengenalanKomputer", new string[]
                {
                    "Assets/_Project/Videos/Komputer_P1.mp4",
                    "Assets/_Project/Videos/Komputer_P2.mp4",
                    // Add up to 16 meetings
                }
            }
        };

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer reference not set. Please assign the VideoPlayer component.");
            return;
        }

        // Get references to the material and meeting from GameManager
        materi = GameManager.Instance._materi;
        pertemuan = GameManager.Instance._pertemuan;

        // Extract the numerical part from the pertemuan string
        int pertemuanIndex = ExtractPertemuanIndex(pertemuan);

        // Log the extracted pertemuan index
        Debug.Log("Extracted pertemuan index: " + pertemuanIndex);

        // Set the video based on material and meeting
        if (videoPaths.ContainsKey(materi))
        {
            if (pertemuanIndex >= 0 && pertemuanIndex < videoPaths[materi].Length)
            {
                videoPlayer.url = videoPaths[materi][pertemuanIndex];
                videoPlayer.Play();
            }
            else
            {
                Debug.LogError("Invalid meeting number.");
            }
        }
        else
        {
            Debug.LogError("Material not found in videoPaths.");
        }

        // Set up the SkipButton function
        skipButton.onClick.AddListener(SkipVideo);

        // Set up event handler for video end
        videoPlayer.started += OnVideoPlay;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Extracts the numerical part from a string like "Pertemuan1" and returns it as an integer
    int ExtractPertemuanIndex(string pertemuan)
    {
        Regex regex = new Regex(@"\d+");
        Match match = regex.Match(pertemuan);
        if (match.Success)
        {
            return int.Parse(match.Value) - 1; // Convert to zero-based index
        }
        else
        {
            Debug.LogError("Invalid pertemuan format.");
            return -1; // Return an invalid index if the format is wrong
        }
    }

    public void SkipVideo()
    {
        videoPlayer.Stop();
        Cutscene = null;
        // Disable the GameObject containing the VideoPlayer
        gameObject.SetActive(false);
        LocalController.Instance.Toggle();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Ensure videoPlayer is not null and not destroyed before accessing it
        if (videoPlayer != null && !videoPlayer.Equals(null))
        {
            // Video has finished, perform desired actions here
            // For example, disable the object containing the video player
            Cutscene = null;
            gameObject.SetActive(false);
            LocalController.Instance.Toggle();
        }
    }
    void OnVideoPlay(VideoPlayer vp)
    {
        // Ensure videoPlayer is not null and not destroyed before accessing it
        if (videoPlayer != null && !videoPlayer.Equals(null))
        {
            LocalController.Instance.Toggle();
        }
    }
}
