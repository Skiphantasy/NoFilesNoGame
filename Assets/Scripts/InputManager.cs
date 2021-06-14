using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class InputManager : MonoBehaviour
{
    EventSystem system;

    void Start()
    {
        system = EventSystem.current;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            try
            {
                Selectable next = GetSelectable();

                if (next != null)
                {
                    InputField inputfield = next.GetComponent<InputField>();

                    if (inputfield != null)
                        inputfield.OnPointerClick(new PointerEventData(system));

                    system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
                }
            } catch
            {
            }
        }
    }

    Selectable GetSelectable()
    {
        Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectable(Vector2.down);

        if (next == null)
        {
            Selectable[] selectables = Selectable.allSelectablesArray;

            next = selectables
                .Where(s => s.name == "Username")
                .FirstOrDefault();
        }

        return next;
    }
}