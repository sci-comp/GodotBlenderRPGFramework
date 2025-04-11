// MIT
// Reference: https://github.com/godotengine/godot/issues/77798

shader_type spatial;
uniform sampler2D DEPTH_TEXTURE : hint_depth_texture, repeat_disable, filter_nearest;

uniform vec3 shallow_color : source_color = vec3(0.22, 0.66, 1.0);
uniform vec3 deep_color : source_color = vec3(0.0, 0.25, 0.45);
uniform vec3 foam_color : source_color = vec3(1);
uniform float absorbance : hint_range(0.0, 30.0) = 10.0;
uniform float depth_fade_distance : hint_range(0.0, 10.0) = 1.0;
uniform float foam_amount : hint_range(0.0, 3.0) = 0.2;
uniform float roughness : hint_range(0.0, 1.0) = 0.05;
uniform float wave_speed : hint_range(0.0, 0.2) = 0.005;
uniform vec2 wave_dir1 = vec2(1.0, 0.0);
uniform vec2 wave_dir2 = vec2(0.0, 1.0);
uniform sampler2D normal1 : hint_normal;
uniform sampler2D normal2 : hint_normal;

varying vec3 uv_world_pos;

vec3 screen(vec3 base, vec3 blend)
{
	return 1.0 - (1.0 - base) * (1.0 - blend);
}

void fragment()
{
	// Depth texture magic
	float depth = texture(DEPTH_TEXTURE, SCREEN_UV, 0.0).r;
  	vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);
	vec4 world = INV_VIEW_MATRIX * INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	float depth_texture_y = world.y / world.w;
	float vertex_y = (INV_VIEW_MATRIX * vec4(VERTEX, 1.0)).y;
	float vertical_depth = vertex_y - depth_texture_y;

	// Changes the color of geometry behind it as the water gets deeper
	float depth_fade_blend = exp(-vertical_depth / depth_fade_distance);
	depth_fade_blend = clamp(depth_fade_blend, 0.0, 1.0);

	// Makes the water more transparent as it gets more shallow
	float alpha_blend = -vertical_depth * absorbance;
	alpha_blend = clamp(1.0 - exp(alpha_blend), 0.0, 1.0);

	// Foam
	float foam_blend = clamp(1.0 - (vertical_depth / foam_amount), 0.0, 1.0);
	vec3 foam = foam_blend * foam_color;

	// Mix
	vec3 color_out = mix(deep_color, shallow_color, depth_fade_blend);
	color_out = screen(color_out, foam);

	// Normal
	vec2 offset1 = (TIME * wave_dir1) * wave_speed;
	vec2 offset2 = (TIME * wave_dir2) * wave_speed;
	vec3 normal_blend = mix(texture(normal1, UV + offset1).rgb, texture(normal2, UV + offset2).rgb, 0.5);

	ALBEDO = color_out;
	ALPHA = alpha_blend;
	ROUGHNESS = roughness;
	NORMAL_MAP = normal_blend;
}