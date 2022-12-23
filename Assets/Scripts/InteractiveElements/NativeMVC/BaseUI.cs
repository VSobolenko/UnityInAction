using System;
using System.Collections;
using System.Collections.Generic;
using InteractiveElements.NativeMVC;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private void OnGUI()
    {
        if (Managers.Inventory == null)
            return;

        var posX = 10;
        var posY = 10;
        var width = 100;
        var height = 30;
        var buffer = 10;

        var itemList = Managers.Inventory.GetItemList();

        if (itemList == null || itemList.Count == 0)
        {
            GUI.Box(new Rect(posX, posY, width, height), "No items");
        }

        if (itemList != null)
        {
            // foreach (var item in itemList)
            // {
            //     var count = Managers.Inventory.GetItemCount(item);
            //     var image = Resources.Load<Texture2D>($"Icons/{item}");
            //     if (image == null)
            //     {
            //         Debug.LogError($"cant find texture2d: {item}");
            //         return;
            //     }
            //
            //     GUI.Box(new Rect(posX, posY, width, Height), new GUIContent($"({count})", image));
            //     posX += width + buffer;
            // }
            foreach (var item in itemList)
            {
                var count = Managers.Inventory.GetItemCount(item);
                GUI.Box(new Rect(posX, posY, width, height), item + $"({count})");
                posX += width + buffer;
            }

            var equipped = Managers.Inventory.equippedItem;
            if (equipped != null)
            {
                posX = Screen.width - (width + buffer);
                var image = Resources.Load<Texture2D>($"Icons/{equipped}");
                GUI.Box(new Rect(posX, posY, width, height), new GUIContent("(Equipped)", image));
            }

            posX = 10;
            posY += height + buffer;

            foreach (var item in itemList)
            {
                if (GUI.Button(new Rect(posX, posY, width, height), "Equip" + item))
                {
                    Managers.Inventory.EquipItem(item);
                }

                posX += width + buffer;
            }
            
            foreach (string item in itemList) {
                if (GUI.Button(new Rect(posX, posY, width, height), "Equip "+item)) {
                    Managers.Inventory.EquipItem(item);
                }
                if (item == "health") { 
                    if (GUI.Button(new Rect(posX, posY + height+buffer, width, height),
                        "Use Health")) {
                        Managers.Inventory.ConsumeItem("health");
                        Managers.Player.ChangeHealth(25);
                    }
                }
                posX += width+buffer;
            }
        }
    }
}
