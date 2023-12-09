using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void OnEnable() => Invoke(nameof(DisableBullet), 0.7f);
    private void DisableBullet() => gameObject.SetActive(false);

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;
        other.gameObject.transform.parent.GetComponent<EnemyController>().EnemyHp -= 0.5f;
        AudioManager.Instance.Play("HitEnemy");
        gameObject.SetActive(false);
    }
}