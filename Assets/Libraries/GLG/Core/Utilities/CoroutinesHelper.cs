using GLG;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Набор готовых методов для анимации с помощью корутин.
/// </summary>
public class CoroutinesHelper : MonoBehaviour
{
	/// <summary>
	/// Анимирует размер объекта по свойству sizeDelta.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевой размер</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateSizeDelta(RectTransform obj, Vector2 target, float duration, System.Action callback = null)
	{
		float t = 0;
		Vector2 startSD = obj.sizeDelta;
		while (t < duration)
		{
			obj.sizeDelta = Vector2.Lerp(startSD, target, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.sizeDelta = target;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует размер объекта по свойству sizeDelta.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевой размер</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="setPositionToTarget"></param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateSizeDelta(RectTransform obj, RectTransform target, float duration, bool setPositionToTarget = false, System.Action callback = null)
	{
		if (setPositionToTarget) obj.position = target.position;
		float t = 0;
		Vector2 startSD = obj.sizeDelta;
		while (t < duration)
		{
			obj.sizeDelta = Vector2.Lerp(startSD, target.sizeDelta, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.sizeDelta = target.sizeDelta;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует позицию объекта.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевой размер</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimatePosition(Transform obj, Vector3 target, float duration, System.Action callback = null)
	{
		float t = 0;
		Vector3 startPos = obj.position;
		while (t < duration)
		{
			obj.position = Vector3.Lerp(startPos, target, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.position = target;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует позицию и поворот объекта.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевой размер</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimatePositionAndRotation(Transform obj, Transform target, float duration, System.Action callback = null, bool fixedUpdate = false)
	{
		float t = 0;
		Vector3 startPos = obj.position;
		Quaternion startRot = obj.rotation;
		while (t < duration)
		{
			obj.position = Vector3.Lerp(startPos, target.position, t / duration);
			obj.rotation = Quaternion.Slerp(startRot, target.rotation, t / duration);
			if (fixedUpdate) yield return new WaitForFixedUpdate();
			else yield return null;
			t += Time.deltaTime;
		}
		obj.position = target.position;
		obj.rotation = target.rotation;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует позицию и поворот объекта.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевой размер</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimatePositionAndRotation(Transform obj, Vector3 targetPosition, Quaternion targetRotation, float duration, System.Action callback = null)
	{
		float t = 0;
		Vector3 startPos = obj.position;
		Quaternion startRot = obj.rotation;
		while (t < duration)
		{
			obj.position = Vector3.Lerp(startPos, targetPosition, t / duration);
			obj.rotation = Quaternion.Slerp(startRot, targetRotation, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.position = targetPosition;
		obj.rotation = targetRotation;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует localPosition объекта.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевая позиция</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateLocalPosition(Transform obj, Vector3 target, float duration, System.Action callback = null)
	{
		float t = 0;
		Vector3 startPos = obj.localPosition;
		while (t < duration)
		{
			obj.localPosition = Vector3.Lerp(startPos, target, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.localPosition = target;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует position объекта.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target"></param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimatePosition(Transform obj, Transform target, float duration, System.Action callback = null)
	{
		float t = 0;
		Vector3 startPos = obj.position;
		while (t < duration)
		{
			obj.position = Vector3.Lerp(startPos, target.position, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.position = target.position;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует localPosition объекта.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевая позиция</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateLocalPosition(Transform obj, Transform target, float duration, System.Action callback = null)
	{
		float t = 0;
		Vector3 startPos = obj.localPosition;
		while (t < duration)
		{
			obj.localPosition = Vector3.Lerp(startPos, target.localPosition, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.localPosition = target.localPosition;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует прозрачность CanvasGroup.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевая прозрачность</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateCanvasGroupAlpha(CanvasGroup obj, float target, float duration, System.Action callback = null)
	{
		float t = 0;
		float startAlpha = obj.alpha;
		while (t < duration)
		{
			obj.alpha = Mathf.Lerp(startAlpha, target, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.alpha = target;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует прозрачность изображения.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевая прозрачность</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateImageColorAlpha(Image obj, float target, float duration, System.Action callback = null)
	{
		float t = 0;
		Color c = obj.color;
		float startAlpha = c.a;
		while (t < duration)
		{
			c.a = Mathf.Lerp(startAlpha, target, t / duration);
			obj.color = c;
			yield return null;
			t += Time.deltaTime;
		}
		c.a = target;
		obj.color = c;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует прозрачность спрайта.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевая прозрачность</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateSpriteColorAlpha(SpriteRenderer obj, float target, float duration, System.Action callback = null)
	{
		float t = 0;
		Color c = obj.color;
		float startAlpha = c.a;
		while (t < duration)
		{
			c.a = Mathf.Lerp(startAlpha, target, t / duration);
			obj.color = c;
			yield return null;
			t += Time.deltaTime;
		}
		c.a = target;
		obj.color = c;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует прозрачность текста.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевой цвет</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateTextColorAlpha(Text obj, float target, float duration, System.Action callback = null)
	{
		float t = 0;
		Color c = obj.color;
		float startAlpha = c.a;
		while (t < duration)
		{
			c.a = Mathf.Lerp(startAlpha, target, t / duration);
			obj.color = c;
			yield return null;
			t += Time.deltaTime;
		}
		c.a = target;
		obj.color = c;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует прозрачность 3D текста.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевой цвет</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateTextColorAlpha(TextMesh obj, float target, float duration, System.Action callback = null)
	{
		float t = 0;
		Color c = obj.color;
		float startAlpha = c.a;
		while (t < duration)
		{
			c.a = Mathf.Lerp(startAlpha, target, t / duration);
			obj.color = c;
			yield return null;
			t += Time.deltaTime;
		}
		c.a = target;
		obj.color = c;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Анимирует localScale объекта.
	/// </summary>
	/// <param name="obj">Объект для анимации</param>
	/// <param name="target">Целевой размер</param>
	/// <param name="duration">Длительность анимации</param>
	/// <param name="callback">Вызывается после окончания анимации</param>
	/// <returns></returns>
	public static IEnumerator AnimateLocalScale(Transform obj, Vector3 target, float duration, System.Action callback = null)
	{
		float t = 0;
		Vector3 startPos = obj.localScale;
		while (t < duration)
		{
			obj.localScale = Vector3.Lerp(startPos, target, t / duration);
			yield return null;
			t += Time.deltaTime;
		}
		obj.localScale = target;
		if (callback != null) callback();
		yield break;
	}
	/// <summary>
	/// Запускает выполнение отложенной функции.
	/// </summary>
	/// <param name="action">Функция</param>
	/// <param name="delay">Задержка</param>
	public static void DoDelayedAction(float delay, System.Action action)
	{
		Kernel.CoroutinesObject.StartCoroutine(DelayedAction(delay, action));
	}
	/// <summary>
	/// Запускает выполнение отложенной функции на указанном объекте.
	/// </summary>
	/// <param name="obj">Объект, в котором будет запущена корутина</param>
	/// <param name="action">Функция</param>
	/// <param name="delay">Задержка</param>
	public static void DoDelayedAction(MonoBehaviour obj, float delay, System.Action action)
	{
		obj.StartCoroutine(DelayedAction(delay, action));
	}
	/// <summary>
	/// Запускает выполнение отложенной функции на указанном объекте в следующем кадре.
	/// </summary>
	/// <param name="action">Функция</param>
	public static void DoOnNextFrame(System.Action action)
	{
		Kernel.CoroutinesObject.StartCoroutine(DelayedAction(action));
	}
	/// <summary>
	/// Запускает выполнение отложенной функции на указанном объекте в следующем кадре.
	/// </summary>
	/// <param name="obj">Объект, в котором будет запущена корутина</param>
	/// <param name="action">Функция</param>
	public static void DoOnNextFrame(MonoBehaviour obj, System.Action action)
	{
		obj.StartCoroutine(DelayedAction(action));
	}

	public static IEnumerator DelayedAction(System.Action action)
	{
		yield return null;
		action();
	}
	public static IEnumerator DelayedAction(float delay, System.Action action)
	{
		yield return new WaitForSeconds(delay);
		action();
	}
    /// <summary>
    /// Перезапускаяет корутину.
    /// </summary>
    /// <param name="obj">Объект, в котором будет запущена корутина</param>
    /// <param name="routineContainer">Контейнер, хранящий корутину</param>
    /// <param name="coroutine">Метод для запуска корутины</param>
    public static void RenewCoroutine(MonoBehaviour obj, ref IEnumerator routineContainer, IEnumerator coroutine)
    {
        if (routineContainer != null) obj.StopCoroutine(routineContainer);
        if (coroutine != null)
        {
            routineContainer = coroutine;
            obj.StartCoroutine(routineContainer);
        }
    }
    /// <summary>
    /// Перезапускаяет корутину.
    /// </summary>
    /// <param name="routineContainer">Контейнер, хранящий корутину</param>
    /// <param name="coroutine">Метод для запуска корутины</param>
    public static void RenewCoroutine(ref IEnumerator routineContainer, IEnumerator coroutine)
    {
        if (routineContainer != null) Kernel.CoroutinesObject.StopCoroutine(routineContainer);
        if (coroutine != null)
        {
            routineContainer = coroutine;
            Kernel.CoroutinesObject.StartCoroutine(routineContainer);
        }
    }
}