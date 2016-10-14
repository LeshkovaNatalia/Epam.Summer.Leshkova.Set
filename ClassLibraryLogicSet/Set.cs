using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryLogicSet
{
    public class Set<T> : IEquatable<Set<T>> where T : class
    {
        #region Fields
        private T[] setElements;
        private int count = 0;
        private const int defaultCount = 10;
        #endregion

        #region Properties
        public T[] SetElements
        {
            get { return setElements; }
            private set { setElements = value; }
        }

        public int Count
        {
            get { return count; }
            private set { count = value; }
        }
        #endregion

        #region Ctors

        /// <summary>
        /// Initializes a new, empty instance of the Array class using 
        /// the default initial capacity equal 10.
        /// </summary>
        public Set()
        {
            SetElements = new T[defaultCount];
        }

        /// <summary>
        /// Initializes a new, empty instance of the Array class using 
        /// the custom initial capacity. 
        /// </summary>
        /// <param name="length">Initial capacity of array.</param>
        public Set(int length)
        {
            SetElements = new T[length];
            Count = length;
        }

        /// <summary>
        /// Initializes a new instance of the Array class using 
        /// the elemets from array. 
        /// </summary>
        /// <param name="elements">Array of elements.</param>
        public Set(T[] elements)
        {
            SetElements = new T[elements.Length];
            for (int i = 0; i < elements.Length; i++)
                if (!SetElements.Contains<T>(elements[i]))
                {
                    SetElements[i] = elements[i];
                    Count++;
                }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Method Add add element in array if its not present in array.
        /// </summary>
        /// <param name="element">Added element.</param>
        public void Add(T element)
        {
            AddElement(element);
        }

        /// <summary>
        /// Method Remove remove element in array if its present in array.
        /// </summary>
        /// <param name="element">Removed element.</param>
        public void Remove(T element)
        {
            RemoveElement(element);
        }

        /// <summary>
        /// Method Union realise operation Union of two sets.
        /// </summary>
        /// <param name="lhs">First set.</param>
        /// <param name="rhs">Second set.</param>
        /// <returns>Set after union of two set.</returns>
        public static Set<T> Union(Set<T> lhs, Set<T> rhs)
        {
            Set<T> resultSet;
            if(lhs.SetElements.Length > rhs.SetElements.Length)
            {
                resultSet = new Set<T>(lhs.SetElements.Length);
                UnionSets(resultSet, lhs, rhs);
            }
            else
            {
                resultSet = new Set<T>(rhs.SetElements.Length);
                UnionSets(resultSet, rhs, lhs);
            }

            return resultSet;
        }

        /// <summary>
        /// Method Intersection realise operation Intersection of two sets.
        /// </summary>
        /// <param name="lhs">First set.</param>
        /// <param name="rhs">Second set.</param>
        /// <returns>Set after intersection of two set.</returns>
        public static Set<T> Intersection(Set<T> lhs, Set<T> rhs)
        {
            Set<T> resultSet = new Set<T>();
            if (lhs.SetElements.Length > rhs.SetElements.Length)                
                IntersectionSets(resultSet, rhs, lhs);
            else
                IntersectionSets(resultSet, lhs, rhs);

            return resultSet;
        }

        /// <summary>
        /// Method Except realise operation Except of two sets 
        /// </summary>
        /// <param name="lhs">First set.</param>
        /// <param name="rhs">Second set.</param>
        /// <returns>Set after except of two set.</returns>
        public static Set<T> Except(Set<T> lhs, Set<T> rhs)
        {
            Set<T> resultSet = new Set<T>();
            if (lhs.SetElements.Length > rhs.SetElements.Length)
                ExceptSets(resultSet, rhs, lhs);
            else
                ExceptSets(resultSet, lhs, rhs);

            return resultSet;
        }

        #region Override Object Methods

        /// <summary>
        /// Overload method Equals.
        /// </summary>
        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return false;

            if (obj.GetType() != this.GetType())
                return false;

            return Equals((Set<T>)obj);
        }

        /// <summary>
        /// Overlod method GetHashCode().
        /// </summary>
        /// <returns>Hashcode.</returns>
        public override int GetHashCode()
        {
            return this.SetElements.GetHashCode();
        }

        #endregion

        #region Method Equals. Overload operations == | !=

        /// <summary>
        /// Realisation of method Equals of interface IEquatable<Set<T>>.
        /// </summary>
        public bool Equals(Set<T> otherSet)
        {
            if (ReferenceEquals(null, otherSet))
                return false;
            if (ReferenceEquals(this, otherSet))
                return false;

            return CompareSets(this, otherSet);
        }

        /// <summary>
        /// Overload operator Equality.
        /// </summary>
        /// <param name="lhs">Left operand.</param>
        /// <param name="rhs">Right operand.</param>
        /// <returns>True if left operand == right operand.</returns>
        public static bool operator ==(Set<T> lhs, Set<T> rhs)
        {
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return Equals(lhs, rhs);

            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Overload operator Inequality.
        /// </summary>
        /// <param name="lhs">Left operand.</param>
        /// <param name="rhs">Right operand.</param>
        /// <returns>True if left operand != right operand.</returns>
        public static bool operator !=(Set<T> lhs, Set<T> rhs) => !(lhs == rhs);

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Method AddElement add element in array if its not present in array.
        /// </summary>
        private void AddElement(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (!SetElements.Contains<T>(element))
            {
                if (Count < SetElements.Length)
                {
                    SetElements[Count] = element;
                    Count++;
                }
                else
                {
                    ResizeSetLength(SetElements);                    
                    SetElements[Count] = element;
                    Count++;
                }
            }
        }

        /// <summary>
        /// Method RemoveElement remove element in array if its present in array.
        /// </summary>
        private void RemoveElement(T element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            if (SetElements.Contains<T>(element))
            {
                T[] resultSet = new T[Count - 1];
                for (int i = 0, j = 0; i < Count; i++)
                    if (SetElements[i] != element)
                    {
                        resultSet[j] = SetElements[i];
                        j++;
                    }
                SetElements = resultSet;
                Count--;
            }
        }

        /// <summary>
        /// Method ResizeSetLength change length of array.
        /// </summary>
        private void ResizeSetLength(T[] set)
        {
            T[] oldElements = new T[SetElements.Length];
            SetElements.CopyTo(oldElements, 0);
            SetElements = new T[oldElements.Length + defaultCount];
            oldElements.CopyTo(SetElements, 0);
        }

        /// <summary>
        /// Method UnionSets realise operation Union of two sets. 
        /// </summary>
        private static void UnionSets(Set<T> resultSet, Set<T> lhs, Set<T> rhs)
        {
            lhs.SetElements.CopyTo(resultSet.SetElements, 0);
            for (int i = 0; i < rhs.Count; i++)
                resultSet.Add(rhs.SetElements[i]);
        }

        /// <summary>
        /// Method IntersectionSets realise operation Intersection of two sets. 
        /// </summary>
        private static void IntersectionSets(Set<T> resultSet, Set<T> lhs, Set<T> rhs)
        {
            for (int i = 0; i < lhs.Count; i++)
                if (lhs.SetElements.Contains(rhs.SetElements[i]))
                    resultSet.Add(lhs.SetElements[i]);
        }

        /// <summary>
        /// Method ExceptSets realise operation Except of two sets. 
        /// </summary>
        private static void ExceptSets(Set<T> resultSet, Set<T> lhs, Set<T> rhs)
        {
            for (int i = 0; i < lhs.Count; i++)
                if (!lhs.SetElements.Contains(rhs.SetElements[i]))
                    resultSet.Add(lhs.SetElements[i]);
        }

        /// <summary>
        /// Method CompareSets compare two sets.
        /// </summary>
        /// <param name="set">First set.</param>
        /// <param name="otherSet">Second set.</param>
        /// <returns>True if sets are equal and false if not.</returns>
        private bool CompareSets(Set<T> set, Set<T> otherSet)
        {
            foreach (var item in otherSet.SetElements)
            {
                if (!set.SetElements.Contains(item))
                    return false;
            }

            return true;
        }

        #endregion
    }
}
