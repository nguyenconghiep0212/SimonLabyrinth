using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStation : MonoBehaviour, EntityInterface
{
    private EntityInteractable baseClass;

    public void Interact()
    {
        baseClass.animator.SetTrigger("Save"); 
        baseClass.animator.SetBool("IsSaving", true); 
        SaveManager.Instance.Save();
        baseClass.animator.SetBool("IsSaving", false);
    }

    // Start is called before the first frame update
    void Awake()
    {
        baseClass = GetComponent<EntityInteractable>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
