using UnityEngine;

public class Character : MonoBehaviour, IDamagable
{
    int health = 100;
    public void Damage()
    {
        if (health > 0)
            health--;
    }
    public void Destroy()
    {
        throw new System.NotImplementedException();
    }
}
