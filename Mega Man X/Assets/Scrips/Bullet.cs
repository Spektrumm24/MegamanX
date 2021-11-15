using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    bool canShoot;
    float dir = 1f;
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime * dir, 0, 0));
        }
    }
    public void Shoot(float dir, float speed)
    {
        canShoot = true;
        this.speed = speed;
        this.dir = dir;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layer = LayerMask.LayerToName(collision.gameObject.layer);
        if(layer == "Ground" || layer == "Enemy")
        {
            canShoot = false;
            myAnimator.SetTrigger("Collided");
            StartCoroutine("Die");
        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

}
