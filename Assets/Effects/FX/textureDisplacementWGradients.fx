sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2); //tex
sampler uImage3 : register(s3); //noise
sampler uImage4 : register(s4);
float uTime;
float offsetX = 1, offsetY = 1;
float2 resolution;
float noiseAdditionFactors[8] = { 0, 1, -0.25, -1, 0.16, 0.51, -0.26, -0.61 }; //leftovers from metaballGradientNoiseTex, no clue what my intent here was

float2 Wrap(float2 uv)
{
    uv %= 1;
    uv += 1;
    uv %= 1;
    return uv;
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 noiseCoords = coords;
    float2 noiseCoords2 = coords;
    float2 noiseCoords3 = coords;
    float2 noiseCoords4 = coords;
    
    noiseCoords.y += uTime * offsetY * noiseAdditionFactors[0];
    noiseCoords.x += uTime * offsetX * noiseAdditionFactors[1];
    //noiseCoords2.y += uTime * offsetY * noiseAdditionFactors[2];
    //noiseCoords2.x += uTime * offsetX * noiseAdditionFactors[3];
    noiseCoords3.y += uTime * offsetY * noiseAdditionFactors[4];
    noiseCoords3.x += uTime * offsetX * noiseAdditionFactors[5];
    //noiseCoords4.y += uTime * offsetY * noiseAdditionFactors[6];
    //noiseCoords4.x += uTime * offsetX * noiseAdditionFactors[7];
    
    noiseCoords = Wrap(noiseCoords);
    //noiseCoords2 = Wrap(noiseCoords2);
    noiseCoords3 = Wrap(noiseCoords3);
    //noiseCoords4 = Wrap(noiseCoords4);
    float4 noise1 = tex2D(uImage3, noiseCoords);
    //float4 noise2 = tex2D(uImage3, noiseCoords2);
    float4 noise3 = tex2D(uImage3, noiseCoords3);
    //float4 noise4 = tex2D(uImage3, noiseCoords4);
    float noiseA = noise1.r - noise3.r;
    float noiseA2 = noise3.r - noise1.r;
    float _uv = coords / resolution;
    float4 c = tex2D(uImage0, coords);
    float ca = max(c.r, max(c.g, c.b));
    coords = Wrap(coords + float2(sin(noiseCoords.x * ca * 0.1f) * _uv * 50, cos(noiseCoords3.y * ca * 0.1f) * _uv * 50));
    c = tex2D(uImage0, coords);
    float a = max(c.r, max(c.g, c.b));
    a *= clamp(-noiseA2 + 1, 0, 1);
    float4 c2 = tex2D(uImage1, float2(a, 0));
    //float4 c3 = tex2D(uImage2, coords);
    //float4 c4 = tex2D(uImage4, float2(a, 0));
    
    return float4(c2.rgb * c.rgb * a * noiseA, ca); //lerp(c2 * a, float4(c3.rgb, a), noiseA * a);
}

technique Technique1
{
    pass textureDisplacementWGradients
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}