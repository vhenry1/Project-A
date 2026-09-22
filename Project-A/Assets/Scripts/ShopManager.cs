using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject shopPanel;
    public Button buySwordButton;
    public Button buyPotionButton;
    public Button buyShieldButton;
    public Button buyFoodButton;
    public TMP_Text swordText;
    public TMP_Text potionText;
    public TMP_Text shieldText;
    public TMP_Text foodText;
    private int numberOfSwords = 5;
    private int numberOfPotions = 5;
    private int numberOfShields = 5;
    private int numberOfFood = 5;
    private PlayerController playerControllerInstance;

    [Header("Optional Toggle Key")]
    public KeyCode shopToggleKey = KeyCode.P;

    private void Start()
    {
        playerControllerInstance = PlayerController.Instance;

        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (buySwordButton != null)
            buySwordButton.onClick.AddListener(BuySword);

        if (buyPotionButton != null)
            buyPotionButton.onClick.AddListener(BuyPotion);

        if (buyShieldButton != null)
            buyShieldButton.onClick.AddListener(BuyShield);

        if (buyFoodButton != null)
            buyFoodButton.onClick.AddListener(BuyFood);
    }

    private void Update()
    {
        if (Input.GetKeyDown(shopToggleKey))
        {
            ToggleShop();
        }

        if (playerControllerInstance == null)
            playerControllerInstance = PlayerController.Instance;
    }

    public void ToggleShop()
    {
        if (shopPanel == null)
            return;

        shopPanel.SetActive(!shopPanel.activeSelf);
    }

    public void BuySword()
    {
        if (numberOfSwords > 0)
        {
            numberOfSwords--;
            swordText.text = "Swords: " + numberOfSwords;
            if (playerControllerInstance != null)
            {
                playerControllerInstance.money -= 10; // Assuming each sword costs 10 money
            }
        }
    }

    public void BuyPotion()
    {
        if (numberOfPotions > 0)
        {
            numberOfPotions--;
            potionText.text = "Potions: " + numberOfPotions;
            if (playerControllerInstance != null)
            {
                playerControllerInstance.money -= 10; // Assuming each potion costs 3 money
            }
        }
    }

    public void BuyShield()
    {
        if (numberOfShields > 0)
        {
            numberOfShields--;
            shieldText.text = "Shields: " + numberOfShields;
            if (playerControllerInstance != null)
            {
                playerControllerInstance.money -= 10; // Assuming each shield costs 8 money
            }
        }
    }

    public void BuyFood()
    {
        if (numberOfFood > 0)
        {
            numberOfFood--;
            foodText.text = "Food: " + numberOfFood;
            if (playerControllerInstance != null)
            {
                playerControllerInstance.money -= 5; // Assuming each food item costs 2 money
            }
        }
    }
}
