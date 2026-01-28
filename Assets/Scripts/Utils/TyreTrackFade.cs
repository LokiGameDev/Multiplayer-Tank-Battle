using UnityEngine;

public class TyreTrackFade : MonoBehaviour
{
    public LineRenderer line;

    public float fadeStartTime = 2f;   // seconds before fade begins
    public float fadeDuration  = 4f;   // seconds to fully fade old points

    float timer;

    void Update()
    {
        if (line.positionCount < 2) return;

        timer += Time.deltaTime;
        if (timer < fadeStartTime) return;

        int count = line.positionCount;

        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[count];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[count];

        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (count - 1); // 0 = oldest, 1 = newest

            // Fade only old parts
            float fade = Mathf.Clamp01(
                (timer - fadeStartTime - t * fadeDuration) / fadeDuration
            );

            float alpha = 1f - fade;

            colorKeys[i] = new GradientColorKey(Color.white, t);
            alphaKeys[i] = new GradientAlphaKey(alpha, t);
        }

        gradient.SetKeys(colorKeys, alphaKeys);
        line.colorGradient = gradient;

        // Destroy when fully faded
        if (alphaKeys[count - 1].alpha <= 0.01f)
            Destroy(gameObject);
    }

}
