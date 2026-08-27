using UnityEngine;

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
    
    private void Start()
    {
        Instance = this;
        if (ShopManager.Instance != null)
        {
            DontDestroyOnLoad(gameObject);
            if (ShopMenu != null)
            {
                DontDestroyOnLoad(ShopMenu.transform.root.gameObject);
                ShopMenu.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (ShopMenu == null || PlayerController.Instance == null)
        {
            return;
        }

        bool playerAtShop = Collider2DIsTouchingShop();
        bool canOpenShop = !PlayerController.Instance.gamePaused && !PlayerController.Instance.isInInventory;
        ShopMenu.SetActive(playerAtShop && canOpenShop);
    }
    private bool Collider2DIsTouchingShop()
    {
        Collider2D playerCollider = PlayerController.Instance.GetComponent<Collider2D>();
        Collider2D shopCollider = Shop != null
            ? Shop.GetComponentInChildren<Collider2D>()
            : GetComponentInChildren<Collider2D>();

        if (playerCollider != null && shopCollider != null)
        {
            return playerCollider.bounds.Intersects(shopCollider.bounds);
        }
        Debug.Log("Player or Shop collider is missing.");

        return false;
    }
    private void updateUI()
    {
        swordText.GetComponent<UnityEngine.UI.Text>().text = "Swords: " + numberOfSwords;
        shieldText.GetComponent<UnityEngine.UI.Text>().text = "Shields: " + numberOfShields;
        potionText.GetComponent<UnityEngine.UI.Text>().text = "Potions: " + numberOfPotions;
        foodText.GetComponent<UnityEngine.UI.Text>().text = "Food: " + numberOfFood;
        goldText.GetComponent<UnityEngine.UI.Text>().text = "Gold: " + PlayerController.Instance.money;
    }
    public void BuyItem(string itemType)
    {
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
        updateUI();
    }

}
