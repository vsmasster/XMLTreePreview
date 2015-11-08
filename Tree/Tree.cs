using System;
using System.Collections.Generic;
using System.Linq;

namespace Tree {
	public class Node<T> {
		private readonly T data;
		private readonly IEnumerable<Node<T>> children;
		public T Data {
			get { return data; }
		}
		public IEnumerable<Node<T>> Children {
			get { return children; }
		}
		//public TTarget Clone<TTarget>(Func<Node<T>, IEnumerable<TTarget>, TTarget> create) { }
		public Node(T data, IEnumerable<Node<T>> children) {
			this.data = data;
			this.children = children;
		}
		public Node(T data) : this(data, Enumerable.Empty<Node<T>>()) { }
	}

	public static class Tree {
		public static IEnumerable<Node<T>> Traverse<T>(this Node<T> node) {
			yield return node;
			foreach (Node<T> child in node.Children)
				foreach (Node<T> item in child.Traverse())
					yield return item;
		}

		public static Node<T> Transform<T>(this Node<T> node, Func<IEnumerable<Node<T>>, IEnumerable<Node<T>>> transform){
			return new Node<T>(node.Data, transform(node.Children).Select(n => Transform(n, transform)));
		}
	}
}
