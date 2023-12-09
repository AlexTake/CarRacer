using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool canMove = true;
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private GameObject _sprite;
    [SerializeField] private Camera _camera;
    [SerializeField] private Image _target;

    private float _carHp = 1f;
    private static readonly float DistanceToWin = 500f;
    private static readonly int Play = Animator.StringToHash("Play");

    public static CarController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public float CarHp
    {
        get => _carHp;
        set
        {
            if (_carHp == value) return;
            _carHp = value;
            OnCarHpChanged();
        }
    }

    public bool CanMove
    {
        get => canMove;
        set
        {
            if (canMove == value) return;
            canMove = value;
            OnCanMoveChanged();
        }
    }

    private void Update()
    {
        if (CanMove)
        {
            transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
            float fillAmount = Mathf.Abs(transform.position.z) / DistanceToWin;
            _target.fillAmount = Mathf.Clamp01(fillAmount);
            if (transform.position.z > DistanceToWin)
                GameController.Instance.EndGame(true);
        }
    }

    private void OnCarHpChanged()
    {
        UpdateHpUI();
        if (CarHp > 0f) return;
        GameController.Instance.EndGame(false);
    }

    private void UpdateHpUI()
    {
        _sprite.transform.localScale = new Vector3(CarHp, 1f, 1f);
    }

    private void OnCanMoveChanged()
    {
        _camera.GetComponent<Animator>().SetBool(Play, CanMove);
        if (CanMove)
        {
            foreach (var system in particleSystems)
                system.Play();
            Invoke(nameof(StartPauseGame), 2f);
        }
        else
        {
            foreach (var system in particleSystems)
                system.Stop();
            Invoke(nameof(StartPauseGame), 0.2f);
        }
    }

    private void StartPauseGame()
    {
        TurretController.Instance.CanShoot = CanMove;
    }
}