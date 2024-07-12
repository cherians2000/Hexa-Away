using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BorderHandle : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hexagon"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
            collision.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder= 0;
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
            StartCoroutine(destroyGameHexogon(collision.gameObject));
            GameManager.HexogonLeftCount.Invoke();
        }
    }

    IEnumerator destroyGameHexogon(GameObject hexagon)
    {
        yield return new WaitForSeconds(5);
        Destroy(hexagon);
    }
}
