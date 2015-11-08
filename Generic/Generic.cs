using System;
using System.Collections;
using System.Collections.Generic;

namespace Generic {
	public class Stack<T> : IEnumerable<T> {
		private T[] array = new T[10];
		private int size = 0;

		public void Push(T elem) {
			if (size == array.Length) {
				T[] tmp = new T[2 * array.Length];
				Array.Copy(array, tmp, array.Length);
				array = tmp;
			}
			array[size] = elem;
			++size;
		}
		public T Pop() {
			T tmp = array[size - 1];
			array[size - 1] = default(T);
			--size;
			return tmp;
		}
		public T Top {
			get { return array[size - 1]; }
			set { array[size - 1] = value; }
		}
		public bool IsEmpty {
			get { return size == 0; }
		}

		public IEnumerator<T> GetEnumerator() {
			for (int i = size - 1; i >= 0; --i)
				yield return array[i];
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
