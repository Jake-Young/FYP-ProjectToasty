using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;

//[CustomEditor(typeof(XRControllerInput))]
internal class XRControllerInputEditor : Editor
{
    SerializedProperty m_Controller;
    SerializedProperty m_UserPresence;
    SerializedProperty m_TriggerButton;
    SerializedProperty m_TriggerButtonValue;
    SerializedProperty m_GripButton;
    SerializedProperty m_GripBUttonValue;
    SerializedProperty m_PrimaryButtonTouched;
    SerializedProperty m_PrimaryButtonPressed;
    SerializedProperty m_SecondaryButtonTouched;
    SerializedProperty m_SecondaryButtonPressed;
    SerializedProperty m_MenuButton;
    SerializedProperty m_Primary2DAxisTouched;
    SerializedProperty m_Primary2DAxisClicked;
    SerializedProperty m_Primary2DAxis;
    SerializedProperty m_Secondary2DAxisTouched;
    SerializedProperty m_Secondary2DAxisClicked;
    SerializedProperty m_Secondary2DAxis;
    SerializedProperty m_OnTriggerButtonPressed;
    SerializedProperty m_OnTriggerPressValue;
    SerializedProperty m_OnGripButtonPressed;
    SerializedProperty m_OnGripPressValue;
    SerializedProperty m_OnPrimaryButtonTouched;
    SerializedProperty m_OnPrimaryButtonPressed;
    SerializedProperty m_OnSecondaryButtonTouched;
    SerializedProperty m_OnSecondaryButtonPressed;
    SerializedProperty m_OnMenuButtonPressed;
    SerializedProperty m_OnPrimary2DAxisTouched;
    SerializedProperty m_OnPrimary2DAxisClicked;
    SerializedProperty m_OnPrimary2DAxisMoved;
    SerializedProperty m_OnSecondary2DAxisTouched;
    SerializedProperty m_OnSecondary2DAxisClicked;
    SerializedProperty m_OnSecondary2DAxisMoved;

    bool m_ShowControllerValues;

    static class Tooltips
    {
        public static readonly GUIContent controller = new GUIContent("Current Controller", "The current controller being used");
        public static readonly GUIContent triggerButton = new GUIContent("Trigger Button Press", "Boolean, if the button is pressed or not");
        public static readonly GUIContent triggerButtonValue = new GUIContent("Trigger Button Press Amount", "Float, the pressure applied to the trigger");
        public static readonly GUIContent gripButton = new GUIContent("Grip Button Press", "Boolean, if the button is presse or not");
        public static readonly GUIContent gripButtonValue = new GUIContent("Grip Button Press Amount", "Float, the pressure applied to the grip");
        public static readonly GUIContent primaryButtonTouched = new GUIContent("Primary Button Press", "Boolean, if the primary button is pressed or not");
        public static readonly GUIContent primaryButtonPressed = new GUIContent("Primary Button Touched", "Boolean, if the primary button is touched or not");
        public static readonly GUIContent secondaryButtonTouched = new GUIContent("Secondary Button Press", "Boolean, if the secondary button is pressed or not");
        public static readonly GUIContent secondaryButtonPressed = new GUIContent("Secondary Button Touched", "Boolean, if the secondary button is touched or not");
        public static readonly GUIContent menuButton = new GUIContent("Menu Button Press", "Boolean, if the menu button is pressed or not");
        public static readonly GUIContent userPresence = new GUIContent("User Presence", "Boolean, is the user present in the HMD?");
        public static readonly GUIContent primary2DAxisTouched = new GUIContent("Primary Axis Touched", "Boolean, is the primary 2D axis touched or not");
        public static readonly GUIContent primary2DAxisClicked = new GUIContent("Primary Axis Clicked", "Boolean, is the primary 2D axis clicked or not");
        public static readonly GUIContent primary2DAxis = new GUIContent("Primary 2d Axis Movement", "Vector2 (x, y), the amount the primary 2D axis is being moved");
        public static readonly GUIContent secondary2DAxisTouched = new GUIContent("Secondary Axis Touched", "Boolean, is the secondary 2D axis touched or not");
        public static readonly GUIContent secondary2DAxisClicked = new GUIContent("Secondary Axis Clicked", "Boolean, is the secondary 2D axis clicked or not");
        public static readonly GUIContent secondary2DAxis = new GUIContent("Secondary Axis Movement", "Vector2 (x, y) the amount the secondary 2D axis is being moved");

        // Do tooltips for unity events 
    }

    private void OnEnable()
    {
        m_Controller = serializedObject.FindProperty("m_Controller");
        m_TriggerButton = serializedObject.FindProperty("m_TriggerButton");
        m_TriggerButtonValue = serializedObject.FindProperty("m_TriggerButtonValue");
        m_GripButton = serializedObject.FindProperty("m_GripButton");
        m_GripBUttonValue = serializedObject.FindProperty("m_GripButtonValue");
        m_PrimaryButtonTouched = serializedObject.FindProperty("m_PrimaryButtonTouched");
        m_PrimaryButtonPressed = serializedObject.FindProperty("m_PrimaryButtonPressed");
        m_SecondaryButtonTouched = serializedObject.FindProperty("m_SecondaryButtonTouched");
        m_SecondaryButtonPressed = serializedObject.FindProperty("m_SecondaryButtonPressed");
        m_MenuButton = serializedObject.FindProperty("m_MenuButton");
        m_UserPresence = serializedObject.FindProperty("m_UserPresence");
        m_Primary2DAxisTouched = serializedObject.FindProperty("m_Primary2DAxisTouched");
        m_Primary2DAxisClicked = serializedObject.FindProperty("m_Primary2DAxisClicked");
        m_Primary2DAxis = serializedObject.FindProperty("m_Primary2DAxis");
        m_Secondary2DAxisTouched = serializedObject.FindProperty("m_Secondary2DAxisTouched");
        m_Secondary2DAxisClicked = serializedObject.FindProperty("m_Secondary2DAxisClicked");
        m_Secondary2DAxis = serializedObject.FindProperty("m_Secondary2DAxis");
        m_OnTriggerButtonPressed = serializedObject.FindProperty("m_OnTriggerButtonPressed");
        m_OnTriggerPressValue = serializedObject.FindProperty("m_OnTriggerPressValue");
        m_OnGripButtonPressed = serializedObject.FindProperty("m_OnGripButtonPressed");
        m_OnGripPressValue = serializedObject.FindProperty("m_OnGripPressValue");
        m_OnPrimaryButtonTouched = serializedObject.FindProperty("m_OnPrimaryButtonTouched");
        m_OnPrimaryButtonPressed = serializedObject.FindProperty("m_OnPrimaryButtonPressed");
        m_OnSecondaryButtonTouched = serializedObject.FindProperty("m_OnSecondaryButtonTouched");
        m_OnSecondaryButtonPressed = serializedObject.FindProperty("m_OnSecondaryButtonPressed");
        m_OnMenuButtonPressed = serializedObject.FindProperty("m_OnMenuButtonPressed");
        m_OnPrimary2DAxisTouched = serializedObject.FindProperty("m_OnPrimary2DAxisTouched");
        m_OnPrimary2DAxisClicked = serializedObject.FindProperty("m_OnPrimary2DAxisClicked");
        m_OnPrimary2DAxisMoved = serializedObject.FindProperty("m_OnPrimary2DAxisMoved");
        m_OnSecondary2DAxisTouched = serializedObject.FindProperty("m_OnSecondary2DAxisTouched");
        m_OnSecondary2DAxisClicked = serializedObject.FindProperty("m_OnSecondary2DAxisClicked");
        m_OnSecondary2DAxisMoved = serializedObject.FindProperty("m_OnSecondary2DAxisMoved");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((XRControllerInput)target), typeof(XRControllerInput), false);
        GUI.enabled = true;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_Controller, Tooltips.controller);
        EditorGUILayout.PropertyField(m_UserPresence, Tooltips.userPresence);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(m_OnTriggerButtonPressed);
        EditorGUILayout.PropertyField(m_OnTriggerPressValue);
        EditorGUILayout.PropertyField(m_OnGripButtonPressed);
        EditorGUILayout.PropertyField(m_OnGripPressValue);
        EditorGUILayout.PropertyField(m_OnPrimaryButtonTouched);
        EditorGUILayout.PropertyField(m_OnPrimaryButtonPressed);
        EditorGUILayout.PropertyField(m_OnSecondaryButtonTouched);
        EditorGUILayout.PropertyField(m_OnSecondaryButtonPressed);
        EditorGUILayout.PropertyField(m_OnMenuButtonPressed);
        EditorGUILayout.PropertyField(m_OnPrimary2DAxisTouched);
        EditorGUILayout.PropertyField(m_OnPrimary2DAxisClicked);
        EditorGUILayout.PropertyField(m_OnPrimary2DAxisMoved);
        EditorGUILayout.PropertyField(m_OnSecondary2DAxisTouched);
        EditorGUILayout.PropertyField(m_OnSecondary2DAxisClicked);
        EditorGUILayout.PropertyField(m_OnSecondary2DAxisMoved);

        EditorGUILayout.Space();

        m_ShowControllerValues = EditorGUILayout.Toggle("Controller Input Values", m_ShowControllerValues);

        EditorGUILayout.Space();

        if (m_ShowControllerValues)
        {
            EditorGUILayout.PropertyField(m_TriggerButton, Tooltips.triggerButton);
            EditorGUILayout.PropertyField(m_TriggerButtonValue, Tooltips.triggerButtonValue);
            EditorGUILayout.PropertyField(m_GripButton, Tooltips.gripButton);
            EditorGUILayout.PropertyField(m_GripBUttonValue, Tooltips.gripButtonValue);
            EditorGUILayout.PropertyField(m_PrimaryButtonTouched, Tooltips.primaryButtonTouched);
            EditorGUILayout.PropertyField(m_PrimaryButtonPressed, Tooltips.primaryButtonPressed);
            EditorGUILayout.PropertyField(m_SecondaryButtonTouched, Tooltips.secondaryButtonTouched);
            EditorGUILayout.PropertyField(m_SecondaryButtonPressed, Tooltips.secondaryButtonPressed);
            EditorGUILayout.PropertyField(m_MenuButton, Tooltips.menuButton);
            EditorGUILayout.PropertyField(m_Primary2DAxisTouched, Tooltips.primary2DAxisTouched);
            EditorGUILayout.PropertyField(m_Primary2DAxisClicked, Tooltips.primary2DAxisClicked);
            EditorGUILayout.PropertyField(m_Primary2DAxis, Tooltips.primary2DAxis);
            EditorGUILayout.PropertyField(m_Secondary2DAxisTouched, Tooltips.secondary2DAxisTouched);
            EditorGUILayout.PropertyField(m_Secondary2DAxisClicked, Tooltips.secondary2DAxisClicked);
            EditorGUILayout.PropertyField(m_Secondary2DAxis, Tooltips.secondary2DAxis);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
