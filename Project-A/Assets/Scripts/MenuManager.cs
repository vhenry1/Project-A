using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {   
        SceneManager.LoadScene(1);
        gameObject.SetActive(false);
        UnityEngine.Debug.Log("Start Game button clicked, loading Lobby");
    }    
}
