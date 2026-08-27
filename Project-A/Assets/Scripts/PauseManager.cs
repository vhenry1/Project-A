using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance; 
    public GameObject PauseMenu;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (PauseMenu != null)
            {
                DontDestroyOnLoad(PauseMenu.transform.root.gameObject);
            }
        }
    }
    private void Start()
    {
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
        }
    }
}
