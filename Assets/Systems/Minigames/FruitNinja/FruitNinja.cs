using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FruitNinja : MinigameMaster
{
    static FruitNinja instance;
    public static FruitNinja main()
    {
        FruitNinja r = instance;
        if (instance == null)
        {
            instance = FindFirstObjectByType<FruitNinja>();
        }
        return r;
    }

    public int MaxHealth = 3;
    public int StartHealths = 3;
    public int CurrentHealth = 9999;
    public int CurrentScore;
    public TrailRenderer mouseTrail;
    [Space]
    public List<Transform> FruitSpawnPositions = new List<Transform>();
    public List<FruitNinjaFruit> Fruits = new List<FruitNinjaFruit>();
    public List<FruitNinjaFruit> instanceFruits = new List<FruitNinjaFruit>();
    [Space]
    public Vector2 SpawnDelayRange;
    public Vector2 SpawnSpeedRange = Vector2.one;
    public float PerLevelSpawnDificulty = 0.3f;

    [Header("UI")]
    public TextMeshProUGUI LABEL_SCORE;
    public TextMeshProUGUI LABEL_HIGHSCORE;
    public TextMeshProUGUI LABEL_HEALTH;
    [Space]
    public GameObject LABEL_GAMEOVER;
    public TextMeshProUGUI LABEL_GAMEOVER_SCORE;
    public Color LABEL_SCORE_COLOR;
    public Color LABEL_SCORE_COLOR_POSITIVE;
    public Color LABEL_SCORE_COLOR_NEGATIVE;


    //Static
    public static Vector3 mouseSpeed;
    static Vector3 mouseLastPos;

    public static Vector3 touchSpeed;
    static Vector2 touchLastPos;

    public static Vector2 saberPosition;

    public FruitNinjaFruit SpawnFruit(FruitNinjaFruit fruit, Transform cannon)
    {
        if (SettingsMaster.gamePaused)
            return null;
        FruitNinjaFruit r = fruit.Spawn(cannon.up * Random.Range(SpawnSpeedRange.x, SpawnSpeedRange.y));
        r.transform.position = cannon.transform.position;
        instanceFruits.Add(r);
        return r;
    }

    public int HighScore;
    public int CheckHighScore()
    {
        int r = 0;
        if (PlayerPrefs.HasKey("Highscore"))
        {
            r = PlayerPrefs.GetInt("Highscore");
        }
        if (r < CurrentScore)
        {
            PlayerPrefs.SetInt("Highscore", CurrentScore);
            r = CurrentScore;
        }

        return r;
    }

    public void OnFruitCut(FruitNinjaFruit fruit)
    {
        CurrentScore += fruit.Score;
        instanceFruits.Remove(fruit);
        LABEL_SCORE.color = LABEL_SCORE_COLOR_POSITIVE;
        MusicFlow.main.Flow += .3f;
        Destroy(fruit.gameObject);
        LABEL_HIGHSCORE.text = "Melhor pontuação: " + CheckHighScore();
    }

    public void OnFruitDie()
    {
        Sound.Play("SFX/beep-04");
        CurrentScore = Mathf.Clamp(CurrentScore -1, 0, int.MaxValue);
        LABEL_SCORE.color = LABEL_SCORE_COLOR_NEGATIVE;
    }

    public void Penality(int penality)
    {
        CurrentHealth -= 1;
        CurrentScore = Mathf.Clamp(CurrentScore - penality, 0, int.MaxValue);
        LABEL_SCORE.color = LABEL_SCORE_COLOR_NEGATIVE;
        foreach (var f in FindObjectsByType<FruitNinjaFruit>(FindObjectsSortMode.None))
        {
            Destroy(f.gameObject);
        }
        instanceFruits.Clear();
        if (CurrentHealth <= 0)
        {
            GameOver();
        }
    }

    public void SpawnRandomFruit()
    {
        SpawnFruit(Fruits[Random.Range(0, Fruits.Count)], FruitSpawnPositions[Random.Range(0, FruitSpawnPositions.Count)]);
        float currentSpawnDelay = Random.Range(SpawnDelayRange.x - (PerLevelSpawnDificulty * (CurrentScore + 30)), SpawnDelayRange.y);

        //currentSpawnDelay = Mathf.Clamp(currentSpawnDelay, Random.Range(SpawnDelayRange.x / 2, SpawnDelayRange.y), SpawnDelayRange.y);
        currentSpawnDelay = Mathf.Clamp(currentSpawnDelay, -1, 99);

        Debug.Log("Current Spawn Delay:" + currentSpawnDelay);
        Invoke(nameof(SpawnRandomFruit), currentSpawnDelay);
    }

    public override void Play()
    {
        base.Play();
        ResetTime();
        LABEL_GAMEOVER.gameObject.SetActive(false);
        Invoke(nameof(SpawnRandomFruit), Random.Range(SpawnDelayRange.x, SpawnDelayRange.y));
    }

    public override void Stop()
    {
        ResetTime();
        CancelInvoke(nameof(SpawnRandomFruit));
        for (int i = 0; i < instanceFruits.Count; i++)
        {
            if (instanceFruits[i])
            {
                Destroy(instanceFruits[i].gameObject);
            }
            else
            {
                instanceFruits.RemoveAt(i);
            }
        }
        timeCountdown = 0;
        instanceFruits.Clear();
        for (int i = 0; i < FruitNinjaFruit.derbis.Count; i++)
        {
            Destroy(FruitNinjaFruit.derbis[i]);
        }
        FruitNinjaFruit.derbis.Clear();
        CurrentHealth = StartHealths;
        CurrentScore = 0;
        base.Stop();
    }

    public override void Restart()
    {
        base.Restart();
        Stop();
        Play();
    }

    public void GameOver()
    {
        LABEL_GAMEOVER.SetActive(true);
        LABEL_GAMEOVER_SCORE.text = "Sua pontuação: <b>" + CurrentScore.ToString() + "</b>";
        Stop();
        SetSlowMo();
    }


    public float timeCountdown { get; set; }

    public void SaberBehaviour(Vector2 screenSaberPosition)
    {
        if (SettingsMaster.gamePaused)
            return;
        saberPosition = Camera.main.ScreenToWorldPoint(screenSaberPosition);
        Ray ray = Camera.main.ScreenPointToRay(screenSaberPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 999))
        {
            FruitNinjaFruit fruit = hit.collider.GetComponent<FruitNinjaFruit>();
            if (fruit != null)
            {
                fruit.Cut();
            }
        }
    }

    private void OnDisable()
    {
        Stop();
    }
    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }

        //Play();
    }
    private void Update()
    {
        if (SettingsMaster.gamePaused)
        {
            Time.timeScale = 0;
        }
        if (SettingsMaster.gamePaused)
        return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            SaberBehaviour(touch.position);
        } else
        {
            if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.01f)
            {
                SaberBehaviour(Input.mousePosition);
            }
        }


        timeCountdown -= Time.fixedDeltaTime;
        if (!SettingsMaster.gamePaused)
        {
            if (timeCountdown <= 0)
            {
                mouseTrail.time = 0.1f;
                Time.timeScale = 1;
                if (timeCountdown != 0)
                {
                    MusicFlow.main.Flow = 1;
                    timeCountdown = 0;
                    MusicFlow.main.lowSource.pitch = 1;
                }
            }
            else
            {
                Time.timeScale = 0.2f;
                mouseTrail.time = 9999;
                MusicFlow.main.Flow = 0;
                MusicFlow.main.lowSource.pitch = 0.5f;
            }
        }


        mouseTrail.transform.position = saberPosition;
        mouseTrail.transform.localPosition = new Vector3(mouseTrail.transform.localPosition.x, mouseTrail.transform.localPosition.y, -100);
    }
    private void FixedUpdate()
    {
        if (SettingsMaster.gamePaused)
            return;
        if (Input.mousePosition != mouseLastPos)
        {
            mouseSpeed = (Input.mousePosition - mouseLastPos) / Time.deltaTime;
            mouseLastPos = Input.mousePosition;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position != touchLastPos)
            {
                touchLastPos = touch.position;
                touchSpeed = (touch.position - touchLastPos) / Time.deltaTime;
            }
        }
    }
    private void LateUpdate()
    {
        if (SettingsMaster.gamePaused)
            return;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        LABEL_HEALTH.text = "";
        for (int i = 0; i < MaxHealth; i++)
        {
            if (i < CurrentHealth)
            {
                LABEL_HEALTH.text += "•";
            } else
            {
                LABEL_HEALTH.text += "";
            }
        }

        LABEL_SCORE.text = CurrentScore.ToString();
        LABEL_SCORE.color = Color.Lerp(LABEL_SCORE.color, LABEL_SCORE_COLOR, 3 * Time.deltaTime);
    }

    public void SetSlowMo()
    {
        timeCountdown = int.MaxValue;
    }

    public void ResetTime()
    {
        timeCountdown = 0;
        Time.timeScale = 1;
    }

    private void OnDrawGizmos()
    {
        foreach (var pos in FruitSpawnPositions)
        {
            Gizmos.DrawLine(pos.position, pos.position + pos.transform.up);
        }
    }
}
