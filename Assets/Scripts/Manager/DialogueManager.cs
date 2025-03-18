using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText,nameText;
    
    public Image Avatar;
    public List<Image> avatarImages = new List<Image>();
    public int currentAvatar;
    
    [TextArea(1, 3)]
    public string[] dialogueLines;
    [SerializeField] private int currentLine;

    private Coroutine typingCoroutine;
    public float waitingSeconds=0.05f;
    private bool isTyping;

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        CheckName();
        StartTyping(dialogueLines[currentLine]);
    }
    
    void Update()
    {
        if(dialogueBox.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (isTyping)
                {
                    CompleteLine();
                    return;
                }
                currentLine++;

                if (currentLine < dialogueLines.Length)
                {
                    CheckName();
                   StartTyping(dialogueLines[currentLine]);
                }
                else
                {
                    dialogueBox.SetActive(false);
                }
            }
        }
    }

    public void ShownDialogue(string[] _newLines)
    {
        dialogueLines = _newLines;
        currentLine = 0;
        
        CheckName();
        
        dialogueText.text = dialogueLines[currentLine];
        dialogueBox.SetActive(true);
    }

    private void CheckName()
    {
        if(dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Replace("n-","");
            /*Avatar.sprite =avatarImages[currentAvatar].sprite;
            currentAvatar++;*/
            currentLine++;
        }
    }
    
    public void StartTyping(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine =StartCoroutine(TypeLine(text));
    }
    
    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        dialogueText.text = text;
        dialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i <= text.Length; i++)
        {
            dialogueText.maxVisibleCharacters=i;//NOTE::逐渐增加可见字符
            yield return new WaitForSeconds(waitingSeconds);
        }
        
        isTyping = false;
    }
    
    public void CompleteLine()
    {
        if (typingCoroutine != null)
        {
            //NOTE::若正在打字则终止协程
            StopCoroutine(typingCoroutine);
        }
        //NOTE::直接输出完整句子
        dialogueText.maxVisibleCharacters =dialogueText.text.Length;
        isTyping = false;
    }
    
    public bool IsTyping()
    {
        return isTyping;
    }
}
