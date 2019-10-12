uniform sampler2D TermTex;

void main()
{
	// lookup the pixel in the texture

	vec4 pixel = texture2D(TermTex, gl_TexCoord[0].xy);

	// multiply it by the color
	gl_FragColor = pixel; //vec4(gl_TexCoord[0].xy, 1.0, 1.0);
}