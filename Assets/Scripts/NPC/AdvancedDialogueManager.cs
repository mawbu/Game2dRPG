using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedDialogueManager : MonoBehaviour
{
    // The NPC DIALOGUE we are currently stepping through
    private AdvancedDialogueSO currentConversation;
    private int stepNum;
    private bool dialogueActivated;

    // UI  References
    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private Image portrait;
    private TMP_Text dialogueText;

    private string currentSpedker;
    private Sprite currentPortrait;

    public ActorSO[] actorSO;


    private GameObject[] optionButton;
    private TMP_Text[] optionButtonText;
    private GameObject optionsPanel;

    [SerializeField]
    private float typingSpeed = 0.02f;
    private Coroutine typeWriteRoutine;
    private bool canCoutinueText = true;

    // Start is called before the first frame update
    void Start()
    {

        optionButton = GameObject.FindGameObjectsWithTag("OptionButton");
        optionsPanel = GameObject.Find("OptionsPanel");
        optionsPanel.SetActive(false);


        optionButtonText = new TMP_Text[optionButton.Length];
        for(int i = 0; i< optionButton.Length; i++)
            optionButtonText[i] = optionButton[i].GetComponentInChildren<TMP_Text>();

        for(int i = 0; i < optionButton.Length; i++)
            optionButton[i].SetActive(false);

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        actor = GameObject.Find("ActorText").GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait").GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TMP_Text>();

        dialogueCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueActivated && Input.GetButtonDown("Interact") && canCoutinueText)
        {
            if(stepNum >= currentConversation.actors.Length)
                TurnOffDialogue();
            else 
                PlayDialogue();   
        }
    }

    void PlayDialogue()
    {
        if(currentConversation.actors[stepNum] == DialogueActors.Random)
            SetActorInfo(false);

        else
            SetActorInfo(true);
        actor.text = currentSpedker;
        portrait.sprite = currentPortrait;

        // If there is a branch..
        if (currentConversation.actors[stepNum] == DialogueActors.Branch)
        {
            for (int i = 0; i < currentConversation.optionText.Length; i++)
            {
                if (currentConversation.optionText[i] == null)
                    optionButton[i].SetActive(false);
                else
                {
                    optionButtonText[i].text = currentConversation.optionText[i];
                    optionButton[i].SetActive(true);
                }

                //Step the first button to be auto-selected
                optionButton[0].GetComponent<Button>().Select();
            }
        }

        //
        if(typeWriteRoutine != null)
            StopCoroutine(typeWriteRoutine);

        if (stepNum < currentConversation.dialogue.Length)
            typeWriteRoutine = StartCoroutine(TypeWriteEffect(dialogueText.text = currentConversation.dialogue[stepNum]));
        else
            optionsPanel.SetActive(true);

        dialogueCanvas.SetActive(true);
        stepNum += 1;
    }

    void SetActorInfo(bool recurringCharacter)
    {
        if(recurringCharacter)
        {
            for(int i = 0; i < actorSO.Length; i++)
            {
                if(actorSO[i].name == currentConversation.actors[stepNum].ToString())
                {
                    currentSpedker = actorSO[i].actorName;
                    currentPortrait = actorSO[i].actorPortrait;
                }
            }
        }
        else
        {
            currentSpedker = currentConversation.ramdomActorName;
            currentPortrait = currentConversation.ramdomActorPortrait;
        }
    }

    public void Option(int optionNum)
    {
        foreach ( GameObject button in optionButton)
            button.SetActive(false);
        if(optionNum == 0)
            currentConversation = currentConversation.option0;
        if(optionNum == 1)
            currentConversation = currentConversation.option1;
        if(optionNum == 2)
            currentConversation = currentConversation.option2;
        if(optionNum == 3)
            currentConversation = currentConversation.option3;

        stepNum = 0;
    }

    private IEnumerator TypeWriteEffect(string line)
    {
        dialogueText.text = "";
        canCoutinueText = false;
        yield return new WaitForSeconds(.5f);
        foreach(char letter in line.ToCharArray())
        {
            if(Input.GetButtonDown("Interact"))
            {
                dialogueText.text = line;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            canCoutinueText = true;
        }
    }
    public void InitiateDialogue(NPCDialogue npcDIalogue)
    {   
        currentConversation = npcDIalogue.conversation[0];
        
        dialogueActivated = true;
        
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;

        dialogueActivated = false;
        optionsPanel.SetActive(false);
        dialogueCanvas.SetActive(false);
    }
}

public enum DialogueActors
{
    Mabu,
    Milo,
    Mile,
    Random,
    Branch
};
