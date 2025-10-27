using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class TextTrigger : MonoBehaviour
{
    public string text;
    public float timeBet = 0.5f;
    public float duration;
    private Image img;
    private TextMeshProUGUI m_TextMeshPro;
    private MeshRenderer mr;
    public MeshRenderer[] optionalAdditionalMeshes;
    private BoxCollider bc;
    int index;

    bool done = false;


    private void Start()
    {
        img = GameObject.FindGameObjectWithTag("Panel").GetComponent<Image>();
        m_TextMeshPro = img.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        mr = gameObject.GetComponent<MeshRenderer>();
        bc = gameObject.GetComponent<BoxCollider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Collision Detected");
            TryLoad();
        }
    }

    public void TryLoad()
    {
        if (!done && m_TextMeshPro.text.Length == 0)
        {
            done = true;
            StartCoroutine(Load());
        }
    }
    

    IEnumerator Load()
    {
        DisableInteraction();
        Debug.Log("IEnumerator started");
        for(int i = 0; i < text.Length; i++)
        {
            m_TextMeshPro.text += text[i];
            yield return new WaitForSeconds(timeBet);
        }
        

        yield return new WaitForSeconds(duration);
        StartCoroutine(Unload());
    }

    IEnumerator Unload()
    {
        Debug.Log("Unload started");
        while (m_TextMeshPro.text.Length > 0)
        {
            m_TextMeshPro.text = m_TextMeshPro.text.Substring(0, m_TextMeshPro.text.Length - 1);
            yield return new WaitForSeconds(timeBet);
        }
        
    }

    private void DisableInteraction()
    {
        if(mr != null)
            mr.enabled = false;
        if(bc != null)
            bc.enabled = false;
        if (optionalAdditionalMeshes != null)
        {
            foreach (MeshRenderer mr in optionalAdditionalMeshes)
            {
                mr.enabled = false;
            }
        }
    }

}
