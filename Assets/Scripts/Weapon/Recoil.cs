using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private Gun gun;

    // Update is called once per frame
    void Update()
    {
        if (targetRotation != Vector3.zero)
        {
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gun.recoildReturnSpeed * Time.deltaTime);
            currentRotation = Vector3.Slerp(currentRotation, targetRotation, 6f * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    public void RecoilAttacking()
    {
        targetRotation += new Vector3(Random.Range(0, gun.recoilX), Random.Range(-gun.recoilY, gun.recoilY), 0);
    }

    public void Switch()
    {
        gun = gameObject.GetComponentInChildren<Gun>();
    }
}
