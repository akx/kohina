
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Kohina
{
	public delegate T CacheSetter<T>();
	public class MRUCache<T> where T: class {
		Dictionary<string, T> items = new Dictionary<string, T>();
		Dictionary<string, uint> uTimes = new Dictionary<string, uint>();
		int maxItems;
		
		public MRUCache(int maxItems)
		{
			this.maxItems = maxItems;
		}
		
		
		public T Get(string key) {
			T val;
			bool has = items.TryGetValue(key, out val);
			if(!has) return null;
			uTimes[key] = (uint)Environment.TickCount;
			return val;
		}
		
		public T Set(string key, T obj) {
			items[key] = obj;
			if(items.Count > maxItems) {
				Cull();
			}
			return obj;
		}
	
		public T GetOrSet(string key, CacheSetter<T> setter) {
			if(items.ContainsKey(key)) return Get(key);
			return Set(key, setter());
		}
		
		public void Cull() {
			int nExtra = items.Count - maxItems;
			if(nExtra <= 0) return;
			KeyValuePair<string, uint>[] kvps = (new List<KeyValuePair<string, uint>>(uTimes)).ToArray();
			Array.Sort(kvps, (ka, kb) => ka.Value.CompareTo(kb.Value));
			for(int i = 0; i < nExtra; i++) {
				Debug.Print("{0} removing item {1}", this.ToString(), kvps[i].Key);
				items.Remove(kvps[i].Key);
			}
		}
	}

	public static class Caches {
		public static readonly MRUCache<Bitmap> BitmapCache = new MRUCache<Bitmap>(30);
	}
}
