using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheatCode : MonoBehaviour
{
    public static string pressedKeys;
    public static List<string> pressedKeysList = new List<string>();

    [System.Serializable]
    public class cheat
    {
        public string cheatCode;
        public Button.ButtonClickedEvent Event;
    }
    [SerializeField]
    public List<cheat> customCheats = new List<cheat>();

    public void OnGUI()
    {
        GUILayout.Label("<b>[Pressed keys]: </b>" + pressedKeys);
    }

    /// <summary>
    /// Check a code (put the keys separated by commas: ",")
    /// </summary>
    /// <param name="cheat"></param>
    /// <returns></returns>
    public static bool CheatCheck(string cheat)
    {
        bool r = false;
        if (pressedKeys.Replace(" ","").Contains(cheat.ToUpper()))
        {
            r = true;
            Debug.Log($"[Cheat Code] Cheat: '{cheat.ToUpper()}' activated!");
            pressedKeys += "\n<b>CHEAT ACTIVATED!</b>\n";
            pressedKeys = "";
            pressedKeysList.Clear();
        }
        return r;
    }
    public void Update()
    {
        var allKeys = System.Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>();
        foreach (var key in allKeys)
        {
            if (Input.GetKeyDown(key))
            {
                pressedKeysList.Add(key.ToString());
            }
        }

        foreach (var c in customCheats)
        {
            if (CheatCheck(c.cheatCode))
            {
                c.Event.Invoke();
            }
        }
    }

    private void LateUpdate()
    {
        pressedKeys = "";
        foreach (var k in pressedKeysList)
        {
            pressedKeys += k + " ,";
        }
    }

    private void Awake()
    {
        pressedKeys = "";
        pressedKeysList = new List<string>();
    }

    private void OnApplicationQuit()
    {
        pressedKeys = "";
    }
}
