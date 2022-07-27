using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDetector : MonoBehaviour
{
    int sButton;
    public event Action<int, bool> hoveredButtons;

    void Update()
    {
        bool isHovered = IsBuildingButtonsHoverd();
        hoveredButtons?.Invoke(sButton, isHovered);
    }


    // Code Monkey
    // https://www.youtube.com/watch?v=ptmum1FXiLE
    // Big kiss
    bool IsBuildingButtonsHoverd()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if (raycastResultList[i].gameObject.GetComponent<BMButton>() != null)
            {
                sButton = raycastResultList[i].gameObject.GetComponent<BMButton>().buttonId;
                return true;
            }
        }

        return false;
    }


}
