using System;
using System.Collections;
using UnityEngine;
using TMPro;
public class IntroScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Intro Setting")]
  public string[] introTexts;
  public float testSpeed=0.05f;
  public float delayText=1f;
  public float textEraseSpeed=0.02f;
  public GameObject blackScreenImage;
  public AudioClip introAudio;
  [Header("Game Object")]
  public GameObject player;
  public GameObject canvas;
  private PlayerController playerController;
  private MeshRenderer playerMesh;
  private CameraController cameraController;
  private TextMeshProUGUI m_TextMeshPro;
  private AudioSource m_audioSource;
    private Animator anim;

    void Start()
    {

        blackScreenImage.SetActive(true);
        anim = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Animator>();
        m_TextMeshPro = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<TextMeshProUGUI>();
        cameraController = Camera.main.GetComponent<CameraController>();
       
        if (!player) player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        playerMesh = player.GetComponent<MeshRenderer>();
       
    //    m_TextMeshPro = canvas.GetComponentInChildren<TextMeshProUGUI>();
      // m_audioSource = Camera.main.GetComponent<AudioSource>();
        Debug.Log("Start");
       StartCoroutine(StartIntro());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator StartIntro()
    {
        Debug.Log("Start");
        playerController.StopMovement=true;
        cameraController.StopMovement=true;
        //anim.SetTrigger("Close",0,1f);
        anim.Play("Close",0,1f);
        yield return new WaitForSeconds(0.3f);
        for(int index=0;index<introTexts.Length-1;index++)
        {
            string text=introTexts[index];
             for (int i = 0; i < text.Length; i++)
            {
                m_TextMeshPro.text += text[i];
                yield return new WaitForSeconds(0.05f);
            }


            yield return new WaitForSeconds(1f);

            while (m_TextMeshPro.text.Length > 0)
            {
                m_TextMeshPro.text = m_TextMeshPro.text.Substring(0, m_TextMeshPro.text.Length - 1);
                yield return new WaitForSeconds(0.02f);
            }
        }
        blackScreenImage.SetActive(false);
        anim.SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        playerController.StopMovement = false;
        cameraController.StopMovement = false;
     string lastText = introTexts[introTexts.Length - 1];
        
        for (int i = 0; i < lastText.Length; i++)
        {
            m_TextMeshPro.text += lastText[i];
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        while (m_TextMeshPro.text.Length > 0)
        {
            m_TextMeshPro.text = m_TextMeshPro.text.Substring(0, m_TextMeshPro.text.Length - 1);
            yield return new WaitForSeconds(0.02f);
        }
        
        enabled=false;
    }
}
