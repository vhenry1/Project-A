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
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
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
