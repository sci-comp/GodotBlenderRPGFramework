@tool
extends EditorPlugin

var editor_utilities = preload("res://addons/EditorToolbox/editor_toolbox_utilities.gd").new()

func execute():
	var editor : EditorInterface = get_editor_interface()
	var selected_paths : PackedStringArray = editor.get_selected_paths()

	var count : int = 0
	for path in selected_paths:
		count += 1
	
	print("Selected paths: ", count)
