using Unity;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace Hank.MainMenu
{
    public class ImageBoardDown : MonoBehaviour
    {
        Vector3 menuContentDefaultPos;
        RectTransform menuContent;

        public Image leftStick;
        public Image rightStick;

        public List<Sprite> normalIcons;
        public List<Sprite> checkedIcons;

        public static int LastIndex { get; set; }
        public static int LastOpenIndex { get; set; }

        static ImageBoardDown()
        {
            LastIndex = 2;
            LastOpenIndex = 2;
        }

        private void Awake()
        {
            menuContent = transform.Find("menuContainer/Icons").gameObject.GetComponent<RectTransform>();
            menuContentDefaultPos = menuContent.GetComponent<RectTransform>().localPosition;            
        }

        private void Start()
        {
            LastIndex = LastOpenIndex;
            setMenuIndex(LastOpenIndex, false);
        }

        int currentMenuIndex = 0;
        GameObject currentMenuItem;
        Image currentMenuItemImage;
        void setMenuIndex(int index, bool animated = true)
        {
            LastIndex = index;
            float animateDuration = .0f;
            if (animated) animateDuration = .3f;

            if (currentMenuItem)
            {
                if (animated) currentMenuItem.transform.DOScale(new Vector3(.62f, .62f, 1), animateDuration);
                currentMenuItemImage = currentMenuItem.GetComponent<Image>();

                switchCurrentMenuItemToNormal();
            }

            currentMenuIndex = index;
            currentMenuItem = menuContent.transform.Find("menuItem" + currentMenuIndex).gameObject;
            currentMenuItemImage = currentMenuItem.GetComponent<Image>();

            switchCurrentMenuItemToChecked();

            currentMenuItem.transform.DOScale(new Vector3(1, 1, 1), animateDuration);

            Vector3 nextPos = menuContentDefaultPos;
            nextPos.x = (menuContentDefaultPos.x) - 60 * (currentMenuIndex) + 120;
            menuContent.DOLocalMove(nextPos, animateDuration);

            if (currentMenuIndex < 2)
            {
                leftStick.DOFade(0, animateDuration);
            }
            else {
                leftStick.DOFade(1, animateDuration);
            }

            if (currentMenuIndex > this.normalIcons.Count - 3)
            {
                rightStick.DOFade(0, animateDuration);
            }
            else {
                rightStick.DOFade(1, animateDuration);
            }
        }

        void switchCurrentMenuItemToNormal()
        {
            this.normalIcons.ForEach(t =>
            {
                if (currentMenuIndex == this.normalIcons.IndexOf(t))
                {
                    currentMenuItemImage.GetComponent<Image>().sprite = t;
                }
            });
        }
        void switchCurrentMenuItemToChecked()
        {
            this.checkedIcons.ForEach(t =>
            {
                if (currentMenuIndex == this.checkedIcons.IndexOf(t))
                {
                    currentMenuItemImage.GetComponent<Image>().sprite = t;
                }
            });
        }

        public int switchNextMenu()
        {
            if (currentMenuIndex < this.normalIcons.Count - 1)
            {
                setMenuIndex(currentMenuIndex + 1);
            }
            return currentMenuIndex;
        }
        public int switchPrevMenu()
        {
            if (currentMenuIndex > 0)
            {
                setMenuIndex(currentMenuIndex - 1);
            }
            return currentMenuIndex;
        }

        void onTap()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                switchPrevMenu();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                switchNextMenu();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // MainMenuView menuView=transform.parent.gameObject;
                // menuView.closeMenu();
            }
        }

        private void Update()
        {
            onTap();
        }
    }
}