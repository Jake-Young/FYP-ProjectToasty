using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class GrabObjectRecorder : MonoBehaviour
{
    public AnimationClip m_Clip;
    public bool m_Record;

    private GameObjectRecorder m_ActionRecorder;

    void Start()
    {
        m_ActionRecorder = new GameObjectRecorder(gameObject);
        EditorCurveBinding binding = EditorCurveBinding.FloatCurve("", typeof(MachineLearningManager), "m_GrabObject");

        m_ActionRecorder.Bind(binding);
    }

    void LateUpdate()
    {
        if (m_Clip == null)
            return;

        if (m_Record)
        {
            // Take a snapshot and record all the bindings values for this frame.
            m_ActionRecorder.TakeSnapshot(Time.deltaTime);
        }
    }

    void OnDisable()
    {
        if (m_Clip == null)
            return;

        if (m_ActionRecorder.isRecording)
        {
            // Save the recorded session to the clip.
            m_ActionRecorder.SaveToClip(m_Clip);
        }
    }
}
