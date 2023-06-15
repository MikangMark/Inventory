using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { WEAPON = 0, AROMER, HEADPHONE, HELMET, VEST, BACKPACK, GLASS, VALUABLE, MEDICAL }
[CreateAssetMenu]
public class ItemData : ScriptableObject//��ũ���ͺ� ������Ʈ��ũ��Ʈ ������ ũ�������� �̹�������
{
    public ItemType type;
    public string itemName;
    public int width = 1;       //����
    public int height = 1;      //ũ��
    public Sprite invenIcon;     //�̹���
    public Sprite equipIcon;    //����� �̹���

    #region ��������Ӽ�
    public int eq_width = 1;    //����� ����ũ��
    public int eq_height = 1;   //����� ����ũ��
    #endregion
    #region �������� ���� �Ӽ�
    #endregion
}
