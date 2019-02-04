using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{    
    [SerializeField] float kickXPower;
    [SerializeField] float kickYPower;
    [SerializeField] float speed;
    [SerializeField] ParticleSystem damagedPrefab;

    PlayerController player; //since there is only 1 player, and it's unlikely to be more
    ParticleSystem damageEffect;
    Collider2D col;
    Vector2[] walkPoints;
    Vector2 walkDirection;
    Animator anim;
    Rigidbody2D rb;
    int elementNum;
    Vector2 spawnPosition;
    Transform cachedTransform;
    ParticleSystem.MainModule particleModule;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        walkPoints = new Vector2[2];  
        cachedTransform = transform;
    }

    void Start()
    {
        spawnPosition = cachedTransform.position;
        damageEffect = Instantiate(damagedPrefab, cachedTransform, false);
        particleModule = damageEffect.main;
    }
    
    private void FixedUpdate()
    {
        anim.SetFloat("direction", walkDirection.x);
        GoToPoint(walkPoints[elementNum]);
        if (Vector2.Distance(rb.position, walkPoints[elementNum]) < 0.1f)
            ChangeToNextWaypoint();
    }

    private void ChangeToNextWaypoint() => 
        elementNum = (elementNum + 1) % walkPoints.Length;
    

    private void GoToPoint(Vector3 target)
    {
        walkDirection = (target - cachedTransform.position).normalized;
        rb.AddForce(walkDirection * speed, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collisionCollider = collision.collider;
        if (collisionCollider.CompareTag("Player"))
        {
            float directionPlayer = player.GetDirection;
            if ((directionPlayer < 0 && walkDirection.x < 0) || (directionPlayer > 0 && walkDirection.x > 0))
            {
                Vector2 forceToAdd = new Vector2(walkDirection.x < 0 ? -kickXPower : kickXPower, kickYPower);
                rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            }
            else
            {
                player.GetDamage(walkDirection.x);
                damageEffect.Play();
            }                   
        }

        if (collisionCollider.CompareTag("Ground"))
        {
            particleModule.loop = false;
            damageEffect.Stop(); //is on electicity while not on ground
            //define new walkpoints for alien
            walkPoints[0] = new Vector2(collisionCollider.bounds.min.x + col.bounds.size.x,
            collisionCollider.bounds.max.y + col.bounds.size.y / 2);
            walkPoints[1] = new Vector2(collisionCollider.bounds.max.x - col.bounds.size.x,
            collisionCollider.bounds.max.y + col.bounds.size.y / 2);
        }    
        
        if (collisionCollider.CompareTag("Alien"))       
            Physics2D.IgnoreCollision(col, collisionCollider);      
    }    

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
          particleModule.loop = true;
          damageEffect.Play(); //is on electicity while not on ground
        }
    }          

    public void Respawn()
    {
        damageEffect.Stop();
        cachedTransform.position = spawnPosition;
    }                                                                 
}                                                                                      