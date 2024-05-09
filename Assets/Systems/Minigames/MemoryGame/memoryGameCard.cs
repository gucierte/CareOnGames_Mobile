using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a card in the memory game.
/// </summary>
public class memoryGameCard : MonoBehaviour
{
    public Button btn; // Type annotation: Button btn
    [Tooltip("The button component for this card")]
    public Animator anim; // Type annotation: Animator anim
    [Tooltip("The animator component for this card")]
    public static memoryGameCard lastFlippedCard; // Type annotation: static memoryGameCard lastFlippedCard
    [Tooltip("The last flipped card")]

    public List<Image> front; // Type annotation: List<Image> front
    [Tooltip("The list of images for the front of the card")]
    public Renderer back; // Type annotation: Renderer back
    [Tooltip("The renderer for the back of the card")]
    [Space]
    public List<Sprite> FrontTextures; // Type annotation: List<Sprite> FrontTextures
    [Tooltip("The list of textures for the front of the card")]
    public Color basicColour = new Color(1, 1, 1, 1);
    [Tooltip("The basic color for the card")]
    public bool flipped;
    [Tooltip("Indicates if the card is flipped")]
    public memoryGameCard ParentCard; // Type annotation: memoryGameCard ParentCard
    [Tooltip("The parent card of this card")]
    [Header("SFXs")]
    public AudioSource SFX_Fail; // Type annotation: AudioSource SFX_Fail
    [Tooltip("The audio source for fail sound effect")]
    public bool Done;
    [Tooltip("Indicates if the action is done")]
    public bool getParentCard;
    [Tooltip("Indicates if the card needs to get the parent card")]
    [Space]
    public string CardName;
    [Tooltip("The name of the card")]
    [TextArea] public string CardDesc;
    [Tooltip("The url of card")]
    public string URL;
    [Tooltip("The description of the card")]
    public int CardMainSprite;
    [Tooltip("The main sprite of the card")]

    /// <summary>
    /// Gets the parent card of the current card.
    /// </summary>
    public void GetParent()
    {
        if (!ParentCard)
        {
            ParentCard = memoryGame.main.cards[memoryGame.main.cards.IndexOf(this) + 1];
            ParentCard.ParentCard = this;
        }
    }
    /// <summary>
    /// Handles the hover behavior of the card.
    /// </summary>
    void Hover()
    {
        CancelInvoke(nameof(HoverOff));
        anim.SetBool("Hover", true);
        Invoke(nameof(HoverOff), Time.deltaTime + Time.fixedDeltaTime);
    }
    /// <summary>
    /// Handles the hover off behavior of the card.
    /// </summary>
    void HoverOff()
    {
        anim.SetBool("Hover", false);
    }
    /// <summary>
    /// Handles the interaction behavior of the card.
    /// </summary>
    public void Interact()
    {
        if (!flipped)
        {
            Flip();
        }
        else
        {
            UnFlip();
        }
    }
    /// <summary>
    /// Flips the card and performs associated actions.
    /// </summary>
    public void Flip()
    {
        MusicFlow.main.Flow += .1f;
        if (memoryGame.gameTime < 5)
        {
            memoryGame.gameTime = 5;
        }
        if (lastFlippedCard == ParentCard)
        {
            MusicFlow.main.Flow += .3f;
            btn.interactable = false;
            ParentCard.btn.interactable = false;
            lastFlippedCard = null;
            anim.SetBool("Done", true);
            ParentCard.anim.SetBool("Done", true);
            ParentCard.Done = true;
            memoryGame.main.ShowCardInfo(this);
            Done = true;
        }
        else
        {
            if (lastFlippedCard)
            {
                lastFlippedCard.UnFlip();
            }
            lastFlippedCard = this;
            SFX_Fail.Play();
        }

        memoryGame.main.CheckFinish();

        flipped = true;
    }
    /// <summary>
    /// Reverses the flip action of the card.
    /// </summary>
    public void UnFlip()
    {
        flipped = false;
        Done = false;
        if (lastFlippedCard == this)
        {
            anim.SetBool("Done", false);
            lastFlippedCard = null;
        }
    }


    //Mono
    /// <summary>
    /// Initializes the front of the card with textures and colors, sets up the parent card, and gets the Animator component.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < front.Count; i++)
        {
            // Set front texture and color
            front[i].sprite = FrontTextures[i];
            front[i].color = basicColour;

            if (ParentCard)
            {
                // Set up the parent card
                ParentCard.ParentCard = this;
                ParentCard.basicColour = basicColour;
                ParentCard.FrontTextures[i] = FrontTextures[i];
                ParentCard.front[i].color = front[i].color;

                // Set card name, description, and main sprite on the parent card
                ParentCard.CardName = CardName;
                ParentCard.CardDesc = CardDesc;
                ParentCard.CardMainSprite = CardMainSprite;
            }

            // Get the Animator component
            anim = GetComponent<Animator>();
        }
    }
    /// <summary>
    /// Updates the rotation and position of the card, and sets the button interactability based on game state.
    /// </summary>
    private void FixedUpdate()
    {
        Vector3 euler = Vector3.zero;
        if (ParentCard)
        {
            ParentCard.ParentCard = this;
        }

        if (memoryGame.Ready && memoryGame.gameTime >= 5)
        {
            if (!flipped)
            {
                euler = new Vector3(0, 180, 0);
            }
        }

        // Update position and rotation
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 3 * Time.deltaTime);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, euler, 5 * Time.deltaTime);

        // Set button interactability based on game state
        btn.interactable = memoryGame.Shuffled && !Done;
    }
    /// <summary>
    /// Draws gizmos to visually represent the card and its parent card.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (ParentCard)
        {
            ParentCard.ParentCard = this;
            ParentCard.basicColour = basicColour;
            for (int i = 0; i < front.Count; i++)
            {
                // Set front texture and color on parent card
                front[i].color = basicColour;
                front[i].sprite = FrontTextures[i];

                ParentCard.FrontTextures[i] = FrontTextures[i];
                ParentCard.front[i].sprite = front[i].sprite;
                ParentCard.front[i].color = front[i].color;

                ParentCard.CardName = CardName;
                ParentCard.CardDesc = CardDesc;
                ParentCard.CardMainSprite = CardMainSprite;
            }
        }
    }
    /// <summary>
    /// Performs validation checks and setups when the script is edited in the Unity Editor.
    /// </summary>
    private void OnValidate()
    {
        // Return if the application is playing
        if (Application.isPlaying)
            return;

        if (ParentCard)
        {
            ParentCard.ParentCard = this;
        }

        if (getParentCard)
        {
            GetParent();
#if UNITY_EDITOR
            // Ping the parent card in the Unity Editor
            UnityEditor.EditorGUIUtility.PingObject(ParentCard);
#endif
            ParentCard.ParentCard = this;
            getParentCard = false;
        }

        if (ParentCard)
        {
            ParentCard.basicColour = basicColour;
            for (int i = 0; i < front.Count; i++)
            {
                // Set front texture and color on parent card
                front[i].color = basicColour;
                front[i].sprite = FrontTextures[i];

                ParentCard.FrontTextures[i] = FrontTextures[i];
                ParentCard.front[i].sprite = front[i].sprite;
                ParentCard.front[i].color = front[i].color;

                ParentCard.CardName = CardName;
                ParentCard.CardDesc = CardDesc;
                ParentCard.CardMainSprite = CardMainSprite;
                ParentCard.URL = URL;
            }
        }
    }
}
