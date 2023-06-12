using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField]
    RectTransform highlighter;//아이템 위에 커서올라갈시 강조효과오브젝트의 RectTransform

    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);// 강조 효과를 활성화 또는 비활성화하여 보여줍니다.
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth; // 강조 효과의 너비를 타겟 아이템의 너비에 그리드 타일 크기를 곱한 값으로 설정합니다.
        size.y = targetItem.HEIGHT * ItemGrid.tileSizeHeight;// 강조 효과의 높이를 타겟 아이템의 높이에 그리드 타일 크기를 곱한 값으로 설정합니다.
        highlighter.sizeDelta = size;// 강조 효과의 사이즈를 설정합니다.
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        SetParent(targetGrid);// 강조 효과의 부모를 타겟 그리드로 설정합니다.

        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);// 타겟 그리드에서 타겟 아이템의 위치를 계산합니다.
        highlighter.localPosition = pos;// 강조 효과의 로컬 위치를 설정합니다.
    }

    public void SetParent(ItemGrid targetGrid)
    {
        if(targetGrid == null)
        {
            return;
        }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());// 강조 효과의 부모를 타겟 그리드의 RectTransform으로 설정합니다.
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);// 타겟 그리드에서 지정된 위치(posX, posY)에 대한 타겟 아이템의 위치를 계산합니다.
        highlighter.localPosition = pos;// 강조 효과의 로컬 위치를 설정합니다.
    }
}
