using UnityEngine;

public class Switching : MonoBehaviour
{
    [SerializeField] private AudioClip weaponSelectClip;

    private int selectedWeapon = 0;
    private Animator _animator;
    private AudioSource audioSource;

    public Animator animator
    {
        get { return _animator; }
        set { _animator = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = weaponSelectClip;
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelect = selectedWeapon;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (selectedWeapon != previousSelect)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            bool isActive = i == selectedWeapon;
            weapon.gameObject.SetActive(isActive);
            if (isActive)
            {
                _animator = weapon.gameObject.GetComponent<Animator>();
                _animator.SetTrigger("Get");
            }
            i++;
        }
        audioSource.Play();
    }
}
