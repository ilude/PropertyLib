using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

namespace ilude
{
	/// <summary>
	/// Summary description for Properties.
	/// </summary>
	public class Properties : IDictionary
	{
		private const string	DEFAULT_NAMESPACE	= "_default_";
		private const string	COMMENT_CHARACTER	= "#";
		private const string	NAMESPACE_START		= "[";
		private const string	NAMESPACE_CLOSE		= "]";
		private const char		EQUALS_SIGN			= '=';

		private ListDictionary	namespaces = null;
		private ListDictionary	current_namespace;
		private string			current_key;

		public Properties()
		{
			Init();
		}

		public Properties(string filename)
		{
			Init();

			string line;
			StreamReader sr = new StreamReader(filename);

			while((line = sr.ReadLine()) != null)
			{
				line = line.Trim();

				if(line.StartsWith(COMMENT_CHARACTER))
				{
					continue;
				}
				else if(line.IndexOf(COMMENT_CHARACTER) > 0)
				{
					line = line.Substring(0, line.IndexOf(COMMENT_CHARACTER)).Trim();
				}

				if(line.StartsWith(NAMESPACE_START) && line.EndsWith(NAMESPACE_CLOSE))
				{
					current_key = line.Substring(1, line.Length - 1);
					current_namespace	= new ListDictionary();
					namespaces[current_key] = current_namespace;
				}
				else if(line.IndexOf(EQUALS_SIGN) > 0)
				{
					string[] values = line.Split(new char[] { EQUALS_SIGN });
					current_namespace[values[0].Trim()] = values[1].Trim();
				}
				else
				{
					// ignore improperly formatted line
				}
			}

			sr.Close();
		}
	
		/// <summary>
		/// Gets or Sets the current namespace 
		/// </summary>
		public string Namespace
		{
			get
			{
				return current_key;
			}
			set
			{
				current_key = value;
				current_namespace = this.getNamespace(current_key);
			}
		}

		private void Init()
		{
			current_namespace	= new ListDictionary();
			current_key			= DEFAULT_NAMESPACE;
			
			namespaces = new ListDictionary();
			namespaces[current_key] = current_namespace;
		}

		private ListDictionary getNamespace(string _namespace)
		{
			if(namespaces.Contains(_namespace))
			{
				return (ListDictionary)namespaces[_namespace];
			}
			else
			{
				throw new Exception("No such namespace");
			}
		}

		#region IDictionary Members

		public IDictionaryEnumerator GetEnumerator()
		{
			return current_namespace.GetEnumerator();
		}

		public IDictionaryEnumerator GetEnumerator(string _namespace)
		{
			return this.getNamespace(_namespace).GetEnumerator();
		}

		public void Remove(object key)
		{
			current_namespace.Remove(key);
		}

		public void Remove(string _namespace, object key)
		{
			this.getNamespace(_namespace).Remove(key);
		}

		public bool Contains(object key)
		{
			return current_namespace.Contains(key);
		}

		public bool Contains(string _namespace, object key)
		{
			return this.getNamespace(_namespace).Contains(key);
		}

		public void Clear()
		{
			current_namespace.Clear();
		}

		public void Clear(string _namespace)
		{
			this.getNamespace(_namespace).Clear();
		}

		public void Add(object key, object value)
		{
			current_namespace.Add(key, value);
		}

		public void Add(string _namespace, object key, object value)
		{
			this.getNamespace(_namespace).Add(key, value);
		}

		#region Non Namespace Members

		public bool IsReadOnly
		{
			get
			{
				return current_namespace.IsReadOnly;
			}
		}

		public object this[object key]
		{
			get
			{
				return current_namespace[key];
			}
			set
			{
				current_namespace[key] = value;
			}
		}

		public ICollection Values
		{
			get
			{
				return current_namespace.Values;
			}
		}

		public ICollection Keys
		{
			get
			{
				return current_namespace.Keys;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return current_namespace.IsFixedSize;
			}
		}

		#endregion

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return current_namespace.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return current_namespace.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			current_namespace.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return current_namespace.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return current_namespace.GetEnumerator();
		}

		#endregion
	}
}
