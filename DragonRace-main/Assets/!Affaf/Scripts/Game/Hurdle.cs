using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    public GameObject Effect;

    public void DestroyHurdle()
    {
        Instantiate(Effect, transform.position, Effect.transform.rotation);
        //Destroy(gameObject);
        StartCoroutine(DisableHurdle());
    }

    IEnumerator DisableHurdle()
    {
        Vector3 orignal = transform.localScale;

        transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(5f);
        transform.localScale = orignal;
    }
}
