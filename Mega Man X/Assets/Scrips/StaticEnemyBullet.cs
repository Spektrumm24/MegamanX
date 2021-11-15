using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyBullet : MonoBehaviour
{
    [SerializeField] float speed;
    void Start()
    {
        
    }
    void Update()
    {
        transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);
        if (layer == "Ground" || layer == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
