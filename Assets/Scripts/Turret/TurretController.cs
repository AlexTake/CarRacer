using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float smoothness = 5f;
    [SerializeField] private float shootingInterval = 0.25f;
    [SerializeField] private float speedFactor = 10f;

    [SerializeField] private Transform firePoint;

    private const float MaxAngle = 60f;
    private float _targetRotation;
    public bool _canShoot;

    public static TurretController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool CanShoot
    {
        get => _canShoot;
        set
        {
            if (_canShoot.Equals(value)) return;
            _canShoot = value;
            StopCoroutine(ShootRoutine());
            StartCoroutine(ShootRoutine());
        }
    }

    private void Start() => StartCoroutine(ShootRoutine());

    void Update()
    {
        float swipeValue = 0f;
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                swipeValue += Input.GetTouch(i).deltaPosition.x;
            }
            rotationSpeed = 10f;
        }
        else if (Input.GetMouseButton(0))
        {
            swipeValue = Input.GetAxis("Mouse X");
            rotationSpeed = 100f;
        }

        _targetRotation += swipeValue * rotationSpeed * Time.deltaTime;

        _targetRotation = Mathf.Clamp(_targetRotation, -MaxAngle, MaxAngle);

        float smoothedRotation =
            Mathf.LerpAngle(transform.localEulerAngles.y, _targetRotation, Time.deltaTime * smoothness);
        transform.localEulerAngles = new Vector3(-90f, smoothedRotation, 0f);
    }

    private IEnumerator ShootRoutine()
    {
        while (_canShoot)
        {
            GameObject bullet = ObjectPooler.Instance.SpawnFromPool("Bullet",
                firePoint.position, firePoint.rotation);
            AudioManager.Instance.Play("Shot");
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            bulletRb.velocity = firePoint.up * speedFactor;

            yield return new WaitForSeconds(shootingInterval);
        }
    }
}
