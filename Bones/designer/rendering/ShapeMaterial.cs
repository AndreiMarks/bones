using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace UnityDesigner
{

	[System.Serializable]
	public class ShapeMaterial {
		
		
		[SerializeField]
		ShapeFill fill;
		

		[SerializeField]
		Material generatedMaterial;



		
		public ShapeMaterial( ShapeFill fill )
		{
			this.fill = fill.Copy();
			
			GenerateMaterial();
		}
		

		bool MatchesShapeFillRequirements( ShapeFill fill )
		{
			return this.fill.IsEquivalent( fill );
		}
		
		void GenerateMaterial()
		{

			string shaderString = fill.isLit ? "Basic Diffuse" : "Basic Flat";

			generatedMaterial = new Material( Shader.Find( "Shapes/"+shaderString ) );
			generatedMaterial.name = shaderString + " " + fill.blendMode + " " + fill.fillType;
			generatedMaterial.hideFlags = HideFlags.HideAndDontSave; //for now
			if( fill.fillType == ShapeFillType.Texture ) generatedMaterial.mainTexture = fill.texture;

			// srcMode * srcColor [BLENDOP] destMode * destColor

			switch( fill.blendMode )
			{
				case ShapeBlendMode.Normal:
					generatedMaterial.SetInt( "_BlendOp", (int)BlendOp.Add );
					generatedMaterial.SetInt( "_SrcMode", (int)BlendMode.SrcAlpha );
					generatedMaterial.SetInt( "_DestMode", (int)BlendMode.OneMinusSrcAlpha );
				break;
				
			  	case ShapeBlendMode.Additive:
			  		generatedMaterial.SetInt( "_BlendOp", (int)BlendOp.Add );
			  		generatedMaterial.SetInt( "_SrcMode", (int)BlendMode.One );
					generatedMaterial.SetInt( "_DestMode", (int)BlendMode.One );
			  	break;

			  	case ShapeBlendMode.Screen:
			  		generatedMaterial.SetInt( "_BlendOp", (int)BlendOp.Max );
			  		generatedMaterial.SetInt( "_SrcMode", (int)BlendMode.One );
					generatedMaterial.SetInt( "_DestMode", (int)BlendMode.One );
			  	break;
			  	
			  	case ShapeBlendMode.Overlay:
			  		generatedMaterial.SetInt( "_BlendOp", (int)BlendOp.Add );
			  		generatedMaterial.SetInt( "_SrcMode", (int)BlendMode.OneMinusDstColor );
					generatedMaterial.SetInt( "_DestMode", (int)BlendMode.One );
			  	break;
			  	
		  		case ShapeBlendMode.Multiply:
		  			generatedMaterial.SetInt( "_BlendOp", (int)BlendOp.Add );
		  			generatedMaterial.SetInt( "_SrcMode", (int)BlendMode.DstColor );
					generatedMaterial.SetInt( "_DestMode", (int)BlendMode.Zero );
		  		break;
			}
		}
		
		
		public Material material
		{
			get{ return generatedMaterial; }
		}






		public static List<ShapeMaterial> shapeMaterials = new List<ShapeMaterial>();
		
		public static int NumMaterials
		{
			get
			{
				return shapeMaterials.Count;
			}
		}
		
		public static Material RequestMaterial( ShapeFill fill )
		{
			ShapeMaterial cachedMaterial = null;
			for(int i = 0; i < shapeMaterials.Count; i++ )
			{
				ShapeMaterial material = shapeMaterials[i];
				if( material.MatchesShapeFillRequirements( fill ) )
				{
					cachedMaterial = material;
					break;
				}	
			}
			
			if( cachedMaterial == null )
			{
				cachedMaterial = new ShapeMaterial( fill );
				shapeMaterials.Add( cachedMaterial );
			}
			
			return cachedMaterial.material;
		}

	}
}