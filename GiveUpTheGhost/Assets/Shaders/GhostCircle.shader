Shader "Unlit/GhostCircle"
{
    Properties
    {
        _Color("Ring Color", Color) = (0.0,0.0,0.0,1)
		_Color2("Ring To Character", Color) = (0.0,0.0,0.0,0.0)
		_RadOne("Outer Radius", Float) = 5
		_RadTwo("Inner Radius", Float) = 3
		_Pos("Character Position", Float) = (0,0,0,0)
		_Ang("Angulr Multiplier", Float) = 1
		_Offset("Anguler Offset", Float) = 0
		_OffMult("Offset Multiplier", Float) = 1
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Cull front
		LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            uniform float4 _Color;
            uniform float4 _Color2;
            uniform float _RadOne;
            uniform float _RadTwo;
            uniform float4 _Pos;
            uniform float _Ang;
            uniform float _Offset;
            uniform float _OffMult;

            struct vertexInput {
				float4 vertex : POSITION;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 position_in_world_space : TEXCOORD0;
			};

			vertexOutput vert(vertexInput input)
			{
					vertexOutput output;

					output.pos = UnityObjectToClipPos(input.vertex);
					output.position_in_world_space =
						mul(unity_ObjectToWorld, input.vertex);
					// transformation of input.vertex from object 
					// coordinates to world coordinates;
					return output;
			}

				float4 frag(vertexOutput input) : COLOR
				{
					float dist = distance(input.position_in_world_space.rg,
					_Pos.rg);
					
					float2 position = float2(input.position_in_world_space.x - _Pos.x, 
					    input.position_in_world_space.y - _Pos.y);
					    
					float angle = atan2(position.y, position.x) * 180 / 3.14 * _Ang;
					
					float distPlus = sin(angle + _Offset) * _OffMult;
                    
					float inside1 = step(dist + distPlus, _RadOne);
					float inside2 = step(dist + distPlus, _RadTwo);
					
					float outside = 1 - inside1;
					float inOne = 1 - inside2;
					
					float blank = float4(0,0,angle,angle);
					// computes the distance between the fragment position 
					// and the origin (the 4th coordinate should always be 
					// 1 for points).

					return outside * blank + inside1 * (_Color * inOne + _Color2 * inside2);

				}
            ENDCG
        }
    }
}
