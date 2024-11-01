shader_type canvas_item;

uniform float outerRadius : hint_range(0.0, 5.0) = 1.0;
uniform float MainAlpha : hint_range(0.0, 1.0) = 1.0;
uniform float Red : hint_range(0.0, 1.0) = 0.0;
uniform float Green : hint_range(0.0, 1.0) = 0.0;
uniform float Blue : hint_range(0.0, 1.0) = 0.0;

void fragment() {
	float x = abs(UV.x-.5)*2.0;
	float y = abs(UV.y-.5)*2.0;
	float v = (sqrt((x*x)+(y*y))/outerRadius);
	COLOR = vec4(Red,Green,Blue,v*MainAlpha);
}