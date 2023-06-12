using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    InventoryController inventoryController;// �κ��丮 ��Ʈ�ѷ� ��ü
    ItemGrid itemGrid;// ������ �׸��� ��ü

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;// �κ��丮 ��Ʈ�ѷ��� ã�Ƽ� �Ҵ��մϴ�.
        itemGrid = GetComponent<ItemGrid>();// ItemGrid ������Ʈ�� �����ɴϴ�.
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;// ���콺�� �׸��� ���� �ö��� ��, �κ��丮 ��Ʈ�ѷ��� ���� �׸��带 ������ �׸���� �����մϴ�.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;// ���콺�� �׸��带 ����� ��, �κ��丮 ��Ʈ�ѷ��� ������ �׸��带 null�� �����Ͽ� ������ �����մϴ�.
    }

}
