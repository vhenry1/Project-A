using UnityEngine;
using TMPro;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public GameObject inventoryMenu;
    public GameObject swordText;
    public GameObject shieldText;
    public GameObject potionText;
    public GameObject foodText;
    private void Start()
    {
        Instance = this;
        if (InventoryManager.Instance != null)
        {
            DontDestroyOnLoad(gameObject);
            if (inventoryMenu != null)
            {
                DontDestroyOnLoad(inventoryMenu.transform.root.gameObject);
            }
        }
    }
    private void Update()
    {
        PlayerController playerControllerInstance = PlayerController.Instance;
        if (playerControllerInstance != null)
        {
            playerControllerInstance.numberOfSwords = playerControllerInstance.numberOfSwords;
            playerControllerInstance.numberOfShields = playerControllerInstance.numberOfShields;
            playerControllerInstance.numberOfPotions = playerControllerInstance.numberOfPotions;
            playerControllerInstance.numberOfFood = playerControllerInstance.numberOfFood;
        }
        UpdateUI();
    }
    private void UpdateUI()
    {
        TMP_Text swordTextComponent = swordText.GetComponent<TMP_Text>();
        if (swordTextComponent != null)
        {
            swordTextComponent.text = "Swords: " + PlayerController.Instance.numberOfSwords;
        }
        TMP_Text shieldTextComponent = shieldText.GetComponent<TMP_Text>();
        if (shieldTextComponent != null)
        {
            shieldTextComponent.text = "Shields: " + PlayerController.Instance.numberOfShields;
        }
        TMP_Text potionTextComponent = potionText.GetComponent<TMP_Text>();
        if (potionTextComponent != null)
        {
            potionTextComponent.text = "Potions: " + PlayerController.Instance.numberOfPotions;
        }
        TMP_Text foodTextComponent = foodText.GetComponent<TMP_Text>();
        if (foodTextComponent != null)
        {
            foodTextComponent.text = "Food: " + PlayerController.Instance.numberOfFood;
        }
    }
}
