using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float delayTime = 3f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionClip;

    private float countdown;
    private bool hasExplode = false;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delayTime;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExplode)
        {
            Explosive();
        }
    }

    void Explosive()
    {
        hasExplode = true;
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        audioSource = explosion.AddComponent<AudioSource>();
        audioSource.clip = explosionClip;
        audioSource.Play();

        Destroy(gameObject);
        Destroy(explosion, 2f);
    }
}
