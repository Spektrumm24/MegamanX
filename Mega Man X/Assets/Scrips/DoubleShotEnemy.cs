using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotEnemy : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] GameObject player;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform point1;
    [SerializeField] Transform point2;
    [SerializeField] int lifePoints;
    Animator myAnimator;
    bool isPlayerInRange;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerInRange = false;
        StartCoroutine("Time");
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
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
        }
        else
        {
            isPlayerInRange = false;
        }
    }
    private void Shoot()
    {
        if (isPlayerInRange)
        {
            Debug.Log("pum!");
            GameObject bullet1 = Instantiate(bullet, point1.position, transform.rotation);
            bullet1.GetComponent<Rigidbody2D>().velocity = new Vector2(-10,5);
            GameObject bullet2 = Instantiate(bullet, point2.position, transform.rotation);
            bullet2.GetComponent<Rigidbody2D>().velocity = new Vector2(10, 5);
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
        Destroy(this.gameObject);
    }
}
