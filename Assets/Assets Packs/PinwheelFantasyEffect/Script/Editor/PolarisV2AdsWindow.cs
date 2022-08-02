using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Pinwheel.FantasyEffect
{
    public class PolarisV2AdsWindow : EditorWindow
    {
        private static GUIStyle titleStyle;
        private static GUIStyle TitleStyle
        {
            get
            {
                if (titleStyle == null)
                {
                    titleStyle = new GUIStyle(EditorStyles.label);
                    titleStyle.fontSize = 20;
                    titleStyle.alignment = TextAnchor.MiddleCenter;
                    titleStyle.fontStyle = FontStyle.Bold;
                    titleStyle.normal.textColor = Color.white;
                }
                return titleStyle;
            }
        }

        private static GUIStyle subTitleStyle;
        private static GUIStyle SubTitleStyle
        {
            get
            {
                if (subTitleStyle == null)
                {
                    subTitleStyle = new GUIStyle(EditorStyles.label);
                    subTitleStyle.fontSize = 13;
                    subTitleStyle.alignment = TextAnchor.MiddleCenter;
                    subTitleStyle.fontStyle = FontStyle.Bold;
                    subTitleStyle.normal.textColor = Color.white;
                }
                return subTitleStyle;
            }
        }

        private static GUIStyle buttonStyle;
        public static GUIStyle ButtonStyle
        {
            get
            {
                if (buttonStyle == null)
                {
                    buttonStyle = new GUIStyle();
                    buttonStyle.alignment = TextAnchor.MiddleCenter;
                    buttonStyle.normal.background = Texture2D.whiteTexture;
                    buttonStyle.fontSize = 13;
                    buttonStyle.fontStyle = FontStyle.Bold;
                }
                return buttonStyle;
            }
        }

        private const string PREF_KEY = "polaris-v2-ads";

        [DidReloadScripts]
        private static void OnScriptReloaded()
        {
            int prefValue = PlayerPrefs.GetInt(PREF_KEY, 0);
            if (prefValue == 0)
            {
                ShowWindow();
            } 
        }

        public static void ShowWindow()
        {
            PlayerPrefs.SetInt(PREF_KEY, 1);
            PolarisV2AdsWindow window = ScriptableObject.CreateInstance<PolarisV2AdsWindow>();
            window.titleContent = new GUIContent("Polaris V2 - Ultimate Low Poly Terrain Engine");
            window.minSize = new Vector2(640, 360);
            window.maxSize = new Vector2(640, 361);
            window.ShowUtility();
        }

        private void OnEnable()
        {
            wantsMouseMove = true;
        }

        public void OnGUI()
        {
            Rect backgroundRect = GUILayoutUtility.GetAspectRect(16f / 9f);
            Texture2D backgroundTex = Resources.Load<Texture2D>("PolarisV2AdsImage");
            if (backgroundTex != null)
            {
                GUI.DrawTexture(backgroundRect, backgroundTex);
            }
            else
            {
                EditorGUI.DrawRect(backgroundRect, Color.black);
                EditorGUI.LabelField(backgroundRect, "Failed to load image.", EditorStyles.whiteLabel);
            }

            Rect iconRect = new Rect();
            iconRect.size = new Vector2(128, 128);
            iconRect.center = backgroundRect.center - new Vector2(0, 64);
            Texture2D iconTex = Resources.Load<Texture2D>("PolarisV2Icon");
            if (iconTex != null)
            {
                Vector2 shadowOffset = new Vector2(1, 1);
                Rect shadowRect = new Rect(iconRect.position + shadowOffset, iconRect.size);
                GUI.DrawTexture(shadowRect, iconTex, ScaleMode.ScaleToFit, true, 1, Color.black, 0, 0);
                GUI.DrawTexture(iconRect, iconTex, ScaleMode.ScaleToFit, true, 1, Color.white, 0, 0);
            }

            Rect titleRect = new Rect();
            titleRect.size = new Vector2(640, 30);
            titleRect.center = backgroundRect.center + new Vector2(0, 30);
            EditorGUI.DropShadowLabel(titleRect, "Polaris V2 - Ultimate Low Poly Engine", TitleStyle);

            Rect subtitleRect = new Rect();
            subtitleRect.size = new Vector2(640, 20);
            subtitleRect.center = backgroundRect.center + new Vector2(0, 55);
            EditorGUI.DropShadowLabel(subtitleRect, "Stunning low poly terrain achieve within clicks!", SubTitleStyle);

            Rect buttonsRect = new Rect();
            buttonsRect.size = new Vector2(640, 30);
            buttonsRect.center = backgroundRect.center + new Vector2(0, 90);

            Rect learnMoreRect = new Rect();
            learnMoreRect.size = new Vector2(140, 30);
            learnMoreRect.center = buttonsRect.center - new Vector2(75, 0);
            EditorGUIUtility.AddCursorRect(learnMoreRect, MouseCursor.Link);
            if (GUI.Button(learnMoreRect, "Learn more", ButtonStyle))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/terrain/polaris-v2-ultimate-low-poly-terrain-engine-144645?aid=1100l3QbW&pubref=editor-ads");
            }

            Rect getFreeRect = new Rect();
            getFreeRect.size = new Vector2(140, 30);
            getFreeRect.center = buttonsRect.center + new Vector2(75, 0);
            EditorGUIUtility.AddCursorRect(getFreeRect, MouseCursor.Link);
            if (GUI.Button(getFreeRect, "Get it FREE", ButtonStyle))
            {
                Application.OpenURL("http://pinwheel.studio/polaris2-evaluation");
            }

            Rect desLinkRect = new Rect();
            desLinkRect.size = new Vector2(640, 15);
            desLinkRect.position = new Vector2(backgroundRect.min.x, backgroundRect.max.y - 15);

            if (learnMoreRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(desLinkRect, Color.white * 0.5f);
                EditorGUI.LabelField(desLinkRect, "https://assetstore.unity.com", EditorStyles.miniLabel);
            }

            if (getFreeRect.Contains(Event.current.mousePosition))
            {
                EditorGUI.DrawRect(desLinkRect, Color.white * 0.5f);
                EditorGUI.LabelField(desLinkRect, "https://pinwheel.studio", EditorStyles.miniLabel);
            }

            if (Event.current != null && Event.current.isMouse)
            {
                Repaint();
            }
        }
    }
}
