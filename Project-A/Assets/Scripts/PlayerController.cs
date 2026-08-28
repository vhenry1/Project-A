using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MapCutout
{
    public Vector2 minimum = new Vector2(-2f, -1f);
    public Vector2 maximum = new Vector2(2f, 1f);
}

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Sprite idleSprite;

    [SerializeField] private Sprite[] walkingSprites;
    [SerializeField] private Sprite[] fightingSprites;
    [SerializeField] private float secondsPerSprite = 0.12f;
    [SerializeField] private Vector2 minimum = new Vector2(-10f, -5f);
	[SerializeField] private Vector2 maximum = new Vector2(10f, 5f);
    [SerializeField] private List<MapCutout> cutouts = new List<MapCutout>();
    public GameObject DungeonDoor;
    public GameObject GreenhouseDoor;
    [SerializeField] private Vector2 dungeonDoorPosition;
    [SerializeField] private Vector2 greenhouseDoorPosition;

    [SerializeField] private float doorInteractionDistance = 1f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;
    private bool wasTouchingDungeonDoor;
    private bool wasTouchingGreenhouseDoor;
    private int walkingSpriteIndex;
    private int fightingSpriteIndex;
    [SerializeField] private bool isFighting;
    private float spriteTimer;
    public GameObject PauseMenu;
    public GameObject inventoryMenu;
    public static PlayerController Instance;
    public bool gamePaused = false;
    public bool isInInventory = false;
    public int numberOfSwords = 0;
    public int numberOfShields = 0;
    public int numberOfPotions = 0;
    public int numberOfFood = 0;
    public int health = 10;
    public int maxHealth = 10;
    public int money = 50;
    public GameObject healthText;
    public GameObject moneyText;


    private void Awake()
    {
        if (Instance != null)
    {
        Destroy(gameObject);
        return;
    }


    Instance = this;
    DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        if (DungeonDoor != null)
        {
            Vector3 currentDoorPosition = DungeonDoor.transform.position;
            DungeonDoor.transform.position = new Vector3(dungeonDoorPosition.x, dungeonDoorPosition.y, currentDoorPosition.z);
        }
        if (GreenhouseDoor != null)
        {
            Vector3 currentDoorPosition = GreenhouseDoor.transform.position;
            GreenhouseDoor.transform.position = new Vector3(greenhouseDoorPosition.x, greenhouseDoorPosition.y, currentDoorPosition.z);
        }
        if (PauseMenu != null)
            PauseMenu.SetActive(false);
            if (PauseMenu != null)
        {
                DontDestroyOnLoad(PauseMenu.transform.root.gameObject);
        }
        if (inventoryMenu != null)
        {
            inventoryMenu.SetActive(false);
            DontDestroyOnLoad(inventoryMenu.transform.root.gameObject);
        }
    }
    private void UpdateUI()
    {
        moneyText.GetComponent<TextMeshProUGUI>().text = money.ToString() + " Gold";
        healthText.GetComponent<TextMeshProUGUI>().text = health.ToString() + " / " + maxHealth.ToString();
    }
  

    private void Update()
    {
        if (isFighting)
        {
            UpdateFightingSprite();
        }
        else if (SceneManager.GetActiveScene().name != "Dungeon" && !gamePaused && !isInInventory)
        {
            Vector2 movement = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"));
            movement = Vector2.ClampMagnitude(movement, 1f);
            rb.linearVelocity = movement * moveSpeed;
            UpdateSprite(movement);
        }
       if (Input.GetButtonDown("Jump"))
        {
            pauseGame();
        }
        if (Input.GetButtonDown("Inventory"))
        {
            Inventory();
        }

        Doors();
        UpdateUI();
    }

    private void FixedUpdate()
    {
        Vector2 nextPosition = rb.position + rb.linearVelocity * Time.fixedDeltaTime;
        Vector2 clampedPosition = ClampPosition(nextPosition);
        if (clampedPosition != rb.position)
        {
            rb.position = clampedPosition;
            rb.linearVelocity = Vector2.zero;
        }
    }
    private void Inventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            isInInventory = !isInInventory;
            if (inventoryMenu != null && !gamePaused)
            {
                inventoryMenu.SetActive(isInInventory);
            }
        }
    }


    private void UpdateSprite(Vector2 movement)
    {
        if (movement.x != 0f)
        {
            spriteRenderer.flipX = movement.x < 0f;
        }

        if (movement.sqrMagnitude == 0f)
        {
            walkingSpriteIndex = 0;
            spriteTimer = 0f;
            spriteRenderer.sprite = idleSprite != null
                ? idleSprite
                : GetFirstWalkingSprite();
            return;
        }

        if (walkingSprites == null || walkingSprites.Length == 0)
        {
            return;
        }

        spriteTimer += Time.deltaTime;
        if (spriteTimer >= secondsPerSprite)
        {
            spriteTimer = 0f;
            walkingSpriteIndex = (walkingSpriteIndex + 1) % walkingSprites.Length;
            spriteRenderer.sprite = walkingSprites[walkingSpriteIndex];
        }
    }

    private void UpdateFightingSprite()
    {
        if (fightingSprites == null || fightingSprites.Length == 0)
        {
            return;
        }

        spriteRenderer.sprite = fightingSprites[fightingSpriteIndex];
        spriteTimer += Time.deltaTime;
        if (spriteTimer >= secondsPerSprite)
        {
            spriteTimer = 0f;
            fightingSpriteIndex = (fightingSpriteIndex + 1) % fightingSprites.Length;
            spriteRenderer.sprite = fightingSprites[fightingSpriteIndex];
        }
    }

    private Sprite GetFirstWalkingSprite()
    {
        return walkingSprites != null && walkingSprites.Length > 0
            ? walkingSprites[0]
            : spriteRenderer.sprite;
    }
     
    private void Doors()
    {
        Collider2D doorCollider = DungeonDoor != null
            ? DungeonDoor.GetComponent<Collider2D>()
            : null;
        bool isTouchingDungeonDoor = false;
        if (DungeonDoor != null)
        {
            isTouchingDungeonDoor = playerCollider != null && doorCollider != null
                ? playerCollider.bounds.Intersects(doorCollider.bounds)
                : Vector2.Distance(transform.position, DungeonDoor.transform.position) <= doorInteractionDistance;
        }

        if (isTouchingDungeonDoor && !wasTouchingDungeonDoor)
        {
            Debug.Log("Player is touching the door");
            SceneManager.LoadScene("Dungeon");
            spriteRenderer.sprite = idleSprite != null
                ? idleSprite
                : GetFirstWalkingSprite();
            rb = GetComponent<Rigidbody2D>();
            rb.position = new Vector2(-2, 0); 
            isFighting = true;
            numberOfSwords = 2;
        }

        wasTouchingDungeonDoor = isTouchingDungeonDoor;
    }

    public void Fight()
    {
        isFighting = true;
        fightingSpriteIndex = 0;
        spriteTimer = 0f;
        if (fightingSprites != null && fightingSprites.Length > 0)
        {
            spriteRenderer.sprite = fightingSprites[0];
        }
        }
    

    

	public Vector2 ClampPosition(Vector2 position)
	{
        Vector2 clampedPosition = new Vector2(
			Mathf.Clamp(position.x, minimum.x, maximum.x),
			Mathf.Clamp(position.y, minimum.y, maximum.y));

        foreach (MapCutout cutout in cutouts)
        {
            clampedPosition = MoveOutsideCutout(clampedPosition, cutout);
        }

        return clampedPosition;
    }

    private Vector2 MoveOutsideCutout(Vector2 position, MapCutout cutout)
    {
        if (position.x <= cutout.minimum.x || position.x >= cutout.maximum.x ||
            position.y <= cutout.minimum.y || position.y >= cutout.maximum.y)
        {
            return position;
        }

        float distanceToLeft = position.x - cutout.minimum.x;
        float distanceToRight = cutout.maximum.x - position.x;
        float distanceToBottom = position.y - cutout.minimum.y;
        float distanceToTop = cutout.maximum.y - position.y;
        float nearestDistance = Mathf.Min(
            distanceToLeft, distanceToRight, distanceToBottom, distanceToTop);

        if (nearestDistance == distanceToLeft)
        {
            position.x = cutout.minimum.x;
        }
        else if (nearestDistance == distanceToRight)
        {
            position.x = cutout.maximum.x;
        }
        else if (nearestDistance == distanceToBottom)
        {
            position.y = cutout.minimum.y;
        }
        else
        {
            position.y = cutout.maximum.y;
        }

        return position;
	}

	private void OnDrawGizmosSelected()
	{
		Vector2 center = (minimum + maximum) * 0.5f;
		Vector2 size = maximum - minimum;

		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.red;
        foreach (MapCutout cutout in cutouts)
        {
            Vector2 cutoutCenter = (cutout.minimum + cutout.maximum) * 0.5f;
            Vector2 cutoutSize = cutout.maximum - cutout.minimum;
            Gizmos.DrawWireCube(cutoutCenter, cutoutSize);
        }
	}
    void pauseGame()
    {
        gamePaused = !gamePaused;
        if (PauseMenu != null && !isInInventory)
        {
            PauseMenu.SetActive(gamePaused);
        }
      
    }

}