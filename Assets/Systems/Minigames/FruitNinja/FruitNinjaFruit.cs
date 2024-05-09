using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FruitNinjaFruit : MonoBehaviour
{
    public static List<GameObject> derbis = new List<GameObject>();

    public Rigidbody rb;
    public int Score;
    public float SpeedMultipiler;
    public Renderer objRenderer;
    public Material SliceMat;
    public GameObject BrokeParticle;
    [System.Serializable]
    public enum specialType
    {
        none,bomb,slowmotion, health
    }
    [SerializeField]
    public specialType SpecialType;

    public void Cut()
    {
        if (SettingsMaster.gamePaused)
            return;
        Vector3 speed = FruitNinja.mouseSpeed + FruitNinja.touchSpeed;
        Transform p = Instantiate(BrokeParticle).transform;
        p.transform.position = transform.position;
        p.transform.up = new Vector3(-speed.y, speed.x, speed.z);
        p.gameObject.SetActive(true);

        GameObject[] parts = objRenderer.gameObject.SliceInstantiate(transform.position, new Vector3(-speed.y, speed.x, speed.z), SliceMat);

        for (int i = 0; i < parts.Length; i++)
        {
            float vel = i * 2;

            MeshCollider c = parts[i].AddComponent<MeshCollider>();
            c.sharedMesh = parts[i].GetComponent<MeshFilter>().mesh;
            c.convex = true;

            c.transform.position = transform.position;
            //c.transform.eulerAngles = transform.eulerAngles + new Vector3(-90,0,0);

            Rigidbody r = parts[i].AddComponent<Rigidbody>();
            r.velocity = rb.velocity;
            r.tag = "Fruit";

            r.AddExplosionForce(2, transform.position, 10, 0, ForceMode.Impulse);
            r.AddTorque((Vector3.one * vel) + (rb.angularVelocity * 2), ForceMode.Impulse);
            derbis.Add(parts[i]);
        }

        switch (SpecialType)
        {
            case specialType.none:
                FruitNinja.main().OnFruitCut(this);
                break;
            case specialType.bomb:
                FruitNinja.main().Penality(Score);
                break;
            case specialType.slowmotion:
                FruitNinja.main().OnFruitCut(this);
                FruitNinja.main().timeCountdown += 3;
                break;
            case specialType.health:
                FruitNinja.main().OnFruitCut(this);
                FruitNinja.main().CurrentHealth = Mathf.Clamp(FruitNinja.main().CurrentHealth + 1, -1, FruitNinja.main().MaxHealth);
                break;
            default:
                break;
        }

        //Destroy(this.gameObject);
    }
    public void Die()
    {
        if (SpecialType != specialType.bomb)
        {
            FruitNinja.main().OnFruitDie();
        }
        Destroy(this.gameObject);
    }

    public FruitNinjaFruit Spawn(Vector3 forceDirection)
    {
        FruitNinjaFruit r = Instantiate(this.gameObject, Vector3.up, Quaternion.identity).GetComponent<FruitNinjaFruit>();
        r.rb.AddForce(forceDirection * SpeedMultipiler, ForceMode.Impulse);
        r.rb.AddTorque(new Vector3(Random.Range(40,-40), Random.Range(40, -40), Random.Range(40, -40)), ForceMode.Impulse);
        return r;
    }

    void OnEnable(){
        this.tag = "Fruit";
    }

    private void OnValidate()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
}