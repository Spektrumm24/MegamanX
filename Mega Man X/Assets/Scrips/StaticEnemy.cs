using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] GameObject player;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform point;
    [SerializeField] int lifePoints;
    [SerializeField] AudioClip sfxDeath;
    Animator myAnimator;

    bool isPlayerRight;
    bool isPlayerInRange;
    private void Start()
    {
        isPlayerRight = false;
        isPlayerInRange = false;
        StartCoroutine("Time");
        myAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        DetectPlayer();
        EnemyDies();
    }
    private void DetectPlayer()
    {
        if (Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player")) != null)
        {
            isPlayerInRange = true;
            if (transform.position.x > player.transform.position.x)
            {
                isPlayerRight = true;
            }
            else
            {
                isPlayerRight = false;
            }
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    private void Shoot()
    {
        if (isPlayerRight && isPlayerInRange)
        {
            Debug.Log("pum!");
            Instantiate(bullet, point.position, transform.rotation);
        }
    }

    IEnumerator Time()
    {
        yield return new WaitForSeconds(1.5f);
        Shoot();
        StartCoroutine("Time");

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, range);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);
        if (layer == "Bullet")
        {
            //take damage
            lifePoints--;
        }
    }
    private void EnemyDies()
    {
        if (lifePoints <= 0)
        {
            //play o set algun boolean o trigger cuando el enemigo muera
            myAnimator.Play("EnemyExposion");
            StartCoroutine("Die");
        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);
        AudioSource.PlayClipAtPoint(sfxDeath, Camera.main.transform.position);
        Destroy(this.gameObject);
    }
}
