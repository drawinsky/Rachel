using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ComposerCode : MonoBehaviour
{
    public AudioSource c4Ado, c4SAdo, d4Ado, d4SAdo, e4Ado, f4Ado, f4SAdo, g4Ado, g4SAdo, a4Ado, a4SAdo, b4Ado;
    public List<AudioSource> audioList;
    public Button c4Btn, c4SBtn, d4Btn, d4SBtn, e4Btn, f4Btn, f4SBtn, g4Btn, g4SBtn, a4Btn, a4SBtn, b4Btn;
    public List<Button> btnList;
    public TextMeshProUGUI noteTxt;
    public Button removeBtn;
    public Button playBtn;

    private List<AudioSource> compositionAudioList;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the audioList and btnList
        audioList = new List<AudioSource> { c4Ado, c4SAdo, d4Ado, d4SAdo, e4Ado, f4Ado, f4SAdo, g4Ado, g4SAdo, a4Ado, a4SAdo, b4Ado };
        btnList = new List<Button> { c4Btn, c4SBtn, d4Btn, d4SBtn, e4Btn, f4Btn, f4SBtn, g4Btn, g4SBtn, a4Btn, a4SBtn, b4Btn };

        // Initialize the compositionAudioList
        compositionAudioList = new List<AudioSource>();


        // Add event listeners to the buttons
        for (int i = 0; i < btnList.Count; i++)
        {
            int index = i;
            btnList[i].onClick.AddListener(() => AddNote(index));
        }

        removeBtn.onClick.AddListener(RemoveComposition);
        playBtn.onClick.AddListener(PlayComposition);
    }

    // Add note to the compositionAudioList
    public void AddNote(int index)
    {
        audioList[index].Play();
        compositionAudioList.Add(audioList[index]);
        noteTxt.text += audioList[index].name + " ";
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            c4Ado.Play();
            //Debug.Log("Play c4Ado");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            d4Ado.Play();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            e4Ado.Play();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            f4Ado.Play();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            g4Ado.Play();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            a4Ado.Play();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            b4Ado.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            c4SAdo.Play();
            //Debug.Log("Play c4SAdo");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            d4SAdo.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            f4SAdo.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            g4SAdo.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            a4SAdo.Play();
        }
    }

    
}
