using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour {

    public delegate void Callback0();
    public static IEnumerator MoveAnchor(RectTransform rec, Vector2 to, float duration, Callback0 fn)
    {
        float elapsed = 0;
        Vector3 from = rec.anchoredPosition;
        while(elapsed <= duration)
        {
            elapsed += Time.deltaTime;
            rec.anchoredPosition = Vector2.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        if (fn != null)
            fn();
    }
	
}
