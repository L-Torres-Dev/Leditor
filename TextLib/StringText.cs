using System;

namespace Leditor.TextLib
{
    public class StringText : IText
    {
        private string str;
        public StringText()
        {
            str = "";
        }

        public int Length => str.Length;

        public void AddText(char c, int index)
        {
            str = str.Insert(index, c.ToString());
        }

        public void AddText(string str, int index)
        {
            this.str = this.str.Insert(index, str);
        }

        public void RemoveText(int index)
        {
            if (str.Length == 0) return;
            str = str.Remove(index, 1);
        }
        
        public void RemoveText(int start, int end)
        {
            str = str.Remove(start, end - start + 1);
        }

        public override string ToString()
        {
            return str;
        }
    }
}
