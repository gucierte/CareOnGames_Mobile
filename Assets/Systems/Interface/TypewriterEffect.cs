using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [TextArea()]
    public string text;
    public TextMeshProUGUI target;
    int currentChar;
    public float delay = 0.1f;

    public void WriteNewChar()
    {
        target.text = text.Substring(0, currentChar);
        currentChar = Mathf.Clamp(currentChar + 1, 0, text.Length);
    }

    private void Start()
    {
        InvokeRepeating("WriteNewChar", 0, delay);
    }
}
