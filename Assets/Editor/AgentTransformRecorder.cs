using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class AgentTransformRecorder : MonoBehaviour
{
    public AnimationClip m_Clip;
    public bool m_Record;

    private GameObjectRecorder m_Recorder;

    void Start()
    {
        m_Recorder = new GameObjectRecorder(gameObject);

        EditorCurveBinding bindingX = EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.x");
        EditorCurveBinding bindingY = EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.y");
        EditorCurveBinding bindingZ = EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.z");

        m_Recorder.Bind(bindingX);
        m_Recorder.Bind(bindingY);
        m_Recorder.Bind(bindingZ);
    }

    void LateUpdate()
    {
        if (m_Clip == null)
            return;

        if (m_Record)
        {
            // Take a snapshot and record all the bindings values for this frame.
            m_Recorder.TakeSnapshot(Time.deltaTime);
        }
    }

    void OnDisable()
    {
        if (m_Clip == null)
            return;

        if (m_Recorder.isRecording)
        {
            // Save the recorded session to the clip.
            m_Recorder.SaveToClip(m_Clip);
        }
    }
}
