using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityInteractable : MonoBehaviour
{
    [SerializeField] float detectionRange = 2f;
    [SerializeField] Transform hintPosition;
    [SerializeField] string hint;

    internal Animator animator;
    internal GameObject hintUI;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Close");
            RemoveHint();
        }
    }
     
    public void DisplayHint()
    {
        hintUI = GameObject.Instantiate(GameManager.Instance.InteractHintUI, GameManager.Instance.CanvasUI.transform);
        hintUI.GetComponent<InteractHintUI>().target = hintPosition;
        hintUI.GetComponent<TextMeshProUGUI>().text = hint;
    }

    private void RemoveHint()
    {
        Destroy(hintUI);
    } 
}
