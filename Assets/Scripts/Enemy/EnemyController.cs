using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject _sprite;
    private float _moveSpeed = 18f;
    private bool _isChasing;
    private Transform _player;
    private Animator _animator;
    public float _enemyHp = 1f;

    private static readonly int CanMove = Animator.StringToHash("CanMove");

    public float EnemyHp
    {
        get => _enemyHp;
        set
        {
            if (_enemyHp == value) return;
            _enemyHp = value;
            OnHpChanged();
        }
    }

    private void OnHpChanged()
    {
        UpdateHpUI();
        if (EnemyHp > 0f) return;
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        EnemyHp = 1f;
        gameObject.SetActive(false);
        _sprite.transform.localScale = Vector3.one;
    }

    private void UpdateHpUI()
    {
        _sprite.transform.localScale = new Vector3(_enemyHp, 1f, 1f);
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _animator = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isChasing = true;
        _animator.SetBool(CanMove, true);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        ResetEnemy();
        CarController.Instance.CarHp -= 0.1f;
        AudioManager.Instance.Play("HitCar");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _isChasing = false;
        _animator.SetBool(CanMove, false);
    }

    private void Update()
    {
        if (_isChasing)
            ChasePlayer();
    }

    private void ChasePlayer()
    {
        transform.LookAt(_player);
        transform.Translate(Vector3.forward * (_moveSpeed * Time.deltaTime));
    }
}