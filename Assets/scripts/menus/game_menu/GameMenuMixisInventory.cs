﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameMenuMixisInventory : GameMenu {

    [SerializeField] Transform m_partyTransform;
    [SerializeField] Transform m_inventoryTransform;

    [SerializeField] float m_partyItemScale = 10.0f;
    [SerializeField] float m_inventoryItemScale = 10.0f;

    DataCharManager m_charManager;
    ProfileManager.Profile m_profile;

    List<GameObject> m_party;
    List<GameObject> m_inventory;

    // Use this for initialization
    void Start () {
        LoadParty();
        LoadInventory();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region LOADING

    public void LoadParty()
    {
        m_profile = ProfileManager.instance.GetProfile();
        m_party = new List<GameObject>();

        for (int i = 0; i < m_profile.CurrentTeam.Count; i++)
        {
            string charId = m_profile.CurrentTeam[i];
            if (charId == null)
            {
                if (m_party[i] != null)
                    Destroy(m_party[i].gameObject);
                continue;
            }
            GameObject go = CreateCharacterUIObject(charId, m_partyItemScale);
            go.GetComponent<UIInventoryDraggableItem>().IsDraggable = false;
            go.GetComponent<UIInventoryDraggableItem>().Menu = this;
            go.transform.SetParent(m_partyTransform, false);

            m_party.Add(go);
        }
    }

    void LoadInventory()
    {
        m_inventory = new List<GameObject>();
        for (int i = 0; i < m_profile.Characters.Count; i++)
        {
            var charData = m_profile.Characters[i];
            //check if not in party
            if (!m_profile.CurrentTeam.Contains(charData.Id))
            {
                GameObject go = CreateCharacterUIObject(charData.Id, m_inventoryItemScale);
                go.transform.SetParent(m_inventoryTransform, false);
                go.GetComponent<UIInventoryDraggableItem>().Menu = this;
                m_inventory.Add(go);
            }
        }
    }

    GameObject CreateCharacterUIObject(string _id, float _scale)
    {
        //Create character
        GameObject character = DataManager.instance.CreateCharacter(_id);
        character.name = _id;
        //convert to ui
        Utils.SetLayerRecursively(character, LayerMask.NameToLayer("SpriteUI"));
        Utils.ConvertToUIImage(character);
        //Set Parent
        GameObject container = new GameObject("Char_" + _id);
        Utils.SetLocalScaleXY(character.transform, _scale, _scale);
        var rect = container.AddComponent<RectTransform>();
        character.transform.SetParent(container.transform, false);
        //Set Draggable
        var uiItem = container.AddComponent<UIInventoryDraggableItem>();
        uiItem.CharId = _id;
        return container;
    }

    #endregion

    public void OnInventoryItemDrag(UIInventoryDraggableItem _item)
    {
        var itemRect = _item.GetComponent<RectTransform>();
        foreach (var chara in m_party)
        {
            bool inDropZone = ItemsOverlap(_item.gameObject, chara.gameObject);
            if( inDropZone)
            {
                Debug.Log("Interserct " + chara.GetComponent<UIInventoryDraggableItem>().CharId);
            }
        }
    }

    public void OnInventoryItemDrop(UIInventoryDraggableItem _item)
    {
        var itemRect = _item.GetComponent<RectTransform>();
        foreach (var chara in m_party)
        {
            bool inDropZone = ItemsOverlap(_item.gameObject, chara.gameObject);
            if (inDropZone)
            {
                Debug.Log("Drop on " + chara.GetComponent<UIInventoryDraggableItem>().CharId);
            }
        }
    }

    bool ItemsOverlap(GameObject obj1, GameObject obj2)
    {
        RectTransform rt1 = obj1.GetComponent<RectTransform>();
        RectTransform rt2 = obj2.GetComponent<RectTransform>();

        var mag = (rt1.position - rt2.position).magnitude;
        if(mag < 50.0f)
        {
            return true;
        }
        return false;
    }
}
