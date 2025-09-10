// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX/VFXShaderUI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        [Enum(Add,1,Alpha,10)]_Dst("BlendMode", Float) = 1
        [Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 0
        [Enum(Less or Equal,4,Always,8)]_ZTestMode("ZTest", Float) = 4
        [Enum(ON,1,OFF,0)]_Zwrite("Zwrite", Float) = 0
        [HDR]_Main_Tex_Color("Main_Tex_Color", Color) = (1,1,1,1)
        [Header(Main_Tex)]_MainTexture("MainTexture", 2D) = "white" {}
        [Enum(R,0,A,1)]_Main_Tex_A_R("Main_Tex_A_R", Float) = 0
        [Enum(Repeat,0,Clmap,1)]_Main_Tex_ClampSwitch("Main_Tex_ClampSwitch", Float) = 0
        _Main_Tex_Rotator("Main_Tex_Rotator", Range( 0 , 360)) = 0
        [Enum(OFF,0,ON,1)]_Main_Tex_Custom_ZW("Main_Tex_Custom_ZW", Float) = 0
        _Main_Tex_U_speed("Main_Tex_U_speed", Float) = 0
        _Main_Tex_V_speed("Main_Tex_V_speed", Float) = 0
        [Header(Mask_Tex)]_Mask_Tex("Mask_Tex", 2D) = "white" {}
        [Enum(R,0,A,1)]_Mask_Tex_A_R("Mask_Tex_A_R", Float) = 0
        [Enum(Repeat,0,Clmap,1)]_Mask_Tex_ClampSwitch("Mask_Tex_ClampSwitch", Float) = 0
        _Mask_Tex_Rotator("Mask_Tex_Rotator", Range( 0 , 360)) = 0
        _Mask_Tex_U_speed("Mask_Tex_U_speed", Float) = 0
        _Mask_Tex_V_speed("Mask_Tex_V_speed", Float) = 0
        [Header(Distortion_Tex)]_Noise_Tex("Noise_Tex", 2D) = "white" {}
        [Enum(R,0,A,1)]_Noise_Tex_A_R("Noise_Tex_A_R", Float) = 0
        _Noise_Tex_Power("Noise_Tex_Power", Float) = 0
        _Noise_Tex_U_speed("Noise_Tex_U_speed", Float) = 0
        _Noise_Tex_V_speed("Noise_Tex_V_speed", Float) = 0
        [Header(Dissolve_Tex)]_Dissolve_Tex("Dissolve_Tex", 2D) = "white" {}
        [Toggle(_DISSOLVE_SWITCH_ON)] _Dissolve_Switch("Dissolve_Switch", Float) = 0
        [Enum(R,0,A,1)]_Dissolve_Tex_A_R("Dissolve_Tex_A_R", Float) = 0
        [Enum(OFF,0,ON,1)]_Dissolve_Tex_Custom("Dissolve_Tex_Custom", Float) = 0
        _Dissolve_Tex_Rotator("Dissolve_Tex_Rotator", Range( 0 , 360)) = 0
        _Dissolve_Tex_smooth("Dissolve_Tex_smooth", Range( 0.5 , 1)) = 0.5
        _Dissolve_Tex_power("Dissolve_Tex_power", Range( 0 , 1)) = 0
        _Dissolve_Tex_U_speed("Dissolve_Tex_U_speed", Float) = 0
        _Dissolve_Tex_V_speed("Dissolve_Tex_V_speed", Float) = 0

    }

    SubShader
    {
		LOD 0

        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Stencil
        {
        	Ref [_Stencil]
        	ReadMask [_StencilReadMask]
        	WriteMask [_StencilWriteMask]
        	Comp [_StencilComp]
        	Pass [_StencilOp]
        }


        Cull [_CullMode]
        Lighting Off
        ZWrite [_Zwrite]
        ZTest [_ZTestMode]
        Blend SrcAlpha [_Dst], SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        
        Pass
        {
            Name "Default"
        CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityShaderVariables.cginc"
            #define ASE_NEEDS_FRAG_COLOR
            #pragma shader_feature _DISSOLVE_SWITCH_ON


            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4  mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
                
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            uniform float _Dst;
            uniform float _CullMode;
            uniform float _ZTestMode;
            uniform float _Zwrite;
            uniform float4 _Main_Tex_Color;
            uniform sampler2D _MainTexture;
            uniform float _Main_Tex_U_speed;
            uniform float _Main_Tex_V_speed;
            uniform float4 _MainTexture_ST;
            uniform float _Main_Tex_Custom_ZW;
            uniform float _Noise_Tex_Power;
            uniform sampler2D _Noise_Tex;
            uniform float _Noise_Tex_U_speed;
            uniform float _Noise_Tex_V_speed;
            uniform float4 _Noise_Tex_ST;
            uniform float _Noise_Tex_A_R;
            uniform float _Main_Tex_Rotator;
            uniform float _Main_Tex_ClampSwitch;
            uniform float _Main_Tex_A_R;
            uniform sampler2D _Mask_Tex;
            uniform float _Mask_Tex_U_speed;
            uniform float _Mask_Tex_V_speed;
            uniform float _Mask_Tex_Rotator;
            uniform float _Mask_Tex_ClampSwitch;
            uniform float _Mask_Tex_A_R;
            uniform float _Dissolve_Tex_smooth;
            uniform float _Dissolve_Tex_Custom;
            uniform sampler2D _Dissolve_Tex;
            uniform float _Dissolve_Tex_U_speed;
            uniform float _Dissolve_Tex_V_speed;
            uniform float4 _Dissolve_Tex_ST;
            uniform float _Dissolve_Tex_Rotator;
            uniform float _Dissolve_Tex_A_R;
            uniform float _Dissolve_Tex_power;

            
            v2f vert(appdata_t v )
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                

                v.vertex.xyz +=  float3( 0, 0, 0 ) ;

                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.texcoord = v.texcoord;
                OUT.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN ) : SV_Target
            {
                //Round up the alpha color coming from the interpolator (to 1.0/256.0 steps)
                //The incoming alpha could have numerical instability, which makes it very sensible to
                //HDR color transparency blend, when it blends with the world's texture.
                const half alphaPrecision = half(0xff);
                const half invAlphaPrecision = half(1.0/alphaPrecision);
                IN.color.a = round(IN.color.a * alphaPrecision)*invAlphaPrecision;

                float2 appendResult49 = (float2(( _Main_Tex_U_speed * _Time.y ) , ( _Time.y * _Main_Tex_V_speed )));
                float2 uv_MainTexture = IN.texcoord.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
                float4 texCoord21 = IN.worldPosition.xyzw;
                texCoord21.xy = IN.worldPosition.xyzw.xy * float2( 1,1 ) + float2( 0,0 );
                float2 appendResult33 = (float2(texCoord21.z , texCoord21.w));
                float2 lerpResult95 = lerp( ( appendResult49 + uv_MainTexture ) , ( uv_MainTexture + appendResult33 ) , _Main_Tex_Custom_ZW);
                float2 appendResult18 = (float2(_Noise_Tex_U_speed , _Noise_Tex_V_speed));
                float2 uv_Noise_Tex = IN.texcoord.xy * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
                float2 panner41 = ( 1.0 * _Time.y * appendResult18 + uv_Noise_Tex);
                float4 tex2DNode46 = tex2D( _Noise_Tex, panner41 );
                float lerpResult254 = lerp( tex2DNode46.r , tex2DNode46.a , _Noise_Tex_A_R);
                float2 ONE121 = ( lerpResult95 + ( _Noise_Tex_Power * (-0.5 + (lerpResult254 - 0.0) * (0.5 - -0.5) / (1.0 - 0.0)) ) );
                float cos153 = cos( ( ( _Main_Tex_Rotator * UNITY_PI ) / 180.0 ) );
                float sin153 = sin( ( ( _Main_Tex_Rotator * UNITY_PI ) / 180.0 ) );
                float2 rotator153 = mul( ONE121 - float2( 0.5,0.5 ) , float2x2( cos153 , -sin153 , sin153 , cos153 )) + float2( 0.5,0.5 );
                float2 lerpResult173 = lerp( rotator153 , saturate( rotator153 ) , _Main_Tex_ClampSwitch);
                float4 tex2DNode178 = tex2D( _MainTexture, lerpResult173 );
                float lerpResult186 = lerp( tex2DNode178.r , tex2DNode178.a , _Main_Tex_A_R);
                float2 appendResult88 = (float2(_Mask_Tex_U_speed , _Mask_Tex_V_speed));
                float2 texCoord91 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
                float2 panner111 = ( 1.0 * _Time.y * appendResult88 + texCoord91);
                float cos117 = cos( ( ( _Mask_Tex_Rotator * UNITY_PI ) / 180.0 ) );
                float sin117 = sin( ( ( _Mask_Tex_Rotator * UNITY_PI ) / 180.0 ) );
                float2 rotator117 = mul( panner111 - float2( 0.5,0.5 ) , float2x2( cos117 , -sin117 , sin117 , cos117 )) + float2( 0.5,0.5 );
                float2 lerpResult146 = lerp( rotator117 , saturate( rotator117 ) , _Mask_Tex_ClampSwitch);
                float4 tex2DNode161 = tex2D( _Mask_Tex, lerpResult146 );
                float lerpResult170 = lerp( tex2DNode161.r , tex2DNode161.a , _Mask_Tex_A_R);
                float two180 = lerpResult170;
                float UV1V250 = texCoord21.y;
                float lerpResult251 = lerp( _Dissolve_Tex_smooth , UV1V250 , _Dissolve_Tex_Custom);
                float2 appendResult220 = (float2(_Dissolve_Tex_U_speed , _Dissolve_Tex_V_speed));
                float2 uv_Dissolve_Tex = IN.texcoord.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
                float cos219 = cos( ( ( _Dissolve_Tex_Rotator * UNITY_PI ) / 180.0 ) );
                float sin219 = sin( ( ( _Dissolve_Tex_Rotator * UNITY_PI ) / 180.0 ) );
                float2 rotator219 = mul( uv_Dissolve_Tex - float2( 0.5,0.5 ) , float2x2( cos219 , -sin219 , sin219 , cos219 )) + float2( 0.5,0.5 );
                float2 panner221 = ( 1.0 * _Time.y * appendResult220 + rotator219);
                float4 tex2DNode226 = tex2D( _Dissolve_Tex, panner221 );
                float lerpResult229 = lerp( tex2DNode226.r , tex2DNode226.a , _Dissolve_Tex_A_R);
                float UV1U56 = texCoord21.x;
                float lerpResult230 = lerp( _Dissolve_Tex_power , UV1U56 , _Dissolve_Tex_Custom);
                float smoothstepResult240 = smoothstep( ( 1.0 - lerpResult251 ) , lerpResult251 , saturate( ( ( lerpResult229 + 1.0 ) - ( lerpResult230 * 2.0 ) ) ));
                #ifdef _DISSOLVE_SWITCH_ON
                float staticSwitch242 = smoothstepResult240;
                #else
                float staticSwitch242 = 1.0;
                #endif
                float Three245 = staticSwitch242;
                

                half4 color = ( ( IN.color * ( _Main_Tex_Color * tex2DNode178 ) ) * ( IN.color.a * _Main_Tex_Color.a * lerpResult186 * two180 * Three245 ) );

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.rgb *= color.a;

                return color;
            }
        ENDCG
        }
    }
    CustomEditor "UICustomShaderGUI"
	
	Fallback Off
}
/*ASEBEGIN
Version=19200
Node;AmplifyShaderEditor.CommentaryNode;248;65.19406,-276.2867;Inherit;False;260;427.1951;Comment;4;207;204;206;205;;1,0,0.8035207,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;12;-2549.29,-917.1772;Inherit;False;1247.404;493.7412;Noise;14;116;51;16;15;46;18;79;52;41;32;29;24;253;254;;1,0.004716992,0.004716992,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;17;-2558.614,-1476.385;Inherit;False;1131.677;554.4718;MainUV;15;56;21;75;30;25;95;68;65;59;49;42;36;31;33;250;;1,0.8624264,0,1;0;0
Node;AmplifyShaderEditor.WireNode;24;-2149.29,-645.1771;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;31;-2516.614,-1332.385;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;32;-2309.29,-709.1773;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-2260.614,-1396.384;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;41;-2213.29,-853.1772;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-2260.614,-1284.384;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;49;-2116.613,-1364.385;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;54;-3269.309,-406.9744;Inherit;False;1727.596;468.6674;MASK;17;167;180;170;136;132;88;109;103;63;87;161;146;117;111;91;77;71;;0.6913667,0,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-1844.61,-1220.384;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;-1844.61,-1332.385;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;86;-1525.527,-402.9807;Inherit;False;447.1464;476.3351;MainRotator;6;122;153;139;130;125;108;;0.283765,1,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;91;-3237.309,-374.9744;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;95;-1604.61,-1332.385;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;111;-2949.309,-358.9744;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;117;-2773.309,-358.9744;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;125;-1477.527,-34.98075;Inherit;False;Constant;_Float6;Float 6;13;0;Create;True;0;0;0;False;0;False;180;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;130;-1285.527,-50.98075;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;144;-1061.29,-341.1774;Inherit;False;317.8248;263.5372;Clamp;2;173;166;;0,1,0.8728237,1;0;0
Node;AmplifyShaderEditor.LerpOp;146;-2421.309,-358.9744;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;166;-1045.29,-245.1775;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;173;-885.2905,-309.1775;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-341.2903,-421.1774;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;209;-3192.756,93.35671;Inherit;False;2326.713;542.8096;Dissolve;33;245;242;216;226;224;240;238;237;236;234;233;232;231;230;229;228;227;225;223;222;221;220;219;218;217;215;214;213;212;211;210;251;252;;0,0.7555048,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-3173.386,546.3824;Inherit;False;Constant;_Float13;Float 13;13;0;Create;True;0;0;0;False;0;False;180;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;212;-2945.549,478.5375;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;213;-2741.019,483.6085;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;214;-2629.428,370.5771;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;215;-2939.428,349.5771;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;219;-2927.641,154.9044;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;220;-2882.575,280.196;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;221;-2715.649,159.151;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;227;-2374.567,519.3452;Inherit;False;Constant;_Float7;Float 7;11;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;228;-2205.881,325.5606;Inherit;False;Constant;_Float3;Float 3;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;229;-2127.189,151.35;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;230;-2394.637,405.1749;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;231;-1940.016,148.9274;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-2224.708,404.7513;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;234;-1832.571,148.606;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;236;-1702.663,147.4494;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;237;-1705.974,234.2118;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;238;-1550.396,133.1141;Inherit;False;Constant;_Float8;Float 8;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;240;-1568.841,207.9575;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;224;-2573.123,446.8323;Inherit;False;56;UV1U;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;216;-3149.967,152.4248;Inherit;False;0;226;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;184;-339.2777,-170.0326;Inherit;False;180;two;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;247;-338.293,-102.2428;Inherit;False;245;Three;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;-99.09818,-443.3436;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;98.46208,-406.5251;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-3248.309,-26.97448;Inherit;False;Constant;_Float11;Float 11;13;0;Create;True;0;0;0;False;0;False;180;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;103;-2964.309,-107.9745;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;109;-2775.309,-42.97446;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;88;-3008.309,-216.9745;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;136;-2583.309,-298.9745;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;189;-616.2905,-666.1773;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-102.616,-333.9647;Inherit;False;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;170;-1937.309,-339.9744;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;180;-1771.309,-340.9744;Inherit;False;two;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;245;-1073.348,204.4869;Inherit;False;Three;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-2247.29,-566.1772;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-1424.796,-816.8579;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-2516.614,-1412.384;Inherit;False;Property;_Main_Tex_U_speed;Main_Tex_U_speed;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2516.614,-1268.384;Inherit;False;Property;_Main_Tex_V_speed;Main_Tex_V_speed;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-1062.29,-60.17749;Inherit;False;Property;_Main_Tex_ClampSwitch;Main_Tex_ClampSwitch;7;1;[Enum];Create;True;0;2;Repeat;0;Clmap;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;205;115.1941,-226.2868;Inherit;False;Property;_Dst;BlendMode;0;1;[Enum];Create;False;0;2;Add;1;Alpha;10;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;121.6791,-141.2552;Inherit;False;Property;_CullMode;CullMode;1;1;[Enum];Create;False;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;206;126.4591,-55.42584;Inherit;False;Property;_ZTestMode;ZTest;2;1;[Enum];Create;False;0;2;Less or Equal;4;Always;8;0;True;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;190;-648.2905,-491.1774;Inherit;False;Property;_Main_Tex_Color;Main_Tex_Color;4;1;[HDR];Create;False;0;0;0;False;0;False;1,1,1,1;1.319508,1.319508,1.319508,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;108;-1525.527,-194.9808;Inherit;False;Property;_Main_Tex_Rotator;Main_Tex_Rotator;8;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;161;-2245.309,-374.9744;Inherit;True;Property;_Mask_Tex;Mask_Tex;12;1;[Header];Create;True;1;Mask_Tex;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;167;-2148.309,-185.9745;Inherit;False;Property;_Mask_Tex_A_R;Mask_Tex_A_R;13;1;[Enum];Create;True;0;2;R;0;A;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-3237.309,-246.9745;Inherit;False;Property;_Mask_Tex_U_speed;Mask_Tex_U_speed;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-3237.309,-166.9745;Inherit;False;Property;_Mask_Tex_V_speed;Mask_Tex_V_speed;17;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-3245.309,-97.9745;Inherit;False;Property;_Mask_Tex_Rotator;Mask_Tex_Rotator;15;0;Create;False;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;226;-2528.26,124.4602;Inherit;True;Property;_Dissolve_Tex;Dissolve_Tex;23;1;[Header];Create;True;1;Dissolve_Tex;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;223;-2396.312,308.7782;Inherit;False;Property;_Dissolve_Tex_A_R;Dissolve_Tex_A_R;25;1;[Enum];Create;True;0;2;R;0;A;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;218;-3175.99,275.3009;Inherit;False;Property;_Dissolve_Tex_U_speed;Dissolve_Tex_U_speed;30;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;217;-3184.105,364.4813;Inherit;False;Property;_Dissolve_Tex_V_speed;Dissolve_Tex_V_speed;31;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;210;-3172.132,472.693;Inherit;False;Property;_Dissolve_Tex_Rotator;Dissolve_Tex_Rotator;27;0;Create;True;0;0;0;False;0;False;0;0;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-2913.167,401.7898;Inherit;False;Property;_Dissolve_Tex_power;Dissolve_Tex_power;29;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2310.786,-1038.837;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;250;-2321.656,-1109.142;Inherit;False;UV1V;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;-2050.711,386.6816;Inherit;False;Property;_Dissolve_Tex_smooth;Dissolve_Tex_smooth;28;0;Create;True;0;0;0;False;0;False;0.5;1;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;252;-1953.397,456.5369;Inherit;False;250;UV1V;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;251;-1762.397,430.5369;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;-2037.29,-885.1772;Inherit;True;Property;_Noise_Tex;Noise_Tex;18;1;[Header];Create;True;1;Distortion_Tex;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;179;-606.1855,-142.2174;Inherit;False;Property;_Main_Tex_A_R;Main_Tex_A_R;6;1;[Enum];Create;False;0;2;R;0;A;1;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;186;-343.5323,-283.0505;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-1514.29,-630.1771;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;52;-1701.29,-585.1771;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.5;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;254;-1740.859,-850.839;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;253;-1935.512,-703.0059;Inherit;False;Property;_Noise_Tex_A_R;Noise_Tex_A_R;19;1;[Enum];Create;False;0;2;R;0;A;1;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1731.29,-657.1773;Inherit;False;Property;_Noise_Tex_Power;Noise_Tex_Power;20;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2533.29,-581.1772;Inherit;False;Property;_Noise_Tex_U_speed;Noise_Tex_U_speed;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2533.29,-501.1774;Inherit;False;Property;_Noise_Tex_V_speed;Noise_Tex_V_speed;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;225;-2645.037,557.2751;Inherit;False;Property;_Dissolve_Tex_Custom;Dissolve_Tex_Custom;26;1;[Enum];Create;True;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;270.7305,-408.2956;Float;False;True;-1;2;UICustomShaderGUI;0;3;VFX/VFXShaderUI;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;2;5;False;;10;True;_Dst;2;5;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;True;True;2;True;_CullMode;False;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;False;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;0;True;_StencilComp;0;True;_StencilOp;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;True;_Zwrite;True;0;True;_ZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.RangedFloatNode;132;-2708.309,-167.9745;Inherit;False;Property;_Mask_Tex_ClampSwitch;Mask_Tex_ClampSwitch;14;1;[Enum];Create;True;0;2;Repeat;0;Clmap;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;242;-1329.514,198.6213;Inherit;False;Property;_Dissolve_Switch;Dissolve_Switch;24;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-2322.614,-1178.384;Inherit;False;UV1U;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;139;-1514.527,-350.9807;Inherit;False;121;ONE;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-2548.614,-1188.384;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;178;-701.2905,-325.1774;Inherit;True;Property;_MainTexture;MainTexture;5;1;[Header];Create;True;1;Main_Tex;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;207;128.1581,37.90856;Inherit;False;Property;_Zwrite;Zwrite;3;1;[Enum];Create;False;0;2;ON;1;OFF;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1956.61,-1108.384;Inherit;False;Property;_Main_Tex_Custom_ZW;Main_Tex_Custom_ZW;9;1;[Enum];Create;False;0;2;OFF;0;ON;1;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-2533.29,-853.1772;Inherit;False;0;46;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;122;-1246.527,-195.9808;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;153;-1307.527,-371.9807;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;-1249.357,-817.902;Inherit;False;ONE;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;59;-2117.913,-1265.784;Inherit;False;0;178;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;24;0;18;0
WireConnection;32;0;24;0
WireConnection;36;0;25;0
WireConnection;36;1;31;0
WireConnection;41;0;29;0
WireConnection;41;2;32;0
WireConnection;42;0;31;0
WireConnection;42;1;30;0
WireConnection;49;0;36;0
WireConnection;49;1;42;0
WireConnection;65;0;59;0
WireConnection;65;1;33;0
WireConnection;68;0;49;0
WireConnection;68;1;59;0
WireConnection;95;0;68;0
WireConnection;95;1;65;0
WireConnection;95;2;75;0
WireConnection;111;0;91;0
WireConnection;111;2;88;0
WireConnection;117;0;111;0
WireConnection;117;2;109;0
WireConnection;130;0;122;0
WireConnection;130;1;125;0
WireConnection;146;0;117;0
WireConnection;146;1;136;0
WireConnection;146;2;132;0
WireConnection;166;0;153;0
WireConnection;173;0;153;0
WireConnection;173;1;166;0
WireConnection;173;2;157;0
WireConnection;196;0;190;0
WireConnection;196;1;178;0
WireConnection;212;0;210;0
WireConnection;213;0;212;0
WireConnection;213;1;211;0
WireConnection;214;0;213;0
WireConnection;215;0;214;0
WireConnection;219;0;216;0
WireConnection;219;2;215;0
WireConnection;220;0;218;0
WireConnection;220;1;217;0
WireConnection;221;0;219;0
WireConnection;221;2;220;0
WireConnection;229;0;226;1
WireConnection;229;1;226;4
WireConnection;229;2;223;0
WireConnection;230;0;222;0
WireConnection;230;1;224;0
WireConnection;230;2;225;0
WireConnection;231;0;229;0
WireConnection;231;1;228;0
WireConnection;232;0;230;0
WireConnection;232;1;227;0
WireConnection;234;0;231;0
WireConnection;234;1;232;0
WireConnection;236;0;234;0
WireConnection;237;0;251;0
WireConnection;240;0;236;0
WireConnection;240;1;237;0
WireConnection;240;2;251;0
WireConnection;191;0;189;0
WireConnection;191;1;196;0
WireConnection;208;0;191;0
WireConnection;208;1;194;0
WireConnection;103;0;63;0
WireConnection;109;0;103;0
WireConnection;109;1;87;0
WireConnection;88;0;77;0
WireConnection;88;1;71;0
WireConnection;136;0;117;0
WireConnection;194;0;189;4
WireConnection;194;1;190;4
WireConnection;194;2;186;0
WireConnection;194;3;184;0
WireConnection;194;4;247;0
WireConnection;170;0;161;1
WireConnection;170;1;161;4
WireConnection;170;2;167;0
WireConnection;180;0;170;0
WireConnection;245;0;242;0
WireConnection;18;0;15;0
WireConnection;18;1;16;0
WireConnection;116;0;95;0
WireConnection;116;1;79;0
WireConnection;161;1;146;0
WireConnection;226;1;221;0
WireConnection;33;0;21;3
WireConnection;33;1;21;4
WireConnection;250;0;21;2
WireConnection;251;0;233;0
WireConnection;251;1;252;0
WireConnection;251;2;225;0
WireConnection;46;1;41;0
WireConnection;186;0;178;1
WireConnection;186;1;178;4
WireConnection;186;2;179;0
WireConnection;79;0;51;0
WireConnection;79;1;52;0
WireConnection;52;0;254;0
WireConnection;254;0;46;1
WireConnection;254;1;46;4
WireConnection;254;2;253;0
WireConnection;0;0;208;0
WireConnection;242;1;238;0
WireConnection;242;0;240;0
WireConnection;56;0;21;1
WireConnection;178;1;173;0
WireConnection;122;0;108;0
WireConnection;153;0;139;0
WireConnection;153;2;130;0
WireConnection;121;0;116;0
ASEEND*/
//CHKSM=1ACEC86F6E1AC8619E166CD3D9847C04494D0AAC