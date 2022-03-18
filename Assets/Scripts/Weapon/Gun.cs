using UnityEngine;

public class Gun : MonoBehaviour
{
    // Scope
    [SerializeField] private int _scopeEquipped = 0;
    [SerializeField] private int _zoomSpeed = 1;
    // Recoil
    [SerializeField] private float _recoilX;
    [SerializeField] private float _recoilY;
    [SerializeField] private float _recoildReturnSpeed;

    // Scope 
    public int zoomSpeed
    {
        get { return _zoomSpeed; }
        set { _zoomSpeed = value; }
    }

    // Recoil
    public float recoilX
    {
        get { return _recoilX; }
        set { _recoilX = value; }
    }
    public float recoilY
    {
        get { return _recoilY; }
        set { _recoilY = value; }
    }
    public float recoildReturnSpeed
    {
        get { return _recoildReturnSpeed; }
        set { _recoildReturnSpeed = value; }
    }

    public int scopeEquipped
    {
        get { return _scopeEquipped; }
        set { _scopeEquipped = value; }
    }
}
