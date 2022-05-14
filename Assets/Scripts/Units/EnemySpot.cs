using UnityEngine;

public class EnemySpot : MonoBehaviour
{
    private float boundary = 10;
    private GameObject enemy;

    private void Awake()
    {
        boundary = GetComponent<MeshRenderer>().bounds.size.z * 2;
        enemy = transform.GetComponentInChildren<IEnemy>().gameObject;
        enemy.SetActive(false);
    }
    void Update()
    {
        if(transform.position.z < GameManager.PlayerPos.z - boundary)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z < GameManager.PlayerPos.z + (6*boundary))
        {
            enemy.SetActive(true);
        }
    }
}
