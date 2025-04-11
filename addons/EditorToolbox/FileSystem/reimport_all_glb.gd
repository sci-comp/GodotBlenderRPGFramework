@tool
extends EditorPlugin

var popup_instance : PopupPanel
var vbox : VBoxContainer
var title : Label
var checkbox_generate_lods : CheckBox
var ok_button : Button

signal done

func execute():
	popup_instance = PopupPanel.new()
	add_child(popup_instance)
	popup_instance.popup_centered(Vector2(300, 100))

	vbox = VBoxContainer.new()
	popup_instance.add_child(vbox)

	var title = Label.new()
	title.text = "Reimport GLB Files"
	vbox.add_child(title)

	# Generate LODs
	checkbox_generate_lods = CheckBox.new()
	checkbox_generate_lods.text = "Generate lods"
	vbox.add_child(checkbox_generate_lods)

	ok_button = Button.new()
	ok_button.text = "Reimport"
	ok_button.pressed.connect(_on_ok_pressed)
	vbox.add_child(ok_button)

func _on_ok_pressed():
	var selected_paths = get_editor_interface().get_selected_paths()
	var editor_file_system = get_editor_interface().get_resource_filesystem()

	var files_to_reimport = PackedStringArray()

	for path in selected_paths:
		if path.ends_with(".glb"):
			var import_path = path + ".import"
			
			var file = FileAccess.open(import_path, FileAccess.READ)
			var text = file.get_as_text()
			file.close()
			
			if "meshes/generate_lods=true" in text:
				if !checkbox_generate_lods.is_pressed():
					text = text.replace("meshes/generate_lods=true", "meshes/generate_lods=false")
			elif "meshes/generate_lods=false" in text:
				if checkbox_generate_lods.is_pressed():
					text = text.replace("meshes/generate_lods=false", "meshes/generate_lods=true")
			
			file = FileAccess.open(import_path, FileAccess.WRITE)
			file.store_string(text)
			file.close()
			
			files_to_reimport.append(path)

		editor_file_system.reimport_files(files_to_reimport)

	popup_instance.queue_free()
	emit_signal("done")





