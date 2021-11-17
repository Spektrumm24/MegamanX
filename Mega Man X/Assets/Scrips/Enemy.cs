using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] GameObject player;
    [SerializeField] int lifePoints;
    [SerializeField] AudioClip sfxDeath;
    Animator myAnimator;
    public AIPath aiPath;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Flip();
        DetectPlayer();
        EnemyDies();
    }
    private void DetectPlayer()
    {
        if (Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Player")) != null)
        {
            Debug.Log("Persigalo!");
            aiPath.canMove = true;
        }
        else
        {
            aiPath.canMove = false;
        }
    }
    private void EnemyDies()
    {
        if (lifePoints <= 0)
        {
            //play o set algun boolean o trigger cuando el enemigo muera
            //myAnimator.Play("EnemyExposion");
            myAnimator.SetBool("Alive", false);
            StartCoroutine("Die");
        }
    }

    private void Flip()
    {
        if(player != null)
        {
            if (aiPath.desiredVelocity.x >= 0.01f) transform.rotation = Quaternion.Euler(0, 180, 0); 
            else if(aiPath.desiredVelocity.x <= -0.01f) transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f,0,0,0.25f);
        Gizmos.DrawSphere(transform.position, range);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);
        if (layer == "Bullet")
        {
            lifePoints--;
        }
    }
    IEnumerator Die()
    {
        
        yield return new WaitForSeconds(0.5f);
        AudioSource.PlayClipAtPoint(sfxDeath, Camera.main.transform.position);
        Destroy(this.gameObject);
        UIManager.instance.killEnemy();
    }

}
