using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DelayActivation : MonoBehaviour
{

    static DelayActivation instance { get; set; }
    public static DelayActivation main()
    {
        if (!instance)
        {
            instance = new GameObject("DelayActivation").AddComponent<DelayActivation>();
        }
        return instance;
    }

    public static void SetActive(float time, GameObject target)
    {
        main().StartCoroutine(main().activateObject((int)time, target));
    }

    public IEnumerator activateObject(int delay, GameObject target)
    {
        yield return new WaitForSeconds(delay);
        target.SetActive(true);
    }
}