
using System;

namespace Kohina
{
	public enum NodeCategoryKind {
		Generator,
		Mixer,
		Modifier,
		Output,
		Utility
	};
	public class NodeCategoryAttribute: Attribute
	{
		DataType primaryOutput;
		NodeCategoryKind categoryKind;
		public NodeCategoryAttribute(DataType primaryOutput, NodeCategoryKind categoryKind)
		{
			this.primaryOutput = primaryOutput;
			this.categoryKind = categoryKind;
		}
		
		public override string ToString()
		{
			return string.Format("{0} - {1}", this.primaryOutput, this.categoryKind);
		}
		
	}
}
