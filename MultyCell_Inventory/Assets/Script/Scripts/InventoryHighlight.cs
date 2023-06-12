using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField]
    RectTransform highlighter;//������ ���� Ŀ���ö󰥽� ����ȿ��������Ʈ�� RectTransform

    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);// ���� ȿ���� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�Ͽ� �����ݴϴ�.
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth; // ���� ȿ���� �ʺ� Ÿ�� �������� �ʺ� �׸��� Ÿ�� ũ�⸦ ���� ������ �����մϴ�.
        size.y = targetItem.HEIGHT * ItemGrid.tileSizeHeight;// ���� ȿ���� ���̸� Ÿ�� �������� ���̿� �׸��� Ÿ�� ũ�⸦ ���� ������ �����մϴ�.
        highlighter.sizeDelta = size;// ���� ȿ���� ����� �����մϴ�.
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        SetParent(targetGrid);// ���� ȿ���� �θ� Ÿ�� �׸���� �����մϴ�.

        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);// Ÿ�� �׸��忡�� Ÿ�� �������� ��ġ�� ����մϴ�.
        highlighter.localPosition = pos;// ���� ȿ���� ���� ��ġ�� �����մϴ�.
    }

    public void SetParent(ItemGrid targetGrid)
    {
        if(targetGrid == null)
        {
            return;
        }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());// ���� ȿ���� �θ� Ÿ�� �׸����� RectTransform���� �����մϴ�.
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);// Ÿ�� �׸��忡�� ������ ��ġ(posX, posY)�� ���� Ÿ�� �������� ��ġ�� ����մϴ�.
        highlighter.localPosition = pos;// ���� ȿ���� ���� ��ġ�� �����մϴ�.
    }
}
