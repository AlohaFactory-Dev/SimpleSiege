using System;
using UnityEngine;
using UnityEditor;


public class WaterShaderGUI : ShaderGUI
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

    static bool _Water_Foldout = true;

    static bool _Foam_Foldout = true;
    
    static bool _SufaceFoam_Foldout = true;

    static bool _Queue_Foldout = false;

  

    MaterialEditor m_MaterialEditor;

    #endregion

    // 물
    #region [물]

    MaterialProperty ShallowWaterColor = null;

    MaterialProperty DeepWaterColor = null;

    MaterialProperty WaterDepth = null;

    MaterialProperty WaveDirection = null;

    MaterialProperty WaveSize = null;

    MaterialProperty FresnelPower = null;

    MaterialProperty NormalsStrength = null;

    MaterialProperty ShallowWaterDistortion = null;


    #endregion

    // 물거품
    #region [물거품]

    MaterialProperty FoamColor = null;

    MaterialProperty FoamAmount = null;

    MaterialProperty FoamCutoff = null;

    MaterialProperty FoamDirection = null;

    MaterialProperty FoamScale = null;


    #endregion

    // 물표면거품
    #region [물표면거품]

    MaterialProperty SufaceFoamSpeedSwitch = null;

    MaterialProperty SufaceFoamTexture = null;

    MaterialProperty SufaceFoamDistortion = null;

    MaterialProperty SufaceFoamIntansity = null;

    MaterialProperty SufaceFoamUV = null;

    MaterialProperty SufaceVisiblity = null;

    MaterialProperty SufaceGradientColor = null;

    MaterialProperty SufaceGradientColorIntensity = null;

    #endregion


    public void FindProperties(MaterialProperty[] props)
    {

        // Water 변수 지정
        #region [물]
        ShallowWaterColor = FindProperty("_Shallowwatercolor", props);

        DeepWaterColor = FindProperty("_Deepwatercolor", props);
     
        WaterDepth = ShaderGUI.FindProperty("_Waterdepth", props);

        WaveDirection = ShaderGUI.FindProperty("_Wavedirection", props);

        WaveSize = ShaderGUI.FindProperty("_Wavesize", props);

        FresnelPower = ShaderGUI.FindProperty("_Fresnelpower", props);
        
        NormalsStrength = ShaderGUI.FindProperty("_Normalsstrength", props);

        ShallowWaterDistortion = ShaderGUI.FindProperty("_Shallowwaterdistortion", props);


        #endregion

        // Foam 변수 지정
        #region [물거품]
        FoamColor = FindProperty("_Foamcolor", props);

        FoamAmount = FindProperty("_Foamamount", props);

        FoamCutoff = ShaderGUI.FindProperty("_Foamcutoff", props);

        FoamDirection = ShaderGUI.FindProperty("_Foamdirection", props);

        FoamScale = ShaderGUI.FindProperty("_Foamscale", props);
    

        #endregion

        // SufaceFoam 변수 지정
        #region [물표면거품]

        SufaceFoamSpeedSwitch = FindProperty("_SUFACEFOAMSPEEDSWITCH", props);

        SufaceFoamTexture = FindProperty("_Sufacefoamtexture", props);

        SufaceFoamDistortion = FindProperty("_Sufacefoamdistortion", props);

        SufaceFoamIntansity = FindProperty("_Sufacefoamintansity", props);

        SufaceFoamUV = FindProperty("_Sufacefoamtexture_ST", props);

        SufaceGradientColor = FindProperty("_Sufacegradientcolor", props);    

        SufaceGradientColorIntensity = FindProperty("_Sufacegradientcolorintensity", props);

        #endregion

   
    }


    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {


        FindProperties(props); 

        m_MaterialEditor = materialEditor;

        Material material = materialEditor.target as Material;


        //물         

         EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Water_Foldout = Foldout(_Water_Foldout, "물");

         if (_Water_Foldout)
         {
            //EditorGUI.indentLevel++;

            GUI_Water();

            //EditorGUI.indentLevel--;
         }
         EditorGUILayout.EndVertical();
            

        // 물거품
           
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Foam_Foldout = Foldout(_Foam_Foldout, "물거품");

        if (_Foam_Foldout)
        {
            //EditorGUI.indentLevel++;
                                
            GUI_Foam();

            //EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
            
       

        // 물표면거품
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _SufaceFoam_Foldout = Foldout(_SufaceFoam_Foldout, "물표면거품");

        if (_SufaceFoam_Foldout)
        {
            //EditorGUI.indentLevel++;
                                
            GUI_SufaceFoam();

            //EditorGUI.indentLevel--;
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

        // 물 메소드
        #region [물]

        public void GUI_Water()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.ShaderProperty(DeepWaterColor, "물 컬러");
            m_MaterialEditor.ShaderProperty(ShallowWaterColor, "물에 잠긴 부분 컬러");
            m_MaterialEditor.ShaderProperty(WaterDepth, "물 잠긴 부분 그라데이션 조정");
            m_MaterialEditor.ShaderProperty(ShallowWaterDistortion, "물 잠긴 부분 웨이브");
            //m_MaterialEditor.TexturePropertySingleLine(new GUIContent("메인텍스쳐"), mainTex_Sampler);
            //m_MaterialEditor.TextureScaleOffsetProperty(mainTex_Sampler);
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.ShaderProperty(WaveDirection, "물 웨이브 방향");
            m_MaterialEditor.ShaderProperty(WaveSize, "물 웨이브 사이즈");
            m_MaterialEditor.ShaderProperty(NormalsStrength, "물 웨이브 크기");
            m_MaterialEditor.ShaderProperty(FresnelPower, "물 표면에 색상 조정");
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        } 
        #endregion

        // 물거품 메소드
        #region [물거품]
        void GUI_Foam()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.ShaderProperty(FoamColor, "물거품 색상");
            m_MaterialEditor.ShaderProperty(FoamAmount, "물거품 영역 크기");
            m_MaterialEditor.ShaderProperty(FoamCutoff, "물거품 적용 정도");   
            m_MaterialEditor.ShaderProperty(FoamDirection, "물거품 흐르는 방향");  
            m_MaterialEditor.ShaderProperty(FoamScale, "물거품 크기");  
            EditorGUILayout.EndVertical();
                
        }
        #endregion

        // 물표면거품 메소드
        #region [물표면거품]
        void GUI_SufaceFoam()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.TexturePropertySingleLine(new GUIContent("포말텍스쳐"), SufaceFoamTexture);
            m_MaterialEditor.TextureScaleOffsetProperty(SufaceFoamTexture);
            m_MaterialEditor.ShaderProperty(SufaceFoamSpeedSwitch, "포말 옵셋과스피드 선택");
            m_MaterialEditor.ShaderProperty(SufaceFoamIntansity, "포말 세기");
            m_MaterialEditor.ShaderProperty(SufaceFoamDistortion, "포말 웨이브 세기");
            EditorGUILayout.EndVertical(); 
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.TexturePropertySingleLine(new GUIContent("물표면컬러텍스쳐"), SufaceGradientColor);
            m_MaterialEditor.ShaderProperty(SufaceGradientColorIntensity, "물표면컬러세기");
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