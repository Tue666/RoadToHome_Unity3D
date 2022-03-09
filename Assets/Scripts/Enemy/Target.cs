using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 50f;
    [SerializeField] private float defense = 5f;
    [SerializeField] private AudioClip deathClip;
    //[SerializeField] private AudioClip takeDamageClip;

    private AudioSource audioSource;
    private Animator animator;
    private bool willDie = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(float amount)
    {
        //audioSource.clip = takeDamageClip;
        //audioSource.Play();
        health -= (amount - defense);
        if (health <= 0 && willDie)
        {
            Die();
        }
    }

    void Die()
    {
        willDie = false;
        animator.SetTrigger("Death");
        audioSource.clip = deathClip;
        audioSource.Play();
        Destroy(gameObject, 1.1f);
    }
}
