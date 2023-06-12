using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    InventoryController inventoryController;// 인벤토리 컨트롤러 객체
    ItemGrid itemGrid;// 아이템 그리드 객체

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;// 인벤토리 컨트롤러를 찾아서 할당합니다.
        itemGrid = GetComponent<ItemGrid>();// ItemGrid 컴포넌트를 가져옵니다.
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;// 마우스가 그리드 위에 올라갔을 때, 인벤토리 컨트롤러에 현재 그리드를 선택한 그리드로 설정합니다.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;// 마우스가 그리드를 벗어났을 때, 인벤토리 컨트롤러에 선택한 그리드를 null로 설정하여 선택을 해제합니다.
    }

}
