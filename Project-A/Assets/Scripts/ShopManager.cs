using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public GameObject ShopMenu;
    public GameObject Shop;

    public GameObject swordText;
    public GameObject shieldText;
    public GameObject potionText;
    public GameObject foodText;

    public int numberOfSwords = 5;
    public int numberOfShields = 5;
    public int numberOfPotions = 5;
    public int numberOfFood = 5;
    public GameObject goldText;
    public bool isPlayerInShop;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // ResolvePrefabReferences();
        DontDestroyOnLoad(gameObject);

        if (ShopMenu != null)
        {
            ShopMenu.SetActive(false);
        }
    }

    
    private void Update()
    {
        if (ShopMenu == null || PlayerController.Instance == null)
        {
            return;
        }

        if (Input.GetButtonDown("Shop"))
        {
            isPlayerInShop = !ShopMenu.activeSelf;
            ShopMenu.SetActive(isPlayerInShop);
            UpdateUI();
        }
    }
    
    private void UpdateUI()
    {
        if (PlayerController.Instance == null)
        {
            return;
        }

        SetText(swordText, "Swords: " + numberOfSwords);
        SetText(shieldText, "Shields: " + numberOfShields);
        SetText(potionText, "Potions: " + numberOfPotions);
        SetText(foodText, "Food: " + numberOfFood);
        SetText(goldText, "Gold: " + PlayerController.Instance.money);
    }

    private void SetText(GameObject target, string value)
    {
        if (target == null)
        {
            return;
        }

        UnityEngine.UI.Text text = target.GetComponent<UnityEngine.UI.Text>();
        if (text != null)
        {
            text.text = value;
            return;
        }

        TMP_Text tmpText = target.GetComponent<TMP_Text>();
        if (tmpText != null)
        {
            tmpText.text = value;
        }
    }

    public void BuyItem(string itemType)
    {
        if (PlayerController.Instance == null)
        {
            return;
        }

        switch (itemType)
        {
            case "Sword":
                if (numberOfSwords > 0)
                {
                    numberOfSwords--;
                    PlayerController.Instance.money -= 5; 
                    PlayerController.Instance.numberOfSwords++;
                }
                break;
            case "Shield":
                if (numberOfShields > 0)
                {
                    numberOfShields--;
                    PlayerController.Instance.money -= 10;
                    PlayerController.Instance.numberOfShields++;
                }
                break;
            case "Potion":
                if (numberOfPotions > 0)
                {
                    numberOfPotions--;
                    PlayerController.Instance.money -= 5;
                    PlayerController.Instance.numberOfPotions++;
                }
                break;
            case "Food":
                if (numberOfFood > 0)
                {
                    numberOfFood--;
                    PlayerController.Instance.money -= 10;
                    PlayerController.Instance.numberOfFood++;
                }
                break;
        }
        UpdateUI();
    }

}
