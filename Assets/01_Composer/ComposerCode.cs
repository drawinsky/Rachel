using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ComposerCode : MonoBehaviour
{
    public AudioSource c4Ado, c4SAdo, d4Ado, d4SAdo, e4Ado, f4Ado, f4SAdo, g4Ado, g4SAdo, a5Ado, a5SAdo, b5Ado;
    public List<AudioSource> audioList;
    public Button c4Btn, c4SBtn, d4Btn, d4SBtn, e4Btn, f4Btn, f4SBtn, g4Btn, g4SBtn, a5Btn, a5SBtn, b5Btn,
                  c3Btn, c3SBtn, d3Btn, d3SBtn, e3Btn, f3Btn, f3SBtn, g3Btn, g3SBtn, a4Btn, a4SBtn, b4Btn,
                  c5Btn, c5SBtn, d5Btn, d5SBtn, e5Btn, f5Btn, f5SBtn, g5Btn, g5SBtn, a6Btn, a6SBtn, b6Btn;
    public List<Button> btnList, btnLowList, btnHighList;
    public TextMeshProUGUI noteTxt;
    public Button removeBtn;
    public Button playBtn;

    private List<KeyCode> btnKeyCode;
    private List<AudioSource> compositionAudioList;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the audioList and btnList
        audioList = new List<AudioSource> { c4Ado, c4SAdo, d4Ado, d4SAdo, e4Ado, f4Ado, f4SAdo, g4Ado, g4SAdo, a5Ado, a5SAdo, b5Ado };
        btnList = new List<Button> { c4Btn, c4SBtn, d4Btn, d4SBtn, e4Btn, f4Btn, f4SBtn, g4Btn, g4SBtn, a5Btn, a5SBtn, b5Btn };
        btnLowList = new List<Button> { c3Btn, c3SBtn, d3Btn, d3SBtn, e3Btn, f3Btn, f3SBtn, g3Btn, g3SBtn, a4Btn, a4SBtn, b4Btn };
        btnHighList = new List<Button> { c5Btn, c5SBtn, d5Btn, d5SBtn, e5Btn, f5Btn, f5SBtn, g5Btn, g5SBtn, a6Btn, a6SBtn, b6Btn };
        btnKeyCode = new List<KeyCode> { KeyCode.Q, KeyCode.Alpha2, KeyCode.W, KeyCode.Alpha3, KeyCode.E, KeyCode.R,
                                         KeyCode.Alpha5, KeyCode.T, KeyCode.Alpha6, KeyCode.Y, KeyCode.Alpha7, KeyCode.U};

        // Initialize the compositionAudioList
        compositionAudioList = new List<AudioSource>();


        // Add event listeners to the buttons
        for (int i = 0; i < btnList.Count; i++)
        {
            int index = i;
            btnList[i].onClick.AddListener(() => AddNote(index));
            btnLowList[i].onClick.AddListener(() => AddNote(index));
            btnHighList[i].onClick.AddListener(() => AddNote(index));
        }

        removeBtn.onClick.AddListener(RemoveComposition);
        playBtn.onClick.AddListener(PlayComposition);
    }

    // Add note to the compositionAudioList
    public void AddNote(int index)
    {
        audioList[index].Play();
        compositionAudioList.Add(audioList[index]);
        noteTxt.text += btnList[index].name + " ";
        //Debug.Log(audioList[index].name);
    }
    public void RemoveComposition()
    {
        if (compositionAudioList.Count > 0)
        {
            Debug.Log("RemoveNote");
            //compositionAudioList.Clear();
            noteTxt.text = "Composition: ";
            for (int i = compositionAudioList.Count; i > 0; i--)
            {
                compositionAudioList.RemoveAt(compositionAudioList.Count - 1);
            }
        }
    }

    public void PlayComposition()
    {
        Debug.Log("PlayNote");
        StartCoroutine(PlaySong());
        
    }

    private IEnumerator PlaySong()
    {
        if (compositionAudioList.Count > 0)
        {
            for (int i = 0; i < compositionAudioList.Count; i++)
            {
                compositionAudioList[i].Play();
                // Delay until the note is done playing.
                while (compositionAudioList[i].isPlaying == true)
                {
                    // This waits one frame IF AND ONLY IF this function is a "coroutine".
                    yield return null;
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // Add note when press the keyboard
        for (int i = 0; i < btnList.Count; i++)
        {
            if (Input.GetKeyDown(btnKeyCode[i]))
            {
                AddNote(i);
                Debug.Log("Play " + audioList[i]);
            }
        }
        
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
            //AddNote(0);
            //c4Ado.Play();
            //Debug.Log("Play c4Ado");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
            //AddNote(1);
            //Debug.Log("Play c4SAdo");
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
            //AddNote(2);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
            //AddNote(3);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
            //AddNote(4);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
            //AddNote(5);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
            //AddNote(6);
        //}
        //if (Input.GetKeyDown(KeyCode.T))
        //{
            //AddNote(7);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
            //AddNote(8);
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
            //AddNote(9);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
            //AddNote(10);
        //}
        //if (Input.GetKeyDown(KeyCode.U))
        //{
            //AddNote(11);
        //}
        //if (Keyboard.current.qKey.wasPressedThisFrame)
        //{
            //AddNote(11);
        //}
    }

    
}
