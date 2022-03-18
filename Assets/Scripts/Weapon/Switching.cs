using UnityEngine;

public class Switching : MonoBehaviour
{
    [SerializeField] private AudioClip weaponSelectClip;
    [SerializeField] private Recoil recoil;
    [SerializeField] private Scope scope;

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
        if (recoil == null) gameObject.GetComponentInParent<Recoil>();
        if (scope == null) scope = gameObject.GetComponent<Scope>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = weaponSelectClip;
        }
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
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
        {
            selectedWeapon = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && transform.childCount >= 6)
        {
            selectedWeapon = 5;
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
        scope.Switch();
        recoil.Switch();
    }
}
