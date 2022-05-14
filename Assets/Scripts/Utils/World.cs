using UnityEngine;

public class World : MonoBehaviour
{

    static private Vector3 limits;

    private Transform[] groundParts = new Transform[3];
    private int currentGroundPart = 0;

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
            groundParts[i] = transform.GetChild(i);

        limits = transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size;
    }

    private void Update()
    {
        if (GameManager.PlayerPos.z > groundParts[currentGroundPart].position.z + Limits.z)
        {
            Vector3 newPos = groundParts[currentGroundPart].position;
            newPos.z = groundParts[nextGroundIndex(currentGroundPart)].position.z + Limits.z*2;
            groundParts[currentGroundPart].position = newPos;
            currentGroundPart++;
            if (currentGroundPart > 2)
                currentGroundPart = 0;
        }
    }

    private int nextGroundIndex(int ind)
    {
        if (ind < 2)
            return ind + 1;
        else
            return 0;
    }

    static public Vector3 Limits
    {
        get { return limits; }
    }
}
