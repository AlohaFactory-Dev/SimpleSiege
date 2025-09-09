using System;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

#if UNITY_EDITOR
    public class UICustomShaderGUI : ShaderGUI
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
        static bool _Base_Foldout = true;

        static bool _Maintextures_Foldout = true;

        static bool _Masktextures_Foldout = true;
        
        static bool _Dissolve_Foldout = true;

        static bool _Noise_Foldout = true;

        static bool _Queue_Foldout = false;

        static bool _Tip_Foldout = false;

        static bool _isMainTex = false;

        static bool _isMaskTex = false; 

        MaterialEditor m_MaterialEditor;

        #endregion

        // 메인
        #region [메인]

        MaterialProperty mainTex_Sampler = null;

        MaterialProperty MainRA = null;

        MaterialProperty MainUspeed = null;

        MaterialProperty MainVspeed = null;

        MaterialProperty MainColor = null;
        
        MaterialProperty MainRotator = null;

        MaterialProperty MainCustom = null;

        #endregion

        // 마스크
        #region [마스크]

        MaterialProperty maskTex_Sampler = null;

        MaterialProperty MaskRA = null;

        MaterialProperty MaskUspeed = null;

        MaterialProperty MaskVspeed = null;

        MaterialProperty MaskRotator = null;

        #endregion

        // 디졸브
        #region [디졸브]
        MaterialProperty dissolveTex_Sampler = null;

        MaterialProperty DissolveRA = null;

        MaterialProperty DissolveSwitch = null;

        MaterialProperty DissolveUspeed = null;

        MaterialProperty DissolveVspeed = null;

        MaterialProperty DissolveRotator = null;

        MaterialProperty DissolvePower = null;

        MaterialProperty DissolveSmooth = null;

        MaterialProperty DissolveCustom = null;

        #endregion

        // 노이즈
        #region [노이즈]
        MaterialProperty noiseTex_Sampler = null;

        MaterialProperty NoiseRA = null;

        MaterialProperty NoiseUSpeed = null;

        MaterialProperty NoiseVSpeed = null;

        MaterialProperty NoisePower = null;
        #endregion

        public void FindProperties(MaterialProperty[] props)
        {

            // Main 변수 지정
            #region [메인]
            mainTex_Sampler = FindProperty("_MainTexture", props);

            MainRA = FindProperty("_Main_Tex_A_R", props);
        
            MainUspeed = ShaderGUI.FindProperty("_Main_Tex_U_speed", props);

            MainVspeed = ShaderGUI.FindProperty("_Main_Tex_V_speed", props);

            MainColor = FindProperty("_Main_Tex_Color", props);

            MainRotator = ShaderGUI.FindProperty("_Main_Tex_Rotator", props);

            MainCustom = ShaderGUI.FindProperty("_Main_Tex_Custom_ZW", props);

            #endregion

            // Mask 변수 지정
            #region [마스크]
            maskTex_Sampler = FindProperty("_Mask_Tex", props);

            MaskRA = FindProperty("_Mask_Tex_A_R", props);

            MaskUspeed = ShaderGUI.FindProperty("_Mask_Tex_U_speed", props);
        
            MaskVspeed = ShaderGUI.FindProperty("_Mask_Tex_V_speed", props);

            MaskRotator = ShaderGUI.FindProperty("_Mask_Tex_Rotator", props);

            #endregion

            // Dissolve 변수 지정
            #region [디졸브]
            dissolveTex_Sampler = FindProperty("_Dissolve_Tex", props);

            DissolveSwitch = FindProperty("_Dissolve_Switch", props);

            DissolveRA = FindProperty("_Dissolve_Tex_A_R", props);

            DissolveUspeed = ShaderGUI.FindProperty("_Dissolve_Tex_U_speed", props);
        
            DissolveVspeed = ShaderGUI.FindProperty("_Dissolve_Tex_V_speed", props);

            DissolveRotator = ShaderGUI.FindProperty("_Dissolve_Tex_Rotator", props);

            DissolvePower = ShaderGUI.FindProperty("_Dissolve_Tex_power", props);

            DissolveSmooth = ShaderGUI.FindProperty("_Dissolve_Tex_smooth", props);
        
            DissolveCustom = ShaderGUI.FindProperty("_Dissolve_Tex_Custom", props);

            #endregion

            // Noise 변수 지정
            #region [노이즈 변수]

            noiseTex_Sampler = FindProperty("_Noise_Tex", props);

            NoiseRA = FindProperty("_Noise_Tex_A_R", props);

            NoisePower = ShaderGUI.FindProperty("_Noise_Tex_Power", props);

            NoiseUSpeed = ShaderGUI.FindProperty("_Noise_Tex_U_speed", props);
        
            NoiseVSpeed = ShaderGUI.FindProperty("_Noise_Tex_V_speed", props);

            #endregion
        }


        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {


            FindProperties(props); 

            m_MaterialEditor = materialEditor;

            Material material = materialEditor.target as Material;

            //쉐이더 기본 셋팅
            #region [기본메뉴세팅]
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _Base_Foldout = Foldout(_Base_Foldout, "기본세팅");

            if (_Base_Foldout)
            {
                EditorGUI.indentLevel++;
                
                GUI_Base(material);

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            #endregion

                if (mainTex_Sampler.textureValue != null)
                {

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    _Maintextures_Foldout = Foldout2(_Maintextures_Foldout, "메인");

                    if (_Maintextures_Foldout)
                    {
                        EditorGUI.indentLevel++;

                        EditorGUILayout.Space();

                        GUI_Maintextures(material);

                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    _Maintextures_Foldout = Foldout2(_Maintextures_Foldout, "메인");

                    if(_Maintextures_Foldout)
                    {
                        EditorGUI.indentLevel++;

                        GUI_Maintextures(material);

                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();

                }

                // Mask
                if (maskTex_Sampler.textureValue != null) 
                {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        _Masktextures_Foldout = Foldout(_Masktextures_Foldout, "마스크");

                        if (_Masktextures_Foldout)
                        {
                            EditorGUI.indentLevel++;
                                    
                            GUI_Mask(material);

                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    _Masktextures_Foldout = Foldout2(_Masktextures_Foldout, "마스크");

                    if(_Masktextures_Foldout)
                    {
                        EditorGUI.indentLevel++;

                        GUI_Mask(material);

                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }

                // Dissolve
                if (dissolveTex_Sampler.textureValue != null) 
                {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        _Dissolve_Foldout = Foldout(_Dissolve_Foldout, "디졸브");

                        if (_Dissolve_Foldout)
                        {
                            EditorGUI.indentLevel++;
                                    
                            GUI_Dissolve(material);

                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    _Dissolve_Foldout = Foldout2(_Dissolve_Foldout, "디졸브");

                    if(_Dissolve_Foldout)
                    {
                        EditorGUI.indentLevel++;

                        GUI_Dissolve(material);

                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }

                // Noise
                if (noiseTex_Sampler.textureValue != null) 
                {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        _Noise_Foldout = Foldout(_Noise_Foldout, "노이즈");

                        if (_Noise_Foldout)
                        {
                            EditorGUI.indentLevel++;
                                    
                            GUI_Noise(material);

                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    _Noise_Foldout = Foldout2(_Noise_Foldout, "노이즈");

                    if(_Noise_Foldout)
                    {
                        EditorGUI.indentLevel++;

                        GUI_Noise(material);

                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }
            
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
                
                // 커스텀데이터설명
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                _Tip_Foldout = Foldout(_Tip_Foldout, "커스텀데이터설명");
                {
                    if(_Tip_Foldout)
                    {
                        EditorGUI.indentLevel++;

                        GUI_Tip(material);

                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUILayout.EndVertical();
        
        }
        
        //기본 셋팅 옵션 클래스 
        void GUI_Base(Material material)
        {
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PrefixLabel("블랜딩 모드");  
            
            
                if (material.GetFloat("_Dst") == 1)
                {
                    if(GUILayout.Button("Add", shortButtonStyle))
                    {
                        material.SetFloat("_Dst", 10);
                        material.EnableKeyword("_ISALPHA_ON");
                    }
                }
                else
                {
                    if(GUILayout.Button("Alpha", shortButtonStyle))
                    {
                        material.SetFloat("_Dst", 1);
                        material.DisableKeyword("_ISALPHA_ON");
                    }
                }
            
                EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("컬링모드");

                if(material.GetFloat("_CullMode") == 0)
                {
                    if(GUILayout.Button("OFF", shortButtonStyle))
                    {
                        material.SetFloat("_CullMode", 1);
                    }
                }
                else
                {
                    if(material.GetFloat("_CullMode") == 1)
                    {
                        if (GUILayout.Button("Front", shortButtonStyle))
                        {
                            material.SetFloat("_CullMode", 2);
                        }
                    }

                    else
                    {
                        if(GUILayout.Button("Back", shortButtonStyle))
                        {
                            material.SetFloat("_CullMode", 0);
                        }
                    }
                }
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Z-테스트 모드");

            if(material.GetFloat("_ZTestMode") == 4)
            {
                if(GUILayout.Button("Less or Equal", shortButtonStyle))
                {
                    material.SetFloat("_ZTestMode", 8);
                }
            }
            else
            {
                if(GUILayout.Button("Always", shortButtonStyle))
                {
                    material.SetFloat("_ZTestMode", 4);
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Z-라이트 모드");

            if(material.GetFloat("_Zwrite") == 0)
            {
                if(GUILayout.Button("OFF", shortButtonStyle))
                {
                    material.SetFloat("_Zwrite", 1);
                }
            }
            else
            {
                if(GUILayout.Button("ON", shortButtonStyle))
                {
                    material.SetFloat("_Zwrite", 0);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
            
        }

        // 메인 메소드
        #region [메인]

        public void GUI_Maintextures(Material material)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.TexturePropertySingleLine(new GUIContent("메인텍스쳐"), mainTex_Sampler, MainColor);

                if(mainTex_Sampler.textureValue != null)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(MainRA, "텍스쳐채널선택");
                    _isMainTex = Foldouts(_isMainTex, "메인텍스쳐 옵션");
                        
                        if(_isMainTex)
                        {
                            EditorGUI.indentLevel++;

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("클램프모드");

                            if(material.GetFloat("_Main_Tex_ClampSwitch") == 0)
                            {
                                if(GUILayout.Button("Repeat", shortButtonStyle))
                                {
                                    material.SetFloat("_Main_Tex_ClampSwitch", 1);
                                }
                            }
                            else
                            {
                                if(GUILayout.Button("Clamp", shortButtonStyle))
                                {
                                    material.SetFloat("_Main_Tex_ClampSwitch", 0);
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            GUILayout.Space(5);
                        }
                    EditorGUILayout.EndVertical();
                }

            m_MaterialEditor.TextureScaleOffsetProperty(mainTex_Sampler);
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            m_MaterialEditor.ShaderProperty(MainRotator, "텍스쳐 회전");
            m_MaterialEditor.ShaderProperty(MainCustom, "스피드 커스텀데이터 사용");
            if(material.GetFloat("_Main_Tex_Custom_ZW") == 0)
            {
                 m_MaterialEditor.ShaderProperty(MainUspeed, "U방향 스피드");
                 m_MaterialEditor.ShaderProperty(MainVspeed, "V방향 스피드");
            }
  
            EditorGUILayout.EndVertical();
        } 
        #endregion

        // 마스크 메소드
        #region [마스크]
        void GUI_Mask(Material material)
        {

                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("마스크텍스쳐"), maskTex_Sampler);

                if(maskTex_Sampler.textureValue != null)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(MaskRA, "텍스쳐채널선택");
                    _isMaskTex = Foldouts(_isMaskTex, "마스크텍스쳐 옵션");

                        if(_isMaskTex)
                        {
                            EditorGUI.indentLevel++;

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PrefixLabel("클램프모드");
                        
                            
                            if(material.GetFloat("_Mask_Tex_ClampSwitch") == 0)
                            {
                                if(GUILayout.Button("Repeat", shortButtonStyle))
                                {
                                    material.SetFloat("_Mask_Tex_ClampSwitch", 1);
                                }
                            }
                            else
                            {
                                if(GUILayout.Button("Clamp", shortButtonStyle))
                                {
                                    material.SetFloat("_Mask_Tex_ClampSwitch", 0);
                                }
                            }
                            EditorGUILayout.EndHorizontal();

                        }
                        
                    EditorGUILayout.EndVertical();

                    m_MaterialEditor.TextureScaleOffsetProperty(maskTex_Sampler);
                    GUILayout.Space(5);

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(MaskRotator, "텍스쳐 회전");   
                    m_MaterialEditor.ShaderProperty(MaskUspeed, "U방향 스피드");
                    m_MaterialEditor.ShaderProperty(MaskVspeed, "V방향 스피드");
        
                    EditorGUILayout.EndVertical();
                }
        }
        #endregion

        // 디졸브 메소드
        #region [디졸브]
        void GUI_Dissolve(Material material)
        {
                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("디졸브텍스쳐"), dissolveTex_Sampler);
            
                if(dissolveTex_Sampler.textureValue != null)
                {
                    m_MaterialEditor.TextureScaleOffsetProperty(dissolveTex_Sampler);
                    GUILayout.Space(5);
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(DissolveSwitch, "디졸브작동");
                    if(material.GetFloat("_Dissolve_Switch") == 1)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        m_MaterialEditor.ShaderProperty(DissolveRA, "텍스쳐채널선택");
                        m_MaterialEditor.ShaderProperty(DissolveRotator, "텍스쳐 회전");
                        m_MaterialEditor.ShaderProperty(DissolveUspeed, "U방향 스피드");
                        m_MaterialEditor.ShaderProperty(DissolveVspeed, "V방향 스피드");
                        m_MaterialEditor.ShaderProperty(DissolveCustom, "디졸브커스텀데이터사용");
                        if(material.GetFloat("_Dissolve_Tex_Custom") == 0)
                        {
                            m_MaterialEditor.ShaderProperty(DissolvePower, "디졸브세기");
                            m_MaterialEditor.ShaderProperty(DissolveSmooth, "디졸브부드럽게");
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndVertical();    
                }
        }
        #endregion

        // 노이즈 메소드
        #region [노이즈]
        void GUI_Noise(Material material)
        {
                m_MaterialEditor.TexturePropertySingleLine(new GUIContent("노이즈텍스쳐"), noiseTex_Sampler);
                if(noiseTex_Sampler.textureValue != null)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_MaterialEditor.ShaderProperty(NoiseRA, "텍스쳐채널선택");
                        
                        EditorGUILayout.EndVertical();

                        m_MaterialEditor.TextureScaleOffsetProperty(noiseTex_Sampler);
                        GUILayout.Space(5);

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        m_MaterialEditor.ShaderProperty(NoisePower, "노이즈 세기");
                        m_MaterialEditor.ShaderProperty(NoiseUSpeed, "U방향 스피드");
                        m_MaterialEditor.ShaderProperty(NoiseVSpeed, "V방향 스피드");
                        
                        EditorGUILayout.EndVertical();

                }
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

        // Tip
        #region [Tip]
        void GUI_Tip(Material material)
        {
            style.fontSize = 15;
            style.normal.textColor = new Color(0.3f, 0.8f, 0.4f);
            style.wordWrap = true;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("1.렌더러 커스텀버텍스스트림에 uv2, custom1.xyzw", style);
            GUILayout.Space(5); GUILayout.Label(" 2.custom1.x는 디졸브 세기", style);
            GUILayout.Space(5); GUILayout.Label(" 3.custom2.y는 디졸브 부드럽게", style);
            GUILayout.Space(5); GUILayout.Label(" 4.custom1.zw는 메인 UV 스피드", style);

            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
        }
        #endregion
    }
#endif