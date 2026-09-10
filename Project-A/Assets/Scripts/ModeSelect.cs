using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelect : MonoBehaviour
{

    public int mode = 0; // 0 = easy, 1 = normal, 2 = hard
    public static ModeSelect Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void EasyMode()
    {
        mode = 0;
        Debug.Log("Easy Mode Selected");
        PlayerController.Instance.BeginDungeonMode();
        SceneManager.LoadScene("Dungeon");
    }
    public void NormalMode()
    {
        mode = 1;
        Debug.Log("Normal Mode Selected");
        PlayerController.Instance.BeginDungeonMode();
        SceneManager.LoadScene("Dungeon");
    }
    public void HardMode()
    {
        mode = 2;
        Debug.Log("Hard Mode Selected");
        PlayerController.Instance.BeginDungeonMode();
        SceneManager.LoadScene("Dungeon");
    }
}
