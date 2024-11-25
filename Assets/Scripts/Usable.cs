using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{

    [SerializeField] GameManager.UsableType usableType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (usableType)
            {
                case GameManager.UsableType.medbag:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().medbag++;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().medbagUI.text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().medbag.ToString();
                    break;
                case GameManager.UsableType.battery:
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().battery++;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().batteryUI.text = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().battery.ToString();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
