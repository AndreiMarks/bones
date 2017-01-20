using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ShapeIcons {

	static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

	public static Texture2D GetIcon( string name )
	{
		Texture2D texture;
		if( !textures.TryGetValue(name, out texture) )
		{
			texture = EditorGUIUtility.Load("Unity Designer/Icons/" + name + ".png") as Texture2D;//Resources.Load("shapes/icons/" + name) as Texture2D;
			textures[name] = texture;
		}

		return texture;
	}
}
