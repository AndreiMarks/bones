using UnityEngine;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class JuicyDOT 
{
    public static TweenerCore<Color, Color, ColorOptions> To( DOGetter<Color> getter, DOSetter<Color> setter, Color endValue, DOTData data )
    {
        return DOTween.To( getter, setter, endValue, data.duration ).SetEase( data.ease );
    }

    public static TweenerCore<float, float, FloatOptions> To( DOGetter<float> getter, DOSetter<float> setter, float endValue, DOTData data )
    {
        return DOTween.To( getter, setter, endValue, data.duration ).SetEase( data.ease );
    }

    public static TweenerCore<Vector3, Vector3, VectorOptions> To( DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 endValue, DOTData data )
    {
        return DOTween.To( getter, setter, endValue, data.duration ).SetEase( data.ease );
    }
}

[System.Serializable]
public class DOTData
{
    public float duration;
    public DG.Tweening.Ease ease;
}
/*
        // implemented
        public static TweenerCore<float, float, FloatOptions> To(DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
        { return ApplyTo<float, float, FloatOptions>(getter, setter, endValue, duration); }
        public static TweenerCore<Vector3, Vector3, VectorOptions> To(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 endValue, float duration)
        { return ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, endValue, duration); }
        public static TweenerCore<Color, Color, ColorOptions> To(DOGetter<Color> getter, DOSetter<Color> setter, Color endValue, float duration)
        { return ApplyTo<Color, Color, ColorOptions>(getter, setter, endValue, duration); }

        // not implemented
        public static Tweener To(DOGetter<int> getter, DOSetter<int> setter, int endValue,float duration)
        { return ApplyTo<int, int, NoOptions>(getter, setter, endValue, duration); }
        public static Tweener To(DOGetter<uint> getter, DOSetter<uint> setter, uint endValue, float duration)
        { return ApplyTo<uint, uint, NoOptions>(getter, setter, endValue, duration); }
        public static Tweener To(DOGetter<long> getter, DOSetter<long> setter, long endValue, float duration)
        { return ApplyTo<long, long, NoOptions>(getter, setter, endValue, duration); }
        public static Tweener To(DOGetter<ulong> getter, DOSetter<ulong> setter, ulong endValue, float duration)
        { return ApplyTo<ulong, ulong, NoOptions>(getter, setter, endValue, duration); }
        public static TweenerCore<string, string, StringOptions> To(DOGetter<string> getter, DOSetter<string> setter, string endValue, float duration)
        { return ApplyTo<string, string, StringOptions>(getter, setter, endValue, duration); }
        public static TweenerCore<Vector2, Vector2, VectorOptions> To(DOGetter<Vector2> getter, DOSetter<Vector2> setter, Vector2 endValue, float duration)
        { return ApplyTo<Vector2, Vector2, VectorOptions>(getter, setter, endValue, duration); }
        public static TweenerCore<Vector4, Vector4, VectorOptions> To(DOGetter<Vector4> getter, DOSetter<Vector4> setter, Vector4 endValue, float duration)
        { return ApplyTo<Vector4, Vector4, VectorOptions>(getter, setter, endValue, duration); }
        public static TweenerCore<Quaternion, Vector3, QuaternionOptions> To(DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, Vector3 endValue, float duration)
        { return ApplyTo<Quaternion, Vector3, QuaternionOptions>(getter, setter, endValue, duration); }
        public static TweenerCore<Rect, Rect, RectOptions> To(DOGetter<Rect> getter, DOSetter<Rect> setter, Rect endValue, float duration)
        { return ApplyTo<Rect, Rect, RectOptions>(getter, setter, endValue, duration); }
        public static Tweener To(DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, RectOffset endValue, float duration)
        { return ApplyTo<RectOffset, RectOffset, NoOptions>(getter, setter, endValue, duration); }

        public static TweenerCore<T1, T2, TPlugOptions> To<T1, T2, TPlugOptions>(
            ABSTweenPlugin<T1, T2, TPlugOptions> plugin, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration
        )
            where TPlugOptions : struct
        { return ApplyTo(getter, setter, endValue, duration, plugin); }

        public static TweenerCore<Vector3, Vector3, VectorOptions> ToAxis(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue, float duration, AxisConstraint axisConstraint = AxisConstraint.X)
        {
            TweenerCore<Vector3, Vector3, VectorOptions> t = ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, new Vector3(endValue, endValue, endValue), duration);
            t.plugOptions.axisConstraint = axisConstraint;
            return t;
        }
        public static Tweener ToAlpha(DOGetter<Color> getter, DOSetter<Color> setter, float endValue, float duration)
        { return ApplyTo<Color, Color, ColorOptions>(getter, setter, new Color(0, 0, 0, endValue), duration).SetOptions(true); }

*/
