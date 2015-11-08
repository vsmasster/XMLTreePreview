using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tree;

namespace TreeTesting {
	[TestClass]
	public class TreeTest {
		[TestMethod]
		public void TestTraverse() {
			Node<int> root = new Node<int>(0, new Node<int>[] {
				new Node<int>(1, new Node<int>[] {
					new Node<int>(2),
					new Node<int>(3)
				}),
				new Node<int>(4, new Node<int>[] {
					new Node<int>(5)
				})
			});
			int index = 0;
			foreach (Node<int> node in root.Traverse()) {
				Assert.AreEqual(node.Data, index);
				++index;
			}
			Assert.AreEqual(index, 6);
		}

		[TestMethod]
		public void TestTraverseLarge() {
			const int length = 50000;
			Node<int>[] children = new Node<int>[length];
			for (int i = 0; i < length; ++i) children[i] = new Node<int>(length + i);
			for (int i = length - 1; i > 0; --i) {
				Node<int> node = new Node<int>(i, children);
				children = new Node<int>[] { node };
			}
			Node<int> root = new Node<int>(0, children);
			int index = 0;
			foreach (Node<int> node in root.Traverse()) {
				Assert.AreEqual(node.Data, index);
				++index;
			}
			Assert.AreEqual(index, 2 * length);
		}
	}
}
