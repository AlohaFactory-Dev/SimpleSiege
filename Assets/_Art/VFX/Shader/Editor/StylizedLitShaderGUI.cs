using System;
using UnityEngine;
using UnityEditor;


public class StylizedLitShaderGUI : ShaderGUI
{

    public GUILayoutOption[] shortButtonStyle = new GUILayoutOption[] { GUILayout.Width(100) };
    public GUIStyle style = new GUIStyle();

    #region [폴더 메뉴]
    static bool Foldout(bool display, string title)
    {

        var style = new GUIStyle("ShurikenModuleTitle");
        style.font = new GUIStyle(EditorStyles.boldLabel).font;
        style.border = new RectOffset(15, 7, 4, 4);
        style.fixedHeight = 22;
        style.contentOffset = new Vector2(20f, -2f);
        style.fontSize = 11;
        style.normal.textColor = new Color(0.7f, 0.8f, 0.9f);


        var rect = GUILayoutUtility.GetRect(16f, 25f, style);
        GUI.Box(rect, title, style);

        var e = Event.current;

        var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
        if (e.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
        }

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            display = !display;
            e.Use();
        }

        return display;
    }



    static bool Foldouts(bool display, string title)
    {



        var style = new GUIStyle("ShurikenModuleTitle");
        style.font = new GUIStyle(EditorStyles.boldLabel).font;
        style.border = new RectOffset(15, 7, 4, 4);
        style.fixedHeight = 18;
        style.contentOffset = new Vector2(30f, -2f);
        style.fontSize = 10;
        style.normal.textColor = new Color(0.75f, 0.75f, 0.75f);


        var rect = GUILayoutUtility.GetRect(16f, 15f, style);
        GUI.Box(rect, title, style);

        var e = Event.current;

        var toggleRect = new Rect(rect.x + 15f, rect.y + 2f, 13f, 13f);
        if (e.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
        }

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            display = !display;
            e.Use();
        }

        return display;
    }



    static bool Foldout2(bool display, string title)
    {


        var style = new GUIStyle("ShurikenModuleTitle");
        style.font = new GUIStyle(EditorStyles.boldLabel).font;
        style.border = new RectOffset(15, 7, 4, 4);
        style.fixedHeight = 22;
        style.contentOffset = new Vector2(20f, -2f);
        style.fontSize = 11;
        style.normal.textColor = new Color(0.65f, 0.55f, 0.55f);


        var rect = GUILayoutUtility.GetRect(16f, 25f, style);
        GUI.Box(rect, title, style);

        var e = Event.current;

        var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
        if (e.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
        }

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            display = !display;
            e.Use();
        }

        return display;
    }

    #endregion



    //GUI 변수명 정의

    #region [GUI]

    static bool _Main_Foldout = true;

    static bool _Emissive_Foldout = true;

    static bool _Specular_Foldout = true;

    static bool _Shadow_Foldout = true;
    
    static bool _Rim_Foldout = true;

    static bool _Queue_Foldout = false;

  

    MaterialEditor m_MaterialEditor;

    #endregion

    // 메인
    #region [메인]

    MaterialProperty mainTex_Sampler = null;

    MaterialProperty ColorIntensity = null;

    MaterialProperty StartColor = null;

    MaterialProperty StartThreshold = null;

    MaterialProperty StartSmooth = null;

    MaterialProperty MiddleColor = null;

    MaterialProperty MiddleThreshold = null;

    MaterialProperty MiddleSmooth = null;

    MaterialProperty endColor = null;

    MaterialProperty endThreshold = null;

    MaterialProperty endSmooth = null;

    #endregion


    // 이미시브
    #region [이미시브]

    MaterialProperty emissiveTex_Sampler = null;

    MaterialProperty EmissiveColor = null;

    MaterialProperty EmissiveOn_Off = null;


    #endregion

     // 스펙큘러
    #region [스펙큘러]

    MaterialProperty SpecularColor = null;

    MaterialProperty SpecularOffset = null;

    MaterialProperty SpecularPower = null;

    MaterialProperty SpecularThreshold = null;

    MaterialProperty SpecularSmooth = null;

    #endregion

    // 그림자
    #region [그림자]

    MaterialProperty ReceiveShadows = null;

    MaterialProperty ShadowColor = null;

    MaterialProperty ShadowSensitivity = null;


    #endregion

    // 림라이트
    #region [림라이트]
    MaterialProperty RimColor= null;

    MaterialProperty RimThreshold = null;

    MaterialProperty RimSmooth = null;


    #endregion


    public void FindProperties(MaterialProperty[] props)
    {

        // Main 변수 지정
        #region [메인]
        mainTex_Sampler = FindProperty("_MainTex", props);

        ColorIntensity = FindProperty("_ColorIntensity", props);
     
        StartColor = ShaderGUI.FindProperty("_ColorA", props);

        StartThreshold = ShaderGUI.FindProperty("_ThresholdA", props);

        StartSmooth = ShaderGUI.FindProperty("_SmoothA", props);

        MiddleColor = ShaderGUI.FindProperty("_ColorB", props);

        MiddleThreshold = ShaderGUI.FindProperty("_ThresholdB", props);

        MiddleSmooth = ShaderGUI.FindProperty("_SmoothB", props);

        endColor = ShaderGUI.FindProperty("_ColorC", props);

        endThreshold = ShaderGUI.FindProperty("_ThresholdC", props);

        endSmooth = ShaderGUI.FindProperty("_SmoothC", props);
        #endregion

        // Emissive 변수 지정
        #region [이미시브]
        emissiveTex_Sampler = FindProperty("_EmissiveTex", props);

        EmissiveColor = FindProperty("_EmissiveColor", props);

        EmissiveOn_Off = ShaderGUI.FindProperty("_EmissiveOn_Off", props);
        #endregion
       
        // Shader 변수 지정
        #region [스펙큘러]
        SpecularColor = FindProperty("_SpecularColor", props);

        SpecularOffset = FindProperty("_SpecularOffset", props);

        SpecularPower = ShaderGUI.FindProperty("_SpecularPower", props);

        SpecularThreshold = ShaderGUI.FindProperty("_SpecularThreshold", props);

        SpecularSmooth = ShaderGUI.FindProperty("_SpecularSmooth", props);
    

        #endregion

        // Shader 변수 지정
        #region [그림자]
        ReceiveShadows = FindProperty("_ReceiveShadows", props);

        ShadowColor = FindProperty("_ShadowColor", props);

        ShadowSensitivity = ShaderGUI.FindProperty("_ShadowSensitivity", props);
    

        #endregion

        // Rim 변수 지정
        #region [림라이트]
        RimColor = FindProperty("_RimColor", props);

        RimThreshold = FindProperty("_RimThreshold", props);

        RimSmooth = FindProperty("_RimSmooth", props);


        #endregion

   
    }


    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {


        FindProperties(props); 

        m_MaterialEditor = materialEditor;

        Material material = materialEditor.target as Material;


        //메인          

         EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Main_Foldout = Foldout(_Main_Foldout, "메인");

         if (_Main_Foldout)
         {
            EditorGUI.indentLevel++;

            GUI_Maintextures(material);

            EditorGUI.indentLevel--;
         }
         EditorGUILayout.EndVertical();

        // 이미시브
           
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Emissive_Foldout = Foldout(_Emissive_Foldout, "이미시브");

        if (_Emissive_Foldout)
        {
            EditorGUI.indentLevel++;
                                
            GUI_Emissive(material);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
            
        // 스펙큘러
           
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Specular_Foldout = Foldout(_Specular_Foldout, "스펙큘러");

        if (_Specular_Foldout)
        {
            EditorGUI.indentLevel++;
                                
            GUI_Specular(material);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // 그림자
           
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Shadow_Foldout = Foldout(_Shadow_Foldout, "그림자");

        if (_Shadow_Foldout)
        {
            EditorGUI.indentLevel++;
                                
            GUI_Shadow(material);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
            
       

        // 림라이트
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Rim_Foldout = Foldout(_Rim_Foldout, "림라이트");

        if (_Rim_Foldout)
        {
            EditorGUI.indentLevel++;
                                
            GUI_Rim(material);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
            
     
           
        // 렌더큐설정
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Queue_Foldout = Foldout(_Queue_Foldout, "렌더큐설정");
        {
            if(_Queue_Foldout)
            {
                EditorGUI.indentLevel++;

                GUI_Queue(material);

                EditorGUI.indentLevel--;
            }
        }
        EditorGUILayout.EndVertical();
            


        // 메인 메소드
        #region [메인]

        void GUI_Maintextures(Material material)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.TexturePropertySingleLine(new GUIContent("메인텍스쳐"), mainTex_Sampler);
            m_MaterialEditor.TextureScaleOffsetProperty(mainTex_Sampler);
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.ShaderProperty(ColorIntensity, "컬러밝기");
            m_MaterialEditor.ShaderProperty(StartColor, "윗컬러");
            m_MaterialEditor.ShaderProperty(StartThreshold, "윗영역 기준점");
            m_MaterialEditor.ShaderProperty(StartSmooth, "윗영역 부드럽게");
            m_MaterialEditor.ShaderProperty(MiddleColor, "중간컬러");
            m_MaterialEditor.ShaderProperty(MiddleThreshold, "중간영역 기준점");
            m_MaterialEditor.ShaderProperty(MiddleSmooth, "중간영역 부드럽게");
            m_MaterialEditor.ShaderProperty(endColor, "아래컬러");
            m_MaterialEditor.ShaderProperty(endThreshold, "아래컬러 기준점");
            m_MaterialEditor.ShaderProperty(endSmooth, "아래영역 부드럽게");
            EditorGUILayout.EndVertical();
        } 
        #endregion

        // 이미시브 메소드
        #region [이미시브]
        void GUI_Emissive(Material material)
        {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("이미시브텍스쳐"), emissiveTex_Sampler);
                m_MaterialEditor.TextureScaleOffsetProperty(emissiveTex_Sampler);
                EditorGUILayout.EndVertical();
                GUILayout.Space(5);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                m_MaterialEditor.ShaderProperty(EmissiveColor, "이미시브 색상");
                m_MaterialEditor.ShaderProperty(EmissiveOn_Off, "이미시브 온오프");
                EditorGUILayout.EndVertical();
                
        }
        #endregion

        // 스펙큘러 메소드
        #region [스펙큘러]
        void GUI_Specular(Material material)
        {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(SpecularColor, "스펙큘러 색상");
                    m_MaterialEditor.ShaderProperty(SpecularOffset, "스펙큘러 위치");
                    m_MaterialEditor.ShaderProperty(SpecularPower, "스펙큘러 세기");  
                    m_MaterialEditor.ShaderProperty(SpecularThreshold, "스펙큘러 영역 기준점");   
                    m_MaterialEditor.ShaderProperty(SpecularSmooth, "스펙큘러 부드럽게");   
                
                    EditorGUILayout.EndVertical();
                
        }
        #endregion

        // 그림자 메소드
        #region [그림자]
        void GUI_Shadow(Material material)
        {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(ReceiveShadows, "그림자 켜기");
                    m_MaterialEditor.ShaderProperty(ShadowColor, "그림자 컬러");
                    m_MaterialEditor.ShaderProperty(ShadowSensitivity, "그림자 세기");   
                
                    EditorGUILayout.EndVertical();
                
        }
        #endregion

        // 디졸브 메소드
        #region [디졸브]
        void GUI_Rim(Material material)
        {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(RimColor, "림라이트 색상");
                    m_MaterialEditor.ShaderProperty(RimThreshold, "림라이트 영역 기준점");
                    m_MaterialEditor.ShaderProperty(RimSmooth, "림라이트 부드럽게");
                    EditorGUILayout.EndVertical();    
        }
        #endregion


        // GUI
        #region [Queue]
        void GUI_Queue(Material material)
        {   
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.BeginChangeCheck();
            {
                MaterialProperty[] props = { };
                base.OnGUI(m_MaterialEditor, props);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }
        #endregion
    }
}
