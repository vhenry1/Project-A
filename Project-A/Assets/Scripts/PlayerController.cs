using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] public float jumpForce = 12f;

    [SerializeField] private Sprite[] walkingSprites;
    [SerializeField] private Sprite[] jumpingSprites;
    [SerializeField] private float secondsPerSprite = 0.12f;
    [SerializeField] private Vector2 minimum = new Vector2(-10f, -5f);
	[SerializeField] private Vector2 maximum = new Vector2(10f, 5f);
    [SerializeField] private List<MapCutout> cutouts = new List<MapCutout>();

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int walkingSpriteIndex;
    private int jumpingSpriteIndex;
    private float spriteTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        movement = Vector2.ClampMagnitude(movement, 1f);
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        rb.linearVelocity = movement * moveSpeed;
        UpdateSprite(movement);
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

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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

    private Sprite GetFirstWalkingSprite()
    {
        return walkingSprites != null && walkingSprites.Length > 0
            ? walkingSprites[0]
            : spriteRenderer.sprite;
    }
        private Sprite GetFirstJumpingSprite()
    {
        return jumpingSprites != null && jumpingSprites.Length > 0
            ? jumpingSprites[0]
            : spriteRenderer.sprite;
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
}