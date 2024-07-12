using UnityEngine;
using System.Collections;

public class HexagonMovement : MonoBehaviour
{
    private Vector3 originalLocalPosition;
    private bool isMoving = false;
    public float moveSpeed = 1;
    private Animator animator;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                GameManager.SoundEffects.Invoke("click");
                StartMovement();
            }
        }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && !isMoving)
        {
        
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    GameManager.SoundEffects.Invoke("click");
                    StartMovement();
                }
            }
        }
#endif
    }

    private void StartMovement()
    {
        GameManager.MovesLeftDecrease.Invoke();
        isMoving = true;
        StartCoroutine(MoveHexagon());
    }

    private IEnumerator MoveHexagon()
    {
        while (isMoving)
        {
            animator.SetBool("Move", isMoving);
            transform.localPosition += Vector3.up * Time.deltaTime * moveSpeed;
            yield return null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hexagon"))
        {
            GameManager.SoundEffects.Invoke("collision");
            Debug.Log("Collided with: " + collision.gameObject.name);
            StopCoroutine(MoveHexagon());

            transform.localPosition = originalLocalPosition;
            isMoving = false;
            animator.SetBool("Move", isMoving);
        }
    }
}
