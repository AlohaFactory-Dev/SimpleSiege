using System;
using UnityEngine;
using UnityEditor;


public class ChShaderGUI : ShaderGUI
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

    static bool _Shadow_Foldout = true;
    
    static bool _Rim_Foldout = true;

    static bool _Queue_Foldout = false;

  

    MaterialEditor m_MaterialEditor;

    #endregion

    // 메인
    #region [메인]

    MaterialProperty mainTex_Sampler = null;

    MaterialProperty ApplyMainLightColor = null;

    MaterialProperty CelCount = null;

    MaterialProperty Threshole = null;



    #endregion

    // 그림자
    #region [그림자]

    MaterialProperty ShadowColor = null;

    MaterialProperty ShadowSize = null;

    MaterialProperty ShadowBlend = null;


    #endregion

    // 림라이트
    #region [림라이트]
    MaterialProperty RimColor= null;

    MaterialProperty RimRange = null;

    MaterialProperty RimBlend = null;


    #endregion


    public void FindProperties(MaterialProperty[] props)
    {

        // Main 변수 지정
        #region [메인]
        mainTex_Sampler = FindProperty("_MainTex", props);

        ApplyMainLightColor = FindProperty("_MainLightColorBlend", props);
     
        CelCount = ShaderGUI.FindProperty("_CelCount", props);

        Threshole = ShaderGUI.FindProperty("Vector1_7733C22C", props);
        #endregion

        // Shader 변수 지정
        #region [그림자]
        ShadowColor = FindProperty("_ShadowColor", props);

        ShadowSize = FindProperty("_ShadowSize", props);

        ShadowBlend = ShaderGUI.FindProperty("_ShadowBlend", props);
    

        #endregion

        // Rim 변수 지정
        #region [림라이트]
        RimColor = FindProperty("Color_4B525E00", props);

        RimRange = FindProperty("_RimSize", props);

        RimBlend = FindProperty("_RimBlend", props);


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
            

    }

        // 메인 메소드
        #region [메인]

        public void GUI_Maintextures(Material material)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.TexturePropertySingleLine(new GUIContent("메인텍스쳐"), mainTex_Sampler);
            m_MaterialEditor.TextureScaleOffsetProperty(mainTex_Sampler);
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.ShaderProperty(ApplyMainLightColor, "메인 라이트의 색상 적용 여부 설정");
            m_MaterialEditor.ShaderProperty(CelCount, "셀 셰이딩 분할 개수");
            m_MaterialEditor.ShaderProperty(Threshole, "셀 셰이딩 기준점 설정");
            EditorGUILayout.EndVertical();
        } 
        #endregion

        // 그림자 메소드
        #region [그림자]
        void GUI_Shadow(Material material)
        {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(ShadowColor, "그림자 색상");
                    m_MaterialEditor.ShaderProperty(ShadowSize, "그림자 영역 크기");
                    m_MaterialEditor.ShaderProperty(ShadowBlend, "부드러운 그림자 적용 정도");   
                
                    EditorGUILayout.EndVertical();
                
        }
        #endregion

        // 디졸브 메소드
        #region [디졸브]
        void GUI_Rim(Material material)
        {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(RimColor, "림라이트 색상");
                    m_MaterialEditor.ShaderProperty(RimRange, "림라이트 영역 크기");
                    m_MaterialEditor.ShaderProperty(RimBlend, "부드러운 림라이트 적용 정도");
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