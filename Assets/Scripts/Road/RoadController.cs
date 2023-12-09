using UnityEngine;

public class RoadController : MonoBehaviour
{
    private const float OffsetFactor = 105f;
    private int _roadChunkIndex = 3;

    private void OnCollisionEnter(Collision other)
    {
        SpawnRoad();
        if(other.gameObject.CompareTag("GroundTemp"))
            Destroy(other.gameObject);
        if (!other.gameObject.CompareTag("Ground")) return;
        other.gameObject.SetActive(false);
        
    }

    private void SpawnRoad()
    {
        GameObject roadChunk = ObjectPooler.Instance.SpawnFromPool("Road",
            new Vector3(0f, 0f, OffsetFactor * _roadChunkIndex),
            Quaternion.Euler(-90f, 0f, 0f));

        _roadChunkIndex++;

        foreach (Transform child in roadChunk.transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject == roadChunk || Random.value > 0.5f) continue;
            ObjectPooler.Instance.SpawnFromPool("Enemy",child.transform.position,
                 Quaternion.Euler(0f, -180f, 0f));
        }
    }
}