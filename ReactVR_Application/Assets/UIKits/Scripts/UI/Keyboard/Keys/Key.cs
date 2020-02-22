using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UIKIT_TMP
using TMPro;
#endif
namespace VRUiKits.Utils
{
    public class Key : MonoBehaviour, IPointerDownHandler
    {
#if UIKIT_TMP
    protected TextMeshProUGUI key;
#else
    protected Text key;
#endif
        public delegate void OnKeyClickedHandler(string key);

        // The event which other objects can subscribe to
        // Uses the function defined above as its type
        public event OnKeyClickedHandler OnKeyClicked;

        public virtual void Awake()
        {
#if UIKIT_TMP
            if (transform.Find("Text").GetComponent<TextMeshProUGUI>())
            {
                key = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogError("Could not find TextMeshProUGUI Component on the key in Keyboard."
                + " Please make sure to convert Text to TextMeshPro.");
            }
#else
            key = transform.Find("Text").GetComponent<Text>();
#endif
            // GetComponent<Button>().onClick.AddListener(() =>
            // {
            //     OnKeyClicked(key.text);
            // });
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            //Output the name of the GameObject that is being clicked
            OnKeyClicked(key.text);
        }

        public virtual void CapsLock(bool isUppercase) { }
        public virtual void ShiftKey() { }
    }
}