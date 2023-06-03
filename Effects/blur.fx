sampler2D inputTexture : register(s0);
float4 renderTargetSize : register(c0);
float bloomThreshold : register(c1);
float bloomIntensity : register(c2);
float4 PS_Blur(float2 texCoord : TEXCOORD0) : COLOR
{
    float4 color = 0.0;
    
    // Horizontal blur
    color += tex2D(inputTexture, texCoord + float2(-1, 0)) * 0.09;
    color += tex2D(inputTexture, texCoord + float2(0, 0)) * 0.11;
    color += tex2D(inputTexture, texCoord + float2(1, 0)) * 0.09;
    
    // Vertical blur
    color += tex2D(inputTexture, texCoord + float2(0, -1)) * 0.11;
    color += tex2D(inputTexture, texCoord + float2(0, 0)) * 0.12;
    color += tex2D(inputTexture, texCoord + float2(0, 1)) * 0.11;
    
    return color;
}
technique Blur
{
    pass P0
    {
        PixelShader = compile ps_2_0 PS_Blur();
    }
}
