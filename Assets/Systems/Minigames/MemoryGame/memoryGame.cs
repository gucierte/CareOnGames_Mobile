using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class memoryGame : MonoBehaviour
{
    static memoryGame instance;

    [Header("CardInfo")]
    public GameObject CARDINFO_CONTENT;
    public Image CARDINFO_IMAGE;
    public TextMeshProUGUI CARDINFO_LABEL;
    public TextMeshProUGUI CARDINFO_DESC;
    public OpenURL CARDINFO_BTN;
    [Space]
    public GameObject FINISH_CONTENT;
    public GameObject GAMEOVER_CONTENT;
    public TextMeshProUGUI FINISH_TIME_LABEL;
    public Slider COUNTDOWN_BAR;
    bool CanHideCard;

    public void ShowCardInfo(memoryGameCard card)
    {
        CanHideCard = false;
        CARDINFO_IMAGE.sprite = card.front[card.CardMainSprite].sprite;
        CARDINFO_IMAGE.color = card.front[card.CardMainSprite].color;
        CARDINFO_DESC.text = card.CardDesc;
        CARDINFO_LABEL.text = card.CardName;
        //MusicFlow.main.Flow = 2;
        Invoke(nameof(CanHideCardInfo), 1);
        CARDINFO_CONTENT.SetActive(true);
        CARDINFO_BTN.defaultURL = card.URL;
        //DelayActivation.SetActive(1, CARDINFO_CONTENT);
        //Invoke(nameof(HideCardInfo), 8);
    }

    void CanHideCardInfo()
    {
        CanHideCard = true;
    }

    public void HideCardInfo()
    {
        CARDINFO_CONTENT.SetActive(false);

        FINISH_CONTENT.SetActive(isFinished);
        //MusicFlow.main.Flow = 1;
        TimeSpan time = TimeSpan.FromSeconds(gameTime);
        FINISH_TIME_LABEL.text = "Seu tempo: <b>" + time.ToString(@"mm\:ss") + "</b>";
    }

    public static memoryGame main
    {
        get
        {
            if (!instance)
            {
                instance = FindFirstObjectByType<memoryGame>();
            }
            return instance;
        }
    }

    public Animator anim;
    public List<Transform> positions = new List<Transform>();
    public List<memoryGameCard> cards = new List<memoryGameCard>();
    public bool get;
    public bool SetParentCards;
    public static bool Ready;
    public static bool Shuffled;
    [Space]
    public AudioSource ClockTickSound;
    public AudioSource DoneSound;
    public AudioClip Music_Low;
    public AudioClip Music_High;
    public bool isPlaying { get; set; }



    void FlipAll()
    {
        foreach (var c in cards)
        {
            c.Flip();
        }
    }

    public void Shuffle()
    {
        List<Transform> tmp = positions.OrderBy(x => Guid.NewGuid()).ToList();
        positions = tmp;

        for (int i = 0; i < positions.Count; i++)
        {
            cards[i].transform.parent = positions[i];
            gameTime = 0;
        }
        foreach (var c in cards)
        {
            c.btn.interactable = true;
        }
        Shuffled = true;
    }
    public static float gameTime;
    public float countdown { get; set; }
    public float StartTime = 78;

    public void Play()
    {
        isPlaying = true;
        this.gameObject.SetActive(true);
        Debug.Log("Play");
        anim.SetBool("Start", true);
        //sceneAnim.gameObject.SetActive(true);
        foreach (var c in cards)
        {
            c.btn.interactable = false;
            c.UnFlip();
            c.Done = false;
            c.flipped = false;
        }
        Shuffled = false;
        Ready = false;
        gameTime = 0;
        isFinished = false;
        HideCardInfo();
        MusicFlow.main.SetMusic(Music_High, Music_Low, .5f);
    }

    public void Stop()
    {
        Debug.Log("Stop");
        gameTime = 0;
        foreach (var item in cards)
        {
            item.UnFlip();
            item.Done = false;
            item.flipped = false;
        }
        anim.SetBool("Start", false);
        //sceneAnim.SetBool("Start", false);
        Shuffled = false;
        Ready = false;
        isPlaying = false;
        isFinished = false;
        Invoke(nameof(DisableThisObject), 3);
    }

    public void DisableThisObject()
    {
        this.gameObject.SetActive(false);
    }

    public void Replay()
    {
        //anim.enabled = false;
        gameTime = 0;
        foreach (var item in cards)
        {
            item.UnFlip();
            item.Done = false;
            item.flipped = false;
        }
        anim.SetBool("Start", false);
        Shuffled = false;
        Ready = false;

        Invoke(nameof(Play), .3f);
        //anim.enabled = true;
    }

    public void CheckFinish()
    {
        bool finished = true;
        foreach (var item in cards)
        {
            if (!item.Done)
            {
                finished = false;
            }
        }

        if (finished == true)
        {
            FinishGame();
        }
    }
    bool isFinished;
    public void FinishGame()
    {
        foreach (var item in cards)
        {
            item.anim.SetBool("Done", false);
        }
        countdown += 10;
        DoneSound.PlayDelayed(1.5f);
        //Invoke(nameof(Replay), 3);
        isFinished = true;
    }


    //Mono
    private void FixedUpdate()
    {
        if (countdown < 0)
        {
            GAMEOVER_CONTENT.gameObject.SetActive(true);
        }
        else
        {
            if (!CARDINFO_CONTENT.activeInHierarchy && CanHideCard && !isFinished)
            {
                GAMEOVER_CONTENT.gameObject.SetActive(false);
                countdown -= Time.fixedDeltaTime;
                COUNTDOWN_BAR.value = countdown / StartTime;
            }
            Color redColor;
            ColorUtility.TryParseHtmlString("#cc525a", out redColor);

            Color whiteColor;
            ColorUtility.TryParseHtmlString("#283840", out whiteColor);

            COUNTDOWN_BAR.targetGraphic.color = Color.Lerp(redColor, whiteColor, (countdown - 15) / 30);
            ClockTickSound.volume = Mathf.Lerp(SettingsMaster.sfxVolume / 2, 0, countdown / 20);
            MusicFlow.main.Flow = Mathf.Lerp(0, 1, countdown);
        }
        ClockTickSound.enabled = !GAMEOVER_CONTENT.activeInHierarchy;


        if (Shuffled)
        {
            if (!isFinished)
            {
                gameTime += Time.fixedDeltaTime;
            }
        }
        else
        {
            gameTime = 0;
        }
        anim.SetBool("IsMobile", false);
        //player.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        countdown = Mathf.Clamp(StartTime + 10, 30, StartTime);
    }
    public void OnDrawGizmos()
    {
        if (get)
        {
            cards = GetComponentsInChildren<memoryGameCard>().ToList();
            foreach (var c in cards)
            {
                positions.Add(c.transform.parent);
            }
            get = false;
        }

        if (SetParentCards)
        {
            foreach (var c in cards)
            {
                if (!c.ParentCard)
                {
                    c.GetParent();
                }
            }
            SetParentCards = false;
        }
    }
    private void Update()
    {
        Ready = Shuffled;

        if (CheatCode.CheatCheck("p,l,a,y,m,e,m,o,r,y"))
        {
            Play();
        }

        if (CheatCode.CheatCheck("s,t,o,p"))
        {
            Stop();
        }

        if (CheatCode.CheatCheck("f,l,i,p,a,l,l"))
        {
            FlipAll();
        }
    }
}
