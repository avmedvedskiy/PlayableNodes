using System;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace PlayableNodes.Editor
{
    public class EaseSelectionWindow : PopupWindowContent
    {
        readonly GUIStyle _easeLabel = new GUIStyle(GUI.skin.label)
            .AddFontSize(12)
            .AddColor(new Color(0.91f, 0.9f, 0.95f))
            .AddAlignment( TextAnchor.LowerCenter)
            .PaddingBottom(3);
        
        readonly Vector2Int _easeSize = new(80, 60);
        readonly Color _selectionColor = Color.green;
        readonly Color _easeColor = Color.magenta;
        
        const int PADDING = 4;
        const int OFFSET = PADDING * 2;
        const int TOTAL_EASES_X_ROW = 6;

        private bool _initialized;
        private int _easeCount;
        private Ease[] _easeNames;
        private Texture2D[] _easeTextures;
        private Ease _selectedEase;

        private Action<Ease> _onSelect; 

        public override void OnOpen()
        {
            base.OnOpen();
            if (!_initialized)
                Init();
        }


        public void Prepare(Ease selectedEase, Action<Ease> onSelect)
        {
            _selectedEase = selectedEase;
            _onSelect = onSelect;
        }

        private void Select(Ease ease)
        {
            _selectedEase = ease;
            _onSelect?.Invoke(_selectedEase);
        }

        private void Init()
        {
            _initialized = true;
            _easeNames = ((Ease[])Enum.GetValues(typeof(Ease)))
                .Where(x=> x is not Ease.Unset and not Ease.INTERNAL_Zero)
                .ToArray();
            _easeCount = _easeNames.Length;
            _easeTextures = new Texture2D[_easeCount];

            for (int i = 0; i < _easeNames.Length; i++)
            {
                _easeTextures[i] = CreateEaseTexture(_easeNames[i]);
            }
        }
        
        public override Vector2 GetWindowSize()
        {
            int totalRows = (int)(_easeNames.Length / (float)TOTAL_EASES_X_ROW);
            return new Vector2(
                _easeSize.x * TOTAL_EASES_X_ROW + OFFSET * (TOTAL_EASES_X_ROW - 1) + PADDING * 2,
                _easeSize.y * totalRows + OFFSET * (totalRows - 1) + PADDING * 2
            );
        }

        private Texture2D CreateEaseTexture(Ease ease)
        {
            var overshootOrAmplitude = GetAmplitudeOvershoot(ease);
            
            Texture2D texture = new Texture2D(_easeSize.x, _easeSize.y);
            Vector2Int size = new Vector2Int(texture.width, texture.height);
            int totalPixels = size.x * size.y;
            int easeH = (int)(size.y * 0.35f);
            int easeBaseY = (int)((size.y - easeH) * 0.5f);
            int easeTopY = easeBaseY + easeH;

            Color32[] colors = new Color32[totalPixels];
            
            for (int i = 0; i < totalPixels; i++) 
                colors[i] = Color.black;
            texture.SetPixels32(colors);
            //
            if (ease != Ease.INTERNAL_Custom) {
                for (int i = 0; i < size.x; ++i) {
                    int x = i;
                    int y = (int)DOVirtual.EasedValue(easeBaseY, easeTopY, (float)i / size.x, ease, overshootOrAmplitude, 0);
                    
                    //texture.SetPixel(x, easeBaseY, _easeLimiterColor);
                    //texture.SetPixel(x, easeTopY, _easeLimiterColor);
                    
                    if (y >= size.y || y <= 0) 
                        continue;
                    texture.SetPixel(x, y, _easeColor);
                }
            }
            texture.Apply();
            return texture;
        }

        private float GetAmplitudeOvershoot(Ease ease)
        {
            float overshootOrAmplitude = 1.70158f;
            bool isBack = ease is Ease.InBack or Ease.OutBack or Ease.InOutBack;
            bool isFlash = !isBack && ease is Ease.Flash or Ease.InFlash or Ease.OutFlash or Ease.InOutFlash;
            
            if (isFlash) 
                overshootOrAmplitude = 8;
            else if (isBack) 
                overshootOrAmplitude = 3;

            return overshootOrAmplitude;
        }

        public override void OnGUI(Rect rect)
        {
            var btR = new Rect(rect.x + PADDING, rect.y + PADDING, _easeSize.x, _easeSize.y);
            
            for (int i = 0; i < _easeNames.Length; i++) 
            {
                if (i % TOTAL_EASES_X_ROW == 0) {
                    if (i > 0) 
                    {
                        btR.x = rect.x + PADDING;
                        btR.y += _easeSize.y + OFFSET;
                    }
                }
                else
                {
                    btR.x += _easeSize.x + OFFSET;
                }
                if (GUI.Button(btR, GUIContent.none, new GUIStyle(GUI.skin.button)))
                {
                    Select(_easeNames[i]);
                    editorWindow.Close();
                    return;
                }
                
                Texture2D tex = _easeTextures[i];
                if (tex == null) 
                    Init();

                if (_easeNames[i] == _selectedEase)
                {
                    using (new ColorScope(null, null, _selectionColor))
                    {
                        var boxRect = new Rect(btR.x - 2, btR.y - 2, btR.width + 4, btR.height + 4);
                        GUI.Box(boxRect, GUIContent.none, GUI.skin.box);
                    }
                }
                
                GUI.DrawTexture(btR, tex);
                EditorGUI.DropShadowLabel(btR, _easeNames[i].ToString(),_easeLabel);
            }
        }
    }
}