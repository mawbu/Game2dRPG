using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    public void DestroyWithEffect() 
    {
        if (destroyVFX != null)
        {
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            GetComponent<PickUpSpawner>().DropItems();
        }
        Destroy(gameObject);
    }
}

