using System;
using System.Globalization;
using DG.Tweening;
using TMPro;

public static class TextMeshProExtension
{
    #region long

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, long value, long newValue, float duration,
        Func<long, string> convert)
    {
        long pValue = 0;
        textMeshPro.text = convert.Invoke(value);
        return DOTween
            .To(() => value, x => value = x, newValue, duration)
            .OnUpdate(() =>
            {
                if (pValue != value)
                {
                    textMeshPro.text = convert.Invoke(value);
                    pValue = value;
                }
            })
            .SetEase(Ease.Linear);
    }

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, long value, long newValue, float duration)
    {
        return textMeshPro.SetSmoothValue(value, newValue, duration, x => x.ToString());
    }

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, long value, long newValue, float duration,
        string format)
    {
        return textMeshPro.SetSmoothValue(value, newValue, duration, x => string.Format(format, x));
    }

    #endregion

    #region int

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, int value, int newValue, float duration,
        Func<int, string> convert)
    {
        int pValue = 0;
        textMeshPro.text = convert.Invoke(value);
        return
            DOTween
                .To(() => value, x => value = x, newValue, duration)
                .OnUpdate(() =>
                {
                    if (pValue != value)
                    {
                        textMeshPro.text = convert.Invoke(value);
                        pValue = value;
                    }
                })
                .SetEase(Ease.Linear);
    }

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, int value, int newValue, float duration)
    {
        return textMeshPro.SetSmoothValue(value, newValue, duration, x => x.ToString());
    }

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, int value, int newValue, float duration,
        string format)
    {
        return textMeshPro.SetSmoothValue(value, newValue, duration, x => string.Format(format, x));
    }

    #endregion

    #region float

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, float value, float newValue, float duration,
        Func<float, string> convert)
    {
        float pValue = 0;
        textMeshPro.text = convert.Invoke(value);
        return
            DOTween
                .To(() => value, x => value = x, newValue, duration)
                .OnUpdate(() =>
                {
                    if (pValue != value)
                    {
                        textMeshPro.text = convert.Invoke(value);
                        pValue = value;
                    }
                })
                .SetEase(Ease.Linear);
    }

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, float value, float newValue, float duration)
    {
        return textMeshPro.SetSmoothValue(value, newValue, duration, x => x.ToString(CultureInfo.CurrentCulture));
    }

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, float value, float newValue, float duration,
        CultureInfo info)
    {
        return textMeshPro.SetSmoothValue(value, newValue, duration, x => x.ToString(info));
    }

    public static Tweener SetSmoothValue(this TMP_Text textMeshPro, float value, float newValue, float duration,
        string format)
    {
        return textMeshPro.SetSmoothValue(value, newValue, duration, x => string.Format(format, x));
    }

    #endregion
}