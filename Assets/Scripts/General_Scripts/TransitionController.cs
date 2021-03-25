using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour {

    private bool isLoading = false;

    public void sceneTransition (int sceneIndex) {
        if(isLoading) return;
        isLoading = true;
        StartCoroutine(sceneChange(sceneIndex));
    }

    private IEnumerator sceneChange (int sceneIndex) {
        AsyncOperation operation = SceneManager.
            LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        operation.allowSceneActivation = false;

        Animator animator = GetComponent<Animator>();

        animator.SetBool("transition", true);

        yield return new WaitForSeconds(1);

        operation.allowSceneActivation = true;
    }
}
