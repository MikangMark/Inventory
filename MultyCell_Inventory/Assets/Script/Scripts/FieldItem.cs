using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [SerializeField]
    ItemIndex index_sc;
    [SerializeField]
    InventoryController inventoryController;
    // Start is called before the first frame update
    void Start()
    {
        inventoryController = GameObject.Find("Main Camera").GetComponent<InventoryController>();
    }
    public void OnClickBtn()
    {
        //inventoryController.CreateItem()
    }
}
