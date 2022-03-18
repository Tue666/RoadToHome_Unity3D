using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 1f;

    private Light lightComponent;

    // Start is called before the first frame update
    void Start()
    {
        lightComponent = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
        lightComponent.intensity = transform.eulerAngles.x > 90 ? 0 : 1;
    }
}
