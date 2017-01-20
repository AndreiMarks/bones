using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using System;


namespace UnityDesigner
{
	[InitializeOnLoad]
	public class SelectionEditor  {

		static string lastHash;
		static Shape[] selection = new Shape[0];
		static Bounds bounds;

		public static event Action onSelectionChangeDelegate;

		static SelectionEditor()
		{
			SceneView.onSceneGUIDelegate -= OnScene;
			SceneView.onSceneGUIDelegate += OnScene;
		}


		static void OnScene(SceneView SceneView)
		{	
			if( SelectionHasChanged() )
			{
				UpdateSelection();
				FireEvent();
			}
			
		}

		static void FireEvent()
		{
			if( onSelectionChangeDelegate != null )
			{
				onSelectionChangeDelegate();
			}
		}

		static void UpdateSelection()
		{
			UnityEngine.Object[] arr = Selection.GetFiltered(typeof(Shape), SelectionMode.Editable | SelectionMode.TopLevel );
			selection = System.Array.ConvertAll<UnityEngine.Object, Shape>( arr, o => (Shape)o );
		}

		static bool SelectionHasChanged()
		{

			string currentHash = GetSelectionHash();
			if( lastHash != currentHash )
			{
				lastHash = currentHash;
				return true;
			} else {
				return false;
			}
		}

		public static string GetSelectionHash()
		{
			return System.String.Join( "," , Selection.instanceIDs.Select( x => x.ToString() ).ToArray() );
		}

		public static bool hasSelection
		{
			get
			{
				return selection.Length > 0 ;
			}
		}

		
		
		public static Shape[] GetSelectedShapes()
		{
			return selection;
		}



		public static Bounds GetSelectedBounds()
		{
			foreach( Shape shape in selection )
			{
				if( shape == null )
				{
					UpdateSelection();
					continue;
				}
			}

			bounds = new Bounds();
			if( selection.Length > 0 )
			{
				bounds = selection[0].bounds;

				foreach( Shape shape in selection )
				{
					if( shape != null ) bounds.Encapsulate( shape.bounds );
				}
			}

			return bounds;
		}

	}
}