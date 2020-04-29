using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Custom unity events for dynamic output values
/// </summary>
/// 

[Serializable]
public class OnTriggerPressValue : UnityEvent<float> { }
[Serializable]
public class OnGripPressValue : UnityEvent<float> { }
[Serializable]
public class OnPrimary2DAxisMoved : UnityEvent<Vector2> { }
[Serializable]
public class OnSecondary2DAxisMoved : UnityEvent<Vector2> { }

[DisallowMultipleComponent]
[AddComponentMenu("XR/XR Controller Input")]
public class XRControllerInput : MonoBehaviour
{

    /*
     * TO DO:
     * 
     * - Use the inspector to allow the user to choose if they want to track certain inputs
     * - Search for accessible input features at the beginning and subscribe to relevant
     *   actions
     * 
     */

    // Control Methods
    public bool m_UseDebug = false;
    public bool m_UseInputManager = false;

    // XR Controllers to manage
    public GameObject m_Controller;
    private XRController m_XRControllerDevice;

    // User presence, detects whether the device can detect a user
    [Header("User Presence ")]
    [SerializeField]
    private bool m_UserPresence = false;

    /// <summary>
    /// Controller Input Variables
    /// </summary>

    // Trigger input and value
    [Header("Trigger Input")]
    [SerializeField]
    private bool m_TriggerButton = false;
    [SerializeField]
    private float m_TriggerButtonValue = 0.0f;

    // Grip input and value
    [Header("Grip Input")]
    [SerializeField]
    private bool m_GripButton = false;
    [SerializeField]
    private float m_GripButtonValue = 0.0f;

    // Primary button input and touch
    [Header("Primary Button Input")]
    [SerializeField]
    private bool m_PrimaryButtonTouched = false;
    [SerializeField]
    private bool m_PrimaryButtonPressed = false;

    // Secondary button input and touch
    [Header("Secondary Button Input")]
    [SerializeField]
    private bool m_SecondaryButtonTouched = false;
    [SerializeField]
    private bool m_SecondaryButtonPressed = false;

    // Menu button
    [Header("Menu Button Input")]
    [SerializeField, HideInInspector]
    private bool m_MenuButton = false;

    // primary 2D axis input (movement, touch, press)
    [Header("Primary 2D Axis Input")]
    [SerializeField]
    private bool m_Primary2DAxisTouched = false;
    [SerializeField]
    private bool m_Primary2DAxisClicked = false;
    [SerializeField]
    private Vector2 m_Primary2DAxis = Vector2.zero;

    // secondary 2D axis input (movement, touch, press)
    [Header("Secondary 2D Axis Input")]
    [SerializeField]
    private bool m_Secondary2DAxisTouched = false;
    [SerializeField]
    private bool m_Secondary2DAxisClicked = false;
    [SerializeField]
    private Vector2 m_Secondary2DAxis = Vector2.zero;

    /// <summary>
    /// Getters and Setters for input variables
    /// </summary>
    public bool triggerButton
    {
        get { return m_TriggerButton; }
    }
    
    public float triggerButtonValue
    {
        get { return m_TriggerButtonValue; }
    }

    public bool gripButton
    {
        get { return m_GripButton; }
    }

    public float gripButtonValue
    {
        get { return m_GripButtonValue; }
    }

    public bool primaryButtonTouched
    {
        get { return m_PrimaryButtonTouched; }
    }

    public bool primaryButtonPressed 
    {
        get { return m_PrimaryButtonPressed; }
    }

    public bool secondaryButtonTouched
    {
        get { return m_SecondaryButtonTouched; }
    }

    public bool secondaryButtonPressed
    {
        get { return m_SecondaryButtonPressed; }
    }

    public bool menuButton
    {
        get { return m_MenuButton; }
    }

    public bool primary2DAxisTouched
    {
        get { return m_Primary2DAxisTouched; }
    }

    public bool primary2DAxisClicked
    {
        get { return m_Primary2DAxisClicked; }
    }

    public Vector2 primary2DAxis
    {
        get { return m_Primary2DAxis; }
    }

    public bool secondary2DAxisTouched
    {
        get { return m_Secondary2DAxisTouched; }
    }

    public bool secondary2DAxisClicked
    {
        get { return m_Secondary2DAxisClicked; }
    }

    public Vector2 secondary2DAxis
    {
        get { return m_Secondary2DAxis; }
    }

    /// <summary>
    /// Events to assign functionality to input
    /// </summary>
 
    [Header("Trigger Events")]
    [Tooltip("Actions to be triggered when the user presses the trigger")]
    public UnityEvent m_OnTriggerButtonPressed;
    [Tooltip("Actions to be triggered when the user applies a certain amount of pressure")]
    public OnTriggerPressValue m_OnTriggerPressValue;

    [Header("Grip Events")]
    [Tooltip("Actions to be triggered when the user presses the grip")]
    public UnityEvent m_OnGripButtonPressed;
    [Tooltip("Actions to be triggered when the user applies a certain amount of pressure")]
    public OnGripPressValue m_OnGripPressValue;

    [Header("Primary Button Events")]
    [Tooltip("Actions to be triggered when the user touches the primary button")]
    public UnityEvent m_OnPrimaryButtonTouched;
    [Tooltip("Actions to be triggered when the user presses the primary button")]
    public UnityEvent m_OnPrimaryButtonPressed;

    [Header("Secondary Button Events")]
    [Tooltip("Actions to be triggered when the user touches the secondary button")]
    public UnityEvent m_OnSecondaryButtonTouched;
    [Tooltip("Actions to be triggered when the user presses the secondary button")]
    public UnityEvent m_OnSecondaryButtonPressed;

    [Header("Menu Button Event")]
    [Tooltip("Actions to be triggered when the user presses the menu button")]
    public UnityEvent m_OnMenuButtonPressed;

    [Header("Primary 2D Axis Events")]
    [Tooltip("Actions to be triggered when the primary axis is touched")]
    public UnityEvent m_OnPrimary2DAxisTouched;
    [Tooltip("Actions to be triggered when the primary axis is clicked")]
    public UnityEvent m_OnPrimary2DAxisClicked;
    [Tooltip("Actions to be triggered when the primary axis is moved")]
    public OnPrimary2DAxisMoved m_OnPrimary2DAxisMoved;

    [Header("Secondary 2D Axis Events")]
    [Tooltip("Actions to be triggered when the secondary axis is touched")]
    public UnityEvent m_OnSecondary2DAxisTouched;
    [Tooltip("Actions to be triggered when the secondary axis is clicked")]
    public UnityEvent m_OnSecondary2DAxisClicked;
    [Tooltip("Actions to be triggered when the secondary axis is moved")]
    public OnSecondary2DAxisMoved m_OnSecondary2DAxisMoved;

    private void OnEnable()
    {
        m_Controller.SetActive(true);
        m_XRControllerDevice = m_Controller.GetComponent<XRController>();
    }

    // Create check to get updated values from each controller
    private void Update()
    {
        InputDevice inputDevice = m_XRControllerDevice.inputDevice;

        // If the dvice is not connected
        if (!inputDevice.isValid)
        {
            return;
        }

        // if the device reports user presence
        if (inputDevice.TryGetFeatureValue(CommonUsages.userPresence, out m_UserPresence))
        {
            // and, if the user is not present do not listen for input
            if (!m_UserPresence)
            {
                return;
            }
        }

        if (m_UseDebug) // Debug Scenario
        {

        }

        if (m_UseInputManager) // Input Manager Scenario
        {
            Debug.Log($"{m_XRControllerDevice.controllerNode} Using Input Manager");

            // if trigger button right pressed, get float amount
            float m_TriggerButtonValue = Input.GetAxis("UnityXR Trigger Value R");
            if (m_TriggerButtonValue > 0.0f)
            {
                if (m_OnTriggerPressValue != null)
                {
                    m_OnTriggerPressValue.Invoke(m_TriggerButtonValue);
                }
            }

            // if trigger button right is pressed, get boolean
            m_TriggerButton = Input.GetButton("UnityXR Trigger Button R");
            if (m_TriggerButton)
            {
                if (m_OnTriggerButtonPressed != null)
                {
                    m_OnTriggerButtonPressed.Invoke();
                }
            }

            // if grip button right pressed, get float amount
            m_GripButtonValue = Input.GetAxis("UnityXR Grip Value R");
            if (m_GripButtonValue > 0.0f)
            {
                if (m_OnGripPressValue != null)
                {
                    m_OnGripPressValue.Invoke(m_GripButtonValue);
                }
            }

            // if grip button is pressed, get boolean
            m_GripButton = Input.GetButton("UnityXR Grip Button R");
            if (m_GripButton)
            {
                if (m_OnGripButtonPressed != null)
                {
                    m_OnGripButtonPressed.Invoke();
                }
            }

            // if primary button is touched, get boolean
            m_PrimaryButtonTouched = Input.GetButton("UnityXR Primary Button Touch R");
            if (m_PrimaryButtonTouched)
            {
                if (m_OnPrimaryButtonTouched != null)
                {
                    m_OnPrimaryButtonTouched.Invoke();
                }
            }

            // if primary button is pressed, get boolean
            m_PrimaryButtonPressed = Input.GetButton("UnityXR Primary Button R");
            if (m_PrimaryButtonPressed)
            {
                if (m_OnPrimaryButtonPressed != null)
                {
                    m_OnPrimaryButtonPressed.Invoke();
                }
            }

            // if secondary button is touched, get boolean
            m_SecondaryButtonTouched = Input.GetButton("UnityXR Secondary Button Touch R");
            if (m_SecondaryButtonTouched)
            {
                if (m_OnSecondaryButtonTouched != null)
                {
                    m_OnSecondaryButtonTouched.Invoke();
                }
            }

            // if secondary button is pressed, get boolean
            m_SecondaryButtonPressed = Input.GetButton("UnityXR Secondary Button R");
            if (m_SecondaryButtonPressed)
            {
                if (m_OnSecondaryButtonPressed != null)
                {
                    m_OnSecondaryButtonPressed.Invoke();
                }
            }

            // if menu button is pressed, get boolean
            m_MenuButton = Input.GetButton("UnityXR Menu Button R");
            if (m_MenuButton)
            {
                if (m_OnMenuButtonPressed != null)
                {
                    m_OnMenuButtonPressed.Invoke();
                }
            }

            // if primary 2D axis is touch, get boolean
            m_Primary2DAxisTouched = Input.GetButton("UnityXR Primary 2D Axis Touch R");
            if (m_Primary2DAxisTouched)
            {
                if (m_OnPrimary2DAxisTouched != null)
                {
                    m_OnPrimary2DAxisTouched.Invoke();
                }
            }

            // if primary 2D axis is pressed, get boolean
            m_Primary2DAxisClicked = Input.GetButton("UnityXR Primary 2D Axis Click R");
            if (m_Primary2DAxisClicked)
            {
                if (m_OnPrimary2DAxisClicked != null)
                {
                    m_OnPrimary2DAxisClicked.Invoke();
                }
            }

            // if primary 2D axis is moved, get vector 2
            float primaryAxisValueX = Input.GetAxis("UnityXR Primary 2D Axis X R");
            float primaryAxisValueY = Input.GetAxis("UnityXR Primary 2D Axis Y R");
            m_Primary2DAxis = new Vector2(primaryAxisValueX, primaryAxisValueY);
            if (m_Primary2DAxis.magnitude > Vector2.zero.magnitude)
            {
                if (m_OnPrimary2DAxisMoved != null)
                {
                    m_OnPrimary2DAxisMoved.Invoke(m_Primary2DAxis);
                }
            }

            return;
        }

        Debug.Log($"{m_XRControllerDevice.controllerNode} Not Using Input Manager");

        // if trigger button pressed, get float amount
        if (inputDevice.TryGetFeatureValue(CommonUsages.trigger, out m_TriggerButtonValue) && m_TriggerButtonValue > 0.0f)
        {
            if (m_OnTriggerPressValue != null)
            {
                m_OnTriggerPressValue.Invoke(m_TriggerButtonValue);
            }
        }
            

        // if trigger button is pressed, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out m_TriggerButton) && m_TriggerButton)
        {
            if (m_OnTriggerButtonPressed != null)
            {
                m_OnTriggerButtonPressed.Invoke();
            }
        }

        // if grip button pressed, get float amount
        if (inputDevice.TryGetFeatureValue(CommonUsages.grip, out m_GripButtonValue) && m_GripButtonValue > 0.0f) 
        {
            if (m_OnGripPressValue != null)
            {
                m_OnGripPressValue.Invoke(m_GripButtonValue);
            }
        }

        // if grip button is pressed, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out m_GripButton) && m_GripButton)
        {
            if (m_OnGripButtonPressed != null)
            {
                m_OnGripButtonPressed.Invoke();
            }
        }

        // if primary button is touched, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out m_PrimaryButtonTouched) && m_PrimaryButtonTouched)
        {
            if (m_OnPrimaryButtonTouched != null)
            {
                m_OnPrimaryButtonTouched.Invoke();
            }
        }

        // if primary button is pressed, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out m_PrimaryButtonPressed) && m_PrimaryButtonPressed)
        {
            if (m_OnPrimaryButtonPressed != null)
            {
                m_OnPrimaryButtonPressed.Invoke();
            }
        }

        // if secondary button is touched, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out m_SecondaryButtonTouched) && m_SecondaryButtonTouched)
        {
            if (m_OnSecondaryButtonTouched != null)
            {
                m_OnSecondaryButtonTouched.Invoke();
            }
        }

        // if secondary button is pressed, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out m_SecondaryButtonPressed) && m_SecondaryButtonPressed)
        {
            if (m_OnSecondaryButtonPressed != null)
            {
                m_OnSecondaryButtonPressed.Invoke();
            }
        }

        // if menu button is pressed, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out m_MenuButton) && m_MenuButton)
        {
            if (m_OnMenuButtonPressed != null)
            {
                m_OnMenuButtonPressed.Invoke();
            }
        }

        // if primary 2D axis is touch, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out m_Primary2DAxisTouched) && m_Primary2DAxisTouched)
        {
            if (m_OnPrimary2DAxisTouched != null)
            {
                m_OnPrimary2DAxisTouched.Invoke();
            }
        }

        // if primary 2D axis is pressed, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out m_Primary2DAxisClicked) && m_Primary2DAxisClicked)
        {
            if (m_OnPrimary2DAxisClicked != null)
            {
                m_OnPrimary2DAxisClicked.Invoke();
            }
        }

        // Check this logic
        // if primary 2D axis is moved, get vector 2
        if (inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out m_Primary2DAxis) && m_Primary2DAxis.magnitude > Vector2.zero.magnitude)
        {
            if (m_OnPrimary2DAxisMoved != null)
            {
                m_OnPrimary2DAxisMoved.Invoke(m_Primary2DAxis);
            }
        }

        // if secondary 2D axis is touched, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.secondary2DAxisTouch, out m_Secondary2DAxisTouched) && m_Secondary2DAxisTouched)
        {
            if (m_OnSecondary2DAxisTouched != null)
            {
                m_OnSecondary2DAxisTouched.Invoke();
            }
        }

        // if secondary 2D axis is pressed, get boolean
        if (inputDevice.TryGetFeatureValue(CommonUsages.secondary2DAxisClick, out m_Secondary2DAxisClicked) && m_Secondary2DAxisClicked)
        {
            if (m_OnSecondary2DAxisClicked != null)
            {
                m_OnSecondary2DAxisClicked.Invoke();
            }
        }

        // if secondary 2D axis is moved, get vector 2
        if (inputDevice.TryGetFeatureValue(CommonUsages.secondary2DAxis, out m_Secondary2DAxis) && m_Secondary2DAxis.magnitude > Vector2.zero.magnitude)
        {
            if (m_OnSecondary2DAxisMoved != null)
            {
                m_OnSecondary2DAxisMoved.Invoke(m_Secondary2DAxis);
            }
        }
    }



}
