using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);
        if (layer == "Ground" || layer == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
