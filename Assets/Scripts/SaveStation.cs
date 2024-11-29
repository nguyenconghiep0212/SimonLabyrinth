using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStation : MonoBehaviour, EntityInterface
{
    private EntityInteractable baseClass;

    public void Interact()
    {
        SaveManager.Instance.Save();
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
