using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffInventory : MonoBehaviour
{
    public GameObject inventoryCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryCanvas.activeSelf)
            {
                inventoryCanvas.SetActive(false);
            }
            else
            {
                inventoryCanvas.SetActive(true);
            }
        }
    }
    public void OnClickInventoryView()
    {
        if (inventoryCanvas.activeSelf)
        {
            inventoryCanvas.SetActive(false);
        }
        else
        {
            inventoryCanvas.SetActive(true);
        }
    }
}
