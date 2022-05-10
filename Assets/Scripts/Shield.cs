using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour,IDamagable
{
    public void Damage()
    {
        Debug.Log("Shield is damaged!");
    }

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }
}
