using UnityEngine;
using System.Collections;

namespace UnityDesigner
{

	[System.Serializable]
	public class ShapeFill
	{
		public bool isVisible = true;
		public ShapeFillType fillType = ShapeFillType.Solid;
		public ShapeFillTypeAdvanced fillTypeAdvanced = ShapeFillTypeAdvanced.None;
		public ShapeBlendMode blendMode = ShapeBlendMode.Normal;
		public bool isLit = false;
		public Color color = Color.white;
		public Color secondaryColor = Color.white;
		public Texture2D texture;
		public Gradient gradient = new Gradient();
		public GradientType gradientType = GradientType.Linear;

		[Range(0,360)]
		public float gradientAngle;



		[SerializeField]
		[Range(0,1)]
		private float _opacity = 1.0f;




		public bool IsEquivalent( ShapeFill fill )
		{
			return 
				this.fillType == fill.fillType && 
				this.fillTypeAdvanced == fill.fillTypeAdvanced && 
				this.blendMode == fill.blendMode &&
				this.isLit == fill.isLit &&
				this.texture == fill.texture;
		}

		public ShapeFill Copy()
		{
			ShapeFill fill = new ShapeFill();
			
			fill.fillType = this.fillType; 
			fill.fillTypeAdvanced = this.fillTypeAdvanced; 
			fill.blendMode = this.blendMode;
			fill.isLit = this.isLit;
			fill.texture = this.texture;

			return fill;
		}


		public Color GetGradientColor( Vector3 v, Vector2 size )
		{
			float t = 0;

			switch( gradientType )
			{
				case GradientType.Linear:
					Vector2 v2 = new Vector2( v.x / size.x, v.y / size.y);
					Vector2 angle = new Vector2( Mathf.Cos( gradientAngle * Mathf.Deg2Rad ), Mathf.Sin( gradientAngle * Mathf.Deg2Rad ) );
					t = 0.5f + Vector2.Dot( v2, angle );
				break;

				case GradientType.Radial:
					t = new Vector2(v.x, v.y).magnitude;
				break;
			}
			return gradient.Evaluate( t );
		}

		public float opacity 
		{
			 get 
			 { 
					return _opacity; 
			 }
			 set 
			 {
					value = Mathf.Clamp(value , 0.0f,1.0f);
					_opacity = value; 
			 }
		}
		
		public Material material
		{
			get
			{
				return ShapeMaterial.RequestMaterial( this );
			}
		}

	}

	public enum ShapeFillType
	{
		Solid,
		Gradient,
		Pattern,
		Texture
	}

	public enum GradientType
	{
		Linear,
		Radial
	}

	public enum ShapeFillTypeAdvanced
	{
		None,
		Metallic,
		Iridescent,
		Glow,
		Chrome
	}

	public enum ShapeBlendMode
	{
		Normal,
		Additive,
		Screen,
		Overlay,
		Multiply
	}

}