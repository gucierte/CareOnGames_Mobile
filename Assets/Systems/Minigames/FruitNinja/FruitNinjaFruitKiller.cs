using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitNinjaFruitKiller : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Fruit")
        {
            if (other.GetComponent<FruitNinjaFruit>())
            {
                other.GetComponent<FruitNinjaFruit>().Die();
            }
            Destroy(other.gameObject);
        }
    }
}
