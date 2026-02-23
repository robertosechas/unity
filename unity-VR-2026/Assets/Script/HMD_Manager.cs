using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMD_Manager : MonoBehaviour
{
    [SerializeField] GameObject xrPlayer;
    [SerializeField] GameObject fpsPlayer;

    void Start()
    {
        Debug.Log("HMD_Manager Start() ran!");
        StartCoroutine(SetupPlayer());
    }

    IEnumerator SetupPlayer()
    {
        // Give XR a moment to initialize (important for OpenXR)
        yield return null;
        yield return new WaitForSeconds(0.25f);

        InputDevice hmd = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        bool hasHmd = hmd.isValid;

        string hmdName = hasHmd ? hmd.name : "(no HMD found)";
        Debug.Log("Using device: " + hmdName);

        if (hasHmd)
        {
            Debug.Log("XR mode enabled. Using XR Player.");
            SetPlayers(xrOn: true);
        }
        else
        {
            Debug.Log("No HMD detected. Using FPS Player.");
            SetPlayers(xrOn: false);
        }
    }

    void SetPlayers(bool xrOn)
    {
        if (fpsPlayer != null) fpsPlayer.SetActive(!xrOn);
        if (xrPlayer != null) xrPlayer.SetActive(xrOn);

        // Fix "There are 2 audio listeners" by enabling only one
        SetAudioListeners(fpsPlayer, enabled: !xrOn);
        SetAudioListeners(xrPlayer, enabled: xrOn);
    }

    void SetAudioListeners(GameObject root, bool enabled)
    {
        if (root == null) return;

        AudioListener[] listeners = root.GetComponentsInChildren<AudioListener>(true);
        foreach (var l in listeners)
        {
            l.enabled = enabled;
        }
    }
}