using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] ParticleSystem dustPrefab;
    [SerializeField] ParticleSystem coinPickUpPrefab;
    [SerializeField] ParticleSystem bloodPrefab;
    [Header("Variables")]
    [SerializeField] float dustLift;
    [SerializeField] float speed;
    [SerializeField] float pushX;
    [SerializeField] float pushY;

    float bloodAngleToRight = 120f; //rotation angle to spawn blood at the right side
    bool isOnGround;
    Collider2D col;
    Rigidbody2D rb;
    Animator anim;
    float walk;
    ParticleSystem bloodEffect;
    ParticleSystem dust;
    ParticleSystem coinPickUpEffect;
    Vector2 spawnPosition;
    CoinManager coinManager;
    Transform cachedTransform;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coinManager = FindObjectOfType<CoinManager>();
        cachedTransform = transform;
    }

    private void Start()
    {
        spawnPosition = cachedTransform.position;
        coinPickUpEffect = Instantiate(coinPickUpPrefab, cachedTransform);
        bloodEffect = Instantiate(bloodPrefab, transform);
        dust = Instantiate(dustPrefab, new Vector2(transform.position.x, col.bounds.min.y + dustLift), Quaternion.identity, transform);
    }

    public float GetDirection
    {
        get { return walk; }
    }

    private bool IsOnGround { get => isOnGround;
        set
        {
            isOnGround = value;
            anim.SetBool("isOnGround", isOnGround);
        }
    }

    private void Update()
    {
        walk = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("movement", walk);
        if (walk == 0 || !IsOnGround)
            dust.Stop();
        if (walk != 0 && IsOnGround)       
            dust.Play();
       
        transform.Translate(Vector3.right * walk * speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) && IsOnGround)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(pushX * walk, pushY));
            IsOnGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && !IsOnGround)       
            IsOnGround = true;                               
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            collision.gameObject.SetActive(false);
            coinPickUpEffect.Play();
            coinManager.CollectCoin();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && IsOnGround)
            IsOnGround = false;      
    }

    public void GetDamage(float hitVector)
    {
        Vector3 tempRotation = bloodEffect.transform.eulerAngles;
        tempRotation.z = hitVector > 0 ? 0 : bloodAngleToRight;
        bloodEffect.transform.eulerAngles = tempRotation;
        bloodEffect.Play();
        GameplayManager.instance.PlayerDamaged(false);      
    }

    public void Respawn()
    {
        bloodEffect.Stop();
        transform.position = spawnPosition;
    }
}