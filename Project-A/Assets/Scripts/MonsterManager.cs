using System.Collections;
using UnityEngine;
using TMPro;

public class MonsterManager : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] costumes;

    [Header("Movement")]
    [SerializeField] private Vector2[] waypoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float endpointPauseDuration = 1f;
    public GameObject MonsterCount;

    private int waypointIndex;
    private int costumeIndex;
    private bool isMoving;
    private int monsterCount = 0;

    private void Reset()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        isMoving = true;
        ResetToStart();
        monsterCount = 0;
    }

    private void Update()
    {
        if (!isMoving || waypoints == null || waypoints.Length == 0 || spriteRenderer == null)
            return;

        Vector3 startPosition = transform.position;
        Vector3 target = new Vector3(waypoints[waypointIndex].x, waypoints[waypointIndex].y, transform.position.z);

        if (target.x != startPosition.x)
        {
            spriteRenderer.flipX = target.x < startPosition.x;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            transform.position = target;

            if (waypointIndex >= Mathf.Min(8, waypoints.Length - 1))
            {
                isMoving = false;
                StartCoroutine(RestartAtBeginning());
                UpdatePlayer();

                return;
            }

            waypointIndex++;
        }
    }

    private IEnumerator RestartAtBeginning()
    {
        yield return new WaitForSeconds(endpointPauseDuration);

        spriteRenderer.enabled = false;
        waypointIndex = 0;
        transform.position = new Vector3(waypoints[0].x, waypoints[0].y, transform.position.z);

        yield return null;

        ChangeCostume();
        spriteRenderer.enabled = true;
        isMoving = true;
    }

    private void ChangeCostume()
    {
        if (costumes == null || costumes.Length == 0)
            return;

        costumeIndex = (costumeIndex + 1) % costumes.Length;
        spriteRenderer.sprite = costumes[costumeIndex];
    }
    public void UpdatePlayer()
    {
        monsterCount++;
        MonsterCount.GetComponent<TextMeshProUGUI>().text = monsterCount.ToString();

        if (ModeSelect.Instance.mode == 0)
        {
            PlayerController.Instance.money += 1;
            Debug.Log("Player money: " + PlayerController.Instance.money);
            Debug.Log("Player health: " + PlayerController.Instance.health);
            PlayerController.Instance.health -= 1;
            PlayerController.Instance.UpdateUI();
        }
        else if (ModeSelect.Instance.mode == 1)
        {
            PlayerController.Instance.money += 2;
            Debug.Log("Player money: " + PlayerController.Instance.money);
            Debug.Log("Player health: " + PlayerController.Instance.health);
            PlayerController.Instance.health -= 2;
            PlayerController.Instance.UpdateUI();
        }
        else if (ModeSelect.Instance.mode == 2)
        {
            PlayerController.Instance.money += 3;
            Debug.Log("Player money: " + PlayerController.Instance.money);
            Debug.Log("Player health: " + PlayerController.Instance.health);
            PlayerController.Instance.health -= 3;
            PlayerController.Instance.UpdateUI();
        }
    }
    

    private void ResetToStart()
    {
        waypointIndex = 0;

        if (costumes != null && costumes.Length > 0)
        {
            costumeIndex = (costumeIndex + 1) % costumes.Length;
            spriteRenderer.sprite = costumes[costumeIndex];
        }

        if (waypoints != null && waypoints.Length > 0)
        {
            transform.position = new Vector3(waypoints[0].x, waypoints[0].y, transform.position.z);
        }

        
    }
}
