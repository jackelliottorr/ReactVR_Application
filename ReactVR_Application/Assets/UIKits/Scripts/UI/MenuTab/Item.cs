using UnityEngine;
using UnityEngine.UI;

namespace VRUiKits.Utils
{
    public class Item : MonoBehaviour
    {
        public Button button;
        public delegate void OnItemSelectedHandler(Item item);
        public event OnItemSelectedHandler OnItemSelected;
        protected Color temp; // record the current normal color of the button
        void Awake()
        {
            if (null == button && null != GetComponent<Button>())
            {
                button = GetComponent<Button>();
            }
            else
            {
                Debug.LogError("Item requires button to be assigned");
            }

            temp = button.colors.normalColor;
            Deactivate();

            button.onClick.AddListener(() =>
            {
                if (null != OnItemSelected)
                {
                    OnItemSelected(this);
                }
            });
        }

        public virtual void Activate()
        {
            //Changes the button's Normal color to the new color.
            ColorBlock cb = button.colors;
            cb.normalColor = button.colors.highlightedColor;
            button.colors = cb;
        }

        public virtual void Deactivate()
        {
            //Changes the button's Normal color to the original color.
            ColorBlock cb = button.colors;
            cb.normalColor = temp;
            button.colors = cb;
        }

        public virtual void DeactivateSubMenu() { }
    }
}
