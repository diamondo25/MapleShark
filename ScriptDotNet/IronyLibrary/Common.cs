#region License
/* **********************************************************************************
 * Copyright (c) Roman Ivantsov
 * This source code is subject to terms and conditions of the MIT License
 * for Irony. A copy of the license can be found in the License.txt file
 * at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * MIT License.
 * You must not remove this notice from this software.
 * **********************************************************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Irony {
  //Some common classes

  //public class StringList : List<string> { }
  public class StringDictionary : Dictionary<string, string> { }
  public class CharList : List<char> { }

#if !SILVERLIGHT
  public class StringSet : HashSet<string>
  {
    public void AddRange(IEnumerable<string> values)
    {
      foreach (string value in values)  Add(value);
    }
    public override string ToString()
    {
      return ToString(" ");
    }
    public string ToString(string separator)
    {
      return TextUtils.JoinStrings(separator, this);
    }
  }
#else
  public class StringSet : ICollection<string>
  {
    Dictionary<string, bool> stringValues = new Dictionary<string, bool>();

    public void AddRange(IEnumerable<string> values)
    {
      foreach (string value in values)
        if (!stringValues.ContainsKey(value))
          Add(value);
    }
    public override string ToString()
    {
      return ToString(" ");
    }
    public string ToString(string separator)
    {
      return TextUtils.JoinStrings(separator, this);
    }

    #region ICollection<string> Members

    public void Add(string item)
    {
      if (!stringValues.ContainsKey(item))
        stringValues.Add(item, true);
    }

    public void Clear()
    {
      stringValues.Clear();
    }

    public bool Contains(string item)
    {
      return stringValues.ContainsKey(item);
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public int Count
    {
      get { return stringValues.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove(string item)
    {
      if (!stringValues.ContainsKey(item))
      {
        stringValues.Remove(item);
        return true;
      }

      return false;
    }

    #endregion

    #region IEnumerable<string> Members

    public IEnumerator<string> GetEnumerator()
    {
      return stringValues.Keys.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator()
    {
      return stringValues.Keys.GetEnumerator();
    }

    #endregion
  }
#endif
  public class StringList : List<string> {
    public StringList() { }
    public StringList(params string[] args) {
      AddRange(args);
    }
    public new void AddRange(IEnumerable<string> keys) {
      foreach (string key in keys)
        this.Add(key);
    }
    public override string ToString() {
      return ToString(" ");
    }
    public string ToString(string separator) {
      return TextUtils.JoinStrings(separator, this);
    }
    //Used in sorting suffixes and prefixes; longer strings must come first in sort order
    public static int LongerFirst(string x, string y) {
      try {//in case any of them is null
        if (x.Length > y.Length) return -1;
      } catch { }
      return 0;
    }

  }//KeyList class

}
