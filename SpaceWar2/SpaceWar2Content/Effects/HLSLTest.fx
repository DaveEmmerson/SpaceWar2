
float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 1.0f;
float AmbientProportion = 0.25f;

struct VertexShaderInput
{

	float4 Position	: POSITION0;
	float4 Color	: COLOR0;
};

struct VertexShaderOutput
{

	float4 Position : POSITION0;
	float4 Color	: COLOR0;

};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
	output.Color	= input.Color;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return lerp(input.Color, AmbientColor * AmbientIntensity, AmbientProportion);
}

technique TestTechnique
{
    pass StandardPass
    {

		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();

    }
}
