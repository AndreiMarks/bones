using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UnityDesigner
{
	[InitializeOnLoad]
	public class TransformEditor {


		static string selectionHash;
		static Bounds selectionBounds;

		static Transform _transform;
		static float currentRotation;
		
		static bool isTransforming = false;
		static bool isRotating = false;
		static bool isCenterBound = false;
		
		static Bounds currentTransformBounds;
		static Shape[] shapes;

		public static Transform transform
		{
			get
			{
				if (_transform == null)
				{
					_transform = new GameObject("Selection Transform").transform;
					_transform.gameObject.hideFlags = HideFlags.HideAndDontSave;
				}
				return _transform;

			}
		}


		static TransformEditor()
		{
			SceneView.onSceneGUIDelegate -= OnScene;
			SceneView.onSceneGUIDelegate += OnScene;
			SelectionEditor.onSelectionChangeDelegate += OnSelectionChange;
		}


		static void OnScene(SceneView SceneView)
		{	
			if( !SelectionEditor.hasSelection ) return;
			UpdateBounds();
			

			//Rotation
			DrawRotateGUI();
			
			//Size
			DrawSizeGUI();

		}


		static void OnSelectionChange()
		{	
			shapes = SelectionEditor.GetSelectedShapes();
			isTransforming = false;
			isRotating = false;
			UpdateBounds();
		}


		static void UpdateBounds()
		{
			if( isTransforming || isRotating ) return;


			if( shapes.Length == 1 )
			{

				selectionBounds = shapes[0].shapeBounds;
		
				transform.rotation = shapes[0].transform.rotation;
				transform.position = selectionBounds.center;
				transform.localScale = shapes[0].transform.localScale;
			} else {
				selectionBounds = SelectionEditor.GetSelectedBounds();
		
				transform.rotation = Quaternion.identity;
				transform.position = selectionBounds.center;
				transform.localScale = Vector3.one;
			}
			
			selectionBounds.center = Vector3.zero;
		}

		static void DrawRotateGUI()
		{
			Handles.color = Color.white;

			ShapeGUI.NameNextHandle("RotateHandle");

			EditorGUI.BeginChangeCheck();

			Vector3 topCenterWorld = transform.TransformPoint( topCenter );
			Vector3 rotateHandle = topCenterWorld + transform.up * HandleUtility.GetHandleSize(topCenterWorld) *.5f;
			rotateHandle = ShapeGUI.VectorHandle( rotateHandle );
			Handles.DrawLine( rotateHandle, transform.TransformPoint( topCenter ) );

			Plane plane = new Plane( transform.forward, transform.position );
			Ray ray = new Ray( rotateHandle, plane.GetSide(rotateHandle) ? -transform.forward : transform.forward );
			float d = 0;
			plane.Raycast( ray, out d );

			rotateHandle = ray.GetPoint(d); 



			Vector3 rotateVector = (rotateHandle - transform.position).normalized;

			float angle = Vector3.Angle( rotateVector, Vector3.up );
			float snappedAngle = Mathf.Round( angle / 15 ) * 15;

			Vector3 snappedVector = Vector2.up.Rotate( snappedAngle );


			if( Event.current.type == EventType.Used )
			{
				isRotating = EditorGUI.EndChangeCheck();
			}


			if( GUI.changed )
			{
				Quaternion newRotation = Quaternion.LookRotation( transform.forward, Event.current.shift ? snappedVector : rotateVector );

				if( transform.rotation != newRotation )
				{

					Transform[] parents = new Transform[ shapes.Length ];
					for( int i = 0; i < shapes.Length; i++)
					{
						Shape shape = shapes[i];
						parents[i] = shape.transform.parent;
						Undo.SetTransformParent( shape.transform, transform, "Rotate Shapes" );
					}

					Undo.RecordObject( transform, "Rotate Shapes" );
					transform.rotation = newRotation;

					for( int i = 0; i < shapes.Length; i++)
					{
						Shape shape = shapes[i];
						Undo.SetTransformParent( shape.transform, parents[i], "Rotate Shapes" );
					}


				}

				GUI.changed = false;
			}
		}

		static void DrawSizeGUI()
		{
			Handles.matrix = transform.localToWorldMatrix;
			Handles.DrawSolidRectangleWithOutline( new Vector3[]{topLeft, topRight, bottomRight, bottomLeft}, Color.clear, ShapeGUI.shapeColor );
			
			Handles.color = ShapeGUI.shapeColor;
			Bounds previousBounds = selectionBounds;

			bool scaleChanged = false;

			ShapeGUI.NameNextHandle("TopLeft");
			scaleChanged = scaleChanged || ScaleHandle( topLeft, ref selectionBounds, ScaleHandleFlags.MinX | ScaleHandleFlags.MaxY );

			ShapeGUI.NameNextHandle("TopCenter");
			scaleChanged = scaleChanged || ScaleHandle( topCenter, ref selectionBounds, ScaleHandleFlags.MaxY );

			ShapeGUI.NameNextHandle("TopRight");
			scaleChanged = scaleChanged || ScaleHandle( topRight, ref selectionBounds, ScaleHandleFlags.MaxX | ScaleHandleFlags.MaxY );

			ShapeGUI.NameNextHandle("MiddleRight");
			scaleChanged = scaleChanged || ScaleHandle( middleRight, ref selectionBounds, ScaleHandleFlags.MaxX );

			ShapeGUI.NameNextHandle("BottomRight");
			scaleChanged = scaleChanged || ScaleHandle( bottomRight, ref selectionBounds, ScaleHandleFlags.MaxX | ScaleHandleFlags.MinY );

			ShapeGUI.NameNextHandle("BottomCenter");
			scaleChanged = scaleChanged || ScaleHandle( bottomCenter, ref selectionBounds, ScaleHandleFlags.MinY );

			ShapeGUI.NameNextHandle("BottomLeft");
			scaleChanged = scaleChanged || ScaleHandle( bottomLeft, ref selectionBounds, ScaleHandleFlags.MinX | ScaleHandleFlags.MinY );

			ShapeGUI.NameNextHandle("MiddleLeft");
			scaleChanged = scaleChanged || ScaleHandle( middleLeft, ref selectionBounds, ScaleHandleFlags.MinX );

			
			if( Event.current.type == EventType.Used )
			{ 
				if( !isTransforming && GUI.changed)
				{
					currentTransformBounds = selectionBounds;
				}
				
				isTransforming = GUI.changed;
			}
			
			if( isTransforming )
			{
				if( Event.current.alt )
				{
					selectionBounds.center = Vector3.zero;
					isCenterBound = true;
					GUI.changed = true;
					scaleChanged = true;
				} else if( !Event.current.alt && isCenterBound ) {
					selectionBounds = currentTransformBounds;
					isCenterBound = false;
					GUI.changed = true;
					scaleChanged = true;
				}
			}

			Handles.matrix = Matrix4x4.identity;


			if( GUI.changed && scaleChanged )
			{

				Bounds adjustedCurrentBounds = Adjust( selectionBounds );
				Bounds adjustedPreviousBounds = Adjust( previousBounds );

				foreach( Shape shape in shapes )
				{
					Undo.RecordObject( shape, "Resize Shapes" );
					Undo.RecordObject( shape.transform, "Resize Shapes" );
					shape.ResizeToBounds( adjustedPreviousBounds, adjustedCurrentBounds );
				}
				GUI.changed = false;
			}
		}

		static Bounds Adjust( Bounds bounds )
		{
			Bounds result = bounds;

			Vector3 min = result.min;
			Vector3 max = result.max;

			if( min.x > max.x )
			{
				float temp = min.x;
				min.x = max.x;
				max.x = temp; 
			}

			if( min.y > max.y )
			{
				float temp = min.y;
				min.y = max.y;
				max.y = temp;
			}

			result.min = min;
			result.max = max;
			result.center = transform.TransformPoint( result.center );

			return result;
		}


		static bool ScaleHandle( Vector3 position, ref Bounds bounds, ScaleHandleFlags modifyFlags )
		{
			Vector3 newPosition = ShapeGUI.VectorHandle( position );

			if( newPosition == position ) return false;

			Vector3 min = bounds.min;
			Vector3 max = bounds.max;

			if( modifyFlags.Contains(ScaleHandleFlags.MinX) ) min.x = newPosition.x;
			if( modifyFlags.Contains(ScaleHandleFlags.MinY) ) min.y = newPosition.y;
			if( modifyFlags.Contains(ScaleHandleFlags.MaxX) ) max.x = newPosition.x;
			if( modifyFlags.Contains(ScaleHandleFlags.MaxY) ) max.y = newPosition.y;

			if( Event.current.shift && currentTransformBounds.extents != Vector3.zero )
			{
				float yRatio = currentTransformBounds.size.y / currentTransformBounds.size.x;
				float zRatio = currentTransformBounds.size.z / currentTransformBounds.size.x;
				
				Vector3 size = max - min;
				size = new Vector3( size.x, size.x * yRatio, size.x * zRatio );
				
				if( modifyFlags.Contains(ScaleHandleFlags.MinX) ) min.x = max.x - size.x;
				if( modifyFlags.Contains(ScaleHandleFlags.MinY) ) min.y = max.y - size.y;
				if( modifyFlags.Contains(ScaleHandleFlags.MaxX) ) max.x = min.x + size.x;
				if( modifyFlags.Contains(ScaleHandleFlags.MaxY) ) max.y = min.y + size.y;
				
				
				//GUI.changed = true;
			}

			bounds.SetMinMax( min, max );

			return true;
		}

		static string GetSelectionHash()
		{
			Shape[] shapes = SelectionEditor.GetSelectedShapes();
			string hash = "";
			foreach( Shape shape in shapes )
			{
				hash += shape.GetInstanceID();
			}

			return hash;
		}




		static Vector3 topLeft {
			get
			{
				return new Vector3( selectionBounds.min.x, selectionBounds.max.y, 0 );
			}
		}

		static Vector3 topRight {
			get
			{
				return new Vector3( selectionBounds.max.x, selectionBounds.max.y, 0 );		
			}
		}

		static Vector3 bottomRight {
			get
			{
				return new Vector3( selectionBounds.max.x, selectionBounds.min.y, 0 );
			}
		}

		static Vector3 bottomLeft {
			get
			{
				return new Vector3( selectionBounds.min.x, selectionBounds.min.y, 0 );
			}
		}

		static Vector3 topCenter {
			get
			{
				return Vector3.Lerp( topLeft, topRight, .5f );
			}
		}

		static Vector3 bottomCenter {
			get
			{
				return Vector3.Lerp( bottomLeft, bottomRight, .5f );
			}
		}

		static Vector3 middleLeft {
			get
			{
				return Vector3.Lerp( topLeft, bottomLeft, .5f );
			}
		}

		static Vector3 middleRight {
			get
			{
				return Vector3.Lerp( topRight, bottomRight, .5f );
			}
		}



	}

	[System.Flags]
	public enum ScaleHandleFlags
	{
		None = 0,
		MinX = 1 << 0,
		MinY = 1 << 1,
		MaxX = 1 << 2,
		MaxY = 1 << 3
	}


	public static class FlagsExtensions
	{
		public static bool Contains(this System.Enum keys, System.Enum flag)
	    {
	        ulong keysVal = System.Convert.ToUInt64(keys);
	        ulong flagVal = System.Convert.ToUInt64(flag);

	        return (keysVal & flagVal) == flagVal;
	    }
	}

}