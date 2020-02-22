using UnityEngine;
using UnityEngine.UI;

namespace VRUiKits.Utils
{
    public class TabItem : Item
    {
        public GameObject relatedPanel;

        public override void Activate()
        {
            base.Activate();

            if (null != relatedPanel)
            {
                relatedPanel.SetActive(true);
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            if (null != relatedPanel)
            {
                relatedPanel.SetActive(false);
            }
        }
    }
}