using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlidersGame : MinigameMaster
{
    [Header("Labels")]
    public TextMeshProUGUI QUESTION_LABEL;
    public TextMeshProUGUI REPLY1_LABEL;
    public TextMeshProUGUI REPLY2_LABEL;
    public TextMeshProUGUI REPLY3_LABEL;
    [Space]
    public GameObject FINISH_MENU;
    public TextMeshProUGUI FINISH_LABEL;
    public TextMeshProUGUI FINISH_TEXT_LABEL;
    public SkinnedMeshRenderer FINISH_GRAPHIC;
    [Space]
    public TextMeshProUGUI FAMILY_LABEL_FINISH;
    public TextMeshProUGUI HEALTH_LABEL_FINISH;
    public TextMeshProUGUI MONEY_LABEL_FINISH;
    public TextMeshProUGUI STATUS_LABEL_FINISH;
    [Space]
    public Slider FAMILY_SLIDER;
    public Slider HEALTH_SLIDER;
    public Slider MONEY_SLIDER;
    public Slider STATUS_SLIDER;

    [Space]
    public static int FamilyPts;
    public static int HealthPts;
    public static int MoneyPts;
    public static int StatusPts;

    [System.Serializable]
    public class reply
    {
        [TextArea]
        public string text;
        [Space]
        public int FamilyPts;
        public int HealthPts;
        public int MoneyPts;
        public int StatusPts;
    }

    [System.Serializable]
    public class Question
    {
        [TextArea()]
        public string question;
        [Space]
        [SerializeField] public reply ReplyA;
        [SerializeField] public reply ReplyB;
        [SerializeField] public reply ReplyC;
    }
    [SerializeField] public List<Question> Questions = new List<Question>();
    public List<Question> finalQuestions = new List<Question>();
    public static int currentQuestionIndex { get; set; }
    public void UpdateLabels(Question q)
    {
        QUESTION_LABEL.text = $"[{finalQuestions.IndexOf(q)}/{finalQuestions.Count}] " + q.question;
        REPLY1_LABEL.text = q.ReplyA.text;
        REPLY2_LABEL.text = q.ReplyB.text;
        REPLY3_LABEL.text = q.ReplyC.text;

        REPLY1_LABEL.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(REPLY1_LABEL.text));
        REPLY2_LABEL.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(REPLY2_LABEL.text));
        REPLY3_LABEL.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(REPLY3_LABEL.text));
    }
    public void ShuffleQuestions()
    {
        finalQuestions.Shuffle();
    }
    public void Reply(int q)
    {
        if (q == 0)
        {
            FamilyPts += finalQuestions[currentQuestionIndex].ReplyA.FamilyPts;
            HealthPts += finalQuestions[currentQuestionIndex].ReplyA.HealthPts;
            MoneyPts += finalQuestions[currentQuestionIndex].ReplyA.MoneyPts;
            StatusPts += finalQuestions[currentQuestionIndex].ReplyA.StatusPts;
        }

        if (q == 1)
        {
            FamilyPts += finalQuestions[currentQuestionIndex].ReplyB.FamilyPts;
            HealthPts += finalQuestions[currentQuestionIndex].ReplyB.HealthPts;
            MoneyPts += finalQuestions[currentQuestionIndex].ReplyB.MoneyPts;
            StatusPts += finalQuestions[currentQuestionIndex].ReplyB.StatusPts;
        }

        if (q == 2)
        {
            FamilyPts += finalQuestions[currentQuestionIndex].ReplyC.FamilyPts;
            HealthPts += finalQuestions[currentQuestionIndex].ReplyC.HealthPts;
            MoneyPts += finalQuestions[currentQuestionIndex].ReplyC.MoneyPts;
            StatusPts += finalQuestions[currentQuestionIndex].ReplyC.StatusPts;
        }

        currentQuestionIndex += 1;

        FamilyPts = Mathf.Clamp(FamilyPts, 0, 100);
        HealthPts = Mathf.Clamp(HealthPts, 0, 100);
        MoneyPts = Mathf.Clamp(MoneyPts, 0, 100);
        StatusPts = Mathf.Clamp(StatusPts, 0, 100);

        Sound.Play("SFX/beep-05");


        CheckFinish();
        UpdateLabels(finalQuestions[Mathf.Clamp(currentQuestionIndex, 0, finalQuestions.Count - 1)]);
    }
    void CheckFinish()
    {
        if (currentQuestionIndex >= finalQuestions.Count)
        {
            Finish();
        }
    }

    /// <summary>
    /// Returns the name of the most pointed (Family | Health | Money | Status)
    /// </summary>
    /// <returns></returns>
    public string GetMostPointed()
    {
        string r = "null";
        if (FamilyPts >= HealthPts && FamilyPts >= MoneyPts && FamilyPts >= StatusPts)
        {
            r = "Family";
        }

        if (HealthPts >= FamilyPts && HealthPts >= MoneyPts && HealthPts >= StatusPts)
        {
            r = "Health";
        }
        if (MoneyPts >= FamilyPts && MoneyPts >= HealthPts && MoneyPts >= StatusPts)
        {
            r = "Money";
        }
        if (StatusPts >= FamilyPts && StatusPts >= MoneyPts && StatusPts >= HealthPts)
        {
            r = "Status";
        }
        return r;
    }
    /// <summary>
    /// Returns the name of the lest pointed (Family | Health | Money | Status)
    /// </summary>
    /// <returns></returns>
    public string GetLessPointed()
    {
        string r = "null";
        if (FamilyPts < HealthPts && FamilyPts < MoneyPts && FamilyPts < StatusPts)
        {
            r = "Family";
        }
        if (HealthPts < FamilyPts && HealthPts < MoneyPts && HealthPts < StatusPts)
        {
            r = "Health";
        }
        if (MoneyPts < FamilyPts && MoneyPts < HealthPts && MoneyPts < StatusPts)
        {
            r = "Money";
        }
        if (StatusPts < FamilyPts && StatusPts < MoneyPts && StatusPts < HealthPts)
        {
            r = "Status";
        }
        return r;
    }

    public void SetGraphic(float Fill, float Family, float Health, float Money, float Status)
    {
        FINISH_GRAPHIC.SetBlendShapeWeight(0, Fill);
        FINISH_GRAPHIC.SetBlendShapeWeight(1, Family);
        FINISH_GRAPHIC.SetBlendShapeWeight(2, Health);
        FINISH_GRAPHIC.SetBlendShapeWeight(3, Money);
        FINISH_GRAPHIC.SetBlendShapeWeight(4, Status);
    }

    public void Finish()
    {
        string m = GetMostPointed();
        Debug.Log(m);
        string l = GetLessPointed();
        Debug.Log(l);
        string txt = "";

        //Most
        if(m == "Family")
        {
            txt = "Voc� gosta de passar tempo de qualidade com minha fam�lia e amigos, pois acredita que s�o essas conex�es que nos enriquecem verdadeiramente";
            FINISH_LABEL.text = "Fam�lia";
        }
        if (m == "Health")
        {
            txt = "A verdadeira riqueza � a sa�de. Voc� dedica tempo para cuidar do meu corpo e da mente, pois entende que um estilo de vida saud�vel � a chave para uma vida longa e feliz";
            FINISH_LABEL.text = "Sa�de";
        }
        if (m == "Money")
        {
            txt = "Voc� entende o valor do dinheiro, acredita que a seguran�a financeira � a chave para uma vida sem estresse";
            FINISH_LABEL.text = "Dinheiro";
        }
        if (m == "Status")
        {
            txt = "Para voc�, o prest�gio n�o � apenas sobre sucesso pessoal, mas tamb�m sobre deixar um legado positivo e inspirador para os outros";
            FINISH_LABEL.text = "Prest�gio";
        }

        //Less
        if (l == "Family")
        {
            txt += ". Nem sempre seu caminho est� alinhado com as expectativas da fam�lia.";
        }
        if (l == "Health")
        {
            txt += ". A vida � curta demais para se preocupar. Voc� prefere aproveitar cada momento ao m�ximo, sem se restringir.";
        }
        if (l == "Money")
        {
            txt += ". A vida n�o se resume a acumular riquezas, o dinheiro � apenas um meio.";
        }
        if (l == "Status")
        {
            txt += ". A verdadeira satisfa��o vem de ser aut�ntico e fiel a si mesmo, n�o de buscar a aprova��o dos outros.";
        }


        if (m == "null" && l == "null")
        {
            txt = "Sem dados";
            FINISH_TEXT_LABEL.text = "Voc� � um mist�rio";
        }

        FAMILY_LABEL_FINISH.text = $"Fam�lia\n<b>{FamilyPts}</b>";
        HEALTH_LABEL_FINISH.text = $"Sa�de\n<b>{HealthPts}</b>";
        MONEY_LABEL_FINISH.text = $"Dinheiro\n<b>{MoneyPts}</b>";
        STATUS_LABEL_FINISH.text = $"Prest�gio\n<b>{StatusPts}</b>";

        FINISH_TEXT_LABEL.text = txt;
        FINISH_MENU.SetActive(true);
    }
    public override void Play()
    {
        base.Play();
        finalQuestions = new List<Question>();
        for (int i = 0; i < Mathf.Clamp(Questions.Count, 0, 10); i++) { finalQuestions.Add(Questions[i]); }
        FamilyPts = 0;
        HealthPts = 0;
        MoneyPts = 0;
        StatusPts = 0;
        currentQuestionIndex = 0;
        UpdateLabels(finalQuestions[0]);
        SetGraphic(0, 0, 0, 0, 0);
        FINISH_MENU.SetActive(false);
    }

    private void OnEnable()
    {
        Play();
    }

    private void LateUpdate()
    {
        FAMILY_SLIDER.value = Mathf.Lerp(FAMILY_SLIDER.value, FamilyPts / 100.0f, 3 * Time.deltaTime);
        HEALTH_SLIDER.value = Mathf.Lerp(HEALTH_SLIDER.value, HealthPts / 100.0f, 3 * Time.deltaTime);
        MONEY_SLIDER.value = Mathf.Lerp(MONEY_SLIDER.value, MoneyPts / 100.0f, 3 * Time.deltaTime);
        STATUS_SLIDER.value = Mathf.Lerp(STATUS_SLIDER.value, StatusPts / 100.0f, 3 * Time.deltaTime);

        if (FINISH_GRAPHIC.gameObject.activeInHierarchy)
        {
            float lerp = 3;
            SetGraphic(0,
                Mathf.Lerp(FINISH_GRAPHIC.GetBlendShapeWeight(1), FamilyPts, lerp * Time.deltaTime),
                Mathf.Lerp(FINISH_GRAPHIC.GetBlendShapeWeight(2), HealthPts, lerp * Time.deltaTime),
                Mathf.Lerp(FINISH_GRAPHIC.GetBlendShapeWeight(3), MoneyPts, lerp * Time.deltaTime),
                Mathf.Lerp(FINISH_GRAPHIC.GetBlendShapeWeight(4), StatusPts, lerp * Time.deltaTime));
        }
    }
}
