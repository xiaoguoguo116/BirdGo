using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRock : MonoBehaviour {

    [SerializeField]
    private GameObject platform;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.name.Equals("KitRock") && transform.position.y < 0f)
        {
            StartCoroutine(ResetPostion());
        }
    }

    IEnumerator ResetPostion()
    {
        transform.position = new Vector2(platform.transform.position.x, platform.transform.position.y + 3f);
        yield return new WaitForSeconds(5f);
    }
}
