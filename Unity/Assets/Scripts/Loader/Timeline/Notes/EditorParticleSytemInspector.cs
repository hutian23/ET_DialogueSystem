using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (EditParticleSystem))]
public class EditParticleSystemInspector: Editor
{
    /// <summary>
    /// 滑动杆的当前时间
    /// </summary>
    private float m_CurTime;

    /// <summary>
    /// 当前是否是预览播放状态
    /// </summary>
    private bool m_Playing;

    /// <summary>
    /// 当前运行时间
    /// </summary>
    private float m_RunningTime;

    /// <summary>
    /// 上一次系统时间
    /// </summary>
    private double m_PreviousTime;

    /// <summary>
    /// 滑动杆总长度
    /// </summary>
    private const float kDuration = 30f;

    private ParticleSystem m_ParticleSystem;

    private EditParticleSystem editAnimator
    {
        get
        {
            return target as EditParticleSystem;
        }
    }

    private ParticleSystem particleSystem
    {
        get
        {
            return m_ParticleSystem? m_ParticleSystem : (m_ParticleSystem = editAnimator.target);
        }
    }

    void OnEnable()
    {
        m_PreviousTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += inspectorUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= inspectorUpdate;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Play"))
        {
            play();
        }

        if (GUILayout.Button("Stop"))
        {
            stop();
        }

        EditorGUILayout.EndHorizontal();
        m_CurTime = EditorGUILayout.Slider("Time:", m_CurTime, 0f, kDuration);
        manualUpdate();
    }

    /// <summary>
    /// 进行预览播放
    /// </summary>
    private void play()
    {
        if (Application.isPlaying || particleSystem == null)
        {
            return;
        }

        m_RunningTime = 0f;
        m_Playing = true;
    }

    /// <summary>
    /// 停止预览播放
    /// </summary>
    private void stop()
    {
        if (Application.isPlaying || particleSystem == null)
        {
            return;
        }

        m_Playing = false;
        m_CurTime = 0f;
    }

    /// <summary>
    /// 预览播放状态下的更新
    /// </summary>
    private void update()
    {
        if (Application.isPlaying || particleSystem == null)
        {
            return;
        }

        if (m_RunningTime >= kDuration)
        {
            m_Playing = false;
            return;
        }

        particleSystem.Simulate(m_RunningTime, true);
        SceneView.RepaintAll();
        Repaint();

        m_CurTime = m_RunningTime;
    }

    /// <summary>
    /// 非预览播放状态下，通过滑杆来播放当前动画帧
    /// </summary>
    private void manualUpdate()
    {
        if (particleSystem && !m_Playing)
        {
            particleSystem.Simulate(m_CurTime, true);
            SceneView.RepaintAll();
        }
    }

    private void inspectorUpdate()
    {
        var delta = EditorApplication.timeSinceStartup - m_PreviousTime;
        m_PreviousTime = EditorApplication.timeSinceStartup;

        if (!Application.isPlaying && m_Playing)
        {
            m_RunningTime = Mathf.Clamp(m_RunningTime + (float)delta, 0f, kDuration);
            update();
        }
    }
}