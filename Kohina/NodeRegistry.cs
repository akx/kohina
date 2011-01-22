
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kohina
{
	/// <summary>
	/// Description of NodeRegistry.
	/// </summary>
	public class NodeRegistry
	{
		public static NodeRegistry Instance = new NodeRegistry();
		private Dictionary<string, Type> registry = new Dictionary<string, Type>();
		
		public NodeRegistry()
		{
			
		}
		
		public void Populate() {
			Type nodeType = typeof(Node);
			//TODO: Use this or something? Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] asms = new Assembly[]{Assembly.GetExecutingAssembly()};
			foreach(Assembly asm in asms) {
				foreach(Type t in asm.GetTypes()) {
					if(t.IsSubclassOf(nodeType)) {
						registry[t.Name.ToLowerInvariant()] = t;
					}
				}
			}
		}
		
		public Type Get(string name) {
			name = name.ToLowerInvariant();
			return (registry.ContainsKey(name) ? registry[name] : null);
		}
		
		public IEnumerable<Type> Enumerate() {
			foreach(Type t in registry.Values) yield return t;
		}
	}
}
