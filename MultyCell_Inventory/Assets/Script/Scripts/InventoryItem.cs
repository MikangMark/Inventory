using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;// ������ �����͸� �����ϴ� ����

    public int HEIGHT// �������� ���̸� ��ȯ�ϴ� �Ӽ�
    {
        get 
        {
            if(rotated == false)// ȸ������ ���� ���
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }
    public int WIDTH// �������� �ʺ� ��ȯ�ϴ� �Ӽ�
    {
        get
        {
            if (rotated == false)// ȸ������ ���� ���
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;// �׸��忡���� X ��ġ
    public int onGridPositionY;// �׸��忡���� Y ��ġ

    public bool rotated = false;// �������� ȸ���Ǿ����� ���θ� ��Ÿ���� ����

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;// ������ ������ ����
        GetComponent<Image>().sprite = itemData.invenIcon;// �̹��� ����

        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;// �������� �ʺ� �׸��� Ÿ�� ũ�⸦ ���Ͽ� X ������ ����
        size.y = itemData.height * ItemGrid.tileSizeHeight;// �������� ���̿� �׸��� Ÿ�� ũ�⸦ ���Ͽ� Y ������ ����
        GetComponent<RectTransform>().sizeDelta = size;// RectTransform�� ������ ����
    }

    internal void Rotate()
    {
        rotated = !rotated;
        RectTransform rectTransform = GetComponent<RectTransform>();// RectTransform ������Ʈ ��������
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);// ȸ�� ���� ����Ͽ� ���� ������Ʈ ȸ�� ����
    }
}
