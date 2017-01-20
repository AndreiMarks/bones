using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class SerializedObjects<T> where T : UnityEngine.Object{

	List<SerializedObject> serializedObjects;
	
	public SerializedObjects( T[] objects )
	{
		Type[] types = objects.Select( x => x.GetType() ).Distinct().ToArray();
		serializedObjects = new List<SerializedObject>();
		foreach( Type type in types )
		{
			serializedObjects.Add( new SerializedObject( objects.Where( x => x.GetType() == type ).ToArray() ) );
		}
	}


	public void PropertyField( string name, SerializedPropertyType type )
	{

		if( serializedObjects.Count == 0 ) return;
		
		EditorGUI.showMixedValue = HasMultipleDifferentValues( name, type ); 
		
		EditorGUI.BeginChangeCheck();

		SerializedProperty property = FindProperty( name );

		DisplayField( property, type );

		if( EditorGUI.EndChangeCheck() )
		{
			ApplyModifiedProperty( name, type );
		}

		EditorGUI.showMixedValue = false; 
	}
	
	bool HasMultipleDifferentValues( string name, SerializedPropertyType propertyType )
	{
		
		SerializedProperty checkProperty = FindProperty(name);
		bool differentValues = checkProperty.hasMultipleDifferentValues;

		if( differentValues ) return true;

		for( int i = 1; i < serializedObjects.Count; i ++ )
		{
			SerializedObject obj = serializedObjects[i];
			SerializedProperty property = obj.FindProperty( name );

			differentValues = differentValues || property.hasMultipleDifferentValues;
			if( differentValues ) break;

			differentValues = differentValues || !ArePropertiesEqual( checkProperty, property, propertyType );			
			if( differentValues ) break;

		}
		
		return differentValues;
	}

	public List<T> targetObjects
	{
		get
		{
			List<T> objects = new List<T>();
			foreach( SerializedObject serializedObject in serializedObjects )
			{
				foreach( T obj in serializedObject.targetObjects )
				{
					objects.Add( obj );	
				}
				
			}

			return objects;
		}
		
	}

	public List<SerializedObject> objects
	{
		get
		{
			return serializedObjects;
		}
	}



	void ApplyModifiedProperty( string name, SerializedPropertyType propertyType )
	{
		SerializedProperty property = FindProperty(name);
		property.serializedObject.ApplyModifiedProperties();

		for( int i = 1; i < serializedObjects.Count; i++ )
		{
			SerializedObject obj = serializedObjects[i];
			SerializedProperty prop = obj.FindProperty( name );
			SetPropertiesEqual( property, prop, propertyType );
			obj.ApplyModifiedProperties();
		}
	}



	SerializedProperty FindProperty( string name )
	{
		if( serializedObjects.Count == 0 ) return null;

		SerializedObject obj = serializedObjects[0];

		SerializedProperty property = obj.FindProperty( name );

		return property;
	}

	
	void DisplayField( SerializedProperty a, SerializedPropertyType propertyType )
	{
		EditorGUI.BeginChangeCheck();

		switch( propertyType )
		{
			case SerializedPropertyType.Integer:
			case SerializedPropertyType.LayerMask:
				var intValue = EditorGUILayout.IntField( a.displayName, a.intValue );
				if( EditorGUI.EndChangeCheck() )
				{
					a.intValue = intValue;
				}
			break;

			case SerializedPropertyType.Boolean:
				var boolValue = EditorGUILayout.Toggle( a.displayName, a.boolValue );
				if( EditorGUI.EndChangeCheck() )
				{
					a.boolValue = boolValue;
				}
			break;

			case SerializedPropertyType.Float:
				var floatValue = EditorGUILayout.FloatField( a.displayName, a.floatValue );
				if( EditorGUI.EndChangeCheck() )
				{
					a.floatValue = floatValue;
				}
			break;

			case SerializedPropertyType.String:
				var stringValue = EditorGUILayout.TextField( a.displayName, a.stringValue );
				if( EditorGUI.EndChangeCheck() )
				{
					a.stringValue = stringValue;
				}
			break;

			case SerializedPropertyType.Color:
				var colorValue = EditorGUILayout.ColorField( a.displayName, a.colorValue );
				if( EditorGUI.EndChangeCheck() )
				{
					a.colorValue = colorValue;
				}
			break;

			case SerializedPropertyType.ObjectReference:
				var objectReferenceValue = EditorGUILayout.ObjectField( a.displayName, a.objectReferenceValue,  a.objectReferenceValue.GetType(), true );
				if( EditorGUI.EndChangeCheck() )
				{
					a.objectReferenceValue = objectReferenceValue;
				}
			break;

			case SerializedPropertyType.Enum:
				var enumValueIndex = EditorGUILayout.Popup( a.displayName, a.enumValueIndex, a.enumDisplayNames );
				if( EditorGUI.EndChangeCheck() )
				{
					a.enumValueIndex = enumValueIndex;
				}
			break;

			case SerializedPropertyType.Vector2:
				var vector2Value = EditorGUILayout.Vector2Field( a.displayName, a.vector2Value );
				if( EditorGUI.EndChangeCheck() )
				{
					a.vector2Value = vector2Value;
				}
			break;

			case SerializedPropertyType.Vector3:
				var vector3Value = EditorGUILayout.Vector3Field( a.displayName, a.vector3Value );
				if( EditorGUI.EndChangeCheck() )
				{
					a.vector3Value = vector3Value;
				}
			break;

			case SerializedPropertyType.Vector4:
				var vector4Value = EditorGUILayout.Vector4Field( a.displayName, a.vector4Value );
				if( EditorGUI.EndChangeCheck() )
				{
					a.vector4Value = vector4Value;
				}
			break;

			case SerializedPropertyType.Rect:
				var rectValue = EditorGUILayout.RectField( a.displayName, a.rectValue );
				if( EditorGUI.EndChangeCheck() )
				{
					a.rectValue = rectValue;
				}
			break;

			case SerializedPropertyType.AnimationCurve:
				var animationCurveValue = EditorGUILayout.CurveField( a.displayName, a.animationCurveValue );
				if( EditorGUI.EndChangeCheck() )
				{
					a.animationCurveValue = animationCurveValue;
				}
			break;

			case SerializedPropertyType.Bounds:
				var boundsValue = EditorGUILayout.BoundsField( a.displayName, a.boundsValue );
				if( EditorGUI.EndChangeCheck() )
				{
					a.boundsValue = boundsValue;
				}
			break;
		}

		a.serializedObject.ApplyModifiedProperties();

	}


	bool ArePropertiesEqual( SerializedProperty a, SerializedProperty b, SerializedPropertyType propertyType )
	{
		bool result = false;

		switch( propertyType )
		{
			case SerializedPropertyType.Integer:
			case SerializedPropertyType.LayerMask:
				result = ( a.intValue == b.intValue );
			break;

			case SerializedPropertyType.Boolean:
				result = ( a.boolValue == b.boolValue );
			break;

			case SerializedPropertyType.Float:
				result = ( a.floatValue == b.floatValue );
			break;

			case SerializedPropertyType.String:
				result = ( a.stringValue == b.stringValue );
			break;

			case SerializedPropertyType.Color:
				result = ( a.colorValue == b.colorValue );
			break;

			case SerializedPropertyType.ObjectReference:
				result = ( a.objectReferenceValue == b.objectReferenceValue );
			break;

			case SerializedPropertyType.Enum:
				result = ( a.enumValueIndex == b.enumValueIndex );
			break;

			case SerializedPropertyType.Vector2:
				result = ( a.vector2Value == b.vector2Value );
			break;

			case SerializedPropertyType.Vector3:
				result = ( a.vector3Value == b.vector3Value );
			break;

			case SerializedPropertyType.Vector4:
				result = ( a.vector4Value == b.vector4Value );
			break;

			case SerializedPropertyType.Rect:
				result = ( a.rectValue == b.rectValue );
			break;

			case SerializedPropertyType.AnimationCurve:
				result = ( a.animationCurveValue == b.animationCurveValue );
			break;

			case SerializedPropertyType.Bounds:
				result = ( a.boundsValue == b.boundsValue );
			break;

		}

		return result;
	}



	void SetPropertiesEqual( SerializedProperty a, SerializedProperty b, SerializedPropertyType propertyType )
	{

		switch( propertyType )
		{
			case SerializedPropertyType.Integer:
			case SerializedPropertyType.LayerMask:
				b.intValue = a.intValue;
			break;

			case SerializedPropertyType.Boolean:
				b.boolValue = a.boolValue;
			break;

			case SerializedPropertyType.Float:
				b.floatValue = a.floatValue;
			break;

			case SerializedPropertyType.String:
				b.stringValue = a.stringValue;
			break;

			case SerializedPropertyType.Color:
				b.colorValue = a.colorValue;
			break;

			case SerializedPropertyType.ObjectReference:
				b.objectReferenceValue = a.objectReferenceValue;
			break;

			case SerializedPropertyType.Enum:
				b.enumValueIndex = a.enumValueIndex;
			break;

			case SerializedPropertyType.Vector2:
				b.vector2Value = a.vector2Value;
			break;

			case SerializedPropertyType.Vector3:
				b.vector3Value = a.vector3Value;
			break;

			case SerializedPropertyType.Vector4:
				b.vector4Value = a.vector4Value;
			break;

			case SerializedPropertyType.Rect:
				b.rectValue = a.rectValue;
			break;

			case SerializedPropertyType.AnimationCurve:
				b.animationCurveValue = a.animationCurveValue;
			break;

			case SerializedPropertyType.Bounds:
				b.boundsValue = a.boundsValue;
			break;

		}

		b.serializedObject.ApplyModifiedProperties();

	}



}
