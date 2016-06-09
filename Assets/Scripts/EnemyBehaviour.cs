using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public static float shotsPerSecond = 0.5f;

    public GameObject projectile;
    public float health = 150f;
    public float projectileSpeed = 10f;
    public int scoreValue = 150;
    public AudioClip fireSound;
    public AudioClip deathSound;
    public GameObject explosion;
    public GameObject hitEffect;

    private ScoreKeeper scoreKeeper;
    private Color hitEffectColor;
    private GameObject[] laserPositions;

    void Start ()
    {
        laserPositions = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            laserPositions[i] = transform.GetChild(i).gameObject;
        }
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }

    void Update()
    {
        
        float probability = Time.deltaTime * shotsPerSecond;
        if (Random.value < probability && health > 0)
        {
            Fire();
        }
    }

    void Fire()
    {
        if (gameObject.transform.childCount > 0)
        {
            Debug.Log("Laser Positions Count : " + laserPositions.Length);
            GameObject missile = Instantiate(projectile, laserPositions[Random.Range(0, laserPositions.Length)].transform.position, Quaternion.Euler(0f, 0f, 180)) as GameObject;
            missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -projectileSpeed);
            AudioSource.PlayClipAtPoint(fireSound, transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();

        if (missile)
        {
            health -= missile.GetDamage();

            Hit(collider);
            missile.Hit();

            if (health <= 0)
            {
                explosion = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                Destroy(explosion, 2f);
                Die();
            }
        }
    }

    void Hit(Collider2D collider)
    {
        if (hitEffect)
        {
            GameObject cloneHitEffect = Instantiate(hitEffect, collider.transform.position, Quaternion.identity) as GameObject;
            hitEffectColor = collider.gameObject.GetComponent<SpriteRenderer>().color;
            cloneHitEffect.GetComponent<ParticleSystem>().startColor = hitEffectColor;
        } 
    }

    void Die()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        scoreKeeper.Score(scoreValue);

        Destroy(gameObject);
    }

    public float ShotsPerSecond
    {
        get
        {
            //Some other code
            return shotsPerSecond;
        }
        set
        {
            //Some other code
            shotsPerSecond = value;
        }
    }

}
