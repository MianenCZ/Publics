using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	[TestClass()]
	public class DequeTests
	{

		[TestMethod()]
		public void Insert()
		{
			var d = new Deque<int>();
			d.Insert(0, 1);
			Assert.AreEqual(d[0], 1);
			Assert.AreEqual(d.First, 1);
			Assert.AreEqual(d.Last, 1);
		}

		[TestMethod()]
		public void InsertAtBeginning()
		{
			var d = new Deque<int>();
			d.Insert(0, 1);
			d.Insert(0, 5);
			Assert.AreEqual(d[0], 5);
			Assert.AreEqual(d[1], 1);
			Assert.AreEqual(d.First, 5);
			Assert.AreEqual(d.Last, 1);
		}

		[TestMethod()]
		public void InsertReversing()
		{
			var d = new Deque<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			var q = new List<int> { 1, 8, 6, 4, 2, 0, 2, 3, 4, 5, 6, 7, 8, 9, 1, 3, 5, 7, 9, 10 };

			for (int i = 0; i < 10; i++)
			{
				d.Insert(1, i);
				d.Reverse();
			}
			Assert.IsTrue(ForEachEqual(d, q));
		}

		[TestMethod()]
		public void Count()
		{
			var d = new Deque<int>();
			d.Insert(0, 1);
			d.Insert(0, 5);
			Assert.AreEqual(d.Count, 2);
		}

		[TestMethod()]
		public void Add()
		{
			var d = new Deque<int>();
			d.Insert(0, 1);
			d.Add(5);
			Assert.AreEqual(5, d.Last);
		}

		[TestMethod()]
		public void ForEach()
		{
			var d = new Deque<int> { -10, -9, -8, -7, -6 };
			var items = new List<int> { -10, -9, -8, -7, -6 };
			
			int Counter = 0;
			foreach (var item in d)
			{
				Assert.IsTrue(item == items[Counter]);
				Counter++;
			}
		}

		[TestMethod()]
		public void ReverseIndexOf()
		{
			var d = new Deque<int> { 1, 2, 3, 4, 5, 6 };
			var reverseView = DequeTest.GetReverseView(d);

			Assert.AreEqual(d.Count - 1, reverseView.IndexOf(1));
			Assert.AreEqual(3, reverseView.IndexOf(3));
		}

		[TestMethod()]
		public void ReverseInsert()
		{
			var d = new Deque<int> { 1, 2, 3, 4, 5 };
			d.Reverse();

			d.Insert(2, 11);
			d.Insert(0, 12);
			d.Insert(7, 13);

			d.RemoveAt(0);
			d.Insert(0, 15);
			d.Insert(0, 16);

			var expected = new List<int> { 16, 15, 5, 4, 11, 3, 2, 1, 13 };

			Assert.IsTrue(ForEachEqual(d, expected));

			expected.Reverse();
			d.Reverse();
			Assert.IsTrue(ForEachEqual(d, expected));
		}

		[TestMethod()]
		public void Wrapping()
		{
			var d = GetWrappedDq();
			Assert.AreEqual(d.Count, 5);

			Assert.IsFalse(d.Remove(5));

			Assert.IsTrue(ForEachEqual(d, new List<int> { 1, 1, 2, 3, 4 }));
		}

		[TestMethod()]
		public void Clear()
		{
			var d = GetSomeDeque();
			Assert.AreNotEqual(0, d.Count);

			d.Clear();

			Assert.AreEqual(0, d.Count);
			Assert.IsTrue(ForEachEqual(d, new List<int>()));

			// works after clearing
			d.Insert(0, 10);
			Assert.AreEqual(10, d[0]);
		}

		[TestMethod()]
		public void Remove()
		{
			var d = new Deque<int>();
			d.Insert(0, 1);
			d.Insert(0, 5);
			Assert.IsTrue(d.Remove(5));
			Assert.AreEqual(d.Count, 1);
			Assert.AreEqual(d[0], 1);


			d.Insert(0, 5);
			d.Insert(0, 6);
		}

		[TestMethod()]
		public void RemoveAt()
		{
			var d = new Deque<int> { 1, 2, 3, 4, 5, 6 };
			d.RemoveAt(3);
			Assert.IsTrue(ForEachEqual(d, new List<int> { 1, 2, 3, 5, 6 }));
			d.RemoveAt(d.Count - 1);
			Assert.IsTrue(ForEachEqual(d, new List<int> { 1, 2, 3, 5 }));
			d.RemoveAt(0);
			Assert.IsTrue(ForEachEqual(d, new List<int> { 2, 3, 5 }));
		}

		[TestMethod()]
		public void RemoveAtBig()
		{
			var d = new Deque<int>();
			var l = new List<int>();


			Random rnd = new Random();
			for (int i = 0; i < 10000; i++)
			{
				d.Add(i);
				l.Add(i);
			}

			for(int i = 0; i < 10000; i++)
			{
				int ind = rnd.Next(0, 10000 - i);
				d.RemoveAt(ind);
				l.RemoveAt(ind);
				Assert.IsTrue(ForEachEqual(d, l));
			}
			
		}

		[TestMethod()]
		public void RemoveNonExistent()
		{
			var d = new Deque<int>();
			d.Insert(0, 1);
			d.Insert(0, 5);

			Assert.IsFalse(d.Remove(2));
			Assert.AreEqual(d.Count, 2);
		}

		[TestMethod()]
		public void RemoveEmpty()
		{
			var d = new Deque<int>();
			Assert.IsFalse(d.Remove(5));
		}

		[TestMethod()]
		public void RemoveAtOutOfRange()
		{
			var d = new Deque<int>();
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => d.RemoveAt(5));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => d.RemoveAt(-1));
		}

		[TestMethod()]
		public void Remove2()
		{
			var d = GetWrappedDq();

			d.RemoveAt(3);
			Assert.IsTrue(ForEachEqual(d, new List<int> { 1, 1, 2, 4 }));

			Assert.IsTrue(d.Remove(1));
			Assert.IsTrue(ForEachEqual(d, new List<int> { 1, 2, 4 }));

			Assert.ThrowsException<ArgumentOutOfRangeException>(() => d.RemoveAt(5));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => d.RemoveAt(-1));
		}

		[TestMethod()]
		public void Indexer()
		{
			var d = GetDeque();
			d.Add(1);
			d.Insert(0, 2);
			d.RemoveAt(0);
			d.Insert(0, 3);

			Assert.AreEqual(d[0], 3);
			Assert.AreEqual(d[1], 1);
		}

		[TestMethod()]
		public void IndexerOutOfRange()
		{
			var d = GetDeque();
			d.Insert(0, 1);

			Assert.ThrowsException <ArgumentOutOfRangeException>(() => d[1]);
			Assert.ThrowsException <ArgumentOutOfRangeException>(() => d[-1]);
			Assert.ThrowsException <ArgumentOutOfRangeException>(() => d[10]);
		}

		[TestMethod()]
		public void Recodex4()
		{
			// z outputu, co mi poslal Honza
			var d = new Deque<int> { 1, 2, 3 };

			Assert.IsTrue(ForEachEqual(d, new List<int> { 1, 2, 3 }));
			d.Insert(0, -1);
			Assert.IsTrue(ForEachEqual(d, new List<int> { -1, 1, 2, 3 }));
			d.Insert(0, -2);
			Assert.IsTrue(ForEachEqual(d, new List<int> { -2, -1, 1, 2, 3 }));
			d.Insert(0, -3);
			Assert.IsTrue(ForEachEqual(d, new List<int> { -3, -2, -1, 1, 2, 3 }));

			d.Remove(1);
			Assert.IsTrue(ForEachEqual(d, new List<int> { -3, -2, -1, 2, 3 }));
			d.Remove(-3);
			Assert.IsTrue(ForEachEqual(d, new List<int> { -2, -1, 2, 3 }));
			d.Remove(3);
			Assert.IsTrue(ForEachEqual(d, new List<int> { -2, -1, 2 }));
		}


		bool ForEachEqual(Deque<int> d, List<int> to)
		{
			var actualItems = new List<int>();

			foreach (var item in d)
			{
				actualItems.Add(item);
			}

			return ListCmp(to, actualItems);
		}

		bool ListCmp(List<int> l1, List<int> l2)
		{
			if (l1.Count != l2.Count)
				return false;

			for (var i = 0; i < l1.Count; ++i)
				if (l1[i] != l2[i])
					return false;

			return true;
		}

		Deque<int> GetDeque()
		{
			return new Deque<int>();
		}

		Deque<int> GetSomeDeque()
		{
			return new Deque<int> { -10, -9, -8, -7, -6 };
		}

		Deque<int> GetWrappedDq()
		{
			var d = new Deque<int> { 1, 1, 1, 1, 1 };

			d.RemoveAt(0);
			d.Add(1);
			Assert.AreEqual(d.Count, 5);

			d.RemoveAt(0);
			d.Add(2);
			Assert.AreEqual(d.Count, 5);

			d.RemoveAt(0);
			d.Add(3);
			Assert.AreEqual(d.Count, 5);

			d.RemoveAt(0);
			d.Add(4);

			return d;
		}

		[TestMethod()]
		public void IndexOf()
		{
			var D = new Deque<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

			Assert.AreEqual(1, D.IndexOf(2));
			Assert.AreEqual(9, D.IndexOf(10));
			D.Reverse();
		}

		[TestMethod()]
		public void IndexOfReverse()
		{
			var D = new Deque<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

			Assert.AreEqual(1, D.IndexOf(2));
			Assert.AreEqual(9, D.IndexOf(10));
			D.Reverse();
			Assert.AreEqual(1, D.IndexOf(9));
			Assert.AreEqual(0, D.IndexOf(10));
		}

		[TestMethod()]
		public void IndexOfNone()
		{
			var D = new Deque<int> { };

			Assert.AreEqual(-1, D.IndexOf(100));

		}
	}
}